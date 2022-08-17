namespace INSS.EIIR.Data.Models
{
    public partial class CiIndivDischarge
    {
        public int CaseNo { get; set; }
        public int IndivNo { get; set; }
        public DateTime? PreviousOrderDate { get; set; }
        public string DischargeType { get; set; } = null!;
        public DateTime? DischargeDate { get; set; }
        public DateTime? SuspensionDate { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public string PreviousOrderStatus { get; set; } = null!;
        public DateTime? PreviousOrderEndDate { get; set; }
    }
}
