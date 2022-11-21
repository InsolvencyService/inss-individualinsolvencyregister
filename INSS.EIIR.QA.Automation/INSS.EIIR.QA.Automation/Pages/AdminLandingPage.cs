using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using System;
using System.Threading;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AdminLandingPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "admin/admin-area");
        private static string expectedPageTitle { get; } = "Admin area - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Admin area";
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By viewFeedbackLink { get; } = By.LinkText("View feedback");
        private static By viewAndUpdateSubscribersLink { get; } = By.LinkText("View and update subscribers");
        private static By addNewSubscriberLink { get; } = By.LinkText("Add new subscriber");
        private static By signOutLink { get; } = By.LinkText("Sign out");



        public static void verifyAdminLandingPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader,WebDriver.FindElement(PageHeader).Text);
        }

        public static void NavigateToAdminLandingPage()
        {
            WebDriver.Navigate().GoToUrl("https://app-uksouth-sit-eiir-web.azurewebsites.net/Admin");
            WebDriver.FindElement(By.Id("username")).SendKeys(Constants.AdminUserName);
            WebDriver.FindElement(By.Id("password")).SendKeys(Constants.AdminPassword);
            WebDriver.FindElement(By.XPath("//*[@id='main-content']/div/div/form/button")).Click();
        }
        public static void clickViewFeedbackLink()
        {
            WebDriver.FindElement(viewFeedbackLink).Click();
        }
        public static void clickviewAndUpdateSubscribersLink()
        {
            WebDriver.FindElement(viewAndUpdateSubscribersLink).Click();
        }
        public static void clickaddNewSubscriberLink()
        {
            WebDriver.FindElement(addNewSubscriberLink).Click();
        }

        public static void clickSignOutLink()
        {
            WebDriver.FindElement(signOutLink).Click();
        }
    }
}
