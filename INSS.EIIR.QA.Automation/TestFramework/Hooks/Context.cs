using System;
using OpenQA.Selenium;
using BoDi;
using INSS.EIIR.QA.Automation.browserHelperFactory;
using RestSharp;


namespace INSS.EIIR.QA.Automation.Hooks
{
    public class Context
    {
        private readonly ChromeBrowserFactory _chromeBrowserFactory;
        private readonly IObjectContainer objectContainer;
        private IWebDriver driver;
        private readonly string APIbaseUrl = "https://app-uksouth-sit-ods-worldpay-api.azurewebsites.net/worldpay/installationid?installationid=1086392";
        public static RestClient client;
        public static IRestRequest request;
        public static IRestResponse response;
        private readonly string browser = "chrome";
        public string content = string.Empty;
        public string statusCode = string.Empty;
        public Context(IObjectContainer objectContainer, ChromeBrowserFactory chromeBrowserFactory)
        {
            this.objectContainer = objectContainer;
            _chromeBrowserFactory = chromeBrowserFactory;
        }

        public void LoadApplication(string baseUrl)

        {
            switch (browser.ToLower())
            {
                case "chrome":
                    driver = _chromeBrowserFactory.Create(objectContainer);
                    break;
                default:
                    driver = _chromeBrowserFactory.Create(objectContainer);
                    break;
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);

            driver.Navigate().GoToUrl(baseUrl);
        }

        public void ShutDownApplication()
        {
            driver?.Quit();
        }
        public void TakeScreeenshotAtThePointOfTestFailure(string directory, string scenarioName)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string path = directory + scenarioName + DateTime.Now.ToString("yyyy-MM-dd") + ".png";
            string Screenshot = screenshot.AsBase64EncodedString;
            byte[] screenshotAsByteArray = screenshot.AsByteArray;
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public void GetMethod(string resource)
        {
            var client = new RestClient(APIbaseUrl);
            var request = new RestRequest(resource, Method.GET);
            var result = client.Execute(request);
            content = result.Content;
            statusCode = result.StatusCode.ToString();
        }
        public object CallPostEndpoint(object requestbody)
        {
            client = new RestClient(APIbaseUrl);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", requestbody, ParameterType.RequestBody);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;

        }
     


    }
}


