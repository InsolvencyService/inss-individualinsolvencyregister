using System;
using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.TestFramework.Helpers;
using INSS.EIIR.QA.Automation.TestFramework.TestSupport;
using INSS.EIIR.QA.Automation.TestFramework.Tests.Pages;


namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU15_Citizen_ViewInitialSearchStartScreenStepDefinitions : ElementHelper
    {
        [Given(@"I navigate to the EIIR home page")]
        public void GivenINavigateToTheEIIRHomePage()
        {
            NavigateTo(Constants.EIIRBaseUrl);
        }

        [Then(@"the header and page title will match the expected values")]
        public void ThenTheHeaderAndPageTitleWillMatchTheExpectedValues()
        {
            EIIRHomePage.VerifyEIIRHomePage();
        }
    }
}
