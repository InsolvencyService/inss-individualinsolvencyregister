using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace INSS.EIIR.QA.Automation.TestFramework.TestSupport
{
    [Binding]
    public class Base
    {
        public static IWebDriver WebDriver { get; private set; }

        [Before]
        public static void SetUp()
        {
            var browser = WebDriverFactory.Config["Browser"];

            switch (browser)
            {
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    //chromeOptions.AddArguments("--incognito", "--headless");
                    //chromeOptions.AddArguments("--incognito");
                    chromeOptions.AddUserProfilePreference("download.default_directory", FileHelper.GetDownloadFolder());
                    WebDriver = new ChromeDriver(chromeOptions);
                    break;

                default:
                    throw new Exception($"Driver name - {browser} does not match OR this framework does not support the webDriver specified");
            }
        }

        [After]
        public void CleanUp()
        {
            WebDriver.Quit();
        }

    }
}
