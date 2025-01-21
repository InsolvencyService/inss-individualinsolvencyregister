using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;


namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public interface ITransformRule
    {
        Task<TransformRuleResponse> Transform(InsolventIndividualRegisterModel model);
    }
}
