using INSS.EIIR.Interfaces.Messaging;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Interfaces.Storage;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using INSS.EIIR.Models.NotificationModels;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Services;
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;
    private readonly ITableStorageRepository<EiirExtractNotification> _tableStorageRepository;
    private readonly ServiceBusConfig _serviceBusConfig;
    private readonly NotifyConfig _notifyConfig;

    public NotificationService(
        ILogger<NotificationService> log,
        IOptions<ServiceBusConfig> sbOptions,
        IOptions<NotifyConfig> nfOptions,
        IServiceBusMessageSender serviceBusMessageSender,
        ITableStorageRepository<EiirExtractNotification> tableStorageRepository)
    {
        _logger = log;  
        _serviceBusConfig = sbOptions.Value;
        _notifyConfig = nfOptions.Value;
        _serviceBusMessageSender = serviceBusMessageSender;
        _tableStorageRepository = tableStorageRepository;

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
        await _serviceBusMessageSender.SendNotifyMessageAsync(message, _serviceBusConfig.NotifyQueue, properties);
    }

    public async Task ScheduleSubscriberNotificationAsync(string filename, IEnumerable<Subscriber> subscribers)
    {
        var currentDt = DateTime.UtcNow;
        var sentNotificationsForFile = GetNotificationsForFile(filename);

        foreach (var subscriber in subscribers)
        {
            foreach (var contact in subscriber.EmailContacts)
            {
                if (sentNotificationsForFile.Any(x => x.SubscriberId == subscriber.SubscriberId && x.EmailAddress == contact.EmailAddress)) continue;

                var body = new
                {
                    TemplateId = _notifyConfig.SubscriberEmailTemplateId,
                    _notifyConfig.ApiKey,
                    contact.EmailAddress,
                    Personalisation = new Dictionary<string, dynamic>
                    {
                        { "subscriber_id", subscriber.SubscriberId },
                        { "subscriber_name", subscriber.OrganisationName },
                        { "date", DateOnly.FromDateTime(currentDt).ToString("dd MMMM yyyy") }
                    }
                };

                var applicationProperties = new Dictionary<string, object>()
                {
                    { "NotificationType", "Email"},
                };
                await CreateNotificationAsync(body, applicationProperties);
                await CreateExtractJobNotification(subscriber.SubscriberId, filename, contact.EmailAddress, currentDt);
            }
        }
    }

    public async Task SendNotificationAsync(NotifcationDetail notification)
    {
        var recipients = notification.Recipients.Split(',');
     
        foreach (var recipient in  recipients)
        { 
            var body = new
            {
                notification.TemplateId,
                _notifyConfig.ApiKey,
                EmailAddress = recipient,
                Personalisation = new Dictionary<string, dynamic>
                {
                    { "subject", notification.Subject },
                    { "body", notification.Body }
                }
            };

            var applicationProperties = new Dictionary<string, object>()
            {
                { "NotificationType", "Email"}
            };
            await CreateNotificationAsync(body, applicationProperties);
        }
    }

    private async Task CreateExtractJobNotification(string subscriberId, string filename, string emailAddress, DateTime sent)
    {
        await _tableStorageRepository.AddEntity(new EiirExtractNotification { SubscriberId = subscriberId, Filename = filename, EmailAddress = emailAddress, Sent = sent });
    }

    private IEnumerable<EiirExtractNotification> GetNotificationsForFile(string filename)
    {
        return  _tableStorageRepository.Query(x => x.Filename.Equals(filename));
    }
}
