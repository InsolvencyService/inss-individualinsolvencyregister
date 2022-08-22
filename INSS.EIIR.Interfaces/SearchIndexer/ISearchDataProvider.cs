using INSS.EIIR.Models;

namespace INSS.EIIR.Interfaces.SearchIndexer;

public interface ISearchDataProvider
{
    IEnumerable<SearchResult> GetIndividualSearchData(string firstName = "", string lastName = "");
}