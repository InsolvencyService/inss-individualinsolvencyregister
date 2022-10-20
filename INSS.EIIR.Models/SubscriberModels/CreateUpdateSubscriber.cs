namespace INSS.EIIR.Models.SubscriberModels
{
    public class CreateUpdateSubscriber
    {
        public string OrganisationName { get; set; } = null!;
        public string OrganisationType { get; set; } = null!;
        public string ContactForename { get; set; } = null!;
        public string ContactSurname { get; set; } = null!;
        public string ContactAddress1 { get; set; } = null!;
        public string ContactAddress2 { get; set; } = null!;
        public string ContactCity { get; set; } = null!;
        public string ContactPostcode { get; set; } = null!;
        public string ContactTelephone { get; set; } = null!;
        public string ContactEmail { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? SubscribedFrom { get; set; }
        public DateTime? SubscribedTo { get; set; }
        public string AccountActive { get; set; } = null!;
        public IList<string> EmailAddresses { get; set; } = null!;
    }
}
