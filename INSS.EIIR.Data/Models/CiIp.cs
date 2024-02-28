namespace INSS.EIIR.Data.Models
{
    public partial class CiIp
    {
        public int IpNo { get; set; }
        public string Title { get; set; } = null!;
        public string Initials { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Forenames { get; set; } = null!;
        public string LicensingBody { get; set; } = null!;
        public DateTime? DateFirstAuth { get; set; }
        public DateTime? DateAuthWithdrawn { get; set; }
        public string ReasonWithdrawn { get; set; } = null!;
        public DateTime? RenewalDate { get; set; }
        public DateTime? OrigVisitDate { get; set; }
        public DateTime? PlannedVisitDate { get; set; }
        public string Authorised { get; set; } = null!;
        public int NoOfCases { get; set; }
        public DateTime? RestrictionExpiryDate { get; set; }
        public string? RestrictionType { get; set; }
        public string? IpEmailAddress { get; set; }
        public string IncludeOnInternet { get; set; } = null!;
        public string? RestrictionDescription { get; set; }
        public string IsedPractitionerTypeCode { get; set; } = null!;
        public int IsedPractitionerId { get; set; }
        public string? RegisteredFirmName { get; set; }
        public string? RegisteredAddressLine1 { get; set; }
        public string? RegisteredAddressLine2 { get; set; }
        public string? RegisteredAddressLine3 { get; set; }
        public string? RegisteredAddressLine4 { get; set; }
        public string? RegisteredAddressLine5 { get; set; }
        public string? RegisteredPostCode { get; set; }
        public string? RegisteredPhone { get; set; }
        public string? RegisteredFax { get; set; }
    }
}
