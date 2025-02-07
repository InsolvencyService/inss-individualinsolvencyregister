using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Interfaces.Services;

public interface IExtractDataProvider
{
    Task<ExtractWithPaging> ListExtractsAsync(PagingParameters pagingParameters);
    Task<Extract> GetLatestExtractForDownload();

    Task<byte[]> DownloadLatestExtractAsync(string blobName);
}
