using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Services;
public class SubscriberDataProvider : ISubscriberDataProvider
{
    private readonly ISubscriberRepository _subscriberRepository;

    public SubscriberDataProvider(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
    }

    public async Task<SubscriberWithPaging> GetSubscribersAsync(PagingParameters pagingParameters)
    {
        var totalSubscribers = await _subscriberRepository.GetSubscribersAsync();
        var pagedSubscribers = totalSubscribers
                                .Skip(pagingParameters.Skip)
                                .Take(pagingParameters.PageSize)
                                .ToList();

        var response = new SubscriberWithPaging
        {
            Paging = new Models.PagingModel(totalSubscribers.Count(), pagingParameters.PageNumber, pagingParameters.PageSize),
            Subscribers = pagedSubscribers
        };

        return response;
    }

    public async Task<Subscriber> GetSubscriberByIdAsync(string subscriberId)
    {
        return await _subscriberRepository.GetSubscriberByIdAsync(subscriberId);
    }

    public async Task<SubscriberWithPaging> GetActiveSubscribersAsync(PagingParameters pagingParameters)
    {
        var totalSubscribers = (await _subscriberRepository.GetSubscribersAsync()).Where(s => s.SubscribedFrom <= DateTime.Today && s.SubscribedTo >= DateTime.Today && s.AccountActive.ToUpperInvariant() == "Y");
        var pagedSubscribers = totalSubscribers
                                .Where(s => s.SubscribedFrom <= DateTime.Today && s.SubscribedTo >= DateTime.Today && s.AccountActive.ToUpperInvariant() == "Y")
                                .Skip(pagingParameters.Skip)
                                .Take(pagingParameters.PageSize)
                                .ToList();

        var response = new SubscriberWithPaging
        {
            Paging = new Models.PagingModel(totalSubscribers.Count(), pagingParameters.PageNumber, pagingParameters.PageSize),
            Subscribers = pagedSubscribers
        };

        return response;
    }

    public async Task<SubscriberWithPaging> GetInActiveSubscribersAsync(PagingParameters pagingParameters)
    {
        var totalSubscribers = (await _subscriberRepository.GetSubscribersAsync()).Where(s => s.SubscribedTo < DateTime.Today || s.AccountActive.ToUpperInvariant() == "N");
        var pagedSubscribers = totalSubscribers
                                .Where(s => s.SubscribedTo < DateTime.Today || s.AccountActive.ToUpperInvariant() == "N")
                                .Skip(pagingParameters.Skip)
                                .Take(pagingParameters.PageSize)
                                .ToList();
        var response = new SubscriberWithPaging
        {
            Paging = new Models.PagingModel(totalSubscribers.Count(), pagingParameters.PageNumber, pagingParameters.PageSize),
            Subscribers = pagedSubscribers
        };

        return response;
    }

    public async Task CreateSubscriberAsync(CreateUpdateSubscriber subscriber)
    {
        await _subscriberRepository.CreateSubscriberAsync(subscriber);
    }

    public async Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber)
    {
        await _subscriberRepository.UpdateSubscriberAsync(subscriberId, subscriber);
    }

    public async Task CreateSubscriberDownload(string subscriberId, SubscriberDownloadDetail subscriberDownload)
    {
        await _subscriberRepository.CreateSubscriberDownload(subscriberId, subscriberDownload);
    }

    public async Task<bool?> IsSubscriberActiveAsync(string subscriberId)
    {
        var subscriber = await GetSubscriberByIdAsync(subscriberId);
        if (subscriber == null) {
            return null;
        }

        var isActive = (subscriber.SubscribedFrom <= DateTime.Today && subscriber.SubscribedTo >= DateTime.Today && subscriber.AccountActive.ToUpperInvariant() == "Y");
        return isActive;
    }
}
