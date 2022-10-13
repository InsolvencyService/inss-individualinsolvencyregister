using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using INSS.EIIR.Models.SubscriberModels;
using INSS.EIIR.Models.ExtractModels;
using AutoMapper;
using INSS.EIIR.Models.IndexModels;

namespace INSS.EIIR.DataAccess
{
    public class ExtractRepository : IExtractRepository
    {
        private readonly EIIRExtractContext _context;
        private readonly DatabaseConfig _databaseConfig;
        private readonly IMapper _mapper;

        public ExtractRepository(
            EIIRExtractContext eiirExtractContext,
            IOptions<DatabaseConfig> options,
            IMapper mapper)
        {
            _context = eiirExtractContext;
            _databaseConfig = options.Value;
            _mapper = mapper;
        }

        public async Task<IList<Subscriber>> GetActiveSubscribers()
        {
            var results = await _context.SubscriberAccounts.Where(x => x.AccountActive.ToLower() == "y").ToListAsync();
            var subscribers = _mapper.Map<IList<SubscriberAccount>, IList<Subscriber>>(results).ToList();

            return subscribers;
        }

        public async Task<SubscriberDetail> GetSubscriberDetails(string subscriberId)
        {
            SubscriberDetail subscriberDetail = null;
            if (int.TryParse(subscriberId, out var subscriberIdInt))
            {
                var result = await _context.SubscriberApplications.Where(x => x.SubscriberId == subscriberIdInt).FirstOrDefaultAsync();
                subscriberDetail = _mapper.Map<SubscriberApplication, SubscriberDetail>(result);
            }
            return subscriberDetail;
        }

        public ExtractAvailable GetExtractAvailable()
        {
            string sql = $"EXEC {_databaseConfig.GetExtractAvailableProcedure}";
            var result = _context.ExtractAvailability.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
            ExtractAvailable extractAvailabe = _mapper.Map<ExtractAvailabilitySP, ExtractAvailable>(result);

            return extractAvailabe;
        }

        public void UpdateExtractAvailable()
        {
            string sql = $"EXEC {_databaseConfig.UpdateExtractAvailableProcedure}";
            _context.Database.ExecuteSqlRaw(sql);
        }
    }
}
