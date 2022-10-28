using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Interfaces.Services
{
    public  interface IFeedbackDataProvider
    {
        Task<FeedbackWithPaging> GetFeedbackAsync(FeedbackBody feedbackBody);

        void CreateFeedback(CreateCaseFeedback feedback);

        bool UpdateFeedbackStatus(int feedbackId, bool status);
    }
}
