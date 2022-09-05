using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Models;

namespace INSS.EIIR.Interfaces.AzureSearch;

public interface IIndividualQueryService
{
    Task<IEnumerable<SearchResult>> SearchIndexAsync(IndividualSearchModel searchModel);
}