using INSS.EIIR.Data.Models;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SyncData;
using Microsoft.EntityFrameworkCore;


namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    /// <summary>
    /// Bankrupcties and IVAs from ISCIS
    /// </summary>
    public class EIIRLocalSQLIVAB : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly EIIRContext _eiirContext;
        private readonly SQLSourceOptions _options;

        public EIIRLocalSQLIVAB(SQLSourceOptions options)
        {

            var dbContextOptsBldr = new DbContextOptionsBuilder<EIIRContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(3600));

            _eiirContext = new EIIRContext(dbContextOptsBldr.Options);
            this._options = options;

        }

        public SyncDataEnums.Datasource Type => SyncDataEnums.Datasource.IscisBKTandIVA;

        public string Description => "ISCIS Bankruptcies and IVAs";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            await foreach (var x in _eiirContext.CaseResults.FromSqlRaw("exec getEiirIndexBIVAonly").AsAsyncEnumerable())
            {
                yield return _options.Mapper.Map<CaseResult, InsolventIndividualRegisterModel>(x);
            }
        }
    }
}
