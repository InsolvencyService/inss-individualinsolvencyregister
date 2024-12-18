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


            if (snapshot.WithinFileSizeLimits)
            {
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
            else 
            {
                _logger.LogInformation($"The file {name} is outside the Maximum Daily Extract File Size limit. " 
                     + $"The file should be applied manually to the EIIR database using a suitable SQL client such as SQLCMD or powershell Invoke-Sqlcmd. " 
                     + $"The file may be too large to be processed with SSMS.  See APP-5666 for further detail.");

                //Throw custom exception so it really stands out in Application Insights
                throw new MaximumFileSizeExceededException($"The file {name} exceeds the maximum file size to be processed automatically by EiirDailyExtract.  See Trace Messages for furher detail.", null);
            
            }

        }
    }


    public class MaximumFileSizeExceededException : Exception
    {
        public MaximumFileSizeExceededException(string message, Exception innerException) : base(message, innerException)
        { }
    }

}
