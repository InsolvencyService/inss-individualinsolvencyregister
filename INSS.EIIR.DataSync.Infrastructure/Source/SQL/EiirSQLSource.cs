using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL.Context;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public  class EiirSQLSource : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly EIIRContext _eiirContext;
        private readonly SQLSourceOptions _options;

        public EiirSQLSource(SQLSourceOptions options) {

            var dbContextOptsBldr = new DbContextOptionsBuilder<EIIRContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));

            _eiirContext = new EIIRContext(dbContextOptsBldr.Options);
            this._options = options;

        }

        public SyncData.Datasource Type => SyncData.Datasource.IscisDRO;

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
