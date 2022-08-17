namespace INSS.EIIR.Data.Models
{
    public partial class FeedBack
    {
        public int Id { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public int? MessageType { get; set; }
        public int? Subject { get; set; }
        public string? Message { get; set; }
        public string? EmailAddress { get; set; }
        public int? Status { get; set; }
        public string? UserAgent { get; set; }
        public string? RemoteHost { get; set; }
        public string? RemoteAddr { get; set; }
    }
}
