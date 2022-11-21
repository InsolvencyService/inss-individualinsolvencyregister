using Flurl;
using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.FeedbackModels;
using Flurl.Http;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Web.Services;

public class ErrorIssuesService : IErrorIssuesService
{
    private readonly IClientService _clientService;
    private readonly ApiSettings _settings;

    public ErrorIssuesService(
        IClientService clientService,
        IOptions<ApiSettings> settings)
    {
        _clientService = clientService;
        _settings = settings.Value;
    }

    public async Task<FeedbackWithPaging> GetFeedbackAsync(FeedbackBody feedbackParameters)
    {
        var apiRoot = "eiir/feedback";

        var feedback = new FeedbackWithPaging();

        var result = await _clientService.PostAsync<FeedbackBody, FeedbackWithPaging>(apiRoot, feedbackParameters);

        if (result != null)
        {
            feedback = result;
        }

        return feedback;
    }

    public async Task UpdateStatusAsync(int feedbackId, bool viewed)
    {
        var apiRoot = $"eiir/feedback/{feedbackId}/viewed/{viewed}";
        await _settings.BaseUrl
        .AppendPathSegment(apiRoot)
            .WithHeader("x-functions-key", _settings.ApiKey)
            .PutJsonAsync(null); 
    }
}