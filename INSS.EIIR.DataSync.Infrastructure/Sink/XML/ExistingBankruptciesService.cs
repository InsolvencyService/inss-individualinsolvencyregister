using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using INSS.EIIR.AzureSearch.IndexMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class ExistingBankruptciesService : IExistingBankruptciesService
    {

        private readonly string _connectionString;
        private readonly string _containerName;

        private BlobClient _blobClient;

        public ExistingBankruptciesService(ExistingBankruptciesOptions options)
        {
            _connectionString = options.BlobStorageConnectionString;
            _containerName = options.BlobStorageContainer;

            _blobClient = new BlobClient(
                options.BlobStorageConnectionString,
                options.BlobStorageContainer,
                options.ExistingBankruptciesFileName,
                new BlobClientOptions()
                {
                    Retry =
                    {
                        MaxRetries = 5,
                        Delay = TimeSpan.FromSeconds(2),
                        Mode = Azure.Core.RetryMode.Exponential

                    }
                }
            );
        }

        async Task<SortedList<int, int>> IExistingBankruptciesService.GetExistingBankruptcies()
        {
            var defaultReturn = new SortedList<int, int>();

            try 
            { 
                BlobDownloadInfo download = await _blobClient.DownloadAsync();
                byte[] result = new byte[download.ContentLength];
                await download.Content.ReadAsync(result, 0, (int)download.ContentLength);

                return JsonSerializer.Deserialize<SortedList<int, int>>(Encoding.UTF8.GetString(result));


            } catch (RequestFailedException)
            {
                return defaultReturn;
            }
        }

        async Task IExistingBankruptciesService.SetExistingBankruptcies(SortedList<int, int> existingBankruptcies)
        {
            await CreateContainer();

            try
            {
                var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, existingBankruptcies);
                stream.Seek(0, SeekOrigin.Begin);   
                await _blobClient.UploadAsync(stream, overwrite: true);
            }
            catch (Exception ex)
            {
                throw new XmlSinkException($"Failed to save existing bankruptcy identifiers", ex);
            }
        }

        private async Task CreateContainer()
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync();
        }
    }
}
