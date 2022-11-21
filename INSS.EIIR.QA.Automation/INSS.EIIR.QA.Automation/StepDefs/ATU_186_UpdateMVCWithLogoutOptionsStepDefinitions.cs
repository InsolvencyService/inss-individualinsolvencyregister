using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_186_UpdateMVCWithLogoutOptionsStepDefinitions : ElementHelper
    {
        string adminLandingPageURL;
        string subscriberListPageURL;
        string subscriberDetailsPageURL;
        string updateSubscriberDetailsPageURL;

        [When(@"I click the Sign out link")]
        public void WhenIClickTheSignOutLink()
        {
            AdminLandingPage.clickSignOutLink();
        }

        [When(@"I attempt to access the Admin landing page using the URL")]
        public void WhenIAttemptToAccessTheAdminLandingPageUsingTheURL()
        {
            WebDriver.Navigate().GoToUrl(adminLandingPageURL);
        }

        [When(@"I attempt to access the subscriber list page using the URL")]
        public void WhenIAttemptToAccessTheSubscriberListPageUsingTheURL()
        {
            WebDriver.Navigate().GoToUrl(adminLandingPageURL);
        }

        [When(@"I attempt to access the subscriber details page using the URL")]
        public void WhenIAttemptToAccessTheSubscriberDetailsPageUsingTheURL()
        {
            WebDriver.Navigate().GoToUrl(subscriberDetailsPageURL);
        }

        [When(@"I attempt to access the update subscriber details page using the URL")]
        public void WhenIAttemptToAccessTheUpdateSubscriberDetailsPageUsingTheURL()
        {
            WebDriver.Navigate().GoToUrl(updateSubscriberDetailsPageURL);
        }

        [Given(@"I navigate to the update subscriber page and capture the URLs on the way so I can try and access them once logged out")]
        public void GivenINavigateToTheUpdateSubscriberPageAndCaptureTheURLsOnTheWaySoICanTryAndAccessThemOnceLoggedOut()
        {
            adminLandingPageURL = WebDriver.Url;
            AdminLandingPage.clickviewAndUpdateSubscribersLink();
            subscriberListPageURL = WebDriver.Url;
            AdminViewSubscriberListPage.ClickSubscriberLink("Insolvency Service Internal Account");
            subscriberDetailsPageURL = WebDriver.Url;
            AdminSubscriberDetailsPage.ClickChangeLink("Organisation Name");
            updateSubscriberDetailsPageURL = WebDriver.Url;   
        }

        [Then(@"I am navigated Admin Login page \(use this temporarily until the capital A is removed from the URL")]
        public void ThenIAmNavigatedAdminLoginPageUseThisTemporarilyUntilTheCapitalAIsRemovedFromTheURL()
        {
            AdminLoginPage.verifyAdminLoginPage1();
        }


    }
}
