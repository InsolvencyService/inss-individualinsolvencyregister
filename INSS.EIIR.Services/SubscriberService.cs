using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ILogger<SubscriberService> _logger;
        private readonly IExtractRepository _eiirRepository;
        private readonly INotificationService _notificationService; 
        private readonly NotifyConfig _notifyConfig;

        public SubscriberService(
            ILogger<SubscriberService> log,
            IExtractRepository eiirRepository,
            INotificationService notificationService,
            IOptions<NotifyConfig> options)
        {
            _logger = log;
            _eiirRepository = eiirRepository;
            _notificationService = notificationService; 
            _notifyConfig = options.Value;

            var subscriberEmailError = "ExtractjobTrigger missing configuration subscriberEmailTemplateId";

            if (string.IsNullOrEmpty(_notifyConfig.SubscriberEmailTemplateId))
            {
                _logger.LogError(subscriberEmailError);
                throw new Exception(subscriberEmailError);
            }
        }

        public async Task<IList<Subscriber>> GetActiveSubscribersAsync()
        {
            var subscribers = await _eiirRepository.GetActiveSubscribers();
            return subscribers;
        }

        public async Task ScheduleSubscriberNotificationAsync(IList<Subscriber> subscribers)
        {
            foreach (var subscriber in subscribers)
            {

                var subscriberNotFound = $"Subscribed details not found for subscriber Id: [{subscriber.SubscriberId}]";
                var subscriberDetails = await _eiirRepository.GetSubscriberDetails(subscriber.SubscriberId);
                if (subscriberDetails == null)
                {
                    _logger.LogError(subscriberNotFound);
                }

                var body = new
                {
                    TemplateId = _notifyConfig.SubscriberEmailTemplateId,
                    _notifyConfig.ApiKey,
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
                await _notificationService.CreateNotificationAsync(body, applicationProperties);
            }
        }
    }
}
