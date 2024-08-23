using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.FeedbackModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;


using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class Feedback
    {
        private readonly ILogger<Feedback> _logger;
        private readonly IFeedbackDataProvider _feedbackDataProvider;

        public Feedback(
            ILogger<Feedback> log,
            IFeedbackDataProvider feedbackDataProvider)
        {
            _logger = log;
            _feedbackDataProvider = feedbackDataProvider;
        }

        [Function("feedback")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Feedback" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FeedbackBody), Required = false, Description = "The body of the request")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(FeedbackWithPaging), Description = "A list of feedback with the paging model")]
        public async Task<IActionResult> GetFeedback(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "eiir/feedback")] HttpRequestData req)
        {
            _logger.LogInformation("Feedback function retrieving all feedback.");

            var bodyParameters = await GetBodyParameters(req);
            var subscribers = await _feedbackDataProvider.GetFeedbackAsync(bodyParameters);

            return new OkObjectResult(subscribers);
        }

        [Function("feedback-create")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Feedback" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateCaseFeedback), Description = "The feedback details to create", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "Feedback details for new record added")]
        public async Task<IActionResult> CreateFeedback(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "eiir/feedback/create")] HttpRequestData req)
        {
            string json = await req.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(json))
            {
                var feedbackDetails = JsonConvert.DeserializeObject<CreateCaseFeedback>(json);
                _logger.LogInformation($"Feedback trigger function Adding Feedback details {json}");

                _feedbackDataProvider.CreateFeedback(feedbackDetails);

                return new OkObjectResult($"New feedback added for feedback details {json}");
            }
            var error = "Create Feedback function missing feedback details.";
            _logger.LogError(error);
            return new BadRequestObjectResult(error);
        }

        [Function("feedback-update-status")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Feedback" })]
        [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
        [OpenApiParameter(name: "feedbackId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The feedback Id")]
        [OpenApiParameter(name: "status", In = ParameterLocation.Path, Required = true, Type = typeof(bool), Description = "The viewed status")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "feedback status updated")]
        public async Task<IActionResult> UpdateFeedbackStatus(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "eiir/feedback/{feedbackId}/viewed/{status}")] HttpRequestData req, int feedbackId, bool status)
        {
            if (feedbackId == 0)
            {
                var feedbackIdError = "Feedback trigger function: missing parameter feedbackId is required.";
                _logger.LogError(feedbackIdError);
                return new BadRequestObjectResult(feedbackIdError);
            }

            _logger.LogInformation($"Update Feedback status trigger function for FeedbackId {feedbackId}");

            var updatedOk = _feedbackDataProvider.UpdateFeedbackStatus(feedbackId, status);
            if (updatedOk)
            {
                return new OkObjectResult($"Feedback status updated for FeedbackId {feedbackId} with viewed status {status}");
            }
            
            var error = $"Feedback for feedback Id {feedbackId} not found.";
            _logger.LogError(error);
            return new NotFoundObjectResult(error);
        }

        private async Task<FeedbackBody> GetBodyParameters(HttpRequestData request)
        {
            FeedbackBody feedbackBody = new();
            if (request?.Body.Length > 0)
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                feedbackBody = JsonConvert.DeserializeObject<FeedbackBody>(content);
                var info = $"Feedback function: Body parameters {feedbackBody}.";
                _logger.LogInformation(info);
            }
            return feedbackBody;
        }
    }
}

