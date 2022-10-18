using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataAccess;

public class SubscriberRepository : ISubscriberRepository
{
    private readonly EIIRContext _context;
    private readonly IMapper _mapper;

    public SubscriberRepository(
        EIIRContext eiirContext,
        IMapper mapper)
    {
        _context = eiirContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Subscriber>> GetSubscribersAsync()
    {
        List<Subscriber> subscribers = new();

        try
        {

            var results = await (from sa in _context.SubscriberAccounts
                                 orderby sa.SubscriberId
                                 join apps in _context.SubscriberApplications
                                   on new { Id = sa.SubscriberId } equals new { Id = apps.SubscriberId.ToString() }
                                 select new { SubscriberAccounts = sa, SubscriberApplications = apps }).ToListAsync();

            var contacts = await _context.SubscriberContacts.ToListAsync();
                
            results.ToList().ForEach(s =>
            {
                subscribers.Add(_mapper.Map<SubscriberAccount, Subscriber>(s.SubscriberAccounts));
                subscribers.ToList().ForEach(x =>
                {
                    var subContacts = contacts.Where(u => x.SubscriberId == u.SubscriberId).ToList();
                    x.SubscriberDetails = _mapper.Map<SubscriberApplication, SubscriberDetail>(s.SubscriberApplications);
                    x.EmailContacts = _mapper.Map<IList<SubscriberContact>, IList<SubscriberEmailContact>>(subContacts);
                });
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return subscribers;
    }
    
    public async Task<Subscriber> GetSubscriberByIdAsync(string subscriberId)
    {
        Subscriber subscriber = null;

        var result = await (from sa in _context.SubscriberAccounts.Where(s => s.SubscriberId.Equals(subscriberId))
                             join apps in _context.SubscriberApplications
                               on new { Id = sa.SubscriberId } equals new { Id = apps.SubscriberId.ToString() }
                            select new { SubscriberAccount = sa, SubscriberApplication = apps }).FirstOrDefaultAsync();

        var contacts = await _context.SubscriberContacts.Where(x => x.SubscriberId == subscriberId).ToListAsync();

        if (result == null) return subscriber;

        subscriber = _mapper.Map<SubscriberAccount, Subscriber>(result.SubscriberAccount);
        subscriber.SubscriberDetails = _mapper.Map<SubscriberApplication, SubscriberDetail>(result.SubscriberApplication);
        subscriber.EmailContacts = _mapper.Map<IList<SubscriberContact>, IList<SubscriberEmailContact>>(contacts).ToList();

        return subscriber;
    }
}
