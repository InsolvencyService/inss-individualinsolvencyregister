using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Web.Services;

public class SubscriberSearch : ISubscriberSearch
{
    private readonly IClientService _clientService;
    
    public SubscriberSearch(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<Subscriber> GetSubscriberByIdAsync(int subscriberId)
    {
        const string apiUrl = "subscribers";

        var parameters = new List<string>
        {
             subscriberId.ToString()
        };

        var result = await _clientService.GetAsync<Subscriber>(apiUrl, parameters);

        return result;
    }

    public async Task<SubscriberWithPaging> GetSubscribersAsync(PagingParameters pagingParameters, string apiUrl = "subscribers")
    {
        var result = await _clientService.PostAsync<PagingParameters, SubscriberWithPaging>(apiUrl, pagingParameters);

        return result;
    }

    public async Task<SubscriberWithPaging> GetActiveSubscribersAsync(PagingParameters pagingParameters)
    {
        const string apiUrl = "subscribers/active";

        return await GetSubscribersAsync(pagingParameters, apiUrl);
    }

    public async Task<SubscriberWithPaging> GetInActiveSubscribersAsync(PagingParameters pagingParameters)
    {
        const string apiUrl = "subscribers/inactive";

        return await GetSubscribersAsync(pagingParameters, apiUrl);
    }
}