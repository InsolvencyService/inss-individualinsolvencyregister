using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Extract
    {
        private readonly ILogger<Extract> _logger;
        private readonly ISubscriberDataProvider _subscriberDataProvider;
        private readonly IExtractDataProvider _extractDataProvider;

        public Extract(            
            ILogger<Extract> log,
            ISubscriberDataProvider subscriberDataProvider,
            IExtractDataProvider extractDataProvider)
        {            
            _logger = log;
            _subscriberDataProvider = subscriberDataProvider;
            _extractDataProvider = extractDataProvider;
        }

        [FunctionName("extracts")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PagingParameters), Required = false, Description = "The Paging Model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ExtractWithPaging), Description = "A list of extracts with the paging model")]
        public async Task<IActionResult> ListExtracts(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "eiir/extracts")] HttpRequest req)
        {
            _logger.LogInformation("Extract function ListExtracts called, retrieving all extracts.");

            var pagingParameters = await GetPagingParameters(req);
            var extractFiles = await _extractDataProvider.ListExtractsAsync(pagingParameters);

            return new OkObjectResult(extractFiles);
        }

        [FunctionName("extract-download")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Extract" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiParameter(name: "subscriberId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The subscriber Id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/octet-stream", bodyType: typeof(string), Description = "The latest extract zip file")]
        public async Task<IActionResult> LatestExtract(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eiir/{subscriberId}/downloads/latest")] HttpRequest req, string subscriberId)
        {
            _logger.LogInformation("Extract function: Endpoint LatestExtract [ retrieving latest zip file.]");

            if (string.IsNullOrEmpty(subscriberId))
            {
                var error = "Extract function: Endpoint LatestExtract [ missing query parameter subscriber Id is required.]";
                _logger.LogError(error);
                return new BadRequestObjectResult(error);
            }

            var isSubscriberActive = await _subscriberDataProvider.IsSubscriberActiveAsync(subscriberId);
            if (isSubscriberActive == null)
            {
                var error = $"Extract function: Endpoint LatestExtract [ subscriber {subscriberId} not found.]";
                _logger.LogError(error);
                return new NotFoundObjectResult(error);
            }
            if (!isSubscriberActive.GetValueOrDefault(false))
            {
                var error = $"Extract function: Endpoint LatestExtract [ subscriber {subscriberId} does not have an active subscription.]";
                _logger.LogError(error);
                return new ObjectResult(error) { StatusCode = (int)HttpStatusCode.Forbidden };
            }

            var latestExtract = await _extractDataProvider.GetLatestExtractForDownload();
            if (latestExtract == null)
            {
                var error = $"Extract function: Endpoint LatestExtract [ Latest Subscriber file for download not found. ]";
                _logger.LogError(error);
                return new NotFoundObjectResult(error);
            }

            var latestFile = $"{latestExtract.ExtractFilename}.zip";

            var extractBytes = await _extractDataProvider.DownloadLatestExtractAsync(latestFile);
            
            var extractFileDownload = new FileContentResult(extractBytes, "application/octet-stream") { FileDownloadName = latestFile };
            var SubscriberDownloadModel = new SubscriberDownloadDetail(latestExtract.ExtractId, GetIPFromRequestHeaders(req), GetServerIP(), latestExtract.DownloadZiplink);

            await _subscriberDataProvider.CreateSubscriberDownload(subscriberId, SubscriberDownloadModel);

            _logger.LogInformation($"Extract function: Endpoint LatestExtract [ subscriber {subscriberId} has successfully downloaded zip file {latestFile}.]");

            return extractFileDownload;
        }

        private async Task<PagingParameters> GetPagingParameters(HttpRequest request)
        {
            PagingParameters pagingParameters = new();
            if (request?.Body.Length > 0)
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                pagingParameters = JsonConvert.DeserializeObject<PagingParameters>(content);
                var info = $"Subscriber trigger function: Paging model parameters {pagingParameters}.";
                _logger.LogInformation(info);
            }
            return pagingParameters;
        }

        private static string GetServerIP()
        {
            string host = Dns.GetHostName();

            IPHostEntry ip = Dns.GetHostEntry(host);
            return ip.AddressList[0].ToString();
        }

        private static string GetIPFromRequestHeaders(HttpRequest request)
        {
            return (request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "").Split(new char[] { ':' }).FirstOrDefault();
        }
    }
}

