using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            // Replace name and input with values relevant for your Durable Functions Activity
            await context.CallActivityAsync(nameof(SyncData));

        }

        //[Function(nameof(SayHello))]
        //public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        //{
        //    ILogger logger = executionContext.GetLogger("SayHello");
        //    logger.LogInformation("Saying hello to {name}.", name);
        //    return $"Hello {name}!";
        //}

        //[Function("SyncDataOrchestrator_HttpStart")]
        //public static async Task<HttpResponseData> HttpStart(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        //    [DurableClient] DurableTaskClient client,
        //    FunctionContext executionContext)
        //{
        //    ILogger logger = executionContext.GetLogger("SyncDataOrchestrator_HttpStart");

        //    // Function input comes from the request content.
        //    string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
        //        nameof(SyncDataOrchestrator));

        //    logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        //    // Returns an HTTP 202 response with an instance management payload.
        //    // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        //    return await client.CreateCheckStatusResponseAsync(req, instanceId);
        //}
    }
}
