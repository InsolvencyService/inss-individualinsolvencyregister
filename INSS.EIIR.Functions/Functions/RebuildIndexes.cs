using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.AzureSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace INSS.EIIR.Functions.Functions;

public class RebuildIndexes
{
    private readonly IEnumerable<IIndexService> _indexServices;
    private readonly ILogger<RebuildIndexes> _logger;

    public RebuildIndexes(
        IEnumerable<IIndexService> indexServices,
        ILogger<RebuildIndexes> logger)
    {
        _indexServices = indexServices;
        _logger = logger;
    }

    [FunctionName("RebuildIndexes")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        var message = $"Eiir RebuildIndexes has been triggered at: {DateTime.Now}";
        _logger.LogInformation(message);

        foreach (var indexService in _indexServices)
        {
            await indexService.DeleteIndexAsync(_logger);
            await indexService.CreateIndexAsync(_logger);
            await indexService.PopulateIndexAsync(_logger);
        }

        return new OkObjectResult(message);
    }
}