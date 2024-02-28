using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Data;
using System;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class ReportAnErrorPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "report-an-error");
        private static string expectedPageTitle { get; } = "Report an error or issue - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Report an error or issue";
        private static By expectedPageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By homeBreadcrumbElement { get; } = By.LinkText("Home");
        private static By SearchTheInsolvencyRegisterBreadcrumbElement { get; } = By.LinkText("Search the Individual Insolvency Register");
        private static By SearchResultsBreadcrumb { get; } = By.LinkText("Search results");
        private static By CaseDetailsBreadcrumb { get; } = By.LinkText("Case details");
        private static By ErrorsOrIssuesBreadcrumb { get; } = By.LinkText("Errors or issues");
        private static By ReportAnErrorOrIssueBreadcrumb { get; } = By.XPath("/html/body/div/div/ol/li[5]");
        private static By ReportAnErrorOrIssueBreadcrumb_FromFeedback { get; } = By.XPath("/html/body/div/div/ol/li[4]");
        private static string expectedHomeBreadcrumbText { get; } = "Home";
        private static string expectedSearchInsolvencyRegisterBreadcrumbText { get; } = "Search the Individual Insolvency Register";
        private static string expectedSearchResultsBreadcrumbText { get; } = "Search results";
        private static string expectedCaseDetailsBreadcrumbText { get; } = "Case details";
        private static string expectedErrorsOrIssuesBreadcrumbText { get; } = "Errors or issues";
        private static string expectedReportAnErrorOrIssueBreadcrumbText { get; } = "Report an error or issue";
        private static By MainErrorMessageElement = By.XPath("//*[@id='main-content']//a");
        private static By SubErrorMessageElement = By.Id("organisationName-error");


        private static By confirmAndSendButton { get; } = By.XPath("//button");
        private static By messageField = By.Id("CaseFeedback.Message");
        private static By fullNameField = By.Id("CaseFeedback.ReporterFullname");
        private static By emailAddressField = By.Id("CaseFeedback.ReporterEmailAddress");
        private static By memberOfThePublicRadioButton = By.Id("organisation1");

        public static void verifyReportAnErrorPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(expectedPageHeaderElement).Text);
        }   


        public static void verifyBreadcrumbText_FromStartPage()
        {
            Assert.AreEqual(expectedHomeBreadcrumbText, WebDriver.FindElement(homeBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchInsolvencyRegisterBreadcrumbText, WebDriver.FindElement(SearchTheInsolvencyRegisterBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchResultsBreadcrumbText, WebDriver.FindElement(SearchResultsBreadcrumb).Text);
            Assert.AreEqual(expectedCaseDetailsBreadcrumbText, WebDriver.FindElement(CaseDetailsBreadcrumb).Text);
            Assert.AreEqual(expectedReportAnErrorOrIssueBreadcrumbText, WebDriver.FindElement(ReportAnErrorOrIssueBreadcrumb).Text);
        }

        public static void verifyBreadcrumbText_FromErrorsOrIssuesPage()
        {
            Assert.AreEqual(expectedHomeBreadcrumbText, WebDriver.FindElement(homeBreadcrumbElement).Text);
            Assert.AreEqual(expectedErrorsOrIssuesBreadcrumbText, WebDriver.FindElement(ErrorsOrIssuesBreadcrumb).Text);
            Assert.AreEqual(expectedCaseDetailsBreadcrumbText, WebDriver.FindElement(CaseDetailsBreadcrumb).Text);
            Assert.AreEqual(expectedReportAnErrorOrIssueBreadcrumbText, WebDriver.FindElement(ReportAnErrorOrIssueBreadcrumb_FromFeedback).Text);
        }


        public static void ClickSearchTheInsolvencyRegisterBreadcrumb()
        {
            ClickButton(SearchTheInsolvencyRegisterBreadcrumbElement);
        }

        public static void ClickHomeBreadcrumb()
        {
            ClickButton(homeBreadcrumbElement);
        }

        public static void ClickSearchResultsBreadcrumb()
        {
            ClickButton(SearchResultsBreadcrumb);
        }

        public static void ClickConfirmAndSendButton()
        {
            ClickButton(confirmAndSendButton);
        }

        public static void ClickCaseDetailsBreadcrumb()
        {
            ClickButton(CaseDetailsBreadcrumb);
        }

        public static void EnterValidReportAnErrorDetails()
        {
            EnterText(messageField, Constants.ReportAnErrorMessage);
            EnterText(fullNameField, Constants.ReportAnErrorFullName);
            EnterText(emailAddressField, Constants.ReportAnErrorEmail);
            ClickElement(memberOfThePublicRadioButton);
            ClickConfirmAndSendButton();
        }
        
        public static void VerifyReportAnErrorDatabaseRecord(string Journey)
        {
            var list = SqlQueries.GetLatestCaseFeedbackRecord();
            DateTime parsedDateTime = Convert.ToDateTime(list[0][0]);
            string dateTime = parsedDateTime.ToString("MM/dd/yyyy");

            Assert.AreEqual(DateTime.Now.ToString("MM/dd/yyyy"), dateTime);
           
            Assert.AreEqual(Constants.ReportAnErrorMessage, Convert.ToString(list[0][2]));
            Assert.AreEqual(Constants.ReportAnErrorFullName, Convert.ToString(list[0][3]));
            Assert.AreEqual(Constants.ReportAnErrorEmail, Convert.ToString(list[0][4]));
            Assert.AreEqual("Member of the public", Convert.ToString(list[0][5]));

            if (Journey == "Start page")
            {
                Assert.AreEqual(Constants.ReportAnErrorCaseID_FromStartPage, Convert.ToString(list[0][1]));
            }
            else
            {
                Assert.AreEqual(Constants.ReportAnErrorCaseID_FromCaseFeedbackPage, Convert.ToString(list[0][1]));
            }
        }

        public static void FillInTextFields()
        {
            ClearText(messageField);
            EnterText(messageField, Constants.ReportAnErrorMessage);
            ClearText(fullNameField);
            EnterText(fullNameField, Constants.ReportAnErrorFullName);
            ClearText(emailAddressField);
            EnterText(emailAddressField, Constants.ReportAnErrorEmail);
            ClickConfirmAndSendButton();
        }

        public static void FillInMessageField()
        {
            ClearText(messageField);
            EnterText(messageField, Constants.ReportAnErrorMessage);
        }

        public static void FillInInvalidTextInMessageField()
        {
            ClearText(messageField);
            EnterText(messageField, "!£$%^&*");
        }

        public static void FillInInvalidTextInNameField()
        {
            ClearText(fullNameField);
            EnterText(fullNameField, "!£$%^&*");
        }

        public static void FillInInvalidTextInEmailField()
        {
            ClearText(emailAddressField);
            EnterText(emailAddressField, "test@test");
        }

        public static void FillInEMailField()
        {
            ClearText(emailAddressField);
            EnterText(emailAddressField, Constants.ReportAnErrorEmail);
        }

        public static void FillInNameField()
        {
            ClearText(fullNameField);
            EnterText(fullNameField, Constants.ReportAnErrorFullName);
        }

        public static void SelectAnOrganisationField()
        {
            ClickElement(memberOfThePublicRadioButton);
        }

        public static void ClearTextFields()
        {
            ClearText(messageField);
            ClearText(fullNameField);
            ClearText(emailAddressField);
        }

        public static void VerifyErrorMessage(string ErrorMessage)
        {
            Assert.AreEqual(ErrorMessage, WebDriver.FindElement(MainErrorMessageElement).Text);
            Assert.AreEqual(ErrorMessage, WebDriver.FindElement(SubErrorMessageElement).Text);
        }

    }
}
