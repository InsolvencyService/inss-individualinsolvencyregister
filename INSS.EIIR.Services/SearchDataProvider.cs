using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Services;

public class SearchDataProvider : ISearchDataProvider
{
    private readonly IIndividualRepository _repository;

    public SearchDataProvider(IIndividualRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<CaseResult> GetIndividualSearchData()
    {
        return _repository.BuildEiirIndex();
    }
}