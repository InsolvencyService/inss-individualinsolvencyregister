//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace INSS.EIIR.Functions.Functions
//{
//    public class ExtractJobScheduler
//    {
//        private readonly ILogger<ExtractJobScheduler> _logger;
//        private readonly IHttpClientFactory _httpClientFactory;

//        public ExtractJobScheduler(
//            ILogger<ExtractJobScheduler> log,
//            IHttpClientFactory httpClientFactory)
//        {
//            _logger = log;
//            _httpClientFactory = httpClientFactory;
//        }

//        [FunctionName("ExtractJobScheduler")]
//        public async Task Run([TimerTrigger("%extractjobtimercron%")]TimerInfo myTimer, ILogger log)
//        {
//            _logger.LogInformation($"ExtractJobScheduler function executed at: {DateTime.Now}");
//            _logger.LogInformation($"Next ExtractJobScheduler scheduled for: {myTimer.ScheduleStatus.Next}");

//            var jobServiceUrlError = "Extract Job Service Url is missing";

//            var extractJobServiceUrl = Environment.GetEnvironmentVariable("extractjobserviceurl");
//            if (string.IsNullOrEmpty(extractJobServiceUrl))
//            {
//                _logger.LogError(jobServiceUrlError);
//                throw new Exception(jobServiceUrlError);
//            }

//            var httpClient = _httpClientFactory.CreateClient();
//            var httpRequestMessage = new HttpRequestMessage(
//                HttpMethod.Post,
//                extractJobServiceUrl);

//            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
//            var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
           
//            if (!httpResponseMessage.IsSuccessStatusCode)
//            {
//                _logger.LogError(responseMessage);
//                throw new Exception(responseMessage);
//            }            

//            _logger.LogInformation(responseMessage);
//        }
//    }
//}
