using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_333_SearchResultsPageStepDefinitions : ElementHelper
    {

        [Given(@"I navigate to the Search results page by searching for ""([^""]*)""")]
        public void GivenINavigateToTheSearchResultsPageBySearchingFor(string SearchTerm)
        {
            SearchPage.NavigateToSearchPage();
            SearchPage.EnterSearchText(SearchTerm);
            SearchPage.ClickSearchButton();
        }

        [Then(@"the URL, page title and page heading will be displayed for the Search results page")]
        public void ThenTheURLPageTitleAndPageHeadingWillBeDisplayedForTheSearchResultsPage()
        {
            SearchResultsPage.verifyEIIRSearchResultPage();
        }

        [Then(@"I click the Search the Individual Insolvency Register breadcrumb")]
        public void ThenIClickTheSearchTheIndividualInsolvencyRegisterBreadcrumb()
        {
            SearchResultsPage.ClickSearchTheInsolvencyRegisterBreadcrumb();
        }

        [Given(@"I click the Home breadcrumb on the Search results page")]
        public void GivenIClickTheHomeBreadcrumbOnTheSearchResultsPage()
        {
            SearchResultsPage.ClickHomeBreadcrumb();
        }

        [Then(@"the Search page for EIIR will be displayed")]
        public void ThenTheSearchPageForEIIRWillBeDisplayed()
        {
            SearchPage.verifyEIIRSearchPage();
        }

        [Then(@"the breadcrumb text will be as expected")]
        public void ThenTheBreadcrumbTextWillBeAsExpected()
        {
            SearchResultsPage.verifyBreadcrumbText();
        }

        [When(@"I click the individual link with postcode ""([^""]*)""")]
        public void WhenIClickTheIndividualLinkWithPostcode(string Postcode)
        {
            SearchResultsPage.ClickIndividualLink(Postcode);
        }

        [Given(@"I click the Tell the Insolvency Service link")]
        public void GivenIClickTheTellTheInsolvencyServiceLink()
        {
            SearchResultsPage.ClickTellTheInsolvencyLink();
        }
    }
}
