using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Interfaces.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using INSS.EIIR.Models.CaseModels;
using System.IO.Compression;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class XMLSink : IDataSink<InsolventIndividualRegisterModel>
    {

        private MemoryStream? _xmlStream;
        private int _recordBufferSize;
        private int _recordCount;
        private readonly IList<string> _blockIDList;
        private BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly string _blobContainerName;
        private readonly string _blobConnectionString;
        private readonly IExtractRepository _eiirRepository;
        private readonly IExistingBankruptciesService _existingBankruptciesService;
        private string? _fileName = null;
        private int? _extractId = null;
        private ExtractVolumes _extractVolumes;
        private SortedList<int, int> _existingBankruptcies;


        public XMLSink(XMLSinkOptions options, IExtractRepository extractRepository, IExistingBankruptciesService existingBankruptcies) 
        { 
            _recordCount = 0;

            _blockIDList = new List<string>();

            _blobContainerName = options.StorageName;
            _blobConnectionString = options.StoragePath;
            _recordBufferSize = options.WriteToBlobRecordBufferSize;

            if (string.IsNullOrEmpty(_blobContainerName))
            {
                throw new XmlSinkException("XML Sink options - No StorageName check XmlContainer setting in configuration", null);
            }
            if (string.IsNullOrEmpty(_blobConnectionString))
            {
                throw new XmlSinkException("XML Sink options - No StoragePath check TargetBlobConnectionString setting in configuration", null);
            }

            _blobServiceClient = new BlobServiceClient(_blobConnectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);

            //IExtractRepository required to get filename and update extract availability though comes with high overhead
            _eiirRepository = extractRepository;

            _extractVolumes = new ExtractVolumes();

            _existingBankruptciesService = existingBankruptcies;
            
        }

        public async Task Start() {

            var extractJob = _eiirRepository.GetExtractAvailable();

            _extractId = extractJob.ExtractId;
            _fileName = extractJob.ExtractFilename;

            _existingBankruptcies = await _existingBankruptciesService.GetExistingBankruptcies();

            _xmlStream = new MemoryStream();

            return; 
        }

        private void WriteIirHeaderToStream()
        {
            IirXMLWriterHelper.WriteIirHeaderToStream(ref _xmlStream, _extractVolumes);
        }

        private void WriteIirFooterToStream()
        {
            IirXMLWriterHelper.WriteIirFooterToStream(ref _xmlStream);
        }

        public async Task<SinkCompleteResponse> Complete(bool commit = true)
        {

            WriteIirFooterToStream();
            //Write out remainder of stream
            await WriteStreamToBlob();

            //Write out header to beginning of blob and commit
            WriteIirHeaderToStream();
            await WriteStreamToBlob(commit: true, appendToBegin: true);

            await _existingBankruptciesService.SetExistingBankruptcies(_existingBankruptcies);

            //Create Zip file
            if (commit)
            { 
                await CreateAndUploadZip(_fileName);
                _eiirRepository.UpdateExtractAvailable();
            }

            _xmlStream.Close();
            
            return new SinkCompleteResponse() { IsError = false };
        }

        public async Task<DataSinkResponse> Sink(InsolventIndividualRegisterModel model)
        {
            var resp = new DataSinkResponse() { IsError = false };

            try 
            {
                WriteIirRecordToStream(model, ref _xmlStream);

                _extractVolumes.TotalEntries++;

                switch (model.RecordType) 
                {
                    case IIRRecordType.BKT:
                    case IIRRecordType.BRO:
                    case IIRRecordType.BRU:
                    case IIRRecordType.IBRO:
                        _extractVolumes.TotalBanks++;
                        if (!_existingBankruptcies.ContainsKey(model.caseNo)){ 
                            _extractVolumes.NewBanks++;
                            _existingBankruptcies.Add(model.caseNo, model.caseNo);
                        }
                        break;
                    case IIRRecordType.DRO:
                    case IIRRecordType.DRRO:
                    case IIRRecordType.DRRU:
                        _extractVolumes.TotalDros++;
                        break;
                    case IIRRecordType.IVA:
                        _extractVolumes.TotalIVAs++;
                        break;
                    default:
                        resp.IsError = true;    
                        resp.ErrorMessage = $"Unable to detemine record type in XML Extract for record: {model.caseNo}";
                        resp.Model = model;
                        break;
                }
                
                _recordCount++;  

            }
            catch (Exception ex)
            {
                //Catch any known exception in here to put in respone
                //Not handling anything because we don't know what we'll uncover
                throw;  
            }

            //Flushout stream to Storage
            try
            {
                //Following if statement commented out pending implementation of write stream to blob
                if (_recordCount % _recordBufferSize == 0)
                    await WriteStreamToBlob();

            }
            catch (Exception ex) 
            {
                //Catch any know exception to put in response
                throw;            
            }

            return resp;
        }

        private async Task WriteStreamToBlob(bool commit = false, bool appendToBegin = false)
        {
            _xmlStream.Position = 0;

            BlockBlobClient blobClient = _containerClient.GetBlockBlobClient($"{_fileName}.xml");

            string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            if (appendToBegin)
                _blockIDList.Insert(0, blockID);    
            else
                _blockIDList.Add(blockID);

            await blobClient.StageBlockAsync(blockID, _xmlStream);

            if (commit)
            {
                await blobClient.CommitBlockListAsync(_blockIDList);
            }

            //Create new stream
            _xmlStream = new MemoryStream();    
        }

        private void WriteIirRecordToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            IirXMLWriterHelper.WriteIirReportRequestToStream(model, ref xmlStream);
        }

        private async Task CreateAndUploadZip(string filename)
        {
            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient($"{filename}.xml");
                if (await blobClient.ExistsAsync())
                {
                    using var xmlFileStream = await blobClient.OpenReadAsync();

                    var blobClientZip = _containerClient.GetBlobClient($"{filename}.zip");
                    using var zipStream = await blobClientZip.OpenWriteAsync(true);
                    using var zip = new ZipArchive(zipStream, ZipArchiveMode.Create);


                    ZipArchiveEntry entry = zip.CreateEntry($"{filename}.xml", CompressionLevel.Optimal);
                    using var innerFile = entry.Open();
                    await xmlFileStream.CopyToAsync(innerFile);
                }
            }
            catch (Exception ex)
            {
                throw new XmlSinkException($"Error creating zip for: {filename}", ex);
            }
        }

    }
}
