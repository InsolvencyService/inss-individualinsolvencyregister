using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.SearchModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace INSS.EIIR.Functions.Functions
{
    public class IndividualSearch
    {
        private readonly ILogger<IndividualSearch> _logger;
        private readonly IIndividualQueryService _queryService;

        public IndividualSearch(
            ILogger<IndividualSearch> log,
            IIndividualQueryService queryService)
        {
            _logger = log;
            _queryService = queryService;
        }

        [FunctionName("IndividualSearch")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Search" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "SearchModel", In = ParameterLocation.Header, Required = true, Type = typeof(IndividualSearchModel), Description = "The SearchModel parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var searchModel = JsonConvert.DeserializeObject<IndividualSearchModel>(requestBody);

            var results = await _queryService.SearchIndexAsync(searchModel);

            return new OkObjectResult(results);
        }
    }
}