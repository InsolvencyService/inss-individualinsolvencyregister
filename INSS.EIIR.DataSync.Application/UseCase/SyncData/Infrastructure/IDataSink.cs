using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public interface IDataSink<TSinkable>
    {
        Models.Constants.SyncData.Mode EnabledCheckBit { get; }
        string Description { get; }
        Task Start();
        Task<DataSinkResponse> Sink(TSinkable model);
        Task<SinkCompleteResponse> Complete(bool commit = true);
    }
}
