using INSS.EIIR.Interfaces.AzureSearch;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    [FunctionName(nameof(RebuildIndexes))]
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