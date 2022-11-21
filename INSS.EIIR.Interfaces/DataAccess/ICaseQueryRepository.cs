using INSS.EIIR.Models.CaseModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface ICaseQueryRepository
{
    Task<CaseResult> GetCaseAsync(CaseRequest searchModel);
}