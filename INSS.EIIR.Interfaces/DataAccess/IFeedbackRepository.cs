using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Interfaces.DataAccess
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<CaseFeedback>> GetFeedbackAsync();

        void CreateFeedback(CreateCaseFeedback feedback);

        bool UpdateFeedbackStatus(int feedbackId, bool status);
    }
}
