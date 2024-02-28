using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Pages;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace INSS.EIIR.QA.Automation
{
    [Binding]
    public class ATU_84_UpdateSubscriberDetailsPageStepDefinitions : ElementHelper
    {
        [When(@"I click on the Subscriber details change link for ""([^""]*)""")]
        public void WhenIClickOnTheSubscriberDetailsChangeLinkFor(string ChangeLink)
        {
            AdminSubscriberDetailsPage.ClickChangeLink(ChangeLink);
        }

        [Then(@"the Update Subscriber Details page is displayed with the expected URL, page title and header")]
        public void ThenTheUpdateSubscriberDetailsPageIsDisplayedWithTheExpectedURLPageTitleAndHeader()
        {
            AdminUpdateSubscriberDetailsPage.verifyUpdateSubscriberDetailsPage();
        }

        [Then(@"I update the Application Date to have a blank Day, Month and Year")]
        public void ThenIUpdateTheApplicationDateToHaveABlankDayMonthAndYear()
        {
            AdminUpdateSubscriberDetailsPage.ClearApplicationDate();
        }

        [When(@"I press the Save and return to subscriber button")]
        [Then(@"I press the Save and return to subscriber button")]
        public void ThenIPressTheSaveAndReturnToSubscriberButton()
        {
            AdminUpdateSubscriberDetailsPage.ClickSaveAndReturnToSubscriberButton();
        }

        [Then(@"the user is shown error messages stating the Day, Month and Year are missing")]
        public void ThenTheUserIsShownErrorMessagesStatingTheDayMonthAndYearAreMissing()
        {
            AdminUpdateSubscriberDetailsPage.verifyNullApplicationDateErrorMessages();
        }

        [When(@"I clear the ""([^""]*)"" field")]
        public void WhenIClearTheField(string fieldName)
        {
            AdminUpdateSubscriberDetailsPage.ClearField(fieldName);
        }



        [Then(@"the user is shown the following error message ""([^""]*)""")]
        public void ThenTheUserIsShownTheFollowingErrorMessage(string errorMessage)
        {
            AdminUpdateSubscriberDetailsPage.ValidateTextFieldErrorMessage(errorMessage);
        }
       

        [When(@"I enter non numeric details in to the Application submitted date fields")]
        public void WhenIEnterNonNumericDetailsInToTheApplicationSubmittedDateFields()
        {
            AdminUpdateSubscriberDetailsPage.EnterNonNumericApplicationDate();
        }

        [Then(@"the user is shown error messages stating which numeric values are acceptable in the application submitted date")]
        public void ThenTheUserIsShownErrorMessagesStatingWhichNumericValuesAreAcceptableInTheApplicationSubmittedDate()
        {
            AdminUpdateSubscriberDetailsPage.verifyNonNumericApplicationDateErrorMessages();
        }

        [When(@"I enter an invalid date in to the Application submitted date fields such as ""([^""]*)""")]
        public void WhenIEnterAnInvalidDateInToTheApplicationSubmittedDateFieldsSuchAs(string p0)
        {
            AdminUpdateSubscriberDetailsPage.EnterInvalidApplicationDate();
        }


        [Then(@"the user is shown error messages stating the application start date entered must be a real date")]
        public void ThenTheUserIsShownErrorMessagesStatingTheApplicationStartDateEnteredMustBeARealDate()
        {
            AdminUpdateSubscriberDetailsPage.verifyInvalidApplicationDateErrorMessage();
        }

        [When(@"I enter an invalid year in to the Application submitted date fields for example ""([^""]*)""")]
        public void WhenIEnterAnInvalidYearInToTheApplicationSubmittedDateFieldsForExample(string p0)
        {
            AdminUpdateSubscriberDetailsPage.EnterInvalidApplicationDateYear();
        }


        [Then(@"the user is shown error messages stating ""([^""]*)""")]
        public void ThenTheUserIsShownErrorMessagesStating(string p0)
        {
            AdminUpdateSubscriberDetailsPage.verifyInvalidApplicationDateYearErrorMessage();
        }

        [When(@"I enter the following invalid ""([^""]*)""")]
        public void WhenIEnterTheFollowingInvalid(string postcode)
        {
            AdminUpdateSubscriberDetailsPage.EnterInvalidPostcode(postcode);
        }

        [When(@"I update the subscriber details using the ""([^""]*)""")]
        public void WhenIUpdateTheSubscriberDetailsUsingThe(string values)
        {
            AdminUpdateSubscriberDetailsPage.UpdateSubscriberDetails(values);
        }

        [Then(@"the Subscriber Details page for ""([^""]*)"" is displayed with the updated details for this subscriber")]
        public void ThenTheSubscriberDetailsPageForIsDisplayedWithTheUpdatedDetailsForThisSubscriber(string subscriber)
        {
            AdminSubscriberDetailsPage.VerifyUpdatedSubscribersDetails();
           
        }

        [Then(@"the updated email addressess for (.*) are displayed for the subscriber")]
        public void ThenTheUpdatedEmailAddressessForAreDisplayedForTheSubscriber(int NumberOfEmailAddresses)
        {
            AdminSubscriberDetailsPage.VerifySubscribersUpdatedEmailAddresses1(NumberOfEmailAddresses);
        }


        [Given(@"I click the Home breadcrumb on the Update subscriber details page")]
        public void GivenIClickTheHomeBreadcrumbOnTheUpdateSubscriberDetailsPage()
        {
            AdminUpdateSubscriberDetailsPage.clickHomeBreadcrumb();
        }


        [Given(@"I click the Subscribers breadcrumb on the Update subscriber details page")]
        public void GivenIClickTheSubscribersBreadcrumbOnTheUpdateSubscriberDetailsPage()
        {
            AdminUpdateSubscriberDetailsPage.clickSubscriberBreadcrumb();
        }

        [When(@"I click the Subscriber details breadcrumb on the Update subscriber details page")]
        public void WhenIClickTheSubscriberDetailsBreadcrumbOnTheUpdateSubscriberDetailsPage()
        {
            AdminUpdateSubscriberDetailsPage.clickSubscriberDetailsBreadcrumb();
        }

        [Given(@"I navigate back to the Update subscriber details page")]
        public void GivenINavigateBackToTheUpdateSubscriberDetailsPage()
        {
            AdminSubscriberDetailsPage.ClickChangeLink("Organisation Name");
        }

        [Given(@"I navigate back to the Update subscriber details page from the Subscriber List page")]
        public void GivenINavigateBackToTheUpdateSubscriberDetailsPageFromTheSubscriberListPage()
        {
            AdminViewSubscriberListPage.ClickSubscriberLink("Insolvency Service Internal Account");
            AdminSubscriberDetailsPage.ClickChangeLink("Organisation Name");
        }


        [When(@"I update (.*) email addresses on the update subscriber details")]
        public void WhenIUpdateEmailAddressesOnTheUpdateSubscriberDetails(int NumberOfEmailAddresses)
        {
            AdminUpdateSubscriberDetailsPage.EnterEmailAddresses(NumberOfEmailAddresses);
        }

        [When(@"I update (.*) email addresses on the update subscriber details with uplicate email addresses")]
        public void WhenIUpdateEmailAddressesOnTheUpdateSubscriberDetailsWithUplicateEmailAddresses(int NumberOfEmailAddresses)
        {
            AdminUpdateSubscriberDetailsPage.EnterDuplicateEmailAddresses(NumberOfEmailAddresses);
        }


        [When(@"I populate the subscription start date to be later than the subscription end date on the update subscriber details page")]
        public void WhenIPopulateTheSubscriptionStartDateToBeLaterThanTheSubscriptionEndDateOnTheUpdateSubscriberDetailsPage()
        {
            AdminUpdateSubscriberDetailsPage.EnterSubscriptionStartDateLaterThanSubscriptionEndDate();
        }

        [Then(@"the user is shown an error message stating the subscription end date must be later than the subscription start date on the update subscriber page")]
        public void ThenTheUserIsShownAnErrorMessageStatingTheSubscriptionEndDateMustBeLaterThanTheSubscriptionStartDateOnTheUpdateSubscriberPage()
        {
            AdminUpdateSubscriberDetailsPage.verifySubStartDateLaterThanEndDateErrorMessage();
        }

        [When(@"I update the ""([^""]*)"" Data extract email address with ""([^""]*)""")]
        public void WhenIUpdateTheDataExtractEmailAddressWith(string EmailField, string EmailAddress)
        {
            AdminUpdateSubscriberDetailsPage.EnterInvalidDataExtractEmailAddresses(EmailField, EmailAddress);
        }

        [Then(@"the user is shown the following error message for invalid data extract email address ""([^""]*)""")]
        public void ThenTheUserIsShownTheFollowingErrorMessageForInvalidDataExtractEmailAddress(string ErrorMessage)
        {
            AdminUpdateSubscriberDetailsPage.ValidateTextFieldErrorMessage(ErrorMessage);
        }


    }
}
