using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class ExtractJobTrigger
    {
        private readonly ILogger<ExtractJobTrigger> _logger;
        private readonly IServiceBusMessageSender _serviceBusMessageSender;
        private readonly IExtractRepository _eiirRepository;

        public ExtractJobTrigger(
            ILogger<ExtractJobTrigger> log,
            IServiceBusMessageSender serviceBusMessageSender,
            IExtractRepository eiirRepository)
        {
            _logger = log;
            _serviceBusMessageSender = serviceBusMessageSender;
            _eiirRepository = eiirRepository;
        }

        [FunctionName("ExtractJobTrigger")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation($"ExtractJobTrigger triggered at: {DateTime.Now}");

            var today = DateOnly.FromDateTime(DateTime.Now);

            var extractJobQueue = Environment.GetEnvironmentVariable("servicebus:extractjobqueue");
            if (string.IsNullOrEmpty(extractJobQueue))
            {
                _logger.LogError("ExtractjobTrigger missing configuration servicebus:extractjobqueue");
                return new BadRequestObjectResult("ExtractjobTrigger missing configuration servicebus:extractjobqueue");
            }

            // 1. Query Extract jobs table and check if extract created for today
            var extractJob = _eiirRepository.GetExtractAvailability();
            if (extractJob == null)
            {
                _logger.LogError($"ExtractJob not found for today [{today}], snapshot has not run", today);
                return new BadRequestObjectResult($"ExtractJob not found for today [{today}], snapshot has not run");
            }

            if (extractJob.SnapshotCompleted?.ToLowerInvariant() == "n")
            {
                _logger.LogError($"Snapshot has not yet run today [{today}]", today);
                return new BadRequestObjectResult($"Snapshot has not yet run today [{today}]");
            }

            if (extractJob.ExtractCompleted?.ToLowerInvariant() == "y")
            {
                return new OkObjectResult($"Subscriber xml / zip file creation has already ran successfully on [{today}]");
            }

            // 2. Create service bus message and send
            var message = new ExtractJobMessage
            {
                ExtractId = extractJob.ExtractId,
                ExtractFilename = extractJob.ExtractFilename!,
            };

            await _serviceBusMessageSender.SendMessageAsync(message, extractJobQueue);

            string responseMessage = $"This eiir subscriber file creation has been triggered at: {DateTime.Now}";

            return new OkObjectResult(responseMessage);
        }
    }
}

