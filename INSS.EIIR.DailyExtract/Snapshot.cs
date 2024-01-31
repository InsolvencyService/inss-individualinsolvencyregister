using System;
using System.Data.SqlTypes;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

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


// SQL queries to produces snapsot table

                //  $query1 = "exec createeiirSnapshotTABLE"

                //  Invoke - Sqlcmd - ServerInstance $AzureSQLServerName - Username $username - Password $password - Database $AzureSQLDatabaseName - Query $query1 - Verbose - QueryTimeout 300
                //
                // $query2 = "EXEC extr_avail_INS"

                //  Invoke - Sqlcmd - ServerInstance $AzureSQLServerName - Username $username - Password $password - Database $AzureSQLDatabaseName - Query $query2 - Verbose - QueryTimeout 300
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}
