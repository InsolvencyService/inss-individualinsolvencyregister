namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberDetail
    {
        public SubscriberDetail(int subscriberId, string organisationName, string contactTitle, string contactForename, string contactSurname, string contactEmail)
        {
            SubscriberId = subscriberId;
            OrganisationName = organisationName;
            ContactTitle = contactTitle;
            ContactForename = contactForename;
            ContactSurname = contactSurname;
            ContactEmail = contactEmail;
        }

        public int SubscriberId { get; set; }
        public string OrganisationName { get; set; } = null!;
        public string ContactTitle { get; set; } = null!;
        public string ContactForename { get; set; } = null!;
        public string ContactSurname { get; set; } = null!;
        public string ContactEmail { get; set; }
    }
}
