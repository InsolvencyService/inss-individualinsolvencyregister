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
        private string? _fileName = null;
        private int? _extractId = null;


        public XMLSink(XMLSinkOptions options, IExtractRepository extractRepository) 
        { 
            _recordCount = 0;

            _blockIDList = new List<string>();

            _blobContainerName = options.StorageName;
            _blobConnectionString = options.StoragePath;
            _recordBufferSize = options.WriteToBlobRecordBufferSize;

            if (string.IsNullOrEmpty(_blobContainerName))
            {
                throw new Exception("XML Sink options - No StorageName check XmlContainer setting in configuration");
            }
            if (string.IsNullOrEmpty(_blobConnectionString))
            {
                throw new Exception("XML Sink options - No StoragePath check TargetBlobConnectionString setting in configuration");
            }

            _blobServiceClient = new BlobServiceClient(_blobConnectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);

            //IExtractRepository required to get filename and update extract availability though comes with high overhead
            _eiirRepository = extractRepository;
            
        }

        public async Task Start() {

            var extractJob = _eiirRepository.GetExtractAvailable();

            _extractId = extractJob.ExtractId;
            _fileName = extractJob.ExtractFilename;
            _xmlStream = new MemoryStream();

            WriteIirHeaderToStream();

            return; 
        }

        private void WriteIirHeaderToStream()
        {
            IirXMLWriterHelper.WriteIirHeaderToStream(ref _xmlStream);
        }

        private void WriteIirFoorterToStream()
        {
            IirXMLWriterHelper.WriteIirFooterToStream(ref _xmlStream);
        }

        public async Task<SinkCompleteResponse> Complete()
        {
            WriteIirFoorterToStream();

            //Write out remainder of stream
            await WriteStreamToBlob(true);

            //Create Zip file
            _eiirRepository.UpdateExtractAvailable();

            _xmlStream.Close();
            
            return new SinkCompleteResponse() { IsError = false };
        }

        public async Task<DataSinkResponse> Sink(InsolventIndividualRegisterModel model)
        {
            var resp = new DataSinkResponse() { IsError = false };

            try 
            {
                WriteIirRecordToStream(model, ref _xmlStream);
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

        private async Task WriteStreamToBlob(bool commit = false)
        {
            _xmlStream.Position = 0;

            BlockBlobClient blobClient = _containerClient.GetBlockBlobClient($"{_fileName}.xml");

            string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            _blockIDList.Add(blockID);

            await blobClient.StageBlockAsync(blockID, _xmlStream);

            if (commit)
            {
                await blobClient.CommitBlockListAsync(_blockIDList);
                //await CreateAndUploadZip(filename);
            }

            //Create new stream
            _xmlStream = new MemoryStream();    
        }

        private void WriteIirRecordToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            IirXMLWriterHelper.WriteIirReportRequestToStream(model, ref xmlStream);
        }
    }
}
