using INSS.EIIR.QA.Automation.Data;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminCaseFeedbackPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "admin/errors-or-issues");
        private static string expectedPageTitle { get; } = "Errors or issues - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Errors or issues";
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By organisationDropdownElement { get; } = By.Id("organisation");
        private static By typeDropdownElement { get; } = By.Id("type");
        private static By statusDropdownElement { get; } = By.Id("status");
        private static By caseDetailsElement { get; } = By.XPath("//*[@id='main-content']/div[2]/div[2]/dl[1]");
        private static By changeViewedStatusLink { get; } = By.LinkText("Change to viewed");
        private static By messageElement { get; } = By.Id("more-detail");
        private static By caseIDText { get; } = By.XPath("//*[@id='main-content']//h2");


        public static void clickHomeBreadcrumb()
        {
            WebDriver.FindElement(homeBreadcrumb).Click();
        }

        public static void clickChangeViewedStatus()
        {
            WebDriver.FindElement(changeViewedStatusLink).Click();
        }

        public static void verifyCaseFeedbackPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(PageHeader).Text);
        }

        public static void verifyCaseDetailsCorrectlyDisplayed(string Organisation, string Type)
        {
            var SqlResults = SqlQueries.GetCaseFeedbackDetails(Organisation, Type);
            string reporterName = Convert.ToString(SqlResults[0][0]);
            string emailAddress = Convert.ToString(SqlResults[0][1]);
            string status = Convert.ToString(SqlResults[0][2]);
            string organisation1 = Convert.ToString(SqlResults[0][3]);
            string Type1 = Convert.ToString(SqlResults[0][4]);
            string caseName = Convert.ToString(SqlResults[0][5]);
            DateTime reportDate = DateTime.Parse(Convert.ToString(SqlResults[0][6]));
            string reportDateConverted = reportDate.ToString("dd MMMM yyyy");
            string caseID = Convert.ToString(SqlResults[0][7]);
            string message = Convert.ToString(SqlResults[0][8]);

            if (Type1 == "I") { Type1 = "IVA"; } else if (Type1 == "D") { Type1 = "Debt relief order"; }  else if (Type1 == "B") { Type1 = "Bankruptcy"; }

            if (status == "False") { status = "NOT VIEWED"; } else if (status == "True") { status = "VIEWED"; } 
           
            Assert.IsTrue(WebDriver.FindElement(caseIDText).Text.Contains(caseID));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(status));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(reportDateConverted));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(caseName));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(Type1));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(reporterName));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(emailAddress));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsElement).Text.Contains(organisation1));
            Assert.IsTrue(WebDriver.FindElement(messageElement).Text.Contains(message));     

        }

        public static void SelectOrganisation(string Organisation)
        {
            switch (Organisation)
            {
                case "Other":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Debt recovery agency":
                    SelectFromDropDownByText(organisationDropdownElement,Organisation);
                    break;
                case "Financial services":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Government department":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Mortgage provider":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Bank or building society":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Credit card issuer":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Credit reference agency":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
                case "Member of the public":
                    SelectFromDropDownByText(organisationDropdownElement, Organisation);
                    break;
            }
        }

        public static void SelectType(string Type)
        {
            switch (Type)
            {
                case "IVAs":
                    SelectFromDropDownByText(typeDropdownElement, Type);
                    break;
                case "Bankruptcies":
                    SelectFromDropDownByText(typeDropdownElement, Type);
                    break;
                case "Debt relief orders":
                    SelectFromDropDownByText(typeDropdownElement, Type);
                    break;
            }
        }
    }
}
