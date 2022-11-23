using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using System.Collections.Generic;
using INSS.EIIR.QA.Automation.Data;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminUpdateSubscriberDetailsPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = "https://app-uksouth-sit-eiir-web.azurewebsites.net/admin/subscriber";
        private static string expectedPageTitle { get; } = "Update subscriber details - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Update subscriber details";
        private static By pageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By subscribersBreadcrumb { get; } = By.LinkText("Subscribers");
        private static By subscriberDetailsBreadcrumb { get; } = By.LinkText("Subscriber details");
        private static By saveButton { get; } = By.ClassName("govuk-button");
        private static By organisationNameTextBoxElement { get; } = By.Name("OrganisationName");
        private static By tradingTypeDropdownElement { get; } = By.Id("OrganisationType");
        private static By firstNameTextBoxElement { get; } = By.Name("ContactForename");
        private static By surnameTextBoxElement { get; } = By.Name("ContactSurname");
        private static By addressLine1TextBoxElement { get; } = By.Name("ContactAddress1");
        private static By addressLine2TextBoxElement { get; } = By.Name("ContactAddress2");
        private static By addressCityTextBoxElement { get; } = By.Name("ContactCity");
        private static By addressPostcodeTextBoxElement { get; } = By.Name("ContactPostcode");
        private static By emailAddressTextBoxElement { get; } = By.Name("ContactEmail");
        private static By telephoneNumberTextBoxElement { get; } = By.Id("ContactTelephone");
        private static By applicationSubmittedDayTextBoxElement { get; } = By.Name("ApplicationDay");
        private static By applicationSubmittedMonthTextBoxElement { get; } = By.Name("ApplicationMonth");
        private static By applicationSubmittedYearTextBoxElement { get; } = By.Name("ApplicationYear");
        private static By startDateDayTextBoxElement { get; } = By.Name("SubscribedFromDay");
        private static By startDateMonthTextBoxElement { get; } = By.Name("SubscribedFromMonth");
        private static By startDateYearTextBoxElement { get; } = By.Name("SubscribedFromYear");
        private static By endDateDayTextBoxElement { get; } = By.Name("SubscribedToDay");
        private static By endDateMonthTextBoxElement { get; } = By.Name("SubscribedToMonth");
        private static By endDateYearTextBoxElement { get; } = By.Name("SubscribedToYear");
        private static By statusActiveRadioElement { get; } = By.XPath("(//*[@type='radio'])[1]");
        private static By statusInactiveRadioElement { get; } = By.XPath("(//*[@type='radio'])[2]");
        private static By emailAddress1Element { get; } = By.Id("EmailAddress1");
        private static By emailAddress2Element { get; } = By.Id("EmailAddress2");
        private static By emailAddress3Element { get; } = By.Id("EmailAddress3");

        private static List<object[]> result;



        public static void verifyUpdateSubscriberDetailsPage()
        {
            //uncomment the following assertions once the URL and page title defect is resolved.

             Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
             Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(pageHeaderElement).Text);
        }

        public static void clickHomeBreadcrumb()
        {
            ClickElement(homeBreadcrumb);
        }

        public static void clickSubscriberBreadcrumb()
        {
            ClickElement(subscribersBreadcrumb);
        }
        public static void clickSubscriberDetailsBreadcrumb()
        {
            ClickElement(subscriberDetailsBreadcrumb);
        }

        public static void ClickSaveAndReturnToSubscriberButton()
        {
            ClickElement(saveButton);
        }

        public static void ClearApplicationDate()
        {
            ClearText(applicationSubmittedDayTextBoxElement);
            ClearText(applicationSubmittedMonthTextBoxElement);
            ClearText(applicationSubmittedYearTextBoxElement);
        }

        public static void EnterNonNumericApplicationDate()
        {
            ClearText(applicationSubmittedDayTextBoxElement);
            ClearText(applicationSubmittedMonthTextBoxElement);
            ClearText(applicationSubmittedYearTextBoxElement);
            EnterText(applicationSubmittedDayTextBoxElement,"ABC");
            EnterText(applicationSubmittedMonthTextBoxElement,"CDE");
            EnterText(applicationSubmittedYearTextBoxElement,"EFG");
        }

        public static void EnterInvalidApplicationDate()
        {
            ClearText(applicationSubmittedDayTextBoxElement);
            ClearText(applicationSubmittedMonthTextBoxElement);
            ClearText(applicationSubmittedYearTextBoxElement);
            EnterText(applicationSubmittedDayTextBoxElement, "30");
            EnterText(applicationSubmittedMonthTextBoxElement, "2");
            EnterText(applicationSubmittedYearTextBoxElement, "2025");
        }

        public static void EnterInvalidApplicationDateYear()
        {
            ClearText(applicationSubmittedDayTextBoxElement);
            ClearText(applicationSubmittedMonthTextBoxElement);
            ClearText(applicationSubmittedYearTextBoxElement);
            EnterText(applicationSubmittedDayTextBoxElement, "12");
            EnterText(applicationSubmittedMonthTextBoxElement, "2");
            EnterText(applicationSubmittedYearTextBoxElement, "100");
        }

        public static void EnterInvalidPostcode(string postcode)
        {
            ClearText(addressPostcodeTextBoxElement);
            EnterText(addressPostcodeTextBoxElement, postcode);
        }
        public static void verifyInvalidApplicationDateYearErrorMessage()
        {
            string ApplicationDayErrorMesage = "Enter a year between 1900 and 3000";

            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationYear']")).Text);
            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][1]")).Text);
        }

        public static void verifyInvalidApplicationDateErrorMessage()
        {
            string ApplicationDayErrorMesage = "The application submitted date must be a real date";

            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationDate']")).Text);
            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][1]")).Text);
        }

        public static void verifyNonNumericApplicationDateErrorMessages()
        {
            string ApplicationDayErrorMesage = "Enter a day between 1 and 31";
            string ApplicationMonthErrorMesage = "Enter a month between 1 and 12";
            string ApplicationYearErrorMesage = "Enter a year between 1900 and 3000";

            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationDay']")).Text);
            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][2]")).Text);

            Assert.AreEqual(ApplicationMonthErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationMonth']")).Text);
            Assert.AreEqual(ApplicationMonthErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][3]")).Text);

            Assert.AreEqual(ApplicationYearErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationYear']")).Text);
            Assert.AreEqual(ApplicationYearErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][4]")).Text);
        }
        public static void verifyNullApplicationDateErrorMessages()
        {
            string ApplicationDayErrorMesage = "The application submitted date must include a day";
            string ApplicationMonthErrorMesage = "The application submitted date must include a month";
            string ApplicationYearErrorMesage = "The application submitted date must include a year";

            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationDay']")).Text);
            Assert.AreEqual(ApplicationDayErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][2]")).Text);

            Assert.AreEqual(ApplicationMonthErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationMonth']")).Text);
            Assert.AreEqual(ApplicationMonthErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][3]")).Text);

            Assert.AreEqual(ApplicationYearErrorMesage, WebDriver.FindElement(By.XPath("//a[@href='#ApplicationYear']")).Text);
            Assert.AreEqual(ApplicationYearErrorMesage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error'][4]")).Text);
        }

        public static void ClearField(string fieldName)
        {
            switch (fieldName)
            {
                case "Organisation Name":
                    ClearText(organisationNameTextBoxElement);
                    break;
                case "Forename":
                    ClearText(firstNameTextBoxElement);
                    break;
                case "Surname":
                    ClearText(surnameTextBoxElement);
                    break;
                case "Address line 1":
                    ClearText(addressLine1TextBoxElement);
                    break;
                case "Town or city":
                    ClearText(addressCityTextBoxElement);
                    break;
                case "Postcode":
                    ClearText(addressPostcodeTextBoxElement);
                    break;
                case "Email address":
                    ClearText(emailAddressTextBoxElement);
                    break;
                case "Telephone number":
                    ClearText(telephoneNumberTextBoxElement);
                    break;
                case "Email address 1":
                    ClearText(emailAddress1Element);
                    break;
            }           
        }

        public static void ValidateTextFieldErrorMessage(string errorMessage)
        {
            Assert.AreEqual(errorMessage, WebDriver.FindElement(By.XPath("//div/ul/li/a")).Text);
            Assert.AreEqual(errorMessage, WebDriver.FindElement(By.XPath("//*[@id='organisationName-error']")).Text);       
        }

        //This may be redundant code - please review and delete if needed
        public static void UpdateSubscriberDetails()
        {
            ClearText(applicationSubmittedDayTextBoxElement);
            ClearText(applicationSubmittedMonthTextBoxElement);
            ClearText(applicationSubmittedYearTextBoxElement);
            EnterText(applicationSubmittedDayTextBoxElement, "12");
            EnterText(applicationSubmittedMonthTextBoxElement, "2");
            EnterText(applicationSubmittedYearTextBoxElement, "100");
        }

        public static void UpdateSubscriberDetails(string values)
        {
            if (values == "Update values")
            {
                ClearText(organisationNameTextBoxElement);
                EnterText(organisationNameTextBoxElement, Constants.UpdatedOrganisationName);
                ClearText(firstNameTextBoxElement);
                EnterText(firstNameTextBoxElement, Constants.UpdatedFirstName);
                ClearText(surnameTextBoxElement);
                EnterText(surnameTextBoxElement, Constants.UpdatedSurname);
                ClearText(addressLine1TextBoxElement);
                EnterText(addressLine1TextBoxElement, Constants.UpdatedAddressLine1);
                ClearText(addressLine2TextBoxElement);
                EnterText(addressLine2TextBoxElement, Constants.UpdatedAddressLine2);
                ClearText(addressCityTextBoxElement);
                EnterText(addressCityTextBoxElement, Constants.UpdatedTown);
                ClearText(addressPostcodeTextBoxElement);
                EnterText(addressPostcodeTextBoxElement, Constants.UpdatedPostcode);
                ClearText(emailAddressTextBoxElement);
                EnterText(emailAddressTextBoxElement, Constants.UpdatedEmailAddress);
                ClearText(telephoneNumberTextBoxElement);
                EnterText(telephoneNumberTextBoxElement, Constants.UpdatedTelephoneNumber);
                SelectFromDropDownByText(tradingTypeDropdownElement, Constants.UpdatedType);
                ClickElement(statusInactiveRadioElement);
            }
            else
            {
                ClearText(organisationNameTextBoxElement);
                EnterText(organisationNameTextBoxElement, Constants.organisationName);
                ClearText(firstNameTextBoxElement);
                EnterText(firstNameTextBoxElement, Constants.firstName);
                ClearText(surnameTextBoxElement);
                EnterText(surnameTextBoxElement, Constants.surname);
                ClearText(addressLine1TextBoxElement);
                EnterText(addressLine1TextBoxElement, Constants.addressLine1);
                ClearText(addressLine2TextBoxElement);
                EnterText(addressLine2TextBoxElement, Constants.addressLine2);
                ClearText(addressCityTextBoxElement);
                EnterText(addressCityTextBoxElement, Constants.town);
                ClearText(addressPostcodeTextBoxElement);
                EnterText(addressPostcodeTextBoxElement, Constants.postcode);
                ClearText(emailAddressTextBoxElement);
                EnterText(emailAddressTextBoxElement, Constants.emailAddress);
                ClearText(telephoneNumberTextBoxElement);
                EnterText(telephoneNumberTextBoxElement, Constants.telephoneNumber);
                ClearText(emailAddress1Element);
                EnterText(emailAddress1Element, Constants.dataExtractEmail1);
                ClearText(emailAddress2Element);
                EnterText(emailAddress2Element, Constants.dataExtractEmail2);
                ClearText(emailAddress3Element);
                EnterText(emailAddress3Element, Constants.dataExtractEmail3);
                ClickElement(statusActiveRadioElement);
                SelectFromDropDownByText(tradingTypeDropdownElement, Constants.type);
                ClickSaveAndReturnToSubscriberButton();
            }
        }

        public static void EnterEmailAddresses(int emailAddresses)
        {
            if (emailAddresses == 3)
            {
                ClearText(emailAddress1Element);
                EnterText(emailAddress1Element, Constants.UpdatedDataExtractEmail1);
                ClearText(emailAddress2Element);
                EnterText(emailAddress2Element, Constants.UpdatedDataExtractEmail2);
                ClearText(emailAddress3Element);
                EnterText(emailAddress3Element, Constants.UpdatedDataExtractEmail3);
            }
            else if (emailAddresses == 2)
            {
                ClearText(emailAddress1Element);
                EnterText(emailAddress1Element, Constants.UpdatedDataExtractEmail1);
                ClearText(emailAddress2Element);
                ClearText(emailAddress3Element);
                EnterText(emailAddress3Element, Constants.UpdatedDataExtractEmail3);
            }
            else
            {
                ClearText(emailAddress1Element);
                EnterText(emailAddress1Element, Constants.UpdatedDataExtractEmail1);
                ClearText(emailAddress2Element);
                ClearText(emailAddress3Element);
            }
        }

    }
}