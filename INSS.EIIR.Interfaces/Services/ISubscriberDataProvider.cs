using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services;

public interface ISubscriberDataProvider
{
    Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);
    Task<IEnumerable<Subscriber>> GetSubscribersAsync();
    Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync();
    Task<IEnumerable<Subscriber>> GetInActiveSubscribersAsync();
}
