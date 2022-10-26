using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.ViewModels
{
    public class SubscriberProfile
    {

        public SubscriberProfile()
        {
        }

        [MaxLength(50)]
        public string OrganisationName { get; set; }

        [MaxLength(40)]
        public string OrganisationType { get; set; }

        [MaxLength(60)]
        public string ContactAddress1 { get; set; }

        [MaxLength(60)]
        public string ContactAddress2 { get; set; }

        [MaxLength(60)]
        public string ContactCity { get; set; }

        [MaxLength(10)]
        public string ContactPostcode { get; set; }

        [EmailAddress]
        [MaxLength(60)]
        public string ContactEmail { get; set; }

        [MaxLength(20)]
        public string ContactTelephone { get; set; }

        [Range(1, 31)]
        [Required]
        public int ApplicationDay { get; set; }

        [Range(1, 12)]
        [Required]
        public int ApplicationMonth { get; set; }

        [Range(1900, 3000)]
        [Required]
        public int ApplicationYear { get; set; }

        [Range(1, 31)]
        public int? SubscribedFromDay { get; set; }

        [Range(1, 12)]
        public int? SubscribedFromMonth { get; set; }

        [Range(1900, 3000)]
        public int? SubscribedFromYear { get; set; }

        [Range(1, 31)]
        public int? SubscribedToDay { get; set; }

        [Range(1, 12)]
        public int? SubscribedToMonth { get; set; }

        [Range(1900, 3000)]
        public int? SubscribedToYear { get; set; }

        [EmailAddress]
        public string EmailAddress1 { get; set; }

        [EmailAddress]
        public string EmailAddress2 { get; set; }

        [EmailAddress]
        public string EmailAddress3 { get; set; }

        [Required]
        public string AccountActive { get; set; }

        [MaxLength(5)]
        public string ContactTitle { get; set; }

        [MaxLength(40)]
        public string ContactForename { get; set; }

        [MaxLength(40)]
        public string ContactSurname { get; set; }

        [HiddenInput]
        public int SubscriberId { get; set; }

        internal DateTime ApplicationDate => new DateTime(ApplicationYear, ApplicationMonth, ApplicationDay);

        internal DateTime? SubscribedFrom => SubscribedFromYear.HasValue && SubscribedFromMonth.HasValue && SubscribedFromDay.HasValue
            ? new DateTime((int)SubscribedFromYear, (int)SubscribedFromMonth, (int)SubscribedFromDay)
            : null;

        internal DateTime? SubscribedTo => SubscribedToYear.HasValue && SubscribedToMonth.HasValue && SubscribedToDay.HasValue
            ? new DateTime((int)SubscribedToYear, (int)SubscribedToMonth, (int)SubscribedToDay)
            : null;

        internal IEnumerable<string> EmailAddresses
        {
            get
            {
                yield return EmailAddress1;

                yield return EmailAddress2;

                yield return EmailAddress3;

            }
        }
    }
}

