using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.DailyExtract
{
    public class EiiRDailyExtract
    {
        [FunctionName("EiiRDailyExtract")]
        public void Run([BlobTrigger("%BlobContainer%", Connection = "SourceBlobConnectionString")] Stream myBlob, string name, ILogger log)
        {

            log.LogInformation($"EiirDailyExtract - Received Blob Notification");

            /*
                Create snapshot object
            */
            ExtractSnapshot snapshot = new ExtractSnapshot(name, myBlob, log);

            /*
                Process the received snapshot file
                */
            snapshot.Process();
            log.LogInformation($"    - Snapshot processed succesfully");

            /*
                Archive the received snapshot file
                */
            snapshot.Archive();
            log.LogInformation($"    - Snapshot archived.");
            log.LogInformation($"Update of Snapshot {name} successfull");

        }
    }
}
