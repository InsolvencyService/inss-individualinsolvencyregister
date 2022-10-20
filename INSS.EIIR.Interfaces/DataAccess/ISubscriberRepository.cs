using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface ISubscriberRepository
{
    Task<IEnumerable<Subscriber>> GetSubscribersAsync();

    Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);

    Task CreateSubscriberAsync(CreateUpdateSubscriber subscriber);

    Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber);
}