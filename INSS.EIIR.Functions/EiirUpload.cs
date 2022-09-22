using Azure.Storage.Blobs;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions
{
    public static class EiirUpload
    {
        public  static async Task UploadBlob(ReportDetails extract , Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            IConfigurationRoot config = EiirDailyExtractHelpers.GetConfig(context);

            var blobServiceClient = new BlobServiceClient(config["connectionString"]);
            string containerName = "EiirRDailyExtracts";

            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            var todaysDate = DateTime.Now.ToString();
            string fileName = "EiirDailyExtract-" + todaysDate + ".xml";

            File.WriteAllText(fileName, extract.ToString());
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileName, true);
            await UploadZipFile(fileName, blobClient);

        }

        private static async Task UploadZipFile(string fileName, BlobClient blobClient)
        {
            var todaysDate = DateTime.Now.ToString();

            string startPath = @".\" + fileName;
            string zipPath = @".\" + "EiirDailyExtract-" + todaysDate + ".zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);
            await blobClient.UploadAsync(zipPath, true);

        }
    }
}