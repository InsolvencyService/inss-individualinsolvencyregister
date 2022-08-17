namespace INSS.EIIR.Data.Models
{
    public partial class CiSelection
    {
        public short SelectionType { get; set; }
        public short SelectionSubtype { get; set; }
        public string SelectionCode { get; set; } = null!;
        public string SelectionValue { get; set; } = null!;
    }
}
