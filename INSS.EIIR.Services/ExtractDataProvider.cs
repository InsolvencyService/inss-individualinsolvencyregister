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

