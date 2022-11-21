using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;


namespace INSS.EIIR.QA.Automation.TestFramework
{
    public static class WebDriverFactory

    {
        private static IConfigurationRoot _config;

        public static IConfigurationRoot Config
        {
            get
            {
                if (_config != null) return _config;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _config = builder.Build();
                return _config;
            }
        }

        public static IWebDriver GetWebDriver(string browser)
        {
            switch (browser)
            {
                case "FireFox":
                    return new FirefoxDriver();
                case "Edge":
                    return new EdgeDriver();
                case "IE":
                    return new InternetExplorerDriver();
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                   // chromeOptions.AddArguments("--incognito", "--headless");
                    //chromeOptions.AddArguments("--incognito");
                    chromeOptions.AddUserProfilePreference("download.default_directory", FileHelper.GetDownloadFolder());
                    return new ChromeDriver(chromeOptions);

                default:
                    throw new Exception($"Driver name - {browser} does not match OR this framework does not support the webDriver specified");
            }
        }
    }
}