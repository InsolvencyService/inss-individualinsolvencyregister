using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Web.Services;

public class CaseService : ICaseService
{
    private const string ApiUrl = "CaseDetails";

    private readonly IClientService _clientService;

    public CaseService(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<CaseResult> GetCaseAsync(int caseNo, int indivNo)
    {
        var caseRequest = new CaseRequest
        {
            CaseNo = caseNo,
            IndivNo = indivNo
        };

        var result = await _clientService.PostAsync<CaseRequest, CaseResult> (ApiUrl, caseRequest);

        return result;
    }
}