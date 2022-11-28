

namespace INSS.EIIR.QA.Automation.TestFramework;

public static class Constants
{    
    public static string StartPageUrl => WebDriverFactory.Config["BaseUrl"];
    public static string Browser => WebDriverFactory.Config["Browser"];
    public static string AdminUserName => WebDriverFactory.Config["AdminUsername"];
    public static string AdminPassword => WebDriverFactory.Config["AdminPassword"];
    //Original Subscriber Details
    public static string organisationName = "Insolvency Service Internal Account";
    public static string type= "Credit card issuer";
    public static string firstName = "Firstname";
    public static string surname = "Surname";
    public static string addressLine1 = "31 Test Street";
    public static string addressLine2 = "Test Area";
    public static string town = "Test City";
    public static string postcode = "T43 6TT";
    public static string emailAddress = "sheila.todd@insolvency.gsi.gov.uk";
    public static string telephoneNumber = "(0)999 999 9999";
    public static string dataExtractEmail1 = "test1@test.com";
    public static string dataExtractEmail2 = "test2@test.com";
    public static string dataExtractEmail3 = "test3@test.com";
    public static string ApplicationDateDay = "11";
    public static string ApplicationDateMonth = "12";
    public static string ApplicationDateYear = "2018";
    public static string StartDateDay = "13";
    public static string StartDateMonth = "12";
    public static string StartDateYear = "2020";
    public static string EndDateDay = "15";
    public static string EndDateMonth = "10";
    public static string EndDateYear = "2026";
    public static string Status = "Active";
    //Updated Subscriber Details
    public static string UpdatedOrganisationName = "Updated Subscriber";
    public static string UpdatedType = "Debt recovery agents";
    public static string UpdatedFirstName = "UpdatedTestname";
    public static string UpdatedSurname = "UpdatedSurname";
    public static string UpdatedAddressLine1 = "31 Updated Street";
    public static string UpdatedAddressLine2 = "Updated Area";
    public static string UpdatedTown = "Updated City";
    public static string UpdatedPostcode = "U43 6UU";
    public static string UpdatedEmailAddress = "Updated@Email.com";
    public static string UpdatedTelephoneNumber = "1234567890";
    public static string UpdatedDataExtractEmail1 = "Updated1@test.com";
    public static string UpdatedDataExtractEmail2 = "Updated2@test.com";
    public static string UpdatedDataExtractEmail3 = "Updated3@test.com";
    public static string UpdatedApplicationDateDay = "29";
    public static string UpdatedApplicationDateMonth = "2";
    public static string UpdatedApplicationDateYear = "2016";
    public static string UpdatedStartDateDay = "29";
    public static string UpdatedStartDateMonth = "2";
    public static string UpdatedStartDateYear = "2020";
    public static string UpdatedEndDateDay = "29";
    public static string UpdatedEndDateMonth = "2";
    public static string UpdatedEndDateYear = "2028";
    public static string UpdatedStatus = "Inactive";
    public static string NewOrganisationName = "New Updated Subscriber";
}