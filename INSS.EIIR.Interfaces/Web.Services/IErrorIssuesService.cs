using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Interfaces.Web.Services;

public interface IErrorIssuesService
{
    Task<FeedbackWithPaging> GetFeedbackAsync(FeedbackBody feedbackParameters);
}