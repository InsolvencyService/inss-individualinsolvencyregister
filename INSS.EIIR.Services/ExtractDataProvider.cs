using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
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
    private readonly ArrayList _blockIDArrayList;
    private readonly DatabaseConfig _dbConfig;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public ExtractDataProvider(
        ILoggerFactory loggerFactory,
        IOptions<DatabaseConfig> dbOptions,
        BlobServiceClient blobServiceClient)
    {
        _logger = loggerFactory.CreateLogger<ExtractDataProvider>();
        _blockIDArrayList = new();
        _dbConfig = dbOptions.Value;
        _blobServiceClient = blobServiceClient;
        var blobContainerName = Environment.GetEnvironmentVariable("blobcontainername");
        if (string.IsNullOrEmpty(blobContainerName))
        {
            throw new Exception("ExtractDataProvider missing blobcontainername configuration");
        }

        _containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
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
}