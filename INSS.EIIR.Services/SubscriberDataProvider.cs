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
                                .Take(pagingParameters.PageSize);

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
        var totalSubscribers = await _subscriberRepository.GetSubscribersAsync();
        var pagedSubscribers = totalSubscribers
                                .Where(s => s.SubscribedFrom <= DateTime.Today && s.SubscribedTo >= DateTime.Today)
                                .Skip(pagingParameters.Skip)
                                .Take(pagingParameters.PageSize);

        var response = new SubscriberWithPaging
        {
            Paging = new Models.PagingModel(totalSubscribers.Count(), pagingParameters.PageNumber, pagingParameters.PageSize),
            Subscribers = pagedSubscribers
        };

        return response;
    }

    public async Task<SubscriberWithPaging> GetInActiveSubscribersAsync(PagingParameters pagingParameters)
    {
        var totalSubscribers = await _subscriberRepository.GetSubscribersAsync();
        var pagedSubscribers = totalSubscribers
                                .Where(s => s.SubscribedTo < DateTime.Today)
                                .Skip(pagingParameters.Skip)
                                .Take(pagingParameters.PageSize);
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
}
