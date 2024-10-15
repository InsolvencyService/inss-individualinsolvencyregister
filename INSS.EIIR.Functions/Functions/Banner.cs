using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData;
using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Model;
using INSS.EIIR.DataSync.Application;
using INSS.EIIR.Models.BannerModels;
using INSS.EIIR.Models.FeedbackModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.DurableTask.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Banner
    {
        private readonly ILogger<Banner> _logger;
        private readonly IResponseUseCase<GetBusinessDataResponse> _getBDService;
        private readonly IRequestResponseUseCase<SetBusinessDataRequest, SetBusinessDataResponse> _setBDService;

        public Banner(ILogger<Banner> logger, 
                        IResponseUseCase<GetBusinessDataResponse> getBDService,
                        IRequestResponseUseCase<SetBusinessDataRequest, SetBusinessDataResponse> setBDService)
        {
            _logger = logger;
            _getBDService = getBDService;
            _setBDService = setBDService;
        }

        [Function("Banner")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Banner" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Models.BannerModels.Banner), Required = false, Description = "The body of the request")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Models.BannerModels.Banner), Description = "The banner message to be displayed on Home Page")]

        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "eiir/banner")] HttpRequestData req)
        {
            var resp = await _getBDService.Handle();

            return new OkObjectResult(new Models.BannerModels.Banner() { Text = resp.Data.BannerText});
        }

        [Function("UpdateBanner")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Banner" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Models.BannerModels.Banner), Required = false, Description = "The body of the request")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Models.BannerModels.Banner), Description = "The banner message to be displayed on Home Page")]
        public async Task<IActionResult> RunPost([HttpTrigger(AuthorizationLevel.Function, "post", Route = "eiir/banner")] HttpRequestData req)
        {

            string json = await req.ReadAsStringAsync();
            var banner = JsonConvert.DeserializeObject<Models.BannerModels.Banner>(json);

            //Get existing business data so we can update existing banner value
            var dataResp = await _getBDService.Handle();
            var updateBusinessData = dataResp.Data;
            updateBusinessData.BannerText = banner.Text;

            var resp = await _setBDService.Handle(new SetBusinessDataRequest() { Data = updateBusinessData });

            //Return updated value - RESTful type operation
            return new OkObjectResult(new Models.BannerModels.Banner() { Text = resp.Data.BannerText });
        }
    }
}
