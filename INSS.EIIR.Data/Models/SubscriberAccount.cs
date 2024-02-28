namespace INSS.EIIR.Data.Models
{
    public partial class SubscriberAccount
    {
        public string SubscriberId { get; set; } = null!;
        public string OrganisationName { get; set; } = null!;
        public string SubscriberLogin { get; set; } = null!;
        public string SubscriberPassword { get; set; } = null!;
        public string AccountActive { get; set; } = null!;
        public string AuthorisedBy { get; set; } = null!;
        public DateTime? AuthorisedDate { get; set; }
        public string AuthorisedIpaddress { get; set; } = null!;
        public DateTime? SubscribedFrom { get; set; }
        public DateTime? SubscribedTo { get; set; }
    }
}
