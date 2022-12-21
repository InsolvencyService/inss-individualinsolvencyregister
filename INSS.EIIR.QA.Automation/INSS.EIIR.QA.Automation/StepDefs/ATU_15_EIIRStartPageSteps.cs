using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;



namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class ATU_15_EIIRStartPageSteps : ElementHelper
    {
        [Given(@"I navigate to the EIIR Start Page")]
        public void GivenINavigateToTheEIIRStartPage()
        {
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);
        }

        [Then(@"the EIIR Start page will be displayed and the URL, page title and H(.*) will be as per requirements")]
        public void ThenTheEIIRStartPageWillBeDisplayedAndTheURLPageTitleAndHWillBeAsPerRequirements(int p0)
        {
            EIIRHomePage.verifyEIIRHomePage();
        }


        [When(@"I click the ""([^""]*)"" link")]
        public void WhenIClickTheLink(string Link)
        {
            EIIRHomePage.ClickLink(Link);
        }

        [When(@"I click the '([^']*)' button")]
        public void WhenIClickTheButton(string p0)
        {
            EIIRHomePage.clickStartButton();
        }


        [Then(@"the following URL is displayed ""([^""]*)""")]
        public void ThenTheFollowingURLIsDisplayed(string URL)
        {
            EIIRHomePage.verifyPageURL(URL);
        }

        [Then(@"I am taken to the Search page")]
        public void ThenIAmTakenToTheSearchPage()
        {
            SearchPage.verifyEIIRSearchPage();
        }

    }
}
