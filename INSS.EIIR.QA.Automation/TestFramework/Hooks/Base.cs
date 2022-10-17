using System;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using INSS.EIIR.QA.Automation.TestFramework.Hooks;

namespace INSS.EIIR.QA.Automation.TestFramework.Hooks
{
    [Binding]
    public class Base
    {
        public static IWebDriver WebDriver { get; private set; }

        [Before]
        public static void SetUp()
        {
            var browser = WebDriverFactory.Config["Browser"];
            WebDriver = WebDriverFactory.GetWebDriver(browser);
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(10);
        }

        [After]
        public static void CleanUp()
        {            
            WebDriver.Dispose();
        }
    }
}
