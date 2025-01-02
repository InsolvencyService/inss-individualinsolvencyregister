using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL.Context;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public class INSSightSQLSource : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly ExternalInssContext _externalInssContext;
        private readonly SQLSourceOptions _options;

        public INSSightSQLSource(SQLSourceOptions options) 
        {
            var dbContextOptsBldr = new DbContextOptionsBuilder<ExternalInssContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));

            _externalInssContext = new ExternalInssContext(dbContextOptsBldr.Options);
            this._options = options;
        }

        public SyncData.Datasource Type =>  SyncData.Datasource.InnSightBKTandIVA;

        public string Description => "INSSight Bankruptcies and IVAs";

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {

            //The following where clause is temporary measure, to retrieve some records from INSSight should it needed "Bankruptcy" should turned into a constant
            await foreach (var x in _externalInssContext.VwEiirs.Where(p => p.InsolvencyType == "Bankruptcy" && p.HasRestrictions == 1).AsNoTracking().AsAsyncEnumerable())
            {
                yield return _options.Mapper.Map<Models.VwEiir, InsolventIndividualRegisterModel>(x);
            }

        }
    }
}
