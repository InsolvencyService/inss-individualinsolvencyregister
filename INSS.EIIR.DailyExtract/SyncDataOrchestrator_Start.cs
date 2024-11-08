using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;

//These don't make sense as other .WebJobs stuff removed moving from 
// in-process -> isolated worker but all examples online point in this direction
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using Microsoft.DurableTask.Client;

namespace INSS.EIIR.DailyExtract
{
    public class SyncDataOrchestrator_Start
    {
        private readonly ILogger<SyncDataOrchestrator_Start> _logger;

        public SyncDataOrchestrator_Start(ILogger<SyncDataOrchestrator_Start> logger)
        {
            _logger = logger;
        }

        [Function("SyncDataOrchestrator_Start")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "EIIR" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(SyncDataOrchestrator));

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
