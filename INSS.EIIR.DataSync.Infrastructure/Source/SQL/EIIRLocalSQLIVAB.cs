using INSS.EIIR.Data.Models;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    /// <summary>
    /// A temporary IDataSourceAsync<InsolventIndividualRegisterModel> implementation to be used as far as SIT environment
    /// To be used to test INSSight Integration functionality prior to actual INSSight feed being finalised
    /// </summary>
    public class EIIRLocalSQLIVAB : IDataSourceAsync<InsolventIndividualRegisterModel>
    {
        private readonly EIIRContext _eiirContext;
        private readonly SQLSourceOptions _options;

        public EIIRLocalSQLIVAB(SQLSourceOptions options)
        {

            var dbContextOptsBldr = new DbContextOptionsBuilder<EIIRContext>();
            dbContextOptsBldr.UseSqlServer(options.ConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(600));

            _eiirContext = new EIIRContext(dbContextOptsBldr.Options);
            this._options = options;

        }

        public async IAsyncEnumerable<InsolventIndividualRegisterModel> GetInsolventIndividualRegistrationsAsync()
        {
            await foreach (var x in _eiirContext.CaseResults.FromSqlRaw("exec getEiirIndexBIVAonly").AsAsyncEnumerable())
            {
                yield return _options.Mapper.Map<CaseResult, InsolventIndividualRegisterModel>(x);
            }
        }
    }
}
