using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Services;
public class CaseDataProvider : ICaseDataProvider
{
    private readonly ICaseQueryRepository _caseQueryRepository;

    public CaseDataProvider(ICaseQueryRepository caseQueryRepository)
    {
        _caseQueryRepository = caseQueryRepository;
    }

    public async Task<CaseResult> GetCaseByCaseNoIndivNoAsync(CaseRequest searchModel)
    {
        return await _caseQueryRepository.GetCaseAsync(searchModel);
    }
}
