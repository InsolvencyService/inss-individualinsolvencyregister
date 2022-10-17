using System;
using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.TestFramework.Hooks;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class Feature1StepDefinitions : Base
    {
        [Given(@"I Navigate to Amazon")]
        public void GivenINavigateToAmazon()
        {
            string baseURL = "https://Amazon.co.uk/";
            WebDriver.Navigate().GoToUrl(baseURL);
        }

        [When(@"I click on Login")]
        public void WhenIClickOnLogin()
        {
            //
        }

        [Then(@"I'll be asked to login")]
        public void ThenIllBeAskedToLogin()
        {
            //
        }
    }
}
