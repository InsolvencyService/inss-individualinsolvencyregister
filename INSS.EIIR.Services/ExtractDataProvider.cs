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
    private readonly ArrayList _blockIDArrayList;
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
        _blockIDArrayList = new();
        _dbConfig = dbOptions.Value;
        _blobServiceClient = blobServiceClient;
        _blobContainerName = Environment.GetEnvironmentVariable("blobcontainername");
        _blobConnectionString = Environment.GetEnvironmentVariable("blobconnectionstring");
        if (string.IsNullOrEmpty(_blobContainerName))
        {
            throw new Exception("ExtractDataProvider missing blobcontainername configuration");
        }
        if (string.IsNullOrEmpty(_blobConnectionString))
        {
            throw new Exception("ExtractDataProvider missing blobconnectionstring configuration");
        }

        _containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
    }
    public async Task GenerateSubscriberFile(string filename)
    {
        using SqlConnection connection = new(_dbConfig.ConnectionString);
        await connection.OpenAsync();
        using SqlCommand command = new($"EXEC {_dbConfig.GetXmlDataProcedure}", connection) { CommandTimeout = _dbConfig.CommandTimeout };

        await _containerClient.CreateIfNotExistsAsync();

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
                do
                {
                    charsRead = await data.ReadAsync(buffer, 0, buffer.Length);
                    await UploadToBlobInBlocks(filename, buffer, false);
                } while (charsRead > 0);
                await UploadToBlobInBlocks(filename, null, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Generating Subscriber File : [{ex}]");
            }
        }
    }

    private async Task UploadToBlobInBlocks(string filename, char[] buffer, bool commit = false)
    {
        try
        {
            BlockBlobClient blobClient = _containerClient.GetBlockBlobClient($"{filename}.xml");
            if (buffer != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(buffer);
                using var stream = new MemoryStream(byteArray);

                string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                _blockIDArrayList.Add(blockID);

                await blobClient.StageBlockAsync(blockID, stream);
            }

            if (commit)
            {
                string[] blockIDArray = (string[])_blockIDArrayList.ToArray(typeof(string));
                var responseXml = await blobClient.CommitBlockListAsync(blockIDArray);
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