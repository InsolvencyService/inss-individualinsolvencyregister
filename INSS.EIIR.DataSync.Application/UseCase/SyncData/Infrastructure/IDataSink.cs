using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.SyncData;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public interface IDataSink<TSinkable>
    {
        SyncDataEnums.Mode EnabledCheckBit { get; }
        string Description { get; }
        Task Start(SyncDataEnums.Datasource specifiedDataSources);
        Task<DataSinkResponse> Sink(TSinkable model);
        Task<SinkCompleteResponse> Complete(bool commit = true);
    }
}
