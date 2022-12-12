using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class CaseDetailsPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "case-details");
        private static string expectedPageTitle { get; } = "Case Details - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Case details:";
        private static By expectedPageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By homeBreadcrumbElement { get; } = By.LinkText("Home");
        private static By SearchTheInsolvencyRegisterBreadcrumbElement { get; } = By.LinkText("Search the Individual Insolvency Register");
        private static By SearchResultsBreadcrumb { get; } = By.LinkText("Search results");
        private static By CaseDetailsBreadcrumb { get; } = By.XPath("/html/body/div/div/ol/li[4]");
            
        private static string expectedHomeBreadcrumbText { get; } = "Home";
        private static string expectedSearchInsolvencyRegisterBreadcrumbText { get; } = "Search the Individual Insolvency Register";
        private static string expectedSearchResultsBreadcrumbText { get; } = "Search results";
        private static string expectedCaseDetailsBreadcrumbText { get; } = "Case Details";

        private static By startNewSearchButton { get; } = By.Id("newSearch");
        private static By reportErrorOrIssueLink { get; } = By.LinkText("Report an error or issue");

        private static string ExpectedPageHeading = "";
        private static By caseDetailsIndividualDetails { get; } = By.XPath("//*[@id='accordion-default-content-1']/dl");
        private static By caseDetailsInsovencyCaseDetails { get; } = By.XPath("//*[@id='accordion-default-content-2']/dl");
        private static By caseDetailsInsovencyContactDetails { get; } = By.XPath("//*[@id='accordion-default']/div[4]");
        private static By ShowInsolvencyCaseDetails = By.XPath("//*[@id='accordion-default']/div[3]/div[1]/h2/button/span[3]/span/span[2]");
        private static By ShowInsolvencyContactDetails = By.XPath("//*[@id='accordion-default']/div[4]/div[1]/h2/button/span[3]/span/span[2]");

        public static void verifyCaseDetailsPage(string Name)
        {
            ExpectedPageHeading = expectedPageHeader + " " + Name.ToUpper();
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(ExpectedPageHeading, WebDriver.FindElement(expectedPageHeaderElement).Text);
        }

        public static void verifyIndividualDetails()
        {
            //verifyIndividualDetails
            Assert.IsTrue(WebDriver.FindElement(caseDetailsIndividualDetails).Text.Contains("ADRIAN"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsIndividualDetails).Text.Contains("ADAMS"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsIndividualDetails).Text.Contains("MALE"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsIndividualDetails).Text.Contains("06/07/1967"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsIndividualDetails).Text.Contains("11 Bexon Court Britland Estate Leigh Street Herts"));

            ClickElement(ShowInsolvencyCaseDetails);
            ClickElement(ShowInsolvencyContactDetails);

            //verifyInsolvencyCaseDetails
            var test = WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text;
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("ADRIAN ADAMS"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("High Court Of Justice"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("Bankruptcy"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("42046891"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("02/02/2005"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyCaseDetails).Text.Contains("CURRENT"));

            //verifyInsolvencyServiceContactDetails
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyContactDetails).Text.Contains("London A"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyContactDetails).Text.Contains("Enquiry Desk"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyContactDetails).Text.Contains("2nd Floor 4 Abbey Orchard Street LONDON United"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyContactDetails).Text.Contains("SW1P 2HT"));
            Assert.IsTrue(WebDriver.FindElement(caseDetailsInsovencyContactDetails).Text.Contains("0207 6371110"));

        }


        public static void verifyBreadcrumbText()
        {
            Assert.AreEqual(expectedHomeBreadcrumbText, WebDriver.FindElement(homeBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchInsolvencyRegisterBreadcrumbText, WebDriver.FindElement(SearchTheInsolvencyRegisterBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchResultsBreadcrumbText, WebDriver.FindElement(SearchResultsBreadcrumb).Text);
            Assert.AreEqual(expectedCaseDetailsBreadcrumbText, WebDriver.FindElement(CaseDetailsBreadcrumb).Text);
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

        public static void ClickStartNewSearchButton()
        {
            ClickButton(startNewSearchButton);
        }
        public static void ClickReportErrorOrIssueLink()
        {
            ClickButton(reportErrorOrIssueLink);
        }
    }
}
