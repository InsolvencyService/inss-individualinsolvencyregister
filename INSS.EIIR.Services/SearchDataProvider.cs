using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.SearchIndexer;
using INSS.EIIR.Models;

namespace INSS.EIIR.Services;

public class SearchDataProvider : ISearchDataProvider
{
    private readonly IIndividualRepository _repository;

    public SearchDataProvider(IIndividualRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<SearchResult> GetIndividualSearchData(string firstName = "", string lastName = "")
    {
        return _repository.SearchByName(firstName, lastName);
    }
}