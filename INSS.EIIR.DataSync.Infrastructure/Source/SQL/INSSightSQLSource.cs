using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL.Context;
using INSS.EIIR.Models.SyncData;
using Microsoft.EntityFrameworkCore;


namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public class INSSightSQLSource : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly ExternalInssContext _externalInssContext;
        private readonly SQLSourceOptions _options;

        public INSSightSQLSource(SQLSourceOptions options) 
        {
            var dbContextOptsBldr = new DbContextOptionsBuilder<ExternalInssContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(3600));

            _externalInssContext = new ExternalInssContext(dbContextOptsBldr.Options);
            this._options = options;
        }

        public SyncDataEnums.Datasource Type =>  SyncDataEnums.Datasource.InnSightBKTandIVA;

        public string Description => "INSSight Bankruptcies and IVAs";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {

            //The following where clause is temporary measure, to retrieve some records from INSSight should it needed "Bankruptcy" should turned into a constant
            await foreach (var x in _externalInssContext.VwEiirs.AsNoTracking().AsAsyncEnumerable())
            {
                yield return _options.Mapper.Map<Models.VwEiir, InsolventIndividualRegisterModel>(x);
            }

        }
    }
}
