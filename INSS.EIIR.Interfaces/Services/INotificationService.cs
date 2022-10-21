using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services;

public interface INotificationService
{
    Task CreateNotificationAsync<T>(T message, Dictionary<string, object> properties);

    Task ScheduleSubscriberNotificationAsync(IEnumerable<Subscriber> subscribers);
}
