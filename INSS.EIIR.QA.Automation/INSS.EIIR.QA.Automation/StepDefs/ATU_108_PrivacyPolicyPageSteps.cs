using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;

namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class ATU_108_PrivacyPolicyPageSteps : ElementHelper
    {
        [Given(@"I navigate to the Privacy policy page")]
        public void GivenINavigateToThePrivacyPolicyPage()
        {
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);
            EIIRHomePage.ClickLink("Privacy - footer");
        }


        [Then(@"the Privacy policy page is displayed with the correct URL, page title and header")]
        public void ThenThePrivacyPolicyPageIsDisplayedWithTheCorrectURLPageTitleAndHeader()
        {
            PrivacyPolicyPage.verifyPrivacyPage();
        }

        [Given(@"I click the ""([^""]*)"" breadcrumb on the Privacy policy page")]
        [Given(@"I click the ""([^""]*)"" link on the Privacy policy page")]
        public void GivenIClickTheLinkOnThePrivacyPolicyPage(string Link)
        {
            PrivacyPolicyPage.ClickLink(Link);
        }

    }
}
