using System;
namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberProfile
    {
        public SubscriberProfile()
        {
        }

        public int DaysToEndSubscription { get; set; }
        public string CompanyName { get; set; }
        public string ContactFullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string ApplicationSubmittedDate { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public string SubscriptionStatus { get; set; }
        public string SubscriptionEmailAddress1 { get; set; }
        public string SubscriptionEmailAddress2 { get; set; }
        public string SubscriptionEmailAddress3 { get; set; }
    }
}

