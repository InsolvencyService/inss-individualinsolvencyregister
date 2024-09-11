using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL
{
    public class SQLSource : IDataSource
    {
        public SQLSource(SQLSourceOptions options) { }

        public async Task<IEnumerable<InsolventIndividualRegisterModel>> GetInsolventIndividualRegistrationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
