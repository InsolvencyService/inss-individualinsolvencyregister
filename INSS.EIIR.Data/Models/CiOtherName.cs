namespace INSS.EIIR.Data.Models
{
    public partial class CiOtherName
    {
        public int CaseNo { get; set; }
        public int IndivNo { get; set; }
        public int AliasNo { get; set; }
        public string AliasType { get; set; } = null!;
        public string Initials { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Forenames { get; set; } = null!;
        public int IsedAliasId { get; set; }
        public int IsedIndividualId { get; set; }
    }
}
