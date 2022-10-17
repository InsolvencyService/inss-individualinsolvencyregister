using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Services;
public class SubscriberDataProvider : ISubscriberDataProvider
{
    private readonly ISubscriberRepository _subscriberRepository;

    public SubscriberDataProvider(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
    }

    public async Task<IEnumerable<Subscriber>> GetSubscribersAsync()
    {
        return await _subscriberRepository.GetSubscribersAsync();
    }

    public async Task<Subscriber> GetSubscriberByIdAsync(string subscriberId)
    {
        return await _subscriberRepository.GetSubscriberByIdAsync(subscriberId);
    }

    public async Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync()
    {
        return (await _subscriberRepository.GetSubscribersAsync()).Where(s => s.SubscribedFrom <= DateTime.Today && s.SubscribedTo >= DateTime.Today);
    }

    public async Task<IEnumerable<Subscriber>> GetInActiveSubscribersAsync()
    {
        return (await _subscriberRepository.GetSubscribersAsync()).Where(s => s.SubscribedTo < DateTime.Today);
    }
}
