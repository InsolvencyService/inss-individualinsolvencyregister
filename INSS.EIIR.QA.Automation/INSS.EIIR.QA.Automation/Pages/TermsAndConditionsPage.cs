using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Threading;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class TermsAndConditionsPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "home/terms-and-conditions");
        private static string expectedPageTitle { get; } = "Terms and conditions - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Terms and conditions";
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
        private static string generalEnquiryPageURL = "https://www.insolvencydirect.bis.gov.uk/ExternalOnlineForms/GeneralEnquiry.aspx";

        public static void verifyTermsAndConditionsPage()
        {
            Console.WriteLine("Page URL is: " + WebDriver.Url);
            Console.WriteLine("Page Title is: " + WebDriver.Title);
            Console.WriteLine("Page Header is: " + WebDriver.FindElement(PageHeader).Text);
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader,WebDriver.FindElement(PageHeader).Text);
        }

        public static void verifyGeneralEnquiryPage()
        {
            Assert.AreEqual(generalEnquiryPageURL, WebDriver.Url);           
        }

        public static void clickTellTheInsolvencyServiceLink()
        {
            WebDriver.FindElement(By.PartialLinkText("the Insolvency Service")).Click();
        }

        public static void clickHomeBreadcrumb()
        {
            WebDriver.FindElement(homeBreadcrumb).Click();
        }

    }
}
