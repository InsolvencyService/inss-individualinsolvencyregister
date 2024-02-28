using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class AccessibilityStatementPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "home/accessibility-statement");
        private static string expectedPageTitle { get; } = "Accessibility statement - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Accessibility statement";
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static By PageHeader { get; } = By.XPath("//*[@id='main-content']//h1");
               
        private static By howToMakeYourDeviceEasierToUseLink { get; } = By.PartialLinkText("device easier to use");
        private static By generalEnquiryFormLink { get; } = By.LinkText("general enquiry form");
        private static By findOutAboutCallChargesLink { get; } = By.LinkText("Find out about call charges");
        private static By contactTheEqualityAdvisoryLink { get; } = By.PartialLinkText("contact the Equality");
        private static By webContentAccessibilityLink { get; } = By.PartialLinkText("Web Content Accessibility");
        private static By thePublicSectorBodiesLink { get; } = By.PartialLinkText("The Public Sector Bodies");



        public static void verifyAccessibilityStatementPage()
        {
            Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
            Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader,WebDriver.FindElement(PageHeader).Text);
        }

             public static void ClickLink(string Linkname)
        {
            switch (Linkname)
            {
                case "how to make your device easier to use":
                    ClickElement(howToMakeYourDeviceEasierToUseLink);
                    break;
                case "general enquiry form":
                    ClickElement(generalEnquiryFormLink);
                    break;
                case "Find out about call charges":
                    ClickElement(findOutAboutCallChargesLink);
                    break;
                case "contact the Equality Advisory and Support Service (EASS)":
                    ClickElement(contactTheEqualityAdvisoryLink);
                    break;
                case "The Public Sector Bodies (Websites and Mobile Applications) (No. 2) Accessibility Regulations 2018":
                    ClickElement(thePublicSectorBodiesLink);
                    break;
                case "Web Content Accessibility Guidelines (WCAG) version 2.1":
                    ClickElement(webContentAccessibilityLink);
                    break;
                case "Home":
                    ClickElement(homeBreadcrumb);
                    break;
            }
        }

    }
}
