using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;



namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class ATU_94_TermsAndConditionsPageSteps : ElementHelper
    {
        [Given(@"I navigate to the Terms and conditions page")]
        public void GivenINavigateToTheTermsAndConditionsPage()
        {
            EIIRHomePage.clickTermsAndConditionsLink();
        }

        [Then(@"the Terms and conditions page is displayed")]
        public void ThenTheTermsAndConditionsPageIsDisplayed()
        {
            TermsAndConditionsPage.verifyTermsAndConditionsPage();
        }

        [When(@"I click the Home breadcrumb on the T&C page")]
        public void WhenIClickTheHomeBreadcrumbOnTheTCPage()
        {
            TermsAndConditionsPage.clickHomeBreadcrumb();
        }

        [Then(@"I am navigated to the Home page of the EIIR service")]
        public void ThenIAmNavigatedToTheHomePageOfTheEIIRService()
        {
            EIIRHomePage.verifyEIIRHomePage();
        }

        [When(@"I click the tell the Insolvency Service link on the T&C page")]
        public void WhenIClickTheTellTheInsolvencyServiceLinkOnTheTCPage()
        {
            TermsAndConditionsPage.clickTellTheInsolvencyServiceLink();
        }

        [Then(@"I am navigated to the General Enquiry page")]
        public void ThenIAmNavigatedToTheGeneralEnquiryPage()
        {
            TermsAndConditionsPage.verifyGeneralEnquiryPage();
        }







    }
}
