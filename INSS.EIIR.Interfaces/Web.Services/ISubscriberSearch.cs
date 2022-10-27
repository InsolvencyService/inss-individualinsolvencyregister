using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Web.Services;

public interface ISubscriberSearch
{
    Task<Subscriber> GetSubscriberByIdAsync(int subscriberId);

    Task<SubscriberWithPaging> GetSubscribersAsync(PagingParameters pagingParameters, string apiUrl = "subscribers");

    Task<SubscriberWithPaging> GetActiveSubscribersAsync(PagingParameters pagingParameters);

    Task<SubscriberWithPaging> GetInActiveSubscribersAsync(PagingParameters pagingParameters);
}