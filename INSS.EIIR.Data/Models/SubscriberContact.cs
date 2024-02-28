namespace INSS.EIIR.Data.Models
{
    public partial class SubscriberContact
    {
        public string SubscriberId { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
