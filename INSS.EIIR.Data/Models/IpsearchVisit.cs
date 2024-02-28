namespace INSS.EIIR.Data.Models
{
    public partial class IpsearchVisit
    {
        public int Id { get; set; }
        public string? UserAgent { get; set; }
        public string? Referer { get; set; }
        public string? Host { get; set; }
        public string? HttpLanguage { get; set; }
        public string? RemoteHost { get; set; }
        public string? RemoteAddr { get; set; }
        public string? LocalAddr { get; set; }
        public DateTime? DateHit { get; set; }
        public string? SearchType { get; set; }
    }
}
