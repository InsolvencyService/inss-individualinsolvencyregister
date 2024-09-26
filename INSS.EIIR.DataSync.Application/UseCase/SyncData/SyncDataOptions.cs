using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData
{
    public class SyncDataOptions
    {
        public IEnumerable<IDataSourceAsync<InsolventIndividualRegisterModel>> DataSources { get; set; }
        public IEnumerable<IDataSink<InsolventIndividualRegisterModel>> DataSinks { get; set; }
        public IEnumerable<ITransformRule> TransformRules { get; set; }
        public IDataSink<SyncFailure> FailureSink { get; set; }
    }
}
