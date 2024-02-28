namespace INSS.EIIR.Data.Models
{
    public partial class CiCase
    {
        public int CaseNo { get; set; }
        public string InsolvencyType { get; set; } = null!;
        public string Court { get; set; } = null!;
        public string CourtNo { get; set; } = null!;
        public int? CaseYear { get; set; }
        public string BatchType { get; set; } = null!;
        public int? BatchNo { get; set; }
        public int? FileNo { get; set; }
        public int? OfficeId { get; set; }
        public string CaseName { get; set; } = null!;
        public string Partnership { get; set; } = null!;
        public DateTime? PetitionDate { get; set; }
        public DateTime? InsolvencyDate { get; set; }
        public DateTime? HtipDate { get; set; }
        public string CaseStatus { get; set; } = null!;
        public string OnBancs { get; set; } = null!;
        public string OnLois { get; set; } = null!;
        public string TradeClassNo { get; set; } = null!;
        public DateTime? TradingStartDate { get; set; }
        public DateTime? TradingEndDate { get; set; }
        public DateTime? MembersApptDate { get; set; }
        public DateTime? GazetteDate { get; set; }
        public string? UserId { get; set; }
        public string? ExaminerId { get; set; }
        public string AnnulmentType { get; set; } = null!;
        public DateTime? AnnulmentDate { get; set; }
        public string StayFlag { get; set; } = null!;
        public DateTime? StayExpiryDate { get; set; }
        public string PreRegisterFlag { get; set; } = null!;
        public DateTime? FinalReleaseDate { get; set; }
        public string? PrimaryTradeClass { get; set; }
        public string? SicCode { get; set; }
        public string? SicSubCode { get; set; }
        public string? SicFreeText { get; set; }
        public string? SicYear { get; set; }
        public string OnEms { get; set; } = null!;
        public int IsedCaseId { get; set; }
        public string? IscisCaseRef { get; set; }
    }
}
