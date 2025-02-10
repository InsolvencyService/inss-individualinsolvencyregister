using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;
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

namespace INSS.EIIR.Functions.Functions;

public class Subscriber
{
    private readonly ILogger<Subscriber> _logger;
    private readonly ISubscriberDataProvider _subscriberDataProvider;

    public Subscriber(
        ILogger<Subscriber> log,
        ISubscriberDataProvider subscriberDataProvider)
    {
        _logger = log;
        _subscriberDataProvider = subscriberDataProvider;   
    }

    [Function("Subscriber")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PagingParameters), Required = false, Description = "The Paging Model")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SubscriberWithPaging), Description = "A list of subscribers with the paging model")]
    public async Task<IActionResult> GetSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "subscribers")] HttpRequestData req)
    {
        _logger.LogInformation("Subscriber trigger function retrieving all subscribers.");

        var pagingParameters = await GetPagingParameters(req);
        var subscribers = await _subscriberDataProvider.GetSubscribersAsync(pagingParameters);

        return new OkObjectResult(subscribers);
    }

    [Function("SubscriberById")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiParameter(name: "subscriberId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The subscriber Id")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Models.SubscriberModels.Subscriber), Description = "Subscriber details for the Id specified")]
    public async Task<IActionResult> GetSubscriberById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscribers/{subscriberId}")] HttpRequestData req, string subscriberId)
    {
        if (string.IsNullOrEmpty(subscriberId))
        {
            var error = "Subscriber trigger function: missing query parameter subscriber Id is required.";
            _logger.LogError(error);
            return new BadRequestObjectResult(error);
        }

        _logger.LogInformation($"Subsciber trigger function retrieving subscriber details for subscriber Id {subscriberId}.");
        
        var subscriber = await _subscriberDataProvider.GetSubscriberByIdAsync(subscriberId);
        if (subscriber == null)
        {
            var error = $"Subscriber function: Endpoint GetSubscriberById [ subscriber {subscriberId} not found.]";
            _logger.LogError(error);
            return new NotFoundObjectResult(error);
        }

        return new OkObjectResult(subscriber);
    }

    [Function("active-subscribers")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PagingParameters), Required = false, Description = "The Paging Model")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SubscriberWithPaging), Description = "A list of active subscribers with the paging model")]
    public async Task<IActionResult> GetActiveSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "subscribers/active")] HttpRequestData req)
    {
        _logger.LogInformation("Subscriber trigger function retrieving active subscribers.");

        var pagingParameters = await GetPagingParameters(req);
        var subscribers = await _subscriberDataProvider.GetActiveSubscribersAsync(pagingParameters);

        return new OkObjectResult(subscribers);
    }

    [Function("inactive-subscribers")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PagingParameters), Required = false, Description = "The Paging Model")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SubscriberWithPaging), Description = "A list of inactive subscribers with the paging model")]
    public async Task<IActionResult> GetInactiveSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "subscribers/inactive")] HttpRequestData req)
    {
        _logger.LogInformation("Subscriber trigger function retrieving inactive subscribers.");

        var pagingParameters = await GetPagingParameters(req);
        var subscribers = await _subscriberDataProvider.GetInActiveSubscribersAsync(pagingParameters);

        return new OkObjectResult(subscribers);
    }

    [Function("subscriber-create")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateUpdateSubscriber), Description = "The subscriber details to create", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "Subscriber details for new subscriber")]
    public async Task<IActionResult> CreateSubscriber(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "subscribers/create")] HttpRequestData req)
    {
        string json = await req.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(json))
        {
            var subscriberDetails = JsonConvert.DeserializeObject<CreateUpdateSubscriber>(json);
            _logger.LogInformation($"Subsciber trigger function Adding subscriber details {json}");

            await _subscriberDataProvider.CreateSubscriberAsync(subscriberDetails);

            return new OkObjectResult($"New subscriber added for subscriber details {json}");
        }
        var error = "Create Subscriber function missing subscriber details.";
        _logger.LogError(error);
        return new BadRequestObjectResult(error);
    }

    [Function("subscriber-update")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("apikeyheader_auth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "x-functions-key")]
    [OpenApiParameter(name: "subscriberId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The subscriber Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateUpdateSubscriber), Description = "The subscriber details to edit", Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "Subscriber details to update")]
    public async Task<IActionResult> UpdateSubscriber(
    [HttpTrigger(AuthorizationLevel.Function, "put", Route = "subscribers/{subscriberId}/update")] HttpRequestData req, string subscriberId)
    {
        if (string.IsNullOrEmpty(subscriberId))
        {
            var subscriberIdIError = "Subscriber trigger function: missing query parameter subscriber Id is required.";
            _logger.LogError(subscriberIdIError);
            return new BadRequestObjectResult(subscriberIdIError);
        }

        string json = await req.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(json))
        {
            var subscriberDetails = JsonConvert.DeserializeObject<CreateUpdateSubscriber>(json);
            _logger.LogInformation($"Update Subscriber trigger function for subscriber details {json}");

            await _subscriberDataProvider.UpdateSubscriberAsync(subscriberId, subscriberDetails);

            return new OkObjectResult($"Subscriber updated for subscriber details {json}");
        }
        var error = "Update Subscriber function missing subscriber details.";
        _logger.LogError(error);
        return new BadRequestObjectResult(error);
    }

    private async Task<PagingParameters> GetPagingParameters(HttpRequestData request)
    {
        PagingParameters pagingParameters = new();

        var content = await request.ReadAsStringAsync();

        if (content.Length > 0)
        {
            pagingParameters = JsonConvert.DeserializeObject<PagingParameters>(content);
            var info = $"Subscriber trigger function: Paging model parameters {pagingParameters}.";
            _logger.LogInformation(info);
        }
        return pagingParameters;
    }
}

