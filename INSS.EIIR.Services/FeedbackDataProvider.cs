using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.Helpers;

namespace INSS.EIIR.Services
{
    public class FeedbackDataProvider : IFeedbackDataProvider
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackDataProvider(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<FeedbackWithPaging> GetFeedbackAsync(FeedbackBody feedbackBody)
        {
            bool? viewedStatus = null;
            if (!string.IsNullOrEmpty(feedbackBody?.Filters?.Status))
            {
                var filter = feedbackBody.Filters.Status.ToEnum<ViewFilter>();
                viewedStatus = filter switch
                {
                    ViewFilter.All => null,
                    ViewFilter.Viewed => true,
                    ViewFilter.Unviewed => false,
                    _ => null
                };
            }
            var organisation = feedbackBody?.Filters?.Organisation;
            var insolvencyType = feedbackBody?.Filters?.InsolvencyType;

            var totalFeedback = await _feedbackRepository.GetFeedbackAsync();
            var pagedFeedback = totalFeedback
                                    .Where(x => x.Viewed.Equals(viewedStatus) || viewedStatus is null)
                                    .Where(x => x.ReporterOrganisation.Equals(organisation) || string.IsNullOrEmpty(organisation))
                                    .Where(x => x.InsolvencyType.Equals(insolvencyType) || string.IsNullOrEmpty(insolvencyType))
                                    .Skip(feedbackBody.PagingModel.Skip)
                                    .Take(feedbackBody.PagingModel.PageSize);

            var response = new FeedbackWithPaging
            {
                Paging = new Models.PagingModel(totalFeedback.Count(), feedbackBody.PagingModel.PageNumber, feedbackBody.PagingModel.PageSize),
                Feedback = pagedFeedback
            };

            return response;
        }

        public void CreateFeedback(CreateCaseFeedback feedback)
        {
            _feedbackRepository.CreateFeedback(feedback);
        }

        public bool UpdateFeedbackStatus(int feedbackId, bool status)
        {
            return _feedbackRepository.UpdateFeedbackStatus(feedbackId, status);   
        }
    }
}
