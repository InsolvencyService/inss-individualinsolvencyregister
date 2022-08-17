namespace INSS.EIIR.Data.Models
{
    public partial class CiIpAppt
    {
        public int CaseNo { get; set; }
        public int IpApptNo { get; set; }
        public int IpNo { get; set; }
        public int? IpAddressNo { get; set; }
        public string IpApptType { get; set; } = null!;
        public DateTime? ApptStartDate { get; set; }
        public DateTime? ApptEndDate { get; set; }
        public int? OldIpAddressNo { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string EndReason { get; set; } = null!;
        public int IsedPractitionerAppointmentId { get; set; }
    }
}
