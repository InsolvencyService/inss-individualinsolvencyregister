
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;


//These don't make sense as other .WebJobs stuff removed moving from 
// in-process -> isolated worker but all examples online point in this direction
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
    public class EiirOrchestrator
    {
        private readonly ILogger<EiirOrchestrator> _logger;

        public EiirOrchestrator(ILogger<EiirOrchestrator> logger) 
        { 
            _logger = logger;
        }

        [Function("EiirOrchestrator_Start")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "EIIR" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]

        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient starter
            )
        {     
            string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync("EiirOrchestrator", null);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return await starter.CreateCheckStatusResponseAsync(req, instanceId);
        }

        [Function("EiirOrchestrator")]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var outputs = new List<string>
            {
                await context.CallActivityAsync<string>(nameof(RebuildIndexes), "RebuildIndexes"),
                
                // The extract job trigger currently doesn't use the index and is trigger by a separate 
                // powershell script. Once the extract job is changed to use the index we will need 
                // to orchestrate the extract job here so it runs after the index has been built.
                //await context.CallActivityAsync<string>(nameof(ExtractJobTrigger), "ExtractJobTrigger")
            };

            return outputs;
        }

    }
}