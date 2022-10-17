namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberEmailContact
    {
        public string SubscriberId { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
