namespace INSS.EIIR.Data.Models
{
    public partial class SecretariatAccount
    {
        public string SecretariatId { get; set; } = null!;
        public string OrganisationName { get; set; } = null!;
        public string SecretariatLogin { get; set; } = null!;
        public string SecretariatPassword { get; set; } = null!;
        public string AccountActive { get; set; } = null!;
    }
}
