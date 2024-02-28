namespace INSS.EIIR.Web.Constants;

public static class ValidationOrder
{
    public static List<string> SubscriberFieldValidationOrder = new()
    {
        "OrganisationName",
        "OrganisationType",
        "ContactForename",
        "ContactSurname",
        "ContactAddress1",
        "ContactAddress2",
        "ContactCity",
        "ContactPostcode",
        "ContactEmail",
        "ContactTelephone",
        "ApplicationDate",
        "ApplicationDay",
        "ApplicationMonth",
        "ApplicationYear",
        "SubscribedFromDate",
        "SubscribedFromDay",
        "SubscribedFromMonth",
        "SubscribedFromYear",
        "SubscribedToDate",
        "SubscribedToDay",
        "SubscribedToMonth",
        "SubscribedToYear",
        "EmailAddresses",
        "EmailAddress1",
        "EmailAddress2",
        "EmailAddress3"
    };

    public static List<string> ErrorReportFieldValidationOrder = new()
    {
        "CaseFeedback.Message",
        "CaseFeedback.ReporterFullname",
        "CaseFeedback.ReporterEmailAddress",
        "CaseFeedback.ReporterOrganisation"
    };
}