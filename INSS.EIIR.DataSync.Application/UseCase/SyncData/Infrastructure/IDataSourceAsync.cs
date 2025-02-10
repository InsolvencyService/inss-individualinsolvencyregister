using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public interface IDataSourceAsync<T>
    {
        IAsyncEnumerable<T> GetInsolventIndividualRegistrationsAsync();

        SyncDataEnums.Datasource Type { get; }

        string Description { get; }
    }
}
