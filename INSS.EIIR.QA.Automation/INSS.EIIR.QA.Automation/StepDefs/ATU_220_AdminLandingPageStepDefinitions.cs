using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;


namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_220_AdminLandingPageStepDefinitions : ElementHelper
    {
        [Given(@"I login as an admin user and navigate to the Admin landing page")]
        public void GivenILoginAsAnAdminUserAndNavigateToTheAdminLandingPage()
        {
            AdminLandingPage.NavigateToAdminLandingPage();
        }

        [Then(@"the Admin landing page will be displayed and the URL, page title and H(.*) will be as per requirements")]
        public void ThenTheAdminLandingPageWillBeDisplayedAndTheURLPageTitleAndHWillBeAsPerRequirements(int p0)
        {
            AdminLandingPage.verifyAdminLandingPage();
        }

        [Then(@"I am navigated to the Admin landing page")]
        public void ThenIAmNavigatedToTheAdminLandingPage()
        {
            AdminLandingPage.verifyAdminLandingPage();
        }

    }
}
