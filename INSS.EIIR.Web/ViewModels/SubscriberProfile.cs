using System.ComponentModel.DataAnnotations;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace INSS.EIIR.Web.ViewModels
{
    public class SubscriberProfile
    {
        private const string NameValidCharacters = @"(^[\s',-.\\a-zA-Z0-9]*$)";
        private const string AddressValidCharacters = @"(^[\s',-.\\&()a-zA-Z0-9]*$)";
        private const string InvalidAddressCharactersValidationError = "{0} can contain letters, numbers, whitespace and special characters ',.\\-";
        private const string InvalidNameCharactersValidationError = "{0} can contain letters, numbers, whitespace and special characters ',.\\-&()";

        public SubscriberProfile()
        {
            Breadcrumbs = new List<BreadcrumbLink>();
            SubscriberParameters = new SubscriberParameters();
        }

        [Required(ErrorMessage = "Enter the name of the company or organisation")]
        [MaxLength(50)]
        [Display(Name = "Name")]
        [RegularExpression(NameValidCharacters, ErrorMessage = InvalidNameCharactersValidationError)]
        public string OrganisationName { get; set; }

        [Required(ErrorMessage = "Select the type of company or organisation")]
        [MaxLength(40)]
        [Display(Name = "Type")]
        public string OrganisationType { get; set; }

        [Required(ErrorMessage = "Enter the first name")]
        [Display(Name = "First name")]
        [MaxLength(40)]       
        [RegularExpression(NameValidCharacters, ErrorMessage = InvalidNameCharactersValidationError)]
        public string ContactForename { get; set; }

        [Required(ErrorMessage = "Enter the last name")]
        [Display(Name = "Last name")]
        [MaxLength(40)]
        [RegularExpression(NameValidCharacters, ErrorMessage = InvalidNameCharactersValidationError)]
        public string ContactSurname { get; set; }

        [Required(ErrorMessage = "Enter line 1 of the address")]
        [Display(Name = "Address line 1")]
        [MaxLength(60)]
        [RegularExpression(AddressValidCharacters, ErrorMessage = InvalidAddressCharactersValidationError)]
        public string ContactAddress1 { get; set; }

        [Display(Name = "Address line 2")]
        [MaxLength(60)]
        [RegularExpression(AddressValidCharacters, ErrorMessage = InvalidAddressCharactersValidationError)]
        public string ContactAddress2 { get; set; }

        [Required(ErrorMessage = "Enter the town or city")]
        [Display(Name = "Town or city")]
        [MaxLength(60)]
        [RegularExpression(AddressValidCharacters, ErrorMessage = InvalidAddressCharactersValidationError)]
        public string ContactCity { get; set; }

        [Required(ErrorMessage = "Enter the postcode")]
        [Display(Name = "Postcode")]
        [RegularExpression("(?i:^(([A-Z]{1,2}[0-9][A-Z0-9]?|ASCN|STHL|TDCU|BBND|[BFS]IQQ|PCRN|TKCA) ?[0-9][A-Z]{2}|BFPO ?[0-9]{1,4}|(KY[0-9]|MSR|VG|AI)[ -]?[0-9]{4}|[A-Z]{2} ?[0-9]{2}|GE ?CX|GIR ?0A{2}|SAN ?TA1)$)", 
            ErrorMessage = "Enter the postcode in the correct format")]
        [MaxLength(10)]
        public string ContactPostcode { get; set; }

        [Required(ErrorMessage = "Enter the contact email address")]
        [Display(Name = "Email address")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "Enter the contact email address in the correct format")]
        [MaxLength(60)]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Enter the telephone number")]
        [Display(Name = "Telephone number")]
        [MaxLength(20)]
        public string ContactTelephone { get; set; }

        [Display(Name = "Application submitted date")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])$", ErrorMessage = "Enter a day between 1 and 31")]
        [Required(ErrorMessage = "The application submitted date must include a day")]
        public string ApplicationDay { get; set; }

        [Display(Name = "Application submitted date")]
        [RegularExpression(@"^([1-9]|1[0-2])$", ErrorMessage = "Enter a month between 1 and 12")]
        [Required(ErrorMessage = "The application submitted date must include a month")]
        public string ApplicationMonth { get; set; }

        [Display(Name = "Application submitted date")]
        [RegularExpression(@"^([1][9]\d{2}|[2]\d{3}|[3](0){3})$", ErrorMessage = "Enter a year between 1900 and 3000")]
        [Required(ErrorMessage = "The application submitted date must include a year")]
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

        [Required(ErrorMessage = "Enter data extract email address 1")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "Enter data extract email address 1 in the correct format")]
        [Display(Name = "Email address")]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email address")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "Enter data extract email address 2 in the correct format")]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Email address")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "Enter data extract email address 3 in the correct format")]
        public string EmailAddress3 { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Select if the status is active or inactive")] 
        public string AccountActive { get; set; }

        [HiddenInput] public int SubscriberId { get; set; }

        [ValidateNever]
        public List<BreadcrumbLink> Breadcrumbs { get; set; }

        [ValidateNever]
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
                    : DateTime.MinValue;
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

