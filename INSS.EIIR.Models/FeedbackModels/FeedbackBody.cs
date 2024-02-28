using INSS.EIIR.Models.Configuration;

namespace INSS.EIIR.Models.FeedbackModels
{
    public class FeedbackBody
    {
        public PagingParameters PagingModel { get; set; }

        public FeedbackFilterModel Filters { get; set; }
    }
}