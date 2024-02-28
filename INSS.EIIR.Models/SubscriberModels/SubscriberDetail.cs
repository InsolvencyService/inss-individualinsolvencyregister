namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberDetail
    {
        public string OrganisationType { get; set; } = null!;
        public string ApplicationIpaddress { get; set; } = null!;
        public string ContactTitle { get; set; } = null!;
        public string ContactForename { get; set; } = null!;
        public string ContactSurname { get; set; } = null!;
        public string ContactAddress1 { get; set; } = null!;
        public string ContactAddress2 { get; set; } = null!;
        public string ContactCity { get; set; } = null!;
        public string ContactPostcode { get; set; } = null!;
        public string ContactTelephone { get; set; } = null!;
        public string ContactEmail { get; set; }
        public string ContactCountry { get; set; } = null!;
        public string OrganisationWebsite { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationViewed { get; set; } = null!;
        public string ApplicationViewedBy { get; set; } = null!;
        public DateTime? ApplicationViewedDate { get; set; }
        public string ApplicationApproved { get; set; } = null!;
        public string ApprovedIpaddress { get; set; } = null!;
    }
}
