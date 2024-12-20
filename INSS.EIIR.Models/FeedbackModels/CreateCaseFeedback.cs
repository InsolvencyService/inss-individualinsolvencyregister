using System.ComponentModel.DataAnnotations;

namespace INSS.EIIR.Models.FeedbackModels
{
    public class CreateCaseFeedback
    {
        public int CaseId { get; set; }
        
        public DateTime FeedbackDate { get; set; }

        [Required(ErrorMessage = "Enter an error or issue")]
        [MaxLength(200, ErrorMessage = "Enter a maximum of 200 characters for error or issue")]
        [RegularExpression(@"(^[\s'-.a-zA-Z0-9]*$)", ErrorMessage = "Enter only letters, numbers, - , or '")]
        public string Message { get; set; } = null!;

        [Required(ErrorMessage = "Enter full name")]
        [RegularExpression(@"(^[\s'-.a-zA-Z0-9]*$)", ErrorMessage = "Enter only letters, numbers, - , or '")]
        public string ReporterFullname { get; set; } = null!;

        [Required(ErrorMessage = "Enter an email address")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"
            ,ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string ReporterEmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Select an organisation")]
        public string ReporterOrganisation { get; set; } = null!;

        public string InsolvencyType { get; set; }

        public string CaseName { get; set; }

        public DateTime? InsolvencyDate { get; set; }

        public int IndivNo { get; set; }

    }
}