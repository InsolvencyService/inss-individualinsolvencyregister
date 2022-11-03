using System.ComponentModel.DataAnnotations;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.ViewModels
{
    public class SubscriberProfile
    {

        public SubscriberProfile()
        {
            Breadcrumbs = new List<BreadcrumbLink>();
        }

        [Required(ErrorMessage = "Enter a name")]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string OrganisationName { get; set; }

        [Required(ErrorMessage = "Select a Type")]
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
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])$", ErrorMessage = "Enter a day between 1 and 31")]
        [Required(ErrorMessage = "Enter an application day")]
        public string ApplicationDay { get; set; }

        [Display(Name = "Application submitted date")]
        [RegularExpression(@"^([1-9]|1[0-2])$", ErrorMessage = "Enter a month between 1 and 12")]
        [Required(ErrorMessage = "Enter an application month")]
        public string ApplicationMonth { get; set; }

        [Display(Name = "Application submitted date")]
        [RegularExpression(@"^([1][9]\d{2}|[2]\d{3}|[3](0){3})$", ErrorMessage = "Enter a year between 1900 and 3000")]
        [Required(ErrorMessage = "Enter an application year")]
        public string ApplicationYear { get; set; }

        [Display(Name = "Start date")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])$", ErrorMessage = "Enter a day between 1 and 31")]
        public string SubscribedFromDay { get; set; }

        [Display(Name = "Start date")]
        [RegularExpression(@"^([1-9]|1[0-2])$", ErrorMessage = "Enter a month between 1 and 12")]
        public string SubscribedFromMonth { get; set; }

        [Display(Name = "Start date")]
        [RegularExpression(@"^([1][9]\d{2}|[2]\d{3}|[3](0){3})$", ErrorMessage = "Enter a year between 1900 and 3000")]
        public string SubscribedFromYear { get; set; }

        [Display(Name = "End date")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])$", ErrorMessage = "Enter a day between 1 and 31")]
        public string SubscribedToDay { get; set; }

        [Display(Name = "End date")]
        [RegularExpression(@"^([1-9]|1[0-2])$", ErrorMessage = "Enter a month between 1 and 12")]
        public string SubscribedToMonth { get; set; }

        [Display(Name = "End date")]
        [RegularExpression(@"^([1][9]\d{2}|[2]\d{3}|[3](0){3})$", ErrorMessage = "Enter a year between 1900 and 3000")]
        public string SubscribedToYear { get; set; }

        [Required(ErrorMessage = "Enter an extract email address")]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress3 { get; set; }

        [Display(Name = "Status")][Required] public string AccountActive { get; set; }

        [Display(Name = "Forename")]
        [MaxLength(40)]
        public string ContactForename { get; set; }

        [Display(Name = "Surname")]
        [MaxLength(40)]
        public string ContactSurname { get; set; }

        [HiddenInput] public int SubscriberId { get; set; }

        public List<BreadcrumbLink> Breadcrumbs { get; set; }

        public SubscriberParameters SubscriberParameters { get; set; }

        internal DateTime ApplicationDate
        {
            get
            {
                var isYear = int.TryParse(ApplicationYear, out var year);
                var isMonth = int.TryParse(ApplicationMonth, out var month);
                var isDay = int.TryParse(ApplicationDay, out var day);

                return isYear && isMonth && isDay
                    ? new DateTime(year, month, day)
                    : throw new ArgumentException("Subscriber ApplicationDate values not valid");
            }
        }

        internal DateTime? SubscribedFrom
        {
            get
            {
                var isYear = int.TryParse(SubscribedFromYear, out var year);
                var isMonth = int.TryParse(SubscribedFromMonth, out var month);
                var isDay = int.TryParse(SubscribedFromDay, out var day);

                return isYear && isMonth && isDay
                        ? new DateTime(year, month, day)
                        : null;
            }
        }


        internal DateTime? SubscribedTo
        {
            get
            {
                var isYear = int.TryParse(SubscribedToYear, out var year);
                var isMonth = int.TryParse(SubscribedToMonth, out var month);
                var isDay = int.TryParse(SubscribedToDay, out var day);

                return isYear && isMonth && isDay
                    ? new DateTime(year, month, day)
                    : null;
            }
        }

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

