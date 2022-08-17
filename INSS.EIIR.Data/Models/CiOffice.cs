namespace INSS.EIIR.Data.Models
{
    public partial class CiOffice
    {
        public int OfficeId { get; set; }
        public int? DatabaseNo { get; set; }
        public string OfficeName { get; set; } = null!;
        public string RegisterContact { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string AddressLine3 { get; set; } = null!;
        public string AddressLine4 { get; set; } = null!;
        public string AddressLine5 { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? HasCases { get; set; }
    }
}
