using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Interfaces.Services;

public interface IExtractDataProvider
{
    Task GenerateSubscriberFile(string filename);

    Task<ExtractWithPaging> ListExtractsAsync(PagingParameters pagingParameters);
}
