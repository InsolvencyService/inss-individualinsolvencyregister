using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface ISubscriberRepository
{
    Task<IEnumerable<Subscriber>> GetSubscribersAsync();

    Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);
}
