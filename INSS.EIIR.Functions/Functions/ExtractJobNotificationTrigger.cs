using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;

using Microsoft.Azure.Functions.Worker;

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions;

public class ExtractJobNotificationTrigger
{
    private readonly ILogger<ExtractJobNotificationTrigger> _logger;
    private readonly ISubscriberDataProvider _subscriberService;
    private readonly INotificationService _notificationService;

    public ExtractJobNotificationTrigger(
        ILogger<ExtractJobNotificationTrigger> log,
        ISubscriberDataProvider subscriberService,
        INotificationService notificationService)
    {
        _logger = log;
        _subscriberService = subscriberService;
        _notificationService = notificationService;
    }

    [Function("ExtractJobNotificationTrigger")]
    public async Task Run([BlobTrigger("%blobcontainername%/{name}.zip", Connection = "storageconnectionstring")] byte[] myBlob, string name, Uri uri)
    {
        string message = $"ExtractJobNotificationTrigger Blob trigger function triggered for blob\n Name: {name}  \n with uri:{uri.AbsoluteUri}";
        _logger.LogInformation(message);

        var activeSubscribers = await _subscriberService.GetActiveSubscribersAsync(new PagingParameters() { PageSize = 1000 });

        await _notificationService.ScheduleSubscriberNotificationAsync(name, activeSubscribers.Subscribers);
    }
}
