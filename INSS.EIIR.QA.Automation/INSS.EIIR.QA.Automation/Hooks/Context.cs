using System;
using OpenQA.Selenium;
using BoDi;
using RestSharp;


namespace INSS.EIIR.QA.Automation.Hooks
{
    public class Context
    {
      
        private readonly IObjectContainer objectContainer;
        private IWebDriver driver;
        public static RestClient client;
        public static IRestRequest request;
        public static IRestResponse response;
        private readonly string browser = "chrome";
        public string content = string.Empty;
        public string statusCode = string.Empty;

        public void TakeScreeenshotAtThePointOfTestFailure(string directory, string scenarioName)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string path = directory + scenarioName + DateTime.Now.ToString("yyyy-MM-dd") + ".png";
            string Screenshot = screenshot.AsBase64EncodedString;
            byte[] screenshotAsByteArray = screenshot.AsByteArray;
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public void GetMethod(string url, string resources)
        {
            var endpoint = (url.ToString() + resources);
            var client = new RestClient(endpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            var result = client.Execute(request);
            content = result.Content;
            statusCode = result.StatusCode.ToString();
        }
        public object CallPostEndpoints(string url, object requestbody)
        {
            string text = "/worldpay/1086392";
            string endpoint = (url + text);
            client = new RestClient(endpoint);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");;
            request.AddParameter("application/json", requestbody, ParameterType.RequestBody);
            request.AddParameter("installationId", 1086392, ParameterType.GetOrPost);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;
        }

        public object CallRefundEndpoint(string url, object requestbody)
        {
            string text = "/worldpay/:1086392/refund";
            string endpoint = (url + text);
            client = new RestClient(endpoint);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json"); ;
            request.AddParameter("application/json", requestbody, ParameterType.RequestBody);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;
        }

        public object CallCancelEndpoint(string url, object requestbody)
        {
            string text = "/worldpay/1086392/cancel/abck";
            string endpoint = (url + text);
            client = new RestClient(endpoint);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json"); ;
            request.AddParameter("application/json", requestbody, ParameterType.RequestBody);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;
        }

        public object CallXmlPostProxyEndpoint(string url, object requestbody)
        {
            string resource = "/proxy";
            string endpoint = (url + resource);
            client = new RestClient(endpoint);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/xml");
            request.AddHeader("Content-Type", "application/xml"); ;
            request.AddParameter("application/xml", requestbody, ParameterType.RequestBody);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;
        }
        public object CallXmlPostMakePaymentEndpoint(string url, object requestbody)
        {
            string resource = "/PaymentService";
            string endpoint = (url + resource);
            client = new RestClient(endpoint);
            request = new RestRequest(Method.POST);
            request.AddHeader("accept", "text/xml charset=utf-8");
            request.AddHeader("Content-Type", "application/xml");
            request.AddParameter("application/xml", requestbody, ParameterType.RequestBody);
            var response = client.Execute(request);
            content = response.Content;
            statusCode = response.StatusCode.ToString();
            return response;
        }
    }
}


