using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.AzureSearch;

public interface IIndividualQueryService
{
    Task<SearchResults> SearchIndexAsync(IndividualSearchModel searchModel);
    Task<CaseResult> SearchDetailIndexAsync(CaseRequest caseModel);
    Task<CaseResult> GetAsync(IndividualSearch individualSearch);
}