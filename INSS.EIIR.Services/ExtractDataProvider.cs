using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Data;
using System.IO.Compression;
using System.Text;

namespace INSS.EIIR.Services;
public class ExtractDataProvider : IExtractDataProvider
{
    private readonly ILogger _logger;
    private readonly IExtractRepository _extractRepository;
    private readonly IList<string> _blockIDList;
    private readonly DatabaseConfig _dbConfig;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;
    private readonly string _blobContainerName;
    private readonly string _blobConnectionString;


    public ExtractDataProvider(
        ILoggerFactory loggerFactory,
        IExtractRepository extractRepository,
        IOptions<DatabaseConfig> dbOptions,
        BlobServiceClient blobServiceClient)
    {
        _logger = loggerFactory.CreateLogger<ExtractDataProvider>();
        _extractRepository = extractRepository;
        _blockIDList = new List<string>();
        _dbConfig = dbOptions.Value;
        _blobServiceClient = blobServiceClient;
        _blobContainerName = Environment.GetEnvironmentVariable("blobcontainername");
        _blobConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
        if (string.IsNullOrEmpty(_blobContainerName))
        {
            throw new Exception("ExtractDataProvider missing blobcontainername configuration");
        }
        if (string.IsNullOrEmpty(_blobConnectionString))
        {
            throw new Exception("ExtractDataProvider missing storageconnectionstring configuration");
        }

        _containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
    }
    public async Task GenerateSubscriberFile(string filename)
    {
        using SqlConnection connection = new(_dbConfig.ConnectionString);
        await connection.OpenAsync();
        using SqlCommand command = new($"EXEC {_dbConfig.GetXmlDataProcedure}", connection) { CommandTimeout = _dbConfig.CommandTimeout };

        // await _containerClient.CreateIfNotExistsAsync();

        // The reader needs to be executed with the SequentialAccess behavior to enable network streaming
        // Otherwise ReadAsync will buffer the entire text document into memory which can cause scalability issues or even OutOfMemoryExceptions
        using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
        while (await reader.ReadAsync())
        {
            try
            {
                char[] buffer = new char[_dbConfig.DataBufferSize];
                int charsRead = 0;
                using TextReader data = reader.GetTextReader(0);

                int ReportRequestOS, EndIndividualDetailsOS, EndReportRequestOS, EndReportDetailsOS;
                ReportRequestOS = EndIndividualDetailsOS = EndReportRequestOS = EndReportDetailsOS = 0;

                do
                {
                    charsRead = await data.ReadAsync(buffer, 0, buffer.Length);
                    if (charsRead > 0) { 
                        byte[] byteArray = Encoding.UTF8.GetBytes(buffer, 0, charsRead);

                        var transposed = byteArray.InsertCrLfAfter(Encoding.UTF8.GetBytes("<ReportRequest>"), ref ReportRequestOS, ref charsRead)
                                                .InsertCrLfAfter(Encoding.UTF8.GetBytes("</IndividualDetails>"), ref EndIndividualDetailsOS, ref charsRead)
                                                .InsertCrLfAfter(Encoding.UTF8.GetBytes("</ReportRequest>"), ref EndReportRequestOS, ref charsRead)
                                                .InsertCrLfAfter(Encoding.UTF8.GetBytes("</ReportDetails>"), ref EndReportDetailsOS, ref charsRead);

                        await UploadToBlobInBlocks(filename, transposed, false);
                    }
                } while (charsRead > 0);
                await UploadToBlobInBlocks(filename, null, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Generating Subscriber File : [{ex}]");
            }
        }
    }

    private async Task UploadToBlobInBlocks(string filename, byte[] byteArray, bool commit = false)
    {
        try
        {
            BlockBlobClient blobClient = _containerClient.GetBlockBlobClient($"{filename}.xml");
            if (byteArray != null)
            {
                using var stream = new MemoryStream(byteArray);

                string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                _blockIDList.Add(blockID);

                await blobClient.StageBlockAsync(blockID, stream);
            }

            if (commit)
            {
                var responseXml = await blobClient.CommitBlockListAsync(_blockIDList);
                await CreateAndUploadZip(filename);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error Upload To Blob In Blocks: [{ex}]");
        }
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
            _logger.LogError($"Error Create And Upload Zip: [{ex}]");
        }
    }

    public async Task<ExtractWithPaging> ListExtractsAsync(PagingParameters pagingParameters)
    {
        var totalRecords = await _extractRepository.GetTotalExtractsAsync();    
        var results = await _extractRepository.GetExtractsAsync(pagingParameters);

        var response = new ExtractWithPaging
        {
            Paging = new Models.PagingModel(totalRecords, pagingParameters.PageNumber, pagingParameters.PageSize),
            Extracts = results,
        };

        return response;
    }

    public async Task<byte[]> DownloadLatestExtractAsync(string blobName)
    {
        BlobClient blobClient = new(_blobConnectionString, _blobContainerName, blobName);
        var downloadContent = await blobClient.DownloadAsync();
        using MemoryStream ms = new();
        await downloadContent.Value.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<Extract> GetLatestExtractForDownload()
    {
        return await _extractRepository.GetLatestExtractForDownload();
    }




}

public static class ByteArrayExtension
{
    /// <summary>
    /// Extension method to insert CfLf after specified elements in XML Stream
    /// </summary>
    /// <param name="self">A byte array as part of larger XML document</param>
    /// <param name="target">A UTF8 byte encoded string of XMLtag to target</param>
    /// <param name="offset">When tag is split between two sets of buffered data the offset is number of bytes truncated from the beginning of the seconf file</param>
    /// <param name="byteCount">The total number of bytes read in, this changes as new bytes are added</param>
    /// <returns></returns>
    public static byte[] InsertCrLfAfter(this byte[] self, byte[] target, ref int offset, ref int byteCount)
    {
        //for calculation of truncated target
        var originalLength = byteCount;

        //new length once additional characters added
        var newLength = originalLength;

        //A new array to be used an source of output with larger size to account for additional bytes
        var outputArray = new byte[((int)(newLength * 2))];

        //start position in self from where current comparison starts
        var startIndex = 0;

        //position in self where next comparison against target will commence
        var currentIndex = 0;

        //length of target to be copied down
        var targetLength = target.Length;

        //where new values will copied to
        var targetIndex = 0;

        while (currentIndex < originalLength)
        {
            //Adjust target for offset at beginning and end
            var tempTarget = target;

            //Adjust target for offset at beginning
            if (currentIndex - offset < 0)
            {
                tempTarget = tempTarget[^(targetLength + (currentIndex - offset))..^0];
            }

            //Adjust target at end of byte array
            if (currentIndex + tempTarget.Length > originalLength)
            {
                tempTarget = tempTarget[0..(originalLength - currentIndex)];
            }

            //Speed improvement - scan for first character in target
            if (tempTarget.Length % targetLength == 0)
                while (self[currentIndex] != tempTarget[0] && currentIndex < originalLength - targetLength)
                    currentIndex++;

            if (tempTarget.SequenceEqual(self[currentIndex..(currentIndex + tempTarget.Length)]))
            {
                //Copy down matching characters from start index
                for (int i = startIndex; i < currentIndex + tempTarget.Length; i++)
                {
                    outputArray[targetIndex] = self[i];
                    targetIndex++;
                }

                //increment start index for new characters
                startIndex = currentIndex + tempTarget.Length;

                //Add CR LF if
                //tempTarget = the end of target
                //and tempTarget.Length  > 0 (this indicates we've reach end of byte array)
                if (tempTarget.SequenceEqual(target[^(tempTarget.Length)..^0]) && tempTarget.Length > 0)
                {
                    outputArray[targetIndex] = 0x0D;
                    targetIndex++;
                    outputArray[targetIndex] = 0x0A;
                    targetIndex++;

                    //adjusts newLength for additional bytes
                    newLength = newLength + 2;
                }

                //We have copied down because we have matched => set new offset
                offset = tempTarget.Length % targetLength;

                //set new currentIndex bearing in mind it is incremented at end of loop
                currentIndex = currentIndex + tempTarget.Length - 1;

            }

            currentIndex++;

        }

        //Copy balance at end
        for (int i = startIndex; i < originalLength; i++)
        {
            outputArray[targetIndex] = self[i];
            targetIndex++;

            //Reset offset here because copying balance at end mean it hasn't been done in while block
            offset = 0;
        }

        //Update byteCount which is passed back such that it can be chained to next InsertCrLfAfter call
        //Important when selecting the last output for last buffer
        byteCount = newLength;

        return outputArray[0..newLength];
    }
}