using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Infrastructure.Source.AzureTable
{
    public class AzureTableSource : IDataSource
    {
        public AzureTableSource(AzureTableSourceOptions options) { }

        public async Task<IEnumerable<InsolventIndividualRegisterModel>> GetInsolventIndividualRegistrationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
