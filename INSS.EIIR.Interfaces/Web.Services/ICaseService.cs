using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.Web.Services;

public interface ICaseService
{
    Task<CaseResult> GetCaseAsync(int caseNo, int indivNo);
}