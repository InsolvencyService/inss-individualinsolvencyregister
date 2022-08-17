namespace INSS.EIIR.Data.Models
{
    public partial class SubjectBro
    {
        public int CaseId { get; set; }
        public int SubjRefno { get; set; }
        public int RegBNo { get; set; }
        public DateTime RepTargetDate { get; set; }
        public DateTime? RepSubmittedDate { get; set; }
        public DateTime? AtpGrantedDate { get; set; }
        public DateTime? AtpRefusedDate { get; set; }
        public string? ReasonNotGranted { get; set; }
        public DateTime? RepResubmittedDate { get; set; }
        public DateTime? AppFiledDate { get; set; }
        public DateTime? BroHearingDate { get; set; }
        public DateTime? BruPropDate { get; set; }
        public DateTime? BruAccptDate { get; set; }
        public DateTime? BroOrderDate { get; set; }
        public DateTime? BroNoOrderDate { get; set; }
        public DateTime? BroAbandonedDate { get; set; }
        public DateTime? OrderVariedDate { get; set; }
        public DateTime? BruVariedDate { get; set; }
        public DateTime? BroAnnulledDate { get; set; }
        public DateTime? BroEndDate { get; set; }
        public string Deselected { get; set; } = null!;
    }
}
