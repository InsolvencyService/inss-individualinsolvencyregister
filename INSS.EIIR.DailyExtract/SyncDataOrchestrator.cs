using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DailyExtract
{
    public class SyncDataOrchestrator
    {
        [Function(nameof(SyncDataOrchestrator))]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(SyncDataOrchestrator));

            logger.LogInformation("Calling SyncData Activity.");

            var inputParams = context.GetInput<SyncDataRequest>();

            // Replace name and input with values relevant for your Durable Functions Activity
            await context.CallActivityAsync(nameof(SyncData), inputParams);

        }

    }
}
