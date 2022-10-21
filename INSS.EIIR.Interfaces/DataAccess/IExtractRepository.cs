using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface IExtractRepository
{
    Extract GetExtractAvailable();
    void UpdateExtractAvailable();
    Task<IEnumerable<Extract>> GetExtractsAsync(PagingParameters pagingParameters);
    Task<int> GetTotalExtractsAsync();
}
