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

        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string OrganisationName { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "Type")]
        public string OrganisationType { get; set; }

        [Display(Name = "Address line 1")]
        [MaxLength(60)]
        public string ContactAddress1 { get; set; }

        [Display(Name = "Address line 2")]
        [MaxLength(60)]
        public string ContactAddress2 { get; set; }

        [Display(Name = "Town or city")]
        [MaxLength(60)]
        public string ContactCity { get; set; }

        [Display(Name = "Postcode")]
        [MaxLength(10)]
        public string ContactPostcode { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        [MaxLength(60)]
        public string ContactEmail { get; set; }

        [Display(Name = "Telephone number")]
        [MaxLength(20)]
        public string ContactTelephone { get; set; }

        [Display(Name = "Application submitted date")]
        [Range(1, 31)]
        [Required]
        public int ApplicationDay { get; set; }

        [Display(Name = "Application submitted date")]
        [Range(1, 12)]
        [Required]
        public int ApplicationMonth { get; set; }

        [Display(Name = "Application submitted date")]
        [Range(1900, 3000)]
        [Required]
        public int ApplicationYear { get; set; }

        [Display(Name = "Start date")]
        [Range(1, 31)]
        public int? SubscribedFromDay { get; set; }

        [Display(Name = "Start date")]
        [Range(1, 12)]
        public int? SubscribedFromMonth { get; set; }

        [Display(Name = "Start date")]
        [Range(1900, 3000)]
        public int? SubscribedFromYear { get; set; }

        [Display(Name = "End date")]
        [Range(1, 31)]
        public int? SubscribedToDay { get; set; }

        [Display(Name = "End date")]
        [Range(1, 12)]
        public int? SubscribedToMonth { get; set; }

        [Display(Name = "End date")]
        [Range(1900, 3000)]
        public int? SubscribedToYear { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress3 { get; set; }

        [Display(Name = "Status")]
        [Required]
        public string AccountActive { get; set; }

        [Display(Name = "Forename")]
        [MaxLength(40)]
        public string ContactForename { get; set; }

        [Display(Name = "Surname")]
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

