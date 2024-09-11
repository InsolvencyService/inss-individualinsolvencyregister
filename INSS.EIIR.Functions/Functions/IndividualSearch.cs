using System.IO;
using System.Net;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;



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

        [Function("IndividualSearch")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Search" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(IndividualSearchModel), Description = "The search model", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SearchResult), Description = "The OK response")]
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