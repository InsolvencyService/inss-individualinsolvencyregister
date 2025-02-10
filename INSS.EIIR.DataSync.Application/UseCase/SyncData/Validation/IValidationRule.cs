using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public interface IValidationRule
    {
        Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model);
    }
}
