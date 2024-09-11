using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.DailyExtract
{
    public class EiiRDailyExtract
    {

        private readonly ILogger<EiiRDailyExtract> _logger;

        public EiiRDailyExtract(ILogger<EiiRDailyExtract> logger)
        {
            _logger = logger;
        }


        [Function("EiiRDailyExtract")]
        public void Run([BlobTrigger("%BlobContainer%", Connection = "SourceBlobConnectionString")] Stream myBlob, string name)
        {

            _logger.LogInformation($"EiirDailyExtract - Received Blob Notification");

            /*
                Create snapshot object
            */
            ExtractSnapshot snapshot = new ExtractSnapshot(name, myBlob, _logger);

            /*
                Process the received snapshot file
                */
            snapshot.Process();
            _logger.LogInformation($"    - Snapshot processed succesfully");

            /*
                Archive the received snapshot file
                */
            snapshot.Archive();
            _logger.LogInformation($"    - Snapshot archived.");
            _logger.LogInformation($"Update of Snapshot {name} successfull");

        }
    }
}
