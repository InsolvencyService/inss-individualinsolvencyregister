using NUnit.Framework;
using OpenQA.Selenium;
using INSS.EIIR.QA.Automation.Helpers;
using INSS.EIIR.QA.Automation.TestFramework;
using System.Collections.Generic;

namespace INSS.EIIR.QA.Automation.Pages
{
    internal class SearchResultsPage : ElementHelper
    {
        private static string expectedPageUrl { get; } = string.Concat(Constants.StartPageUrl, "search-results/");
        private static string expectedPageTitle { get; } = "Search results - Individual Insolvency Register";
        private static string expectedPageHeader { get; } = "Search results";
        private static By expectedPageHeaderElement { get; } = By.XPath("//*[@id='main-content']//h1");
        private static By homeBreadcrumbElement { get; } = By.LinkText("Home");
        private static By SearchTheInsolvencyRegisterBreadcrumbElement { get; } = By.LinkText("Search the Individual Insolvency Register");
        private static By SearchResultsBreadcrumbElement { get; } = By.XPath("/html/body/div/div/ol/li[3]");
        public static string ExpectedXpath = "";
        private static string expectedHomeBreadcrumbText { get; } = "Home";
        private static string expectedSearchInsolvencyRegisterBreadcrumbText { get; } = "Search the Individual Insolvency Register";
        private static string expectedSearchResultsBreadcrumbText { get; } = "Search results";
        private static By tellTheInsolvencyLink { get; } = By.LinkText("tell the Insolvency Service");

        private static By ResultsPostcodeColumnElement { get; } = By.XPath("//*[@id='main-content']//td[4]");

        public static void verifyEIIRSearchResultPage()
        {
             Assert.IsTrue(WebDriver.Url.Contains(expectedPageUrl));
             Assert.AreEqual(expectedPageTitle, WebDriver.Title);
            Assert.AreEqual(expectedPageHeader, WebDriver.FindElement(expectedPageHeaderElement).Text);
        }

        public static void verifyBreadcrumbText()
        {
            Assert.AreEqual(expectedHomeBreadcrumbText, WebDriver.FindElement(homeBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchInsolvencyRegisterBreadcrumbText, WebDriver.FindElement(SearchTheInsolvencyRegisterBreadcrumbElement).Text);
            Assert.AreEqual(expectedSearchResultsBreadcrumbText, WebDriver.FindElement(SearchResultsBreadcrumbElement).Text);

        }


        public static void ClickSearchTheInsolvencyRegisterBreadcrumb()
        {
            ClickButton(SearchTheInsolvencyRegisterBreadcrumbElement);
        }

        public static void ClickTellTheInsolvencyLink()
        {
            ClickElement(tellTheInsolvencyLink);
        }

        public static void ClickHomeBreadcrumb()
        {
            ClickButton(homeBreadcrumbElement);
        }

        public static void ClickIndividualLink(string Postcode)
        {
            
            IList<IWebElement> all = WebDriver.FindElements(ResultsPostcodeColumnElement);

            //intialise the counter to 0
            int i = 0;

            //loop through the list looking for your result. Update the counter each time you loop around 
            foreach (IWebElement element in all)
            {
                //take the postcode from the list and store it in to the string variable
                string ScreenText = element.Text;

                //increment the counter by 1
                i = i + 1;

                if (ScreenText == Postcode)
                {
                    ExpectedXpath = "//*[@id='main-content']/div[2]/div[1]/table/tbody/tr[" + i + "]/td[1]/a";
                    WebDriver.FindElement(By.XPath(ExpectedXpath)).Click();
                    break;
                }
            }
        }
    }
}
