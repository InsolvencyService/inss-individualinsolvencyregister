using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace INSS.EIIR.DailyExtract
{
    public class Snapshot
    {
        [FunctionName("Snapshot")]
        public void Run([TimerTrigger("%snapshotTimercron%")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"EIIR Daily Extract Snapshot function executed at: {DateTime.Now}");
                log.LogInformation($"Next EIIR Daily Extract Snapshot scheduled for: {myTimer.ScheduleStatus.Next}");

                runProceedure("createeiirSnapshotTABLE");
                log.LogInformation("createeiirSnapshotTABLE execution successfull");
                
                runProceedure("extr_avail_INS");
                log.LogInformation("extr_avail_INS execution sucessfull");

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
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
    }
}
