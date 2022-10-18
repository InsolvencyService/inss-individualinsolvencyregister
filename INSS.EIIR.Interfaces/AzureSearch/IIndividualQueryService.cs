using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.AzureSearch;

public interface IIndividualQueryService
{
    Task<SearchResults> SearchIndexAsync(IndividualSearchModel searchModel);
}