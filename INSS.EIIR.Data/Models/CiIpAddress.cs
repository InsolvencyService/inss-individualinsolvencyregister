namespace INSS.EIIR.Data.Models
{
    public partial class CiIpAddress
    {
        public int IpNo { get; set; }
        public int IpAddressNo { get; set; }
        public string IpFirmName { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string AddressLine3 { get; set; } = null!;
        public string AddressLine4 { get; set; } = null!;
        public string AddressLine5 { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string? DxSortCode { get; set; }
        public string? DxNumber { get; set; }
        public string? DxExchange { get; set; }
        public string? DxOnBancs { get; set; }
        public string Phone { get; set; } = null!;
        public string FaxNo { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string RegionCode { get; set; } = null!;
        public string AddressType { get; set; } = null!;
        public string CurrentAddress { get; set; } = null!;
        public int IsedFirmLocationId { get; set; }
        public int IsedFirmId { get; set; }
        public int IsedPractitionerFirmLocationId { get; set; }
    }
}
