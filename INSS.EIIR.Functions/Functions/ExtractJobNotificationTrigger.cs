//using INSS.EIIR.Interfaces.Services;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace INSS.EIIR.Functions.Functions
//{
//    public class ExtractJobNotificationTrigger
//    {
//        private readonly ILogger<ExtractJobNotificationTrigger> _logger;
//        private readonly ISubscriberService _subscriberService;

//        public ExtractJobNotificationTrigger(
//            ILogger<ExtractJobNotificationTrigger> log,
//            ISubscriberService subscriberService)
//        {
//            _logger = log;
//            _subscriberService = subscriberService;
//        }

//        [FunctionName("ExtractJobNotificationTrigger")]
//        public async Task Run([BlobTrigger("%blob:containername%/{name}.zip", Connection = "blob:connectionstring")] byte[] myBlob, string name, Uri uri)
//        {
//            string message = $"ExtractJobNotificationTrigger Blob trigger function triggered for blob\n Name: {name}  \n with uri:{uri.AbsoluteUri}";
//            _logger.LogInformation(message);

//            var activeSubscribers = await _subscriberService.GetActiveSubscribersAsync();

//            await _subscriberService.ScheduleSubscriberNotificationAsync(activeSubscribers);
//        }
//    }
//}
