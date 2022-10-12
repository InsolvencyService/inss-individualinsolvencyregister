using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Models.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class ExtractJobNotificationTrigger
    {
        private readonly ILogger<ExtractJobNotificationTrigger> _logger;
        private readonly IServiceBusMessageSender _serviceBusMessageSender;
        private readonly IExtractRepository _eiirRepository;
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly NotifyConfig _notifyConfig;

        public ExtractJobNotificationTrigger(
            ILogger<ExtractJobNotificationTrigger> log,
            IOptions<ServiceBusConfig> sbOptions,
            IOptions<NotifyConfig> nfOptions,
            IServiceBusMessageSender serviceBusMessageSender,
            IExtractRepository eiirRepository)
        {
            _logger = log;
            _serviceBusConfig = sbOptions.Value;
            _notifyConfig = nfOptions.Value;
            _serviceBusMessageSender = serviceBusMessageSender;
            _eiirRepository = eiirRepository;
        }

        [FunctionName("ExtractJobNotificationTrigger")]
        public async Task Run([BlobTrigger("%blob:containername%/{name}.zip", Connection = "blob:connectionstring")] byte[] myBlob, string name, Uri uri)
        {
            string message = $"ExtractJobNotificationTrigger Blob trigger function triggered for blob\n Name: {name}  \n with uri:{uri.AbsoluteUri}";
            _logger.LogInformation(message);

            if (string.IsNullOrEmpty(_serviceBusConfig.NotifyQueue))
            {
                _logger.LogError("ExtractjobTrigger missing configuration servicebus:notifyqueue");
                throw new Exception("ExtractjobTrigger missing configuration servicebus:notifyqueue");
            }

            if (string.IsNullOrEmpty(_notifyConfig.SubscriberEmailTemplateId))
            {
                _logger.LogError("ExtractjobTrigger missing configuration subscriberEmailTemplateId");
                throw new Exception("ExtractjobTrigger missing configuration subscriberEmailTemplateId");
            }

            // 1. Get a list of Active subscribers
            var activeSubscribers = await _eiirRepository.GetActiveSubscribers();

            // 2. Send Notification message that subscriber file has been generated
            foreach (var subscriber in activeSubscribers)
            {
                var subscriberDetails = await _eiirRepository.GetSubscriberDetails(subscriber.SubscriberId);
                if (subscriberDetails == null)
                {
                    _logger.LogError($"Subscribed details not found for subscriber Id: [{subscriber.SubscriberId}]");
                    throw new Exception($"Subscribed details not found for subscriber Id: [{subscriber.SubscriberId}]");
                }

                var body = new
                {
                    TemplateId = _notifyConfig.SubscriberEmailTemplateId,
                    ApiKey = _notifyConfig.ApiKey,
                    EmailAddress = subscriberDetails.ContactEmail,
                    Personalisation = new Dictionary<string, dynamic>
                    {
                        { "subscriber_id", subscriber.SubscriberId },
                        { "subscriber_name", subscriber.OrganisationName },
                        { "date", DateOnly.FromDateTime(DateTime.Now).ToString("dd MMMM yyyy") }
                    }
                };

                var applicationProperties = new Dictionary<string, object>()
                {
                    { "NotificationType", "Email"},
                };
                await _serviceBusMessageSender.SendMessageAsync(body, _serviceBusConfig.NotifyQueue, applicationProperties);
            }
        }
    }
}
