using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface IIndividualRepository
{
    IEnumerable<SearchResult> BuildEiirSearchIndex();
}