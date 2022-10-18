using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.AzureSearch;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.Functions.Functions
{
    public class RebuildIndexes
    {
        private readonly IEnumerable<IIndexService> _indexServices;

        public RebuildIndexes(IEnumerable<IIndexService> indexServices)
        {
            _indexServices = indexServices;
        }

        [FunctionName("RebuildIndexes")]
        public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            foreach (var indexService in _indexServices)
            {
                await indexService.DeleteIndexAsync(log);
                await indexService.CreateIndexAsync(log);
                await indexService.PopulateIndexAsync(log);
            }
        }
    }
}