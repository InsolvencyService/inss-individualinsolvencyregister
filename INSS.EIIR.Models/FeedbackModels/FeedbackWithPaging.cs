namespace INSS.EIIR.Models.FeedbackModels
{
    public class FeedbackWithPaging
    {
        public PagingModel Paging { get; set; }
        public IEnumerable<CaseFeedback> Feedback { get; set; }
    }
}
