namespace INSS.EIIR.Data.Models
{
    public partial class CiIpAuthorisingBody
    {
        public string? AuthBodyCode { get; set; }
        public string AuthBodyName { get; set; } = null!;
        public string AuthBodyAddressLine1 { get; set; } = null!;
        public string AuthBodyAddressLine2 { get; set; } = null!;
        public string AuthBodyAddressLine3 { get; set; } = null!;
        public string AuthBodyAddressLine4 { get; set; } = null!;
        public string AuthBodyAddressLine5 { get; set; } = null!;
        public string AuthBodyPostcode { get; set; } = null!;
        public string AuthBodyPhone { get; set; } = null!;
        public string AuthBodyFaxNo { get; set; } = null!;
        public string AuthBodyWebsite { get; set; } = null!;
    }
}
