using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services
{
    public interface ISubscriberService
    {
        Task<IList<Subscriber>> GetActiveSubscribersAsync();
        Subscriber GetSubscriber(int subscriberId);
        Task ScheduleSubscriberNotificationAsync(IList<Subscriber> subscribers);
    }
}
