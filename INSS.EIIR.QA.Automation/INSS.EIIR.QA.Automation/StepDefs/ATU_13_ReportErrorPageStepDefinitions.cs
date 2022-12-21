using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using INSS.EIIR.QA.Automation.Data;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_13_ReportErrorPageStepDefinitions
    {
        [Given(@"I click the Report an error or issue link on the Case Details page")]
        [When(@"I click the Report an error or issue link on the Case Details page")]
        public void WhenIClickTheReportAnErrorOrIssueLinkOnTheCaseDetailsPage()
        {
            CaseDetailsPage.ClickReportErrorOrIssueLink();
        }

        [Then(@"the Report an error page is displayed")]
        public void ThenTheReportAnErrorPageIsDisplayed()
        {
            ReportAnErrorPage.verifyReportAnErrorPage();
        }

        [Then(@"the breadcrumb text will be as expected on the Report an error page when the journey started from the Start page")]
        public void ThenTheBreadcrumbTextWillBeAsExpectedOnTheReportAnErrorPageWhenTheJourneyStartedFromTheStartPage()
        {
            ReportAnErrorPage.verifyBreadcrumbText_FromStartPage();
        }

        [When(@"I click the Home breadcrumb on the Report an error page")]
        public void WhenIClickTheHomeBreadcrumbOnTheReportAnErrorPage()
        {
            ReportAnErrorPage.ClickHomeBreadcrumb();
        }

        [Given(@"I navigate to the Report an error page from the Start page by searching for ""([^""]*)"" with Postcode ""([^""]*)""")]
        public void GivenINavigateToTheReportAnErrorPageFromTheStartPageBySearchingForWithPostcode(string SearchTerm, string Postcode)
        {
            SearchPage.NavigateToSearchPage();
            SearchPage.EnterSearchText(SearchTerm);
            SearchPage.ClickSearchButton();
            SearchResultsPage.ClickIndividualLink(Postcode);
            CaseDetailsPage.ClickReportErrorOrIssueLink();
        }

        [Given(@"I click the Search the Individual Insolvency Register breadcrumb on the Report an error page")]
        public void GivenIClickTheSearchTheIndividualInsolvencyRegisterBreadcrumbOnTheReportAnErrorPage()
        {
            ReportAnErrorPage.ClickSearchTheInsolvencyRegisterBreadcrumb();
        }

        [Given(@"I click the Search results breadcrumb on the Report an error page")]
        public void GivenIClickTheSearchResultsBreadcrumbOnTheReportAnErrorPage()
        {
            ReportAnErrorPage.ClickSearchResultsBreadcrumb();
        }

        [Given(@"I click the Case details breadcrumb on the Report an error page")]
        public void GivenIClickTheCaseDetailsBreadcrumbOnTheReportAnErrorPage()
        {
            ReportAnErrorPage.ClickCaseDetailsBreadcrumb();
        }

        [Given(@"I click the case name ""([^""]*)""")]
        public void GivenIClickTheCaseName(string CaseName)
        {
            AdminCaseFeedbackPage.clickCaseLink();
        }

        [Then(@"the breadcrumb text will be as expected on the Report an error page when the journey started from the Feedback page")]
        public void ThenTheBreadcrumbTextWillBeAsExpectedOnTheReportAnErrorPageWhenTheJourneyStartedFromTheFeedbackPage()
        {
            ReportAnErrorPage.verifyBreadcrumbText_FromErrorsOrIssuesPage();
        }

        [Given(@"I fill in all of the fields with valid values and press Confirm and send")]
        [When(@"I fill in all of the fields with valid values and press Confirm and send")]
        public void WhenIFillInAllOfTheFieldsWithValidValuesAndPressConfirmAndSend()
        {
            ReportAnErrorPage.EnterValidReportAnErrorDetails();
        }

        [Then(@"the Report an error record will be written to the database using case data from ""([^""]*)"" journey")]
        public void ThenTheReportAnErrorRecordWillBeWrittenToTheDatabaseUsingCaseDataFromJourney(string Journey)
        {
            ReportAnErrorPage.VerifyReportAnErrorDatabaseRecord(Journey);
        }

        [When(@"I don't select an organisation and I press Confirm and send")]
        public void WhenIDontSelectAnOrganisationAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.FillInTextFields();
        }

        [Then(@"the user is shown the following error ""([^""]*)""")]
        public void ThenTheUserIsShownTheFollowingError(string ErrorMessage)
        {
            ReportAnErrorPage.VerifyErrorMessage(ErrorMessage);
        }

        [When(@"I don't enter a description and I press Confirm and send")]
        public void WhenIDontEnterADescriptionAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInNameField();
            ReportAnErrorPage.FillInEMailField();
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [When(@"I enter a description with invalid characters and I press Confirm and send")]
        public void WhenIEnterADescriptionWithInvalidCharactersAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInInvalidTextInMessageField();
            ReportAnErrorPage.FillInNameField();
            ReportAnErrorPage.FillInEMailField();
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [When(@"I don't enter a Full name and I press Confirm and send")]
        public void WhenIDontEnterAFullNameAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInMessageField();
            ReportAnErrorPage.FillInEMailField();
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [When(@"I enter a Full name with invalid characters and I press Confirm and send")]
        public void WhenIEnterAFullNameWithInvalidCharactersAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInMessageField();
            ReportAnErrorPage.FillInInvalidTextInNameField();
            ReportAnErrorPage.FillInEMailField();         
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [When(@"I don't enter an email address and I press Confirm and send")]
        public void WhenIDontEnterAnEmailAddressAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInMessageField();
            ReportAnErrorPage.FillInNameField();
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [When(@"I enter an invalid email address and I press Confirm and send")]
        public void WhenIEnterAnInvalidEmailAddressAndIPressConfirmAndSend()
        {
            ReportAnErrorPage.ClearTextFields();
            ReportAnErrorPage.FillInMessageField();
            ReportAnErrorPage.FillInNameField();
            ReportAnErrorPage.FillInInvalidTextInEmailField();            
            ReportAnErrorPage.SelectAnOrganisationField();
            ReportAnErrorPage.ClickConfirmAndSendButton();
        }

        [Given(@"I first clear the database records for Case Feedback")]
        public void GivenIFirstClearTheDatabaseRecordsForCaseFeedback()
        {
            SqlQueries.deleteFeedbackData();
        }

        [Given(@"I click the case name link")]
        public void GivenIClickTheCaseNameLink()
        {
            AdminCaseFeedbackPage.ClickCaseNameLink();
        }

    }
}
