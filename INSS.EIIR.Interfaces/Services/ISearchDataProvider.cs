using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.Services;

public interface ISearchDataProvider
{
    IEnumerable<CaseResult> GetIndividualSearchData();
}