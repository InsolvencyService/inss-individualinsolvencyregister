using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        var results = await (from sa in _context.SubscriberAccounts
                                orderby sa.SubscribedTo descending,
                                        sa.OrganisationName
                                    join apps in _context.SubscriberApplications
                                    on new { Id = sa.SubscriberId } equals new { Id = apps.SubscriberId.ToString() }
                                    select new { SubscriberAccounts = sa, SubscriberApplications = apps }).ToListAsync();

        var contacts = await _context.SubscriberContacts.ToListAsync();
                
        results.ToList().ForEach(s =>
        {
            var subContacts = contacts.Where(sc => s.SubscriberAccounts.SubscriberId == sc.SubscriberId).ToList();
            var subscriber = _mapper.Map<SubscriberAccount, Subscriber>(s.SubscriberAccounts);
            var application = _mapper.Map<SubscriberApplication, SubscriberDetail>(s.SubscriberApplications);
            var emailContacts = _mapper.Map<IList<SubscriberContact>, IList<SubscriberEmailContact>>(subContacts);

            subscriber.SubscriberDetails = application;
            subscriber.EmailContacts = emailContacts;
            subscribers.Add(subscriber);
        });

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

    public async Task CreateSubscriberAsync(CreateUpdateSubscriber subscriber)
    {
        string sql = "EXEC subscriber_create ";
        await CreateUpdateSubscriber(sql, subscriber);
    }

    public async Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber)
    {
        string sql = "EXEC subscriber_update @SubscriberId, ";
        await CreateUpdateSubscriber(sql, subscriber, subscriberId);
    }

    public async Task CreateSubscriberDownload(string subscriberId, SubscriberDownloadDetail subscriberDownload)
    {
        string sql = "EXEC subscr_download_INS @ExtractID, @SubscriberID, @DownloadIPAddress, @DownloadServer, @ExtractZipDownload";

        List<SqlParameter> sqlParams = new()
        {
            new SqlParameter { ParameterName = "@ExtractID", Value = subscriberDownload.ExtractId },
            new SqlParameter { ParameterName = "@SubscriberID", Value = subscriberId },
            new SqlParameter { ParameterName = "@DownloadIPAddress", Value = subscriberDownload.IPAddress },
            new SqlParameter { ParameterName = "@DownloadServer", Value = subscriberDownload.Server },
            new SqlParameter { ParameterName = "@ExtractZipDownload", Value = subscriberDownload.ExtractZipDownload },
        };

        await _context.Database.ExecuteSqlRawAsync(sql, sqlParams.ToArray());
    }

    private async Task CreateUpdateSubscriber(string sql, CreateUpdateSubscriber subscriber, string subscriberId = null)
    {
        List<SqlParameter> sqlParams = new();

        sql = sql +
            "@OrganisationName, " +
            "@OrganisationType, " +
            "@ContactForename, " +
            "@ContactSurname," +
            "@ContactAddress1, " +
            "@ContactAddress2, " +
            "@ContactCity, " +
            "@ContactPostcode, " +
            "@ContactTelephone, " +
            "@ContactEmail, " +
            "@ApplicationDate, " +
            "@SubscribedFrom, " +
            "@SubscribedTo, " +
            "@AccountActive, " +
            "@EmailContacts";

        var emailContacts = "";
        if (subscriber.EmailAddresses.Any())
        {
            var emails = subscriber.EmailAddresses.Where(x => !string.IsNullOrWhiteSpace(x));
            emailContacts = string.Join(",", emails);
            emailContacts = emailContacts.TrimEnd(',');
        }

        if (!string.IsNullOrEmpty(subscriberId)) {
            sqlParams.Add(new SqlParameter { ParameterName = "@SubscriberId", Value = subscriberId });
        }

        List<SqlParameter> sqlParamsCore = new()
        {
            new SqlParameter { ParameterName = "@OrganisationName", Value = subscriber.OrganisationName },
            new SqlParameter { ParameterName = "@OrganisationType", Value = subscriber.OrganisationType },
            new SqlParameter { ParameterName = "@ContactForename", Value = subscriber.ContactForename },
            new SqlParameter { ParameterName = "@ContactSurname", Value = subscriber.ContactSurname },
            new SqlParameter { ParameterName = "@ContactAddress1", Value = subscriber.ContactAddress1 },
            new SqlParameter { ParameterName = "@ContactAddress2", Value = subscriber.ContactAddress2 },
            new SqlParameter { ParameterName = "@ContactCity", Value = subscriber.ContactCity },
            new SqlParameter { ParameterName = "@ContactPostcode", Value = subscriber.ContactPostcode },
            new SqlParameter { ParameterName = "@ContactTelephone", Value = subscriber.ContactTelephone },
            new SqlParameter { ParameterName = "@ContactEmail", Value = subscriber.ContactEmail },
            new SqlParameter { ParameterName = "@ApplicationDate", Value = subscriber.ApplicationDate },
            new SqlParameter { ParameterName = "@AccountActive", Value = subscriber.AccountActive },
            new SqlParameter { ParameterName = "@EmailContacts", Value = emailContacts },
        };

        if (subscriber.SubscribedFrom.HasValue) {
            sqlParamsCore.Add(new SqlParameter { ParameterName = "@SubscribedFrom", Value = subscriber.SubscribedFrom });
        } else {
            sqlParamsCore.Add(new SqlParameter { ParameterName = "@SubscribedFrom", Value = DBNull.Value });
        }

        if (subscriber.SubscribedTo.HasValue) {
            sqlParamsCore.Add(new SqlParameter { ParameterName = "@SubscribedTo", Value = subscriber.SubscribedTo });
        } else {
            sqlParamsCore.Add(new SqlParameter { ParameterName = "@SubscribedTo", Value = DBNull.Value });
        }

        sqlParams.AddRange(sqlParamsCore);
        await _context.Database.ExecuteSqlRawAsync(sql, sqlParams.ToArray());
    }
}
