using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
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
    public class CaseDetails
    {
        private readonly ILogger<CaseDetails> _logger;
        private readonly ICaseDataProvider _caseDataProvider;

        public CaseDetails(ILogger<CaseDetails> log, 
            ICaseDataProvider caseDataProvider)
        {
            _logger = log;
            _caseDataProvider = caseDataProvider;
        }

        [FunctionName("CaseDetails")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Case" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "CaseRequest", In = ParameterLocation.Header, Required = true, Type = typeof(CaseRequest), Description = "The CaseRequest parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetCaseDetails([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
    
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var caseRequest = JsonConvert.DeserializeObject<CaseRequest>(requestBody);

            var result = await _caseDataProvider.GetCaseByCaseNoIndivNoAsync(caseRequest);

            return new OkObjectResult(result);

        }
    }
}
