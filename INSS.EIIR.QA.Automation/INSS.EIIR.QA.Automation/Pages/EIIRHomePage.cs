using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Threading;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class EIIRHomePage : ElementHelper
    {
        private static string pageUrl { get; } = "https://app-uksouth-sit-eiir-web.azurewebsites.net/";
        private static string pageTitle { get; } = "Individual Insolvency Register - Individual Insolvency Register";
        private static string pageHeader { get; } = "Individual Insolvency Register";
        private static By pageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By termsAndConditionsLink { get; } = By.LinkText("terms and conditions");
        private static By theInsolvencyServiceLink { get; } = By.PartialLinkText("Insolvency Service");
        private static By privacyFooterLink { get; } = By.XPath("//footer//li[1]/a");
        private static By accessibilityFooterLink { get; } = By.XPath("//footer//li[2]/a");
        private static By termsAndConditionsFooterLink { get; } = By.XPath("//footer//li[3]/a");
        private static By GetHelpFromTheInsolvencyLink { get; } = By.PartialLinkText("Get help");
        private static By FindOutMoreLink { get; } = By.PartialLinkText("Find out more");
        private static By GiveFeedbackLink { get; } = By.PartialLinkText("Give feedback");
        private static By startButton { get; } = By.Id("CTA_StartNow");

        public static void verifyEIIRHomePage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(pageUrl));
            Assert.AreEqual(pageTitle, WebDriver.Title);
            Assert.AreEqual(pageHeader, WebDriver.FindElement(pageHeaderElement).Text);
        }

        public static void ClickLink(string Linkname)
        {
            switch (Linkname)
            {
                case "Terms and conditions - footer":
                    ClickElement(termsAndConditionsFooterLink);
                    break;
                case "Privacy - footer":
                    ClickElement(privacyFooterLink);
                    break;
                case "Accessibility statement - footer":
                    ClickElement(accessibilityFooterLink);
                    break;
                case "main content Terms and conditions":
                    ClickElement(termsAndConditionsLink);
                    break;
                case "main content Insolvency Service":
                    ClickElement(theInsolvencyServiceLink);
                    break;
                case "Related content - Get help from the Insolvency Service":
                    ClickElement(GetHelpFromTheInsolvencyLink);
                    break;
                case "Related content - Find out more about bankruptcy and insolvency":
                    ClickElement(FindOutMoreLink);
                    break;
                case "Related content - Give feedback about the Individual Insolvency Register":
                    ClickElement(GiveFeedbackLink);
                    break;
            }
        }     

        public static void verifyPageURL(string URL)
        {
            Assert.AreEqual(URL, WebDriver.Url);
        }

        public static void clickTermsAndConditionsLink()
        {
            ClickElement(termsAndConditionsFooterLink);
        }

        public static void clickStartButton()
        {
            ClickElement(startButton);
        }
    }
}
