using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData;
using INSS.EIIR.Models.FeedbackModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Banner
    {
        private readonly ILogger<Banner> _logger;

        public Banner(ILogger<Banner> logger)
        {
            _logger = logger;
        }

        [Function("Banner")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Banner" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Banner), Required = false, Description = "The body of the request")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Banner), Description = "The banner message to be displayed on Home Page")]

        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "eiir/banner")] HttpRequest req)
        {
            var resp = await new GetBusinessData().Handle();

            return new OkObjectResult(new Models.BannerModels.Banner() { Text = resp.Data.BannerText});
        }
    }
}
