using INSS.EIIR.Interfaces.AzureSearch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;

namespace INSS.EIIR.Functions.Functions;

public class RebuildIndexes
{
    private readonly IEnumerable<IIndexService> _indexServices;
    private readonly ILogger<RebuildIndexes> _logger;

    public RebuildIndexes(
        IEnumerable<IIndexService> indexServices,
        ILogger<RebuildIndexes> logger)
    {
        _indexServices = indexServices;
        _logger = logger;
    }

    [Function(nameof(RebuildIndexes))]
    public async Task<string> Run([ActivityTrigger] string name)
    {
        var message = $"Eiir RebuildIndexes has been triggered at: {DateTime.Now}";
        _logger.LogInformation(message);

        foreach (var indexService in _indexServices)
        {
            await indexService.DeleteIndexAsync(_logger);
            await indexService.CreateIndexAsync(_logger);
            await indexService.PopulateIndexAsync(_logger);
        }

        return $"Eiir RebuildIndexes completed successfully at: {DateTime.Now}";
    }
}