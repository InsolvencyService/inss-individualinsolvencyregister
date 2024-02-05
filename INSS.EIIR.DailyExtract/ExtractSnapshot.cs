using System;
using Microsoft.Extensions.Logging;
using System.IO;
using Azure.Storage.Blobs;
using System.Data.SqlClient;


namespace INSS.EIIR.DailyExtract
{
    internal class ExtractSnapshot
    {
        private string _name = string.Empty;
        private Stream _blob = null;
        private ILogger _log = null;

        public ExtractSnapshot(string name, Stream blob, ILogger log) 
        {
            _name = name;
            _blob = blob;
            _log = log;
        }


        /// <summary>
        /// Processes the file information. Runs the script against SQL Server.
        /// </summary>
        public void Process()
        {
            // read content of blob
            using (var reader = new StreamReader(_blob))
            {
                string script = reader.ReadToEnd();

                // Update database with received script
                string SqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                using (SqlConnection conn = new (SqlConnectionString))
                {
                    conn.OpenAsync();

                    using (SqlCommand command = new (script,conn))
                    {
                        // Execute the supplied script against the database
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        /// <summary>
        /// Copies/Moves the file to an archive folder.
        /// </summary>
        public async void Archive()
        {
            // Source blob details
            BlobServiceClient sourceBlobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("SourceBlobConnectionString"));

            BlobContainerClient sourceContainerClient = sourceBlobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("SourceContainer"));

            BlobClient sourceClient = sourceContainerClient.GetBlobClient(_name);

            //Target blob details
            BlobServiceClient targetBlobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("TargetBlobConnectionString"));

            BlobContainerClient targetContainerClient = targetBlobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("TargetContainer"));

            BlobClient targetClient = targetContainerClient.GetBlobClient(_name);

            //  Copy source to target
            await targetClient.StartCopyFromUriAsync(sourceClient.Uri);

            // Delete source if 'Move'
            if (Environment.GetEnvironmentVariable("DeleteSourceAfterCopy") == "1")
            {
                await sourceClient.DeleteIfExistsAsync();
            }
        }
    }
}
