using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Functions.Worker;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions;

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

    [Function("ExtractJobTrigger")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
    {
        _logger.LogInformation($"ExtractJobTrigger triggered at: {DateTime.Now}");

        var today = DateOnly.FromDateTime(DateTime.Now);

        var extractjobqueueError = "ExtractjobTrigger missing configuration servicebus__extractjobqueue";
        var extractjobError = $"ExtractJob not found for today [{today}], snapshot has not run";
        var snapshotError = $"Snapshot has not yet run today [{today}]";

        var extractJobQueue = Environment.GetEnvironmentVariable("servicebusextractjobqueue");
        if (string.IsNullOrEmpty(extractJobQueue))
        {
            _logger.LogError(extractjobqueueError);
            return new BadRequestObjectResult(extractjobqueueError);
        }

        var extractJob = _eiirRepository.GetExtractAvailable();
        if (extractJob == null)
        {
            _logger.LogError(extractjobError);
            return new BadRequestObjectResult(extractjobError);
        }

        if (extractJob.SnapshotCompleted?.ToLowerInvariant() == "n")
        {
            _logger.LogError(snapshotError);
            return new BadRequestObjectResult(snapshotError);
        }

        if (extractJob.ExtractCompleted?.ToLowerInvariant() == "y")
        {
            return new OkObjectResult($"Subscriber xml / zip file creation has already ran successfully on [{today}]");
        }

        var message = new ExtractJobMessage
        {
            ExtractId = extractJob.ExtractId,
            ExtractFilename = extractJob.ExtractFilename!,
        };

        await _serviceBusMessageSender.SendExtractJobMessageAsync(message, extractJobQueue);

        string responseMessage = $"This eiir subscriber file creation has been triggered at: {DateTime.Now}";

        return new OkObjectResult(responseMessage);
    }
}

