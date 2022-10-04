using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.API.ExtractService
{
    public class DeleteOldExtracts
    {
        [FunctionName("DeleteOldExtracts")]
        public Task RunAsync([TimerTrigger("0 0 22 * * *")]TimerInfo myTimer, ILogger log , Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            var sourceContainer = "eiirdailyextracts";
            var config = new ConfigurationBuilder()
                                .SetBasePath(context.FunctionAppDirectory)
                                .AddJsonFile("local.settings.json", true, true)
                                .AddJsonFile("appsettings.json", true, true)
                                .AddEnvironmentVariables().Build();

            BlobServiceClient serviceClient = new BlobServiceClient(config["StorageConnectionString"]);
            BlobContainerClient sourceContainerClient = serviceClient.GetBlobContainerClient(sourceContainer);

            var blobs = sourceContainerClient.GetBlobs();


            foreach (BlobItem blobItem in blobs)
            {
                if (IsOlderThan30Days(blobItem))
                {

                    BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(blobItem.Name);
                    sourceBlobClient.DeleteIfExists();
                    log.LogInformation($"Blob Name {blobItem.Name} is deleted successfully.");
                }

            }

            return Task.CompletedTask;
        }

        private static bool IsOlderThan30Days(BlobItem blobItem)
        {
           
           TimeSpan timespan = (TimeSpan)(DateTimeOffset.Now - blobItem.Properties.CreatedOn );
           if (timespan.Days > 30)
                return true;
            return false;
        }
    }
}
