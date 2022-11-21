using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class SearchPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "search");
        private static string expectedPageTitle { get; } = "Search - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Search the Individual Insolvency Register";
        private static By expectedPageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By searchField { get; } = By.Id("searchTerm");
        private static By searchButton { get; } = By.Id("searchButton");
        private static By homeBreadcrumb { get; } = By.LinkText("Home");
        private static string expectedNoTextEnteredErrorMessage { get; } = "Enter the name of an individual or their trading name";
        private static By errorMainErrorMessageElement { get; } = By.XPath("//div[1]/div/div/div/ul/li/a");
        private static By errorSubErrorMessageElement { get; } = By.XPath("//*[@id='search-error']/span");
        private static string expectedNoResultsReturnedMessage { get; } = "We didn't find any results for 'This search term will return nothing'. Try searching again.";
        private static By noResultsReturnedTextElement { get; } = By.XPath("//*[@id='main-content']/div[2]/div/p");

        public static void verifyEIIRSearchPage()
        {
          //  Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
           // Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(expectedPageHeaderElement).Text);
        }

        public static void EnterSearchText(string Text)
        {
            EnterText(searchField, Text);
        }

        public static void ClickSearchButton()
        {
            ClickButton(searchButton);
        }

        public static void ClickHomeBreadcrumb()
        {
            ClickButton(homeBreadcrumb);
        }

        public static void VerifyErrorMessage()
        {
            Assert.AreEqual(expectedNoTextEnteredErrorMessage, WebDriver.FindElement(errorMainErrorMessageElement).Text);
            Assert.AreEqual(expectedNoTextEnteredErrorMessage, WebDriver.FindElement(errorSubErrorMessageElement).Text);
        }

        public static void ValidateNoResultsReturnedPage()
        {
            Assert.AreEqual(expectedNoResultsReturnedMessage, WebDriver.FindElement(noResultsReturnedTextElement).Text);

        }

        public static void NavigateToSearchPage()
        {
            WebDriver.Navigate().GoToUrl(Constants.StartPageUrl);
            EIIRHomePage.clickStartButton();
        }
    }
}
