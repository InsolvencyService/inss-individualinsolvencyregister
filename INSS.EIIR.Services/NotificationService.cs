using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IServiceBusMessageSender _serviceBusMessageSender;
        private readonly ServiceBusConfig _serviceBusConfig;

        public NotificationService(
            ILogger<NotificationService> log,
            IOptions<ServiceBusConfig> sbOptions,
            IOptions<NotifyConfig> nfOptions,
            IServiceBusMessageSender serviceBusMessageSender)
        {
            _logger = log;  
            _serviceBusConfig = sbOptions.Value;
            _serviceBusMessageSender = serviceBusMessageSender;

            var notifyqueueError = "ExtractjobTrigger missing configuration servicebus:notifyqueue";

            if (string.IsNullOrEmpty(_serviceBusConfig.NotifyQueue))
            {
                _logger.LogError(notifyqueueError);
                throw new Exception(notifyqueueError);
            }
        }

        public async Task CreateNotificationAsync<T>(T message, Dictionary<string, object> properties)
        {           
            await _serviceBusMessageSender.SendMessageAsync(message, _serviceBusConfig.NotifyQueue, properties);
        }
    }
}
