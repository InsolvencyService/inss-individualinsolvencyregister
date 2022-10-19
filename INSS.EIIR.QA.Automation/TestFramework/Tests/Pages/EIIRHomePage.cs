using INSS.EIIR.QA.Automation.TestFramework.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;

namespace INSS.EIIR.QA.Automation.TestFramework.Tests.Pages
{
    public class EIIRHomePage : ElementHelper
    {

        private static string EIIRHomePageUrl { get; } = "https://app-uksouth-sit-eiir-web.azurewebsites.net/";
        private static string EIIRHomePageTitle { get; } = "Individual Insolvency Register - INSS.EIIR.Web";
        private static string EIIRHomePageHeader { get; } = "Individual Insolvency Register";
        private static By pageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
       

        public static void VerifyEIIRHomePage()
        {
            Assert.AreEqual(EIIRHomePageTitle, WebDriver.Title);
            Assert.AreEqual(EIIRHomePageHeader, WebDriver.FindElement(pageHeader).Text);
            Assert.IsTrue(WebDriver.Url.Contains(EIIRHomePageUrl));
        }
    }
}
