namespace INSS.EIIR.Data.Models
{
    public partial class CiTrade
    {
        public int CaseNo { get; set; }
        public int TradingNo { get; set; }
        public string TradingName { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string AddressLine3 { get; set; } = null!;
        public string AddressLine4 { get; set; } = null!;
        public string AddressLine5 { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string AddressWithheldFlag { get; set; } = null!;
    }
}
