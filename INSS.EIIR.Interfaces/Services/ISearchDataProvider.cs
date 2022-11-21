using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.Services;

public interface ISearchDataProvider
{
    IEnumerable<SearchResult> GetIndividualSearchData();
}