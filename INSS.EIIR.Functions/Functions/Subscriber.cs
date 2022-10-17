using INSS.EIIR.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

    [FunctionName("Subscriber")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The list of active and inactive subscribers")]
    public async Task<IActionResult> GetSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscribers")] HttpRequest req)
    {
        _logger.LogInformation("Subscriber trigger function retreiving all subscribers.");

        var subscribers = await _subscriberDataProvider.GetSubscribersAsync();

        return new OkObjectResult(subscribers);
    }

    [FunctionName("SubscriberById")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The subscriber Id")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Subscriber details for the Id specified")]
    public async Task<IActionResult> GetSubscriberById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscribers/{id}")] HttpRequest req)
    {
        string id = req.Query["id"];
        if (string.IsNullOrEmpty(id))
        {
            var error = "Subscriber trigger function: missing query parameter subscriber Id is required.";
            _logger.LogError(error);
            return new BadRequestObjectResult(error);
        }

        _logger.LogInformation($"Subsciber trigger function retreiving subscriber details for subscriber Id {id}.");
        
        var subscribers = await _subscriberDataProvider.GetSubscriberByIdAsync(id);

        return new OkObjectResult(subscribers);
    }

    [FunctionName("active-subscribers")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The list of active subscribers")]
    public async Task<IActionResult> GetActiveSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscribers/active")] HttpRequest req)
    {
        _logger.LogInformation("Subscriber trigger function retreiving active subscribers.");

        var subscribers = await _subscriberDataProvider.GetActiveSubscribersAsync();

        return new OkObjectResult(subscribers);
    }

    [FunctionName("inactive-subscribers")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Subscriber" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The list of inactive subscribers")]
    public async Task<IActionResult> GetInactiveSubscribers(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscribers/inactive")] HttpRequest req)
    {
        _logger.LogInformation("Subscriber trigger function retreiving inactive subscribers.");

        var subscribers = await _subscriberDataProvider.GetInActiveSubscribersAsync();

        return new OkObjectResult(subscribers);
    }
}

