namespace INSS.EIIR.Data.Models
{
    public partial class WebMessage
    {
        public int Id { get; set; }
        public string Application { get; set; } = null!;
        public string? Message { get; set; }
        public string? HideSearch { get; set; }
    }
}
