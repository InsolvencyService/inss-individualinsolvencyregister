namespace INSS.EIIR.Data.Models
{
    public partial class CiIndividual
    {
        public int CaseNo { get; set; }
        public int IndivNo { get; set; }
        public string IndivType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Initials { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Forenames { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? AddressLine5 { get; set; }
        public string? Postcode { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string JobTitle { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public string NiNumber { get; set; } = null!;
        public DateTime? DeceasedDate { get; set; }
        public string AddressType { get; set; } = null!;
        public string SummaryCertFlag { get; set; } = null!;
        public string? OnRegisterFlag { get; set; }
        public DateTime? RegisterExpiryDate { get; set; }
        public string RegisterLastAmendBy { get; set; } = null!;
        public DateTime? RegisterLastAmendDate { get; set; }
        public string AddressWithheldFlag { get; set; } = null!;
        public string? SocCode { get; set; }
        public string? SocSubCode { get; set; }
        public string? SocYear { get; set; }
        public string? PrimaryOccClass { get; set; }
        public int IsedIndividualId { get; set; }
        public DateTime? DtSubjectAnnulled { get; set; }
        public string? SubjAnnulmentType { get; set; }
    }
}
