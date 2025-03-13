using System;

using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using INSS.EIIR.Models.SyncData;
using Newtonsoft.Json;
using System.Linq;


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

                SyncDataRequest syncDataRequestSettings = null;
                string functionName = "";

                getSettings(out functionName, out syncDataRequestSettings);

                callPostHttpFunction(functionName, syncDataRequestSettings);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw(new Exception(ex.Message));
            }
        }

        /// <summary>
        /// Gets the function name to be called
        /// and should it be SyncDataOrchestrator_Start the SyncDataRequest to be used based on current environment variables.
        /// </summary>
        /// <param name="settings">A SyncDataRequest object containing request body to be used in request</param>
        /// <param name="functionName">The function to be called</param>
        private void getSettings(out string functionName, out SyncDataRequest settings)
        {

            Boolean useFakeData = false;
            Boolean.TryParse(Environment.GetEnvironmentVariable("UseFakedDataSources"), out useFakeData);

            Boolean useINSSightData = false;
            Boolean.TryParse(Environment.GetEnvironmentVariable("INSSightDataFeedEnabled"), out useINSSightData);

            SyncDataEnums.Mode syncDataMode = SyncDataEnums.Mode.Default;
            GetSyncDataMode(Environment.GetEnvironmentVariable("SyncDataMode"), ref syncDataMode);


            functionName = "SyncDataOrchestrator_Start";

            if (useFakeData)
            {
                settings = new SyncDataRequest()
                {
                    Modes = syncDataMode,
                    DataSources = SyncDataEnums.Datasource.FakeBKTandIVA | SyncDataEnums.Datasource.FakeDRO
                };
                _logger.LogInformation("Calling SyncData - Using Faked data from searchdata.json for BKTs & IVAs, ISCIS for DROs");
            }
            else if (useINSSightData)
            {
                settings = new SyncDataRequest()
                {
                    Modes = syncDataMode,
                    DataSources = SyncDataEnums.Datasource.INSSightBKTandIVA | SyncDataEnums.Datasource.IscisDRO
                };
                _logger.LogInformation("Calling SyncData - Using INSSight data feeds for BKTs & IVAs, ISCIS for DROs");
            }
            else
            {
                settings = new SyncDataRequest()
                {
                    Modes = syncDataMode,
                    DataSources = SyncDataEnums.Datasource.IscisBKTandIVA | SyncDataEnums.Datasource.IscisDRO
                };
                _logger.LogInformation("Calling SyncData - Using ISCIS data feeds for BKTs, IVAs & DROs");
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

        private async System.Threading.Tasks.Task callPostHttpFunction(string function, object requestBody = null)
        {
            string functionURL = Environment.GetEnvironmentVariable("functionURL");
            string apiKey = Environment.GetEnvironmentVariable("functionAPIKey");
            string functionEndpoint = functionURL + function;

            using (var client = new HttpClient())
            {
                // Setting the function key header
                client.DefaultRequestHeaders.Add("x-functions-key", apiKey);

                var body = requestBody != null ? JsonConvert.SerializeObject(requestBody): "";

                // Making the POST request
                var response = await client.PostAsync(functionEndpoint, new StringContent(body, System.Text.Encoding.UTF8, "application/json"));

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

        /// <summary>
        /// Converts a delimited ('|') string (also works just as well for an int)
        /// trys parsing each element as SyncDataEnums.Mode, failures => 0
        /// Then applys 'bitwise or' to them all using Linq Aggregator returning a single bitwise value
        /// Parses the likes of "DisableXMLExtract|EnableValidations" => 4 + 16 => 20
        /// </summary>
        /// <param name="config"></param>
        /// <returns>SyncDataEnums.Datasource</returns>
        private static void GetSyncDataMode(string setting, ref SyncDataEnums.Mode value)
        {
            if (setting != null)
            { 
                if (!Enum.TryParse(setting, true, out value))
                {
                    value = setting.Split('|').ToList().Select(e =>
                    {
                        SyncDataEnums.Mode aMode;
                        Enum.TryParse(e.Trim(), true, out aMode);
                        return aMode;
                    }).Aggregate((accumulatedValue, nextValue) => accumulatedValue = accumulatedValue | nextValue);
                }
            }
        }


    }
}
