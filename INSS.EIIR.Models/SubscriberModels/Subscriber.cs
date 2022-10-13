namespace INSS.EIIR.Models.SubscriberModels
{
    public class Subscriber
    {
        public Subscriber(string subscriberId, string organisationName, string accountActive, DateTime? subscribedFrom, DateTime? subscribedTo)
        {
            SubscriberId = subscriberId;
            OrganisationName = organisationName;    
            AccountActive = accountActive;  
            SubscribedFrom = subscribedFrom;    
            SubscribedTo = subscribedTo;
        }

        public string SubscriberId { get; set; } = null!;
        public string OrganisationName { get; set; } = null!;
        public string AccountActive { get; set; } = null!;
        public DateTime? SubscribedFrom { get; set; }
        public DateTime? SubscribedTo { get; set; }
    }
}
