using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Web.Services;

public class ErrorIssuesService : IErrorIssuesService
{
    private readonly IClientService _clientService;

    public ErrorIssuesService(IClientService clientService)
    {
        _clientService = clientService;
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
}