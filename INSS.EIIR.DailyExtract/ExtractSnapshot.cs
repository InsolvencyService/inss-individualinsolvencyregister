using System;
using Microsoft.Extensions.Logging;
using System.IO;
using Azure.Storage.Blobs;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using AutoMapper.Configuration.Annotations;
using System.Reflection.Metadata;



namespace INSS.EIIR.DailyExtract
{
    internal class ExtractSnapshot
    {
        private string _name = string.Empty;
        private Stream _blob = null;
        private ILogger<EiiRDailyExtract> _log = null;

        private const long defaultMaxFileSize = 50; 


        public ExtractSnapshot(string name, Stream blob, ILogger<EiiRDailyExtract> log) 
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

                SqlConnection conn = new SqlConnection(SqlConnectionString);
                ServerConnection svrConnection = new ServerConnection(conn);
                Server server = new Server(svrConnection);
                try
                {
                    _log.LogInformation("Executing script");

                    server.ConnectionContext.ExecuteNonQuery(script, ExecutionTypes.ContinueOnError);
                    _log.LogInformation("Executed Script");

                }
                catch (Exception ex)
                {
                    _log.LogError("Execution of script errored with message : " + ex.Message);
                    //throw ex;
                }

                server.ConnectionContext.SqlConnectionObject.Close();
            }

        }

        /// <summary>
        /// Copies/Moves the file to an archive folder.
        /// </summary>
        public async System.Threading.Tasks.Task Archive()
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

        public bool WithinFileSizeLimits 
        { 
            get { 

                var result = false;

                BlobServiceClient sourceBlobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("SourceBlobConnectionString"));

                BlobContainerClient sourceContainerClient = sourceBlobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("SourceContainer"));

                string maxFileSizeTxt = Environment.GetEnvironmentVariable("MaxEiirDailyExtractFileSize").IsNullOrWhiteSpace() ? $"{defaultMaxFileSize}" : Environment.GetEnvironmentVariable("MaxEiirDailyExtractFileSize");
                long maxFilesSize;

                if (!long.TryParse(maxFileSizeTxt, out maxFilesSize))
                {
                    _log.LogWarning($"Unable to parse MaxEiirDailyExtractFileSize default of {defaultMaxFileSize} MB set.");
                    maxFilesSize = defaultMaxFileSize;  
                } 

                BlobClient blob = sourceContainerClient.GetBlobClient(_name);
                if (blob != null)
                {
                    var properties = (blob.GetProperties()).Value;

                    return properties.ContentLength < maxFilesSize * 1000000;
                }

                return result;

            }
        }
    }
}
