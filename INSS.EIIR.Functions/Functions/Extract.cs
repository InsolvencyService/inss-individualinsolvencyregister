using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Extract
    {
        private readonly ILogger<Extract> _logger;
        private readonly IExtractDataProvider _extractDataProvider;

        public Extract(
            ILogger<Extract> log,
            IExtractDataProvider extractDataProvider)
        {
            _logger = log;
            _extractDataProvider = extractDataProvider; 
        }

        [FunctionName("extracts")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "PagingModel", In = ParameterLocation.Query, Required = false, Type = typeof(PagingParameters), Description = "The Paging Model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ExtractWithPaging), Description = "A list of extracts with the paging model")]
        public async Task<IActionResult> ListExtracts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eiir/extracts")] HttpRequest req)
        {
            _logger.LogInformation("Extract function ListExtracts called, retrieving all extracts.");

            var pagingParameters = GetPagingParameters(req);
            var extractFiles = await _extractDataProvider.ListExtractsAsync(pagingParameters);

            return new OkObjectResult(extractFiles);
        }

        private PagingParameters GetPagingParameters(HttpRequest request)
        {
            PagingParameters pagingParameters = new PagingParameters();
            if (!string.IsNullOrEmpty(request?.Query?["PagingModel"]))
            {
                pagingParameters = JsonConvert.DeserializeObject<PagingParameters>(request?.Query["PagingModel"]);
                var info = $"Subscriber trigger function: Paging model parameters {pagingParameters}.";
                _logger.LogInformation(info);
            }
            return pagingParameters;
        }
    }
}

