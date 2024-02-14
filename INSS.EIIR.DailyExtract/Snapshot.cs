using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Net.Http;
using System.IO;

namespace INSS.EIIR.DailyExtract
{
    public class Snapshot
    {

        private ILogger _log;

        [FunctionName("Snapshot")]
        public void Run([TimerTrigger("%snapshotTimercron%")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                _log = log;
                _log.LogInformation($"EIIR Daily Extract Snapshot function executed at: {DateTime.Now}");
                _log.LogInformation($"Next EIIR Daily Extract Snapshot scheduled for: {myTimer.ScheduleStatus.Next}");

                runProceedure("createeiirSnapshotTABLE");
                _log.LogInformation("createeiirSnapshotTABLE execution successfull");
                
                runProceedure("extr_avail_INS");
                _log.LogInformation("extr_avail_INS execution sucessfull");

                //start orchestration
                _log.LogInformation("Calling Start Orchestration");
                callPostHttpFunction("EiirOrchestrator_Start");

                //rebuild Indexes
                _log.LogInformation("Calling Extract Job Trigger");
                callGetHttpFunction("ExtractJobTrigger");

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
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

        private async void callGetHttpFunction(string function)
        {
            string functionURL = Environment.GetEnvironmentVariable("functionURL");
            string apiKey = Environment.GetEnvironmentVariable("functionAPIKey");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", apiKey);

            string functionEndpoint = functionURL + function;
            var response = await client.GetAsync(functionEndpoint);

            if (response.IsSuccessStatusCode)
            {
                _log.LogInformation($"{functionEndpoint} called.");
            }
            else
            {
                _log.LogInformation($"{functionEndpoint} failed with. {response.StatusCode}");

            }
        }

        private async void callPostHttpFunction(string function)
        {
            string functionURL = Environment.GetEnvironmentVariable("functionURL");
            string apiKey = Environment.GetEnvironmentVariable("functionAPIKey");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-functions-key", apiKey);

            string functionEndpoint = functionURL + function;
            var response = await client.GetAsync(functionEndpoint);

            if (response.IsSuccessStatusCode)
            {
                _log.LogInformation($"{functionEndpoint} called.");
            }
            else
            {
                _log.LogInformation($"{functionEndpoint} failed with. {response.StatusCode}");

            }
        }
    }
}
