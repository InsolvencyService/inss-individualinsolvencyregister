using Microsoft.Extensions.Logging;

namespace INSS.EIIR.Interfaces.SearchIndexer;

public interface IIndexService
{
    Task CreateIndexAsync(ILogger logger);

    Task DeleteIndexAsync(ILogger logger);

    Task PopulateIndexAsync(ILogger logger);
}