using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public static class EiirOrchestrator
    {
        [FunctionName("EiirOrchestrator_Start")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "EIIR" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]

        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("EiirOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("EiirOrchestrator")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>
            {
                await context.CallActivityAsync<string>(nameof(RebuildIndexes), "RebuildIndexes"),
                
                // The extract job trigger currently doesn't use the index and is trigger by a separate 
                // powershell script. Once the extract job is changed to use the index we will need 
                // to orchestrate the extract job here so it runs after the index has been built.
                // await context.CallActivityAsync<string>(nameof(ExtractJobTrigger), "ExtractJobTrigger")
            };

            return outputs;
        }

    }
}