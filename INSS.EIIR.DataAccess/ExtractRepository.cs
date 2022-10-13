using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using INSS.EIIR.Models.SubscriberModels;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.DataAccess
{
    public class ExtractRepository : IExtractRepository
    {
        private readonly EIIRExtractContext _context;
        private readonly DatabaseConfig _databaseConfig;

        public ExtractRepository(
            EIIRExtractContext eiirExtractContext,
            IOptions<DatabaseConfig> options)
        {
            _context = eiirExtractContext;
            _databaseConfig = options.Value;
        }

        public async Task<IList<Subscriber>> GetActiveSubscribers()
        {
            var subscribers = new List<Subscriber>();
            var results = await _context.SubscriberAccounts.Where(x => x.AccountActive.ToLower() == "y").ToListAsync();

            results.ForEach(x =>
            {
                subscribers.Add(new(x.SubscriberId, x.OrganisationName, x.AccountActive, x.SubscribedFrom, x.SubscribedTo));
            });

            return subscribers;
        }

        public async Task<SubscriberDetail> GetSubscriberDetails(string subscriberId)
        {
            SubscriberDetail subscriberDetail = null;
            if (int.TryParse(subscriberId, out var subscriberIdInt))
            {
                var result = await _context.SubscriberApplications.Where(x => x.SubscriberId == subscriberIdInt).FirstOrDefaultAsync();
                if (result != null)
                {
                    subscriberDetail = new(result.SubscriberId, result.OrganisationName, result.ContactTitle, result.ContactForename, result.ContactSurname, result.ContactEmail);
                }
            }
            return subscriberDetail;
        }

        public ExtractAvailable GetExtractAvailable()
        {
            ExtractAvailable extractAvailabe = null;
            string sql = $"EXEC {_databaseConfig.GetExtractAvailableProcedure}";
            var result = _context.ExtractAvailability.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
            if (result != null)
            {
                extractAvailabe = new(result.ExtractId, result.Currentdate, result.SnapshotCompleted, result.SnapshotDate,
                    result.ExtractCompleted, result.ExtractDate, result.ExtractEntries, result.ExtractBanks, result.ExtractIvas, result.NewCases,
                    result.NewBanks, result.NewIvas, result.ExtractFilename, result.DownloadLink, result.DownloadZiplink, result.ExtractDros, result.NewDros);
            }

            return extractAvailabe;
        }

        public void UpdateExtractAvailable()
        {
            string sql = $"EXEC {_databaseConfig.UpdateExtractAvailableProcedure}";
            _context.Database.ExecuteSqlRaw(sql);
        }
    }
}
