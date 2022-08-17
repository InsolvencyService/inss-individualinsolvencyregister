namespace INSS.EIIR.Data.Models
{
    public partial class EiirSnapshotTablepreviousDay
    {
        public string Title { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public int CaseNo { get; set; }
        public int IndivNo { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime? Deceased { get; set; }
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? Address5 { get; set; }
        public string? PostCode { get; set; }
        public string Occupation { get; set; } = null!;
        public string Wflag { get; set; } = null!;
        public string Court { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string? Dronumber { get; set; }
        public DateTime? DateofOrder { get; set; }
        public int? Year { get; set; }
        public string Type { get; set; } = null!;
        public int? Office { get; set; }
        public string? CaseName { get; set; }
        public string AnnulmentTypeCase { get; set; } = null!;
        public DateTime? AnnulmentDateCase { get; set; }
        public string? AnnulmentTypePartner { get; set; }
        public DateTime? AnnulmentDatePartner { get; set; }
    }
}
