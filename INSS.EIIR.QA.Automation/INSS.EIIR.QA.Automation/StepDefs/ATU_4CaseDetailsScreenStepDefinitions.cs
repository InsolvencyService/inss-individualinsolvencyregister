using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_4CaseDetailsScreenStepDefinitions
    {
        [Then(@"the URL, page title and page heading will be displayed for the Case Details page for ""([^""]*)""")]
        public void ThenTheURLPageTitleAndPageHeadingWillBeDisplayedForTheCaseDetailsPageFor(string CaseName)
        {
            CaseDetailsPage.verifyCaseDetailsPage(CaseName);
        }

        [Then(@"the breadcrumb text will be as expected on the Case Details page")]
        public void ThenTheBreadcrumbTextWillBeAsExpectedOnTheCaseDetailsPage()
        {
            CaseDetailsPage.verifyBreadcrumbText();
        }

        [When(@"I click the Home breadcrumb on the Case Details page")]
        public void WhenIClickTheHomeBreadcrumbOnTheCaseDetailsPage()
        {
            CaseDetailsPage.ClickHomeBreadcrumb();
        }

        [Given(@"I click the Search the Individual Insolvency Register breadcrumb on the Case Details page")]
        public void GivenIClickTheSearchTheIndividualInsolvencyRegisterBreadcrumbOnTheCaseDetailsPage()
        {
            CaseDetailsPage.ClickSearchTheInsolvencyRegisterBreadcrumb();
        }

        [Given(@"I click the Search results breadcrumb on the Case Details page")]
        public void GivenIClickTheSearchResultsBreadcrumbOnTheCaseDetailsPage()
        {
            CaseDetailsPage.ClickSearchResultsBreadcrumb();
        }

        [Given(@"I navigate to the Search results page by searching for ""([^""]*)"" with Postcode ""([^""]*)""")]
        public void GivenINavigateToTheSearchResultsPageBySearchingForWithPostcode(string SearchTerm, string Postcode)
        {
            SearchPage.NavigateToSearchPage();
            SearchPage.EnterSearchText(SearchTerm);
            SearchPage.ClickSearchButton();
            SearchResultsPage.ClickIndividualLink(Postcode);
        }

        [Given(@"I click the Start new search button on the Case Details page")]
        public void GivenIClickTheStartNewSearchButtonOnTheCaseDetailsPage()
        {
            CaseDetailsPage.ClickStartNewSearchButton();
        }

        [Then(@"the Individual case details are displayed")]
        public void ThenTheIndividualDetailsAreDisplayed()
        {
            CaseDetailsPage.verifyIndividualDetails();
        }

        [Then(@"the breadcrumb text will be as expected on the Case Details page when coming to this page from the Case Feedback page")]
        public void ThenTheBreadcrumbTextWillBeAsExpectedOnTheCaseDetailsPageWhenComingToThisPageFromTheCaseFeedbackPage()
        {
            CaseDetailsPage.verifyBreadcrumbTextWhenComingFromCaseFeedbackPage();
        }



    }
}
