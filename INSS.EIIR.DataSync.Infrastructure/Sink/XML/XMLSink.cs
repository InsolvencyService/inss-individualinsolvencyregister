using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class XMLSink : IDataSink<InsolventIndividualRegisterModel>
    {
        public XMLSink(XMLSinkOptions options) { }

        public async Task Start() { return; }

        public async Task<SinkCompleteResponse> Complete()
        {
            throw new NotImplementedException();
        }

        public async Task<DataSinkResponse> Sink(InsolventIndividualRegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
