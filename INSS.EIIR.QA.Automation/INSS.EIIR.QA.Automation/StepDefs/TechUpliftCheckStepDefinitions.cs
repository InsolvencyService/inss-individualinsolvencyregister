using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.StepDefs;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using NUnit.Framework;


namespace INSS.EIIR.QA.Automation.StepDefs
{
    [Binding]
    public class TechUpliftCheckStepDefinitions : ElementHelper
    {
     

        [Given(@"I navigate to Amazon Login page")]
        public void GivenINavigateToAmazonLoginPage()
        {           
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);            
        }

        [Given(@"I navigate to the EIIR Home Page")]
        public void GivenINavigateToTheEIIRHomePage()
        {
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);
        }

        [Then(@"the URL will be '([^']*)'")]
        public void ThenTheURLWillBe(string expectedURL)
        {
            EIIRHomePage.verifyEIIRHomePage();
        }

        [Given(@"SOmething Happens")]
        public void GivenSOmethingHappens()
        {
           //
        }

        [Then(@"again something happens")]
        public void ThenAgainSomethingHappens()
        {
            //
        }




    }
}
