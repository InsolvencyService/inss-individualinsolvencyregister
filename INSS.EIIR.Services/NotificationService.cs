using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Services;
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;
    private readonly ServiceBusConfig _serviceBusConfig;
    private readonly NotifyConfig _notifyConfig;

    public NotificationService(
        ILogger<NotificationService> log,
        IOptions<ServiceBusConfig> sbOptions,
        IOptions<NotifyConfig> nfOptions,
        IServiceBusMessageSender serviceBusMessageSender)
    {
        _logger = log;  
        _serviceBusConfig = sbOptions.Value;
        _notifyConfig = nfOptions.Value;
        _serviceBusMessageSender = serviceBusMessageSender;

        var notifyqueueError = "ExtractjobTrigger missing configuration servicebus__notifyqueue";
        var subscriberEmailError = "ExtractjobTrigger missing configuration subscriberEmailTemplateId";

        if (string.IsNullOrEmpty(_serviceBusConfig.NotifyQueue))
        {
            _logger.LogError(notifyqueueError);
            throw new Exception(notifyqueueError);
        }

        if (string.IsNullOrEmpty(_notifyConfig.SubscriberEmailTemplateId))
        {
            _logger.LogError(subscriberEmailError);
            throw new Exception(subscriberEmailError);
        }
    }

    public async Task CreateNotificationAsync<T>(T message, Dictionary<string, object> properties)
    {           
        await _serviceBusMessageSender.SendMessageAsync(message, _serviceBusConfig.NotifyQueue, properties);
    }

    public async Task ScheduleSubscriberNotificationAsync(IEnumerable<Subscriber> subscribers)
    {
        foreach (var subscriber in subscribers)
        {
            foreach (var contact in subscriber.EmailContacts)
            {
                var body = new
                {
                    TemplateId = _notifyConfig.SubscriberEmailTemplateId,
                    _notifyConfig.ApiKey,
                    contact.EmailAddress,
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
                await CreateNotificationAsync(body, applicationProperties);
            }
        }
    }
}
