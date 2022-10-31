using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Configuration;

namespace INSS.EIIR.Interfaces.Services;

public interface ICaseDataProvider
{
    Task<CaseResult> GetCaseByCaseNoIndivNoAsync(CaseRequest searchModel);
}
 