using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Collections.Generic;
using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminSubscriberDetailsPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "admin/subscriber");
        private static string expectedPageTitle { get; } = "Subscriber details - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Subscriber details";
        private static string emailAddress1;
        private static string emailAddress2;
        private static string emailAddress3;
        private static By pageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By subscribersBreadcrumb { get; } = By.LinkText("Subscribers");
        private static By returnToAdminAreaButton { get; } = By.ClassName("govuk-button");
        private static By viewAnotherSubscriberLink { get; } = By.LinkText("View another subscriber");
        private static By SubscriptionEndsInXDaysElement { get; } = By.XPath("//div[1]/strong");
        private static By subscriptionEndText { get; } = By.XPath("//strong");

        private static By organisationNameElement { get; } = By.XPath("//dl[1]/div/dd[1]");
        private static By organisationTypeElement { get; } = By.XPath("//dl[1]/div[2]/dd[1]");
        private static By tradingTypeElement { get; } = By.XPath("//dl[1]/div/dd[1]");
        private static By fullNameElement { get; } = By.XPath("//dl[2]/div[1]/dd[1]");
        private static By addressElement { get; } = By.XPath("//dl[2]/div[2]/dd[1]");
        private static By emailAddressElement { get; } = By.XPath("//dl[2]/div[3]/dd[1]");
        private static By telephoneNumberElement { get; } = By.XPath("//dl[2]/div[4]/dd[1]");
        private static By applicationSubmittedDateElement { get; } = By.XPath("//dl[3]/div[1]/dd[1]");
        private static By startDateElement { get; } = By.XPath("//dl[3]/div[2]/dd[1]");
        private static By endDateElement { get; } = By.XPath("//dl[3]/div[3]/dd[1]");
        private static By statusElement { get; } = By.XPath("//dl[3]/div[4]/dd[1]");
        private static By emailAddress1Element { get; } = By.XPath("//dl[4]/div[1]/dt[2]");
        private static By emailAddress2Element { get; } = By.XPath("//dl[4]/div[2]/dt[2]");
        private static By emailAddress3Element { get; } = By.XPath("//dl[4]/div[3]/dt[2]");

        private static List<object[]> result;
       
        //Change Links
        private static By ChangeOrganisationNameLink { get; } = By.XPath("//dl[1]/div[1]/dd[2]/a");
        private static By ChangeNameLink { get; } = By.XPath("//dl[1]/div[2]/dd[2]/a");
        private static By ChangeAddressLink { get; } = By.XPath("//dl[2]/div[1]/dd[2]/a");

        public static void verifySubscriberDetailsPage()
        {
            Console.WriteLine(expectedPageUrl);
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

        private static string convertDate(DateTime date)
        {
            string convertedDate = date.ToString("dd/MM/yyyy");
            return convertedDate;
        }

        private static string convertStatus(string Status)
        {
            string convertedStatus;

            if (Status == "Y")
            {
                convertedStatus = "Active";
            }
            else
            {
                convertedStatus = "Inactive";
            }

            return convertedStatus;
        }

        public static void VerifySubscribersDetails(string Subscriber)
        {
            result = SqlQueries.GetSubscriberDetails(Subscriber);


            string organisationName = Convert.ToString(result[0][1]);
            string status = Convert.ToString(result[0][2]);
            DateTime startDate = DateTime.Parse(Convert.ToString(result[0][3]));
            DateTime endDate = DateTime.Parse(Convert.ToString(result[0][4]));
            string fullName = Convert.ToString(result[0][5]) + " " + Convert.ToString(result[0][6]);
            string addressLine1 = Convert.ToString(result[0][7]);
            string addressLine2 = Convert.ToString(result[0][8]);
            string addressLine3 = Convert.ToString(result[0][9]);
            string addressLine4 = Convert.ToString(result[0][13]);
            string addressLine5 = Convert.ToString(result[0][10]);
            string telephoneNumber = Convert.ToString(result[0][11]);
            string emailAddress = Convert.ToString(result[0][12]);
            DateTime applicationSubmittedDate = DateTime.Parse(Convert.ToString(result[0][15]));
            string organisationType = Convert.ToString(result[0][14]);

            Assert.AreEqual(organisationName, WebDriver.FindElement(organisationNameElement).Text);
            Assert.AreEqual(organisationType, WebDriver.FindElement(organisationTypeElement).Text);
            Assert.AreEqual(fullName, WebDriver.FindElement(fullNameElement).Text);

            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine1));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine2));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine3));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine4));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine5));

            Assert.AreEqual(emailAddress, WebDriver.FindElement(emailAddressElement).Text);
            Assert.AreEqual(telephoneNumber, WebDriver.FindElement(telephoneNumberElement).Text);

            Assert.AreEqual(convertDate(startDate), convertDate(DateTime.Parse(WebDriver.FindElement(startDateElement).Text)));
            Assert.AreEqual(convertDate(endDate), convertDate(DateTime.Parse(WebDriver.FindElement(endDateElement).Text)));
            Assert.AreEqual(convertDate(applicationSubmittedDate), convertDate(DateTime.Parse(WebDriver.FindElement(applicationSubmittedDateElement).Text)));

            Assert.AreEqual(convertStatus(status), WebDriver.FindElement(statusElement).Text);
        }

        public static void VerifySubscribersEmailAddresses(string Subscriber)
        {
            result = SqlQueries.GetSubscriberEmailAddresses(Subscriber);
            SqlQueries.DeleteSubscriber(Subscriber);
            int emailCount = result.Count;
            if (emailCount == 1)
            {
                emailAddress1 = Convert.ToString(result[0][0]);
                Assert.AreEqual(emailAddress1, WebDriver.FindElement(emailAddress1Element).Text);
            }
            else if (emailCount == 2)
            {
                emailAddress1 = Convert.ToString(result[0][0]);
                emailAddress2 = Convert.ToString(result[1][0]);
                Assert.AreEqual(emailAddress1, WebDriver.FindElement(emailAddress1Element).Text);
                Assert.AreEqual(emailAddress2, WebDriver.FindElement(emailAddress2Element).Text);
            }
            else if (emailCount == 3)
            {
                emailAddress1 = Convert.ToString(result[0][0]);
                emailAddress2 = Convert.ToString(result[1][0]);
                emailAddress3 = Convert.ToString(result[2][0]);

                Assert.AreEqual(emailAddress1, WebDriver.FindElement(emailAddress1Element).Text);
                Assert.AreEqual(emailAddress2, WebDriver.FindElement(emailAddress2Element).Text);
                Assert.AreEqual(emailAddress3, WebDriver.FindElement(emailAddress3Element).Text);
            }
            else
            {
                Console.WriteLine("No email address to validate");
            }
           
        }

        public static void VerifySubscriptionEndsInXDaysValue(string Subscriber)
        {
            result = SqlQueries.GetSubscriberDetails(Subscriber);
        
            DateTime endDate = DateTime.Parse(Convert.ToString(result[0][4]));
            int wholeDays = (endDate - DateTime.Today).Days;
                     
            string ExpectedExpirationMessage = "This subscription ends in " + Convert.ToString(wholeDays) + " days";
            Assert.IsTrue(WebDriver.FindElement(subscriptionEndText).Text.Contains(ExpectedExpirationMessage));
        }

        public static void ClickReturnToAdminAreaButton()
        {
            ClickElement(returnToAdminAreaButton);
        }

        public static void ClickViewAnotherSubscriberLink()
        {
            ClickElement(viewAnotherSubscriberLink);
        }

        public static void ClickChangeLink(string ChangeLink)
        {
            switch (ChangeLink)
            {
                case "Organisation Name":
                    Console.WriteLine("User is currently on page: " + WebDriver.Url);
                    ClickElement(ChangeOrganisationNameLink);
                    break;
            }
        }


        public static void VerifyUpdatedSubscribersDetails()
        {
            string organisationName = Constants.UpdatedOrganisationName;
          
            string fullName = Constants.UpdatedFirstName + " " + Constants.UpdatedSurname;
            string addressLine1 = Constants.UpdatedAddressLine1;
            string addressLine2 = Constants.UpdatedAddressLine1;
            string addressLine3 = Constants.UpdatedAddressLine1;
            string addressLine4 = Constants.UpdatedAddressLine1;
            string addressLine5 = Constants.UpdatedAddressLine1;
            string telephoneNumber = Constants.UpdatedTelephoneNumber;
            string emailAddress = Constants.UpdatedEmailAddress;
            string status = Constants.UpdatedStatus;

            string organisationType = Constants.UpdatedType;

            Assert.AreEqual(organisationName, WebDriver.FindElement(organisationNameElement).Text);
            Assert.AreEqual(organisationType, WebDriver.FindElement(organisationTypeElement).Text);
            Assert.AreEqual(fullName, WebDriver.FindElement(fullNameElement).Text);

            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine1));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine2));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine3));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine4));
            Assert.IsTrue(WebDriver.FindElement(addressElement).Text.Contains(addressLine5));

            Assert.AreEqual(emailAddress, WebDriver.FindElement(emailAddressElement).Text);
            Assert.AreEqual(telephoneNumber, WebDriver.FindElement(telephoneNumberElement).Text);
                     
            Assert.AreEqual(status, WebDriver.FindElement(statusElement).Text);
        }

        //public static void VerifySubscribersUpdatedEmailAddresses()
        //{
        //    emailAddress1 = Constants.UpdatedDataExtractEmail1;
        //    emailAddress2 = Constants.UpdatedDataExtractEmail2;
        //    emailAddress3 = Constants.UpdatedDataExtractEmail3;

        //    Assert.AreEqual(emailAddress1, WebDriver.FindElement(emailAddress1Element).Text);
        //    Assert.AreEqual(emailAddress2, WebDriver.FindElement(emailAddress2Element).Text);
        //    Assert.AreEqual(emailAddress3, WebDriver.FindElement(emailAddress3Element).Text);
        //}

        public static void VerifySubscribersUpdatedEmailAddresses(int NoOfEmailAddresses)
        {
            if (NoOfEmailAddresses == 3)
            {
                Assert.AreEqual(Constants.UpdatedDataExtractEmail1, WebDriver.FindElement(emailAddress1Element).Text);
                Assert.AreEqual(Constants.UpdatedDataExtractEmail2, WebDriver.FindElement(emailAddress2Element).Text);
                Assert.AreEqual(Constants.UpdatedDataExtractEmail3, WebDriver.FindElement(emailAddress3Element).Text);
            }
            else if (NoOfEmailAddresses == 2)
            {
                Assert.AreEqual(Constants.UpdatedDataExtractEmail1, WebDriver.FindElement(emailAddress1Element).Text);
                Assert.AreEqual(Constants.UpdatedDataExtractEmail3, WebDriver.FindElement(emailAddress2Element).Text);              
            }
            else if (NoOfEmailAddresses == 1)
            {  
                Assert.AreEqual(Constants.UpdatedDataExtractEmail1, WebDriver.FindElement(emailAddress1Element).Text);            
            }
        }
    }
}