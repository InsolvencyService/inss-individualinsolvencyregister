namespace INSS.EIIR.Models.SubscriberModels
{
    public class Subscriber
    {
        public string SubscriberId { get; set; } = null!;
        public string OrganisationName { get; set; } = null!;
        public string AccountActive { get; set; } = null!;
        public string AuthorisedBy { get; set; } = null!;
        public DateTime? AuthorisedDate { get; set; }
        public DateTime? SubscribedFrom { get; set; }
        public DateTime? SubscribedTo { get; set; }
        public SubscriberDetail SubscriberDetails { get; set; }
        public IList<SubscriberEmailContact> EmailContacts { get; set; }
    }
}
