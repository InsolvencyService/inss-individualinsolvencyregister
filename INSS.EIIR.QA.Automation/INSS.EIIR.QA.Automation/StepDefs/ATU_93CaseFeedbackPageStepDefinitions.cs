using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using INSS.EIIR.QA.Automation.Data;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_93CaseFeedbackPageStepDefinitions
    {
        [Given(@"I click the View feedback link")]
        public void GivenIClickTheViewFeedbackLink()
        {
            AdminLandingPage.clickViewFeedbackLink();
        }

        [Then(@"I am taken to the Admin - View case errors or issues page")]
        public void ThenIAmTakenToTheAdmin_ViewCaseErrorsOrIssuesPage()
        {
            AdminCaseFeedbackPage.verifyCaseFeedbackPage();
        }

        [When(@"I click the home breadcrumb on the case feedback page")]
        public void WhenIClickTheHomeBreadcrumbOnTheCaseFeedbackPage()
        {
            AdminCaseFeedbackPage.clickHomeBreadcrumb();
        }

        [Given(@"I create case feedback data for this test")]
        public void GivenICreateCaseFeedbackDataForThisTest()
        {
            SqlQueries.createCaseFeedbackData();
        }

        //[Given(@"the case feedback details are displayed")]
        //public void GivenTheCaseFeedbackDetailsAreDisplayed()
        //{
        //    AdminCaseFeedbackPage.verifyCaseDetailsCorrectlyDisplayed();
        //}

        [Given(@"I select the ""([^""]*)"" link in the organisation dropdown on the Case Feedback page")]
        public void GivenISelectTheLinkInTheOrganisationDropdownOnTheCaseFeedbackPage(string OrganisationType)
        {
            AdminCaseFeedbackPage.SelectOrganisation(OrganisationType);
        }

        [When(@"I select the ""([^""]*)"" dropdown list in the organisation dropdown on the Case Feedback page")]
        [Given(@"I select the ""([^""]*)"" dropdown list in the organisation dropdown on the Case Feedback page")]
        public void GivenISelectTheDropdownListInTheOrganisationDropdownOnTheCaseFeedbackPage(string organisation)
        {
            AdminCaseFeedbackPage.SelectOrganisation(organisation);
        }

        [When(@"I select the ""([^""]*)"" dropdpwn in the organisation dropdown on the Case Feedback page")]
        [Given(@"I select the ""([^""]*)"" dropdpwn in the organisation dropdown on the Case Feedback page")]
        public void GivenISelectTheDropdpwnInTheOrganisationDropdownOnTheCaseFeedbackPage(string type)
        {
            AdminCaseFeedbackPage.SelectType(type);
        }

        [Then(@"the case feedback details are displayed ""([^""]*)"" ""([^""]*)""")]
        public void ThenTheCaseFeedbackDetailsAreDisplayed(string Organisation, string Type)
        {
            AdminCaseFeedbackPage.verifyCaseDetailsCorrectlyDisplayed(Organisation, Type);
        }

        [Given(@"I select ""([^""]*)"" in the status dropdown")]
        public void GivenISelectInTheStatusDropdown(string statusOption)
        {
            AdminCaseFeedbackPage.SelectStatusOption(statusOption);
        }

        [When(@"I click the Change to viewed link")]
        public void WhenIClickTheChangeToViewedLink()
        {
            AdminCaseFeedbackPage.clickChangeViewedStatus();
        }

        [When(@"I select ""([^""]*)"" in the status dropdown")]
        public void WhenISelectInTheStatusDropdown(string statusOption)
        {
            AdminCaseFeedbackPage.SelectStatusOption(statusOption);
        }

        [Then(@"the case status will show ""([^""]*)""")]
        public void ThenTheCaseStatusWillShow(string Status)
        {
            AdminCaseFeedbackPage.verifyStatus(Status);
        }

        [Then(@"the case feedback page will show no results for the options selected")]
        public void ThenTheCaseFeedbackPageWillShowNoResultsForTheOptionsSelected()
        {
            AdminCaseFeedbackPage.VerifyNoCaseResultsShown();
        }


    }
}
