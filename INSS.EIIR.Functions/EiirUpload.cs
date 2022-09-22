using Azure.Storage.Blobs;
using INSS.EIIR.Models.ExtractModels;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions
{
    public static class EiirUpload
    {
        public  static async Task UploadBlob(ReportDetails extract)
        {

            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol = https; AccountName = stuksouthdeveiir; AccountKey = CRvs0OLcGWTwzdnCFMLkR2Os / EupysgMzHXGGsHQtiY0Wh2RO8iV19OilM6KRv2w3Z + 3VoJKzEMp + AStbksnSw ==; EndpointSuffix = core.windows.net");
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