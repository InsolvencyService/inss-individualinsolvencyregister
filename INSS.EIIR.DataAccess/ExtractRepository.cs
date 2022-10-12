using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IList<SubscriberAccount>> GetActiveSubscribers()
        {
            var results = await _context.SubscriberAccounts.Where(x => x.AccountActive.ToLower() == "y").ToListAsync();
            return results;
        }

        public async Task<SubscriberApplication> GetSubscriberDetails(string subscriberId)
        {
            if (int.TryParse(subscriberId, out var subscriberIdInt))
            {
                var result = await _context.SubscriberApplications.Where(x => x.SubscriberId == subscriberIdInt).FirstOrDefaultAsync();
                return result;
            }
            return null;
        }

        public ExtractAvailabilitySP GetExtractAvailability()
        {
            string sql = $"EXEC {_databaseConfig.GetExtractAvailableProcedure}";
            var result = _context.ExtractAvailability.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();

            return result;
        }

        public void UpdateExtractAvailability()
        {
            string sql = $"EXEC {_databaseConfig.UpdateExtractAvailableProcedure}";
            _context.Database.ExecuteSqlRaw(sql);
        }
    }
}
