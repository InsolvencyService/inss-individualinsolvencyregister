using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.NotificationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Notification
    {
        private readonly ILogger<Notification> _logger;
        private readonly INotificationService _notificationService;

        public Notification(ILogger
            <Notification> log,
            INotificationService notificationService)
        {
            _logger = log;
            _notificationService = notificationService;
        }

        [FunctionName("send-notification")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Notifications" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NotifcationDetail), Description = "The NotifcationDetail details", Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "notifications/send")] HttpRequest req)
        {
            _logger.LogInformation($"HTTP trigger function processed a request for {req.Body}.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<NotifcationDetail>(requestBody);

            await _notificationService.SendNotificationAsync(data);
            
            return new OkObjectResult("");
        }
    }
}

