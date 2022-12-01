using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface IIndividualRepository
{
    IEnumerable<CaseResult> BuildEiirIndex();
}