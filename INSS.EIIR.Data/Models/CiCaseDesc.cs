namespace INSS.EIIR.Data.Models
{
    public partial class CiCaseDesc
    {
        public int CaseNo { get; set; }
        public int CaseDescNo { get; set; }
        public int CaseDescLineNo { get; set; }
        public string CaseDescLine { get; set; } = null!;
    }
}
