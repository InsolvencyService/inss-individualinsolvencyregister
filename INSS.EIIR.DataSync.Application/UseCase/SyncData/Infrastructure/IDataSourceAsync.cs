
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public interface IDataSourceAsync<T>
    {
        IAsyncEnumerable<T> GetInsolventIndividualRegistrationsAsync();

        Models.Constants.SyncData.Datasource Type { get; }

        string Description { get; }
    }
}
