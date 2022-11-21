using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class PrivacyPolicyPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "home/privacy");
        private static string expectedPageTitle { get; } = "Privacy policy - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Privacy policy";
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
               
        private static By personalInformationCharterLink { get; } = By.LinkText("personal information charter");
        private static By cookiesLink { get; } = By.LinkText("cookies on GOV.UK");
        



        public static void verifyPrivacyPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader,WebDriver.FindElement(PageHeader).Text);
        }

             public static void ClickLink(string Linkname)
        {
            switch (Linkname)
            {
                case "personal information charter":
                    ClickElement(personalInformationCharterLink);
                    break;
                case "cookies on GOV UK":
                    ClickElement(cookiesLink);
                    break;
                case "Home":
                    ClickElement(homeBreadcrumb);
                    break;
            }
        }

    }
}
