using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;



namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class ATU_53_AccessibilityStatementPageSteps : ElementHelper
    {
        [When(@"I click the ""([^""]*)"" link on the Accessibility Statement page")]
        public void WhenIClickTheLinkOnTheAccessibilityStatementPage(string Link)
        {
            AccessibilityStatementPage.ClickLink(Link);
        }

        [Then(@"the Accessibility statement page will be displayed and the URL, page title and H(.*) will be as per requirements")]
        public void ThenTheAccessibilityStatementPageWillBeDisplayedAndTheURLPageTitleAndHWillBeAsPerRequirements(int p0)
        {
            AccessibilityStatementPage.verifyAccessibilityStatementPage();
        }

        [Given(@"I navigate to the Accessibility Statement Page")]
        public void GivenINavigateToTheAccessibilityStatementPage()
        {
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);
            EIIRHomePage.ClickLink("Accessibility statement - footer");
        }
    }
}
