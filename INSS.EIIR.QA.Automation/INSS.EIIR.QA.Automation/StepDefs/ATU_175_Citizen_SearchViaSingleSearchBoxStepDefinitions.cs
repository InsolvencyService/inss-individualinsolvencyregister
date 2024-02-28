using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using TechTalk.SpecFlow;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_175_Citizen_SearchViaSingleSearchBoxStepDefinitions
    {
        [Given(@"I navigate to the Citizen Search page")]
        public void GivenINavigateToTheCitizenSearchPage()
        {
            SearchPage.NavigateToSearchPage();
        }

        [Then(@"the URL, page title and page heading will be as per the requirements")]
        public void ThenTheURLPageTitleAndPageHeadingWillBeAsPerTheRequirements()
        {
            SearchPage.verifyEIIRSearchPage();
        }

        [When(@"I click the Home breadcrumb on the Citizen Search page")]
        public void WhenIClickTheHomeBreadcrumbOnTheCitizenSearchPage()
        {
            SearchPage.ClickHomeBreadcrumb();
        }

        [When(@"I click the Search button without entering any text")]
        public void WhenIClickTheSearchButtonWithoutEnteringAnyText()
        {
            SearchPage.ClickSearchButton();
        }

        [Then(@"I am shown an error message stating a name or trading name must be entered")]
        public void ThenIAmShownAnErrorMessageStatingANameOrTradingNameMustBeEntered()
        {
            SearchPage.VerifyErrorMessage();
        }

        [When(@"I enter a search term which returns no results")]
        public void WhenIEnterASearchTermWhichReturnsNoResults()
        {
            SearchPage.EnterSearchText("This search term will return nothing");
            SearchPage.ClickSearchButton();
        }

        [Then(@"I am shown a message stating no results have been returned")]
        public void ThenIAmShownAMessageStatingNoResultsHaveBeenReturned()
        {
            SearchPage.ValidateNoResultsReturnedPage();
        }




    }
}
