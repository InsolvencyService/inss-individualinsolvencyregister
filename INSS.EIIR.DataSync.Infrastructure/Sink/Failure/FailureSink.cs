using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.Failure
{
    public class FailureSink : IDataSink<SyncFailure>
    {
        public FailureSink(FailureSinkOptions options)
        {

        }
        public async Task Start() { return; }


        public async Task<SinkCompleteResponse> Complete(bool commit = true)
        {
            return new SinkCompleteResponse() { IsError = false };
        }

        public async Task<DataSinkResponse> Sink(SyncFailure model)
        {
            return new DataSinkResponse() { IsError = false };
        }
    }
}
