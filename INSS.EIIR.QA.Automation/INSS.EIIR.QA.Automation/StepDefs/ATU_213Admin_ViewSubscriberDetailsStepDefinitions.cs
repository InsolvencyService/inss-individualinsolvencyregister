using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Pages;


namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_213Admin_ViewSubscriberDetailsStepDefinitions : ElementHelper
    {

        [Then(@"I click the link for subscriber ""([^""]*)""")]
        public void ThenIClickTheLinkForSubscriber(string Subscriber)
        {
            AdminViewSubscriberListPage.ClickSubscriberLink(Subscriber);
          
        }

        [Then(@"the Subscriber Details page is displayed with the expected URL, page title and header")]
        public void ThenTheSubscriberDetailsPageIsDisplayedWithTheExpectedURLPageTitleAndHeader()
        {
            AdminSubscriberDetailsPage.verifySubscriberDetailsPage();
        }


        [Then(@"the warning for when the subscription will end is shown for subscriber ""([^""]*)""")]
        public void ThenTheWarningForWhenTheSubscriptionWillEndIsShownForSubscriber(string Subscriber)
        {
            AdminSubscriberDetailsPage.VerifySubscriptionEndsInXDaysValue(Subscriber);
        }


        [Then(@"the Subscriber Details page for ""([^""]*)"" is displayed with details for this subscriber")]
        public void ThenTheSubscriberDetailsPageForIsDisplayedWithDetailsForThisSubscriber(string Subscriber)
        {
            AdminSubscriberDetailsPage.VerifySubscribersDetails(Subscriber);
        }

        [Then(@"the correct email addressess are displayed for the subscriber ""([^""]*)""")]
        public void ThenTheCorrectEmailAddressessAreDisplayedForTheSubscriber(string subscriber)
        {
            AdminSubscriberDetailsPage.VerifySubscribersEmailAddresses(subscriber);
        }

        [When(@"I click the Return to admin area button")]
        public void WhenIClickTheReturnToAdminAreaButton()
        {
            AdminSubscriberDetailsPage.ClickReturnToAdminAreaButton(); ;
        }

        [When(@"I click the View another subscriber link")]
        public void WhenIClickTheViewAnotherSubscriberLink()
        {
            AdminSubscriberDetailsPage.ClickViewAnotherSubscriberLink();
        }

        [When(@"I click the Subscribers breadcrumb on the Subscriber details page")]
        public void WhenIClickTheSubscribersBreadcrumbOnTheSubscriberDetailsPage()
        {
            AdminSubscriberDetailsPage.clickSubscriberBreadcrumb();
        }

        [When(@"I click the Home breadcrumb on the Subscriber details page")]
        public void WhenIClickTheHomeBreadcrumbOnTheSubscriberDetailsPage()
        {
            AdminSubscriberDetailsPage.clickHomeBreadcrumb();
        }
    }
}
