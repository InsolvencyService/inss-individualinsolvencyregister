namespace INSS.EIIR.Models.FeedbackModels
{
    public class CreateCaseFeedback
    {
        public DateTime FeedbackDate { get; set; }
        public string Message { get; set; } = null!;
        public string ReporterFullname { get; set; } = null!;
        public string ReporterEmailAddress { get; set; } = null!;
        public string ReporterOrganisation { get; set; } = null!;
        public int CaseId { get; set; }
    }
}
