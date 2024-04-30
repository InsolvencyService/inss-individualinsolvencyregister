using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
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

namespace INSS.EIIR.Functions.Functions;

public class ExtractJobServiceTrigger
{
    private readonly ILogger<ExtractJobServiceTrigger> _logger;
    private readonly IExtractRepository _eiirRepository;
    private readonly IExtractDataProvider _extractService;

    public ExtractJobServiceTrigger(
        ILogger<ExtractJobServiceTrigger> log,
        IExtractRepository eiirRepository,
        IExtractDataProvider extractService)
    {
        _logger = log;
        _eiirRepository = eiirRepository;
        _extractService = extractService;
    }

    [FunctionName("ExtractJobServiceTrigger")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
    {
        private_run(new ExtractJobMessage() { ExtractFilename = "Glen_Test_OrderBy_IPAddress", ExtractId = 123 });
        string responseMessage = $"This eiir subscriber file creation has been triggered at: {DateTime.Now}";
        return new OkObjectResult(responseMessage);
    }

    public async Task Runs([ServiceBusTrigger("%servicebusextractjobqueue%", Connection = "servicebussubscriberconnectionstring")] ExtractJobMessage message)
    {
        private_run(message);
    }

    private async Task private_run(ExtractJobMessage message)
    {
        var now = DateTime.Now;

        _logger.LogInformation($"ExtractJobServiceTrigger received message: {message} on {now}");

        try
        {
            _extractService.GenerateSubscriberFile(message.ExtractFilename);

            _eiirRepository.UpdateExtractAvailable();

            _logger.LogInformation($"ExtractJobServiceTrigger ran succssfully on: {now} xml/zip file created with name: {message.ExtractFilename}");
        }
        catch (Exception ex)
        {
            var error = $"ExtractJobServiceTrigger failed on: {now} with : {ex}";
            _logger.LogError(error);
            throw new Exception(error);
        }
    }


    //[FunctionName("ExtractJobServiceTrigger")]
    //public async Task Run([ServiceBusTrigger("%servicebusextractjobqueue%", Connection = "servicebussubscriberconnectionstring")] ExtractJobMessage message)
    //{
    //    var now = DateTime.Now;

    //    _logger.LogInformation($"ExtractJobServiceTrigger received message: {message} on {now}");

    //    try
    //    {
    //        await _extractService.GenerateSubscriberFile(message.ExtractFilename);

    //        _eiirRepository.UpdateExtractAvailable();
            
    //        _logger.LogInformation($"ExtractJobServiceTrigger ran succssfully on: {now} xml/zip file created with name: {message.ExtractFilename}");
    //    }
    //    catch (Exception ex)
    //    {
    //        var error = $"ExtractJobServiceTrigger failed on: {now} with : {ex}";
    //        _logger.LogError(error);
    //        throw new Exception(error);
    //    }
    //}
}
