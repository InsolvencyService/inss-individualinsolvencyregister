using System;

using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Net.Http;
using System.IO;
using System.Net;
using Microsoft.Azure.Functions.Worker;

namespace INSS.EIIR.DailyExtract
{
    public class Snapshot
    {

        private readonly ILogger<Snapshot> _logger;

        public Snapshot(ILogger<Snapshot> logger)
        {
            _logger = logger;
        }

        [Function("Snapshot")]
        public void Run([TimerTrigger("%snapshotTimercron%")] TimerInfo myTimer)
        {
            try
            {
                _logger.LogInformation($"EIIR Daily Extract Snapshot function executed at: {DateTime.Now}");
                _logger.LogInformation($"Next EIIR Daily Extract Snapshot scheduled for: {myTimer.ScheduleStatus.Next}");

                runProceedure("createeiirSnapshotTABLE");
                _logger.LogInformation("createeiirSnapshotTABLE execution successfull");
                
                runProceedure("extr_avail_INS");
                _logger.LogInformation("extr_avail_INS execution sucessfull");

                //start orchestration
                _logger.LogInformation("Calling Start Orchestration");
                callPostHttpFunction("EiirOrchestrator_Start");

                //rebuild Indexes
               // _log.LogInformation("Calling Extract Job Trigger");
               // callGetHttpFunction("ExtractJobTrigger");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw(new Exception(ex.Message));
            }
        }

        /*
        
            Runs named proceedure against the database
        
        */
        private void runProceedure(string proceedureName)
        {
            // Update database with received script
            string SqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

            SqlConnection conn = new SqlConnection(SqlConnectionString);
            ServerConnection svrConnection = new ServerConnection(conn);
            Server server = new Server(svrConnection);

            server.ConnectionContext.ExecuteNonQuery("exec " + proceedureName);
            server.ConnectionContext.SqlConnectionObject.Close();
        }

        private async System.Threading.Tasks.Task callGetHttpFunction(string function)
        {
            string functionURL = Environment.GetEnvironmentVariable("functionURL");
            string apiKey = Environment.GetEnvironmentVariable("functionAPIKey");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", apiKey);

            string functionEndpoint = functionURL + function;
            var response = await client.GetAsync(functionEndpoint);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{functionEndpoint} called.");
            }
            else
            {
                _logger.LogInformation($"{functionEndpoint} failed with. {response.StatusCode}");

            }
        }

        private async System.Threading.Tasks.Task callPostHttpFunction(string function)
        {
            string functionURL = Environment.GetEnvironmentVariable("functionURL");
            string apiKey = Environment.GetEnvironmentVariable("functionAPIKey");
            string functionEndpoint = functionURL + function;

            using (var client = new HttpClient())
            {
                // Setting the function key header
                client.DefaultRequestHeaders.Add("x-functions-key", apiKey);

                // Making the POST request
                var response = await client.PostAsync(functionEndpoint, new StringContent("", System.Text.Encoding.UTF8, "application/json"));

                // Writing the responsefunctionEndpoint
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"{functionEndpoint} called.");
                }
                else
                {
                    _logger.LogInformation($"{functionEndpoint} failed with. {response.StatusCode}");
                }
            }

        }
    }
}
