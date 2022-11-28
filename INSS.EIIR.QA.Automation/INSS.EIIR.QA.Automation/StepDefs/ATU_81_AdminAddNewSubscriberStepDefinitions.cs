using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.Pages;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_81_AdminAddNewSubscriberStepDefinitions : ElementHelper
    {
        [When(@"I click the Home breadcrumb on the Add new subscriber page")]
        public void WhenIClickTheHomeBreadcrumbOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.clickHomeBreadcrumb();
        }


        [Then(@"the Add New Subscriber page is displayed with the expected URL, page title and header")]
        public void ThenTheAddNewSubscriberPageIsDisplayedWithTheExpectedURLPageTitleAndHeader()
        {
           AdminAddNewSubscriber.verifyAddNewSubscriberPage();
        }

        [Then(@"the status is set to Active by default")]
        public void ThenTheStatusIsSetToActiveByDefault()
        {
            AdminAddNewSubscriber.verifyDefaultStatus();

        }




        [Given(@"I click the Add New Subscriber link")]
        public void GivenIClickTheAddNewSubscriberLink()
        {
            AdminLandingPage.clickaddNewSubscriberLink();
        }

        [Given(@"I enter new subscriber details in the text fields")]
        public void GivenIEnterNewSubscriberDetailsInTheTextFields()
        {
            AdminAddNewSubscriber.EnterNewSubscriberDetails();
        }

        [When(@"I press the Save and continue button")]
        [Given(@"I press the Save and continue button")]
        public void GivenIPressTheSaveAndContinueButton()
        {
            AdminAddNewSubscriber.ClickSaveAndContinueButton();
        }

        [Given(@"I enter (.*) email addresses")]
        public void GivenIEnterEmailAddresses(int emailAddresses)
        {
            AdminAddNewSubscriber.EnterEmailAddresses(emailAddresses);
        }

        [Given(@"I populate the subscription start date to be later than the subscription end date")]
        public void GivenIPopulateTheSubscriptionStartDateToBeLaterThanTheSubscriptionEndDate()
        {
            AdminAddNewSubscriber.EnterSubscriptionStartDateLaterThanSubscriptionEndDate();
        }


        [Given(@"I enter the application date, start date and end date")]
        public void GivenIEnterTheApplicationDateStartDateAndEndDate()
        {
            AdminAddNewSubscriber.EnterDates();
        }

        [When(@"I navigate to the Subscriber List page and I search for my new subscriber ""([^""]*)""")]
        public void WhenINavigateToTheSubscriberListPageAndISearchForMyNewSubscriber(string Subscriber)
        {
            AdminLandingPage.clickviewAndUpdateSubscribersLink();
            AdminViewSubscriberListPage.ClickSubscriberLink(Subscriber);
        }

        [Then(@"the new subscription for subscriber ""([^""]*)"" is deleted in readiness for the next test run")]
        public void ThenTheNewSubscriptionForSubscriberIsDeletedInReadinessForTheNextTestRun(string Subscriber)
        {
            SqlQueries.DeleteSubscriber(Subscriber);
        }

        [Given(@"I enter the following invalid ""([^""]*)""")]
        public void GivenIEnterTheFollowingInvalid(string Postcode)
        {
            AdminAddNewSubscriber.EnterInvalidPostcode(Postcode);
        }

        [Then(@"the user is shown the following error message ""([^""]*)"" on Add new subscriber page")]
        public void ThenTheUserIsShownTheFollowingErrorMessageOnAddNewSubscriberPage(string errorMessage)
        {
            AdminAddNewSubscriber.ValidateTextFieldErrorMessage(errorMessage);
        }

        [Given(@"I first populate all of the fields on the Add new subscriber page")]
        public void GivenIFirstPopulateAllOfTheFieldsOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.EnterNewSubscriberDetails();
            AdminAddNewSubscriber.EnterEmailAddresses(3);
            AdminAddNewSubscriber.EnterDates();
        }

        [When(@"I clear the ""([^""]*)"" field on the Add subscriber page")]
        public void WhenIClearTheFieldOnTheAddSubscriberPage(string TextField)
        {
            AdminAddNewSubscriber.ClearField(TextField);
        }

        [When(@"I update the Add new subscriber page Application Date to have a blank Day, Month and Year")]
        public void WhenIUpdateTheAddNewSubscriberPageApplicationDateToHaveABlankDayMonthAndYear()
        {
            AdminAddNewSubscriber.ClearApplicationDate();       
        }

        [Then(@"the user is shown error messages stating the Day, Month and Year are missing on the Add new subscriber page")]
        public void ThenTheUserIsShownErrorMessagesStatingTheDayMonthAndYearAreMissingOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.verifyNullApplicationDateErrorMessages();
        }

        [When(@"I enter non numeric details in to the Application submitted date fields on the Add new subscriber page")]
        public void WhenIEnterNonNumericDetailsInToTheApplicationSubmittedDateFieldsOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.EnterNonNumericApplicationDate();
        }

        [Then(@"the user is shown error messages stating which numeric values are acceptable in the application submitted date on the Add new subscriber page")]
        public void ThenTheUserIsShownErrorMessagesStatingWhichNumericValuesAreAcceptableInTheApplicationSubmittedDateOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.verifyNonNumericApplicationDateErrorMessages();
        }

        [When(@"I enter an invalid date in to the on the Add new subscriber page Application submitted date fields such as ""([^""]*)""")]
        public void WhenIEnterAnInvalidDateInToTheOnTheAddNewSubscriberPageApplicationSubmittedDateFieldsSuchAs(string p0)
        {
            AdminAddNewSubscriber.EnterInvalidApplicationDate();
        }

        [Then(@"the user is shown error messages stating the Add new subscriber page, application start date entered must be a real date")]
        public void ThenTheUserIsShownErrorMessagesStatingTheAddNewSubscriberPageApplicationStartDateEnteredMustBeARealDate()
        {
            AdminAddNewSubscriber.verifyInvalidApplicationDateErrorMessage();
        }

        [When(@"I enter an invalid year in to the Add new subscriber page, Application submitted date fields for example ""([^""]*)""")]
        public void WhenIEnterAnInvalidYearInToTheAddNewSubscriberPageApplicationSubmittedDateFieldsForExample(string p0)
        {
            AdminAddNewSubscriber.EnterInvalidApplicationDateYear();
        }

        [Then(@"the user is shown an error message stating ""([^""]*)"" on the Add new subscriber page")]
        public void ThenTheUserIsShownAnErrorMessageStatingOnTheAddNewSubscriberPage(string p0)
        {
            AdminAddNewSubscriber.verifyInvalidApplicationDateYearErrorMessage();
        }

        [Then(@"the user is shown an error message stating the subscription end date must be later than the subscription start date on the add new subscriber page")]
        public void ThenTheUserIsShownAnErrorMessageStatingTheSubscriptionEndDateMustBeLaterThanTheSubscriptionStartDateOnTheAddNewSubscriberPage()
        {
            AdminAddNewSubscriber.verifySubStartDateLaterThanEndDateErrorMessage();
        }


    }
}

