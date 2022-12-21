using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Threading;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminLoginPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "admin");
        private static string expectedPageTitle { get; } = "Admin login – Search the Individual Insolvency Register - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Admin sign in";
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
        private static string generalEnquiryPageURL = "https://www.insolvencydirect.bis.gov.uk/ExternalOnlineForms/GeneralEnquiry.aspx";
        private static By mainErrorMessageElement1 { get; } = By.XPath("//*[@id='main-content']//li[1]/a");
        private static By mainErrorMessageElement2 { get; } = By.XPath("//*[@id='main-content']//li[2]/a");
        private static By subErrorMessageUserName { get; } = By.XPath("//*[@id='username-error']/span");
        private static By subErrorMessagePasswordMessage { get; } = By.XPath("//*[@id='password-error']/span");
        private static string expectedUsernameErrorMessage { get; } = "Enter username";
        private static string expectedPasswordErrorMessage { get; } = "Enter password";
        private static string expectedIncorrectUsernamePasswordMessage { get; } = "Your login details are incorrect";
        private static By subErrorMessageIncorrectLoginDetailsMessage { get; } = By.XPath("TBC");
        private static By usernameField { get; } = By.Id("username");
        private static By passwordField { get; } = By.Id("password");
        private static By signInButton { get; } = By.XPath("//*[@id='main-content']//button");


        public static void navigateToAdminLoginPage()
        {
            WebDriver.Navigate().GoToUrl(expectedPageUrl);
        }

        public static void verifyAdminLoginPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader,WebDriver.FindElement(PageHeader).Text);
        }

        public static void enterUsernameOnly()
        {
            WebDriver.FindElement(usernameField).Clear();
            WebDriver.FindElement(passwordField).Clear();
            WebDriver.FindElement(usernameField).SendKeys("Test");
            var element = WebDriver.FindElement(signInButton);
            WaitForPageElementBy(10, signInButton);
            ClickButton(signInButton);
        }
        public static void enterPasswordOnly()
        {
            WebDriver.FindElement(usernameField).Clear();
            WebDriver.FindElement(passwordField).Clear();
            WebDriver.FindElement(passwordField).SendKeys("Test");
            var element = WebDriver.FindElement(signInButton);
            WaitForPageElementBy(10, signInButton);
            ClickButton(signInButton);
        }
        public static void clearTextFields()
        {
            WebDriver.FindElement(usernameField).Clear();
            WebDriver.FindElement(passwordField).Clear();
            var element = WebDriver.FindElement(signInButton);
            WaitForPageElementBy(10, signInButton);
            ClickButton(signInButton);
        }

        public static void enterInvalidLoginDetails()
        {
            WebDriver.FindElement(usernameField).Clear();
            WebDriver.FindElement(passwordField).Clear();
            WebDriver.FindElement(usernameField).SendKeys("Test");
            WebDriver.FindElement(passwordField).SendKeys("Test");
            var element = WebDriver.FindElement(signInButton);
            WaitForPageElementBy(10, signInButton);
            ClickButton(signInButton);
        }
        public static void verifyNullUsernameErrorMessage()
        {
            Assert.AreEqual(expectedUsernameErrorMessage, WebDriver.FindElement(mainErrorMessageElement1).Text);
            Assert.AreEqual(expectedUsernameErrorMessage, WebDriver.FindElement(subErrorMessageUserName).Text);
        }

        public static void verifyNullPasswordErrorMessage()
        {
            Assert.AreEqual(expectedPasswordErrorMessage, WebDriver.FindElement(mainErrorMessageElement1).Text);
            Assert.AreEqual(expectedPasswordErrorMessage, WebDriver.FindElement(subErrorMessagePasswordMessage).Text);
        }

        public static void verifyNullPasswordAndUsernameErrorMessage()
        {     
            Assert.AreEqual(expectedUsernameErrorMessage, WebDriver.FindElement(mainErrorMessageElement1).Text);
            Assert.AreEqual(expectedUsernameErrorMessage, WebDriver.FindElement(subErrorMessageUserName).Text);

            Assert.AreEqual(expectedPasswordErrorMessage, WebDriver.FindElement(mainErrorMessageElement2).Text);
            Assert.AreEqual(expectedPasswordErrorMessage, WebDriver.FindElement(subErrorMessagePasswordMessage).Text);
        }

        public static void verifyInvalidLoginDetailsErrorMessage()
        {
            Assert.AreEqual(expectedIncorrectUsernamePasswordMessage, WebDriver.FindElement(mainErrorMessageElement1).Text);
          //  Assert.AreEqual(expectedIncorrectUsernamePasswordMessage, WebDriver.FindElement(subErrorMessageIncorrectLoginDetailsMessage).Text);
        }
        public static void clickTellTheInsolvencyServiceLink()
        {
            WebDriver.FindElement(By.PartialLinkText("the Insolvency Service")).Click();
        }

        public static void clickHomeBreadcrumb()
        {
            WebDriver.FindElement(homeBreadcrumb).Click();
        }

        //public static void verifyAdminLoginPage1()
        //{
        //    Assert.IsTrue(WebDriver.Url.Contains(string.Concat(Constants.StartPageUrl, "Admin")));
        //    Assert.AreEqual(expectedPageTitle, WebDriver.Title);
        //    Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(PageHeader).Text);
        //}

    }
}
