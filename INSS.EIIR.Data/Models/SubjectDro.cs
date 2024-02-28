namespace INSS.EIIR.Data.Models
{
    public partial class SubjectDro
    {
        public int CaseId { get; set; }
        public int CidebtorSubjectNo { get; set; }
        public int CasdrocaseDetailId { get; set; }
        public DateTime? MoratoriumPeriodEndingDate { get; set; }
        public string? Nationality { get; set; }
        public DateTime? RevokedDate { get; set; }
        public int? RevocationReason { get; set; }
        public string? ExperianReference { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedUser { get; set; } = null!;
        public int? ApplicationId { get; set; }
    }
}
