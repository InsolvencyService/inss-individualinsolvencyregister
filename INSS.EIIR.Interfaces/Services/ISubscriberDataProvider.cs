using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Services;

public interface ISubscriberDataProvider
{
    Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);
    Task<SubscriberWithPaging> GetSubscribersAsync(PagingParameters pagingParameters);
    Task<SubscriberWithPaging> GetActiveSubscribersAsync(PagingParameters pagingParameters);
    Task<SubscriberWithPaging> GetInActiveSubscribersAsync(PagingParameters pagingParameters);
    Task CreateSubscriberAsync(CreateUpdateSubscriber subscriber);
    Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber);
}
