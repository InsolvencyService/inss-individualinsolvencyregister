using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services;

public interface ISubscriberDataProvider
{
    Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);
    Task<IEnumerable<Subscriber>> GetSubscribersAsync(PagingParameters pagingParameters);
    Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync(PagingParameters pagingParameters);
    Task<IEnumerable<Subscriber>> GetInActiveSubscribersAsync(PagingParameters pagingParameters);
}
