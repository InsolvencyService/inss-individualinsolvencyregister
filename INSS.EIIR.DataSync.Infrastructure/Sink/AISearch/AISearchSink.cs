using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public class AISearchSink : IDataSink<InsolventIndividualRegisterModel>
    {
        public AISearchSink(AISearchSinkOptions options) { }

        public Task Start() { return Task.CompletedTask; }

        public Task<SinkCompleteResponse> Complete()
        {
            throw new NotImplementedException();
        }

        public Task<DataSinkResponse> Sink(InsolventIndividualRegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
