using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using Microsoft.EntityFrameworkCore;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public  class EiirSQLSource : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly EIIRContext _eiirContext;
        private readonly SQLSourceOptions _options;

        public EiirSQLSource(SQLSourceOptions options) {

            var dbContextOptsBldr = new DbContextOptionsBuilder<EIIRContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(0));

            _eiirContext = new EIIRContext(dbContextOptsBldr.Options);
            this._options = options;

        }

        public SyncDataEnums.Datasource Type => SyncDataEnums.Datasource.IscisDRO;

        public string Description => "ISCIS Debt Relief Orders";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            await foreach (var x in _eiirContext.CaseResults.FromSqlRaw("exec getEiirIndexDROonly").AsAsyncEnumerable())
            {
                yield return _options.Mapper.Map<CaseResult, InsolventIndividualRegisterModel>(x);
            }
        }
    }
}
