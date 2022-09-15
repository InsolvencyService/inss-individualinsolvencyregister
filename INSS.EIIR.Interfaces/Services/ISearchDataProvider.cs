using INSS.EIIR.Models;

namespace INSS.EIIR.Interfaces.Services;

public interface ISearchDataProvider
{
    IEnumerable<SearchResult> GetIndividualSearchData(string firstName = "", string lastName = "");
}