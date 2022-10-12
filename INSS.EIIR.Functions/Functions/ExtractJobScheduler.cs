using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class ExtractJobScheduler
    {
        private readonly ILogger<ExtractJobScheduler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExtractJobScheduler(
            ILogger<ExtractJobScheduler> log,
            IHttpClientFactory httpClientFactory)
        {
            _logger = log;
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName("ExtractJobScheduler")]
        public async Task Run([TimerTrigger("%extractjobtimercron%")]TimerInfo myTimer, ILogger log)
        {
            _logger.LogInformation($"ExtractJobScheduler function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next ExtractJobScheduler scheduled for: {myTimer.ScheduleStatus.Next}");

            var extractjobserviceurl = Environment.GetEnvironmentVariable("extractjobserviceurl");
            if (string.IsNullOrEmpty(extractjobserviceurl))
            {
                _logger.LogError("Extract Job Service Url is missing");
                throw new Exception("Extract Job Service Url is missing");
            }

            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                extractjobserviceurl);

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError($"ExtractJobScheduler failed to respond successfully on: {DateTime.Now}");
                throw new Exception();
            }
            var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();

            _logger.LogInformation($"{responseMessage}");
        }
    }
}
