using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services
{
    public interface ISubscriberService
    {
        Task<IList<Subscriber>> GetActiveSubscribersAsync();
        Task ScheduleSubscriberNotificationAsync(IList<Subscriber> subscribers);
    }
}
