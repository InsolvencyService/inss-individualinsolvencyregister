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
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using INSS.EIIR.Models.SyncData;


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
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SyncDataRequest),
                Description = "The Modes and Datasources to be applied by SyncData. Both are bitwise values each covering multiple options, seek guidance for usage.", Required = false)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            string json = await req.ReadAsStringAsync();
            SyncDataRequest syncDataRequest;

            if (json.IsNullOrEmpty())
            {
                syncDataRequest = new SyncDataRequest();
                _logger.LogWarning("SyncData settings unable to be determined from request body, defaults");
            } else
            {
                syncDataRequest = JsonConvert.DeserializeObject<SyncDataRequest>(json);
            }

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(SyncDataOrchestrator), syncDataRequest);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
