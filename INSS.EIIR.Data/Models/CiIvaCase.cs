namespace INSS.EIIR.Data.Models
{
    public partial class CiIvaCase
    {
        public int CaseNo { get; set; }
        public string IvaCourt { get; set; } = null!;
        public string IvaNumber { get; set; } = null!;
        public DateTime? DateOfRegistration { get; set; }
        public DateTime? DateFeePaid { get; set; }
        public DateTime? DateOfFailure { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public DateTime? DateOfRevocation { get; set; }
        public DateTime? DateOfSuspension { get; set; }
        public DateTime? DateSuspensionLifted { get; set; }
        public DateTime? DateOfNotification { get; set; }
    }
}
