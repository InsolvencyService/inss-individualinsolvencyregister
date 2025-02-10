using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using INSS.EIIR.Models.SyncData;


namespace INSS.EIIR.DataSync.Application.UseCase.SyncData
{
    public class SyncDataOptions
    {
        public SyncDataEnums.Datasource PermittedDataSources { get; set; }  
        public IEnumerable<IDataSourceAsync<InsolventIndividualRegisterModel>> DataSources { get; set; }
        public IEnumerable<IDataSink<InsolventIndividualRegisterModel>> DataSinks { get; set; }
        public IEnumerable<ITransformRule> TransformRules { get; set; }
        public IDataSink<SyncFailure> FailureSink { get; set; }
        public IEnumerable<IValidationRule> ValidationRules { get; set; }
    }
}
