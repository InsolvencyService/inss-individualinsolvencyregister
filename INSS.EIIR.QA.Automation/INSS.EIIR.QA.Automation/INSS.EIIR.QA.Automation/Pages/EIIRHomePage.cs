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
        private static string pageTitle { get; } = "Home Page - INSS.EIIR.Web";
        private static string pageHeader { get; } = "Default page template";

        public static void verifyEIIRHomePage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(pageUrl));
            Assert.AreEqual(pageTitle, WebDriver.Title);
        }

    }
}
