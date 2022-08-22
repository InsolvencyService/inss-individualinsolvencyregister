using System;
using System.Collections.Generic;
using INSS.EIIR.Interfaces.SearchIndexer;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.Functions
{
    public class RebuildIndexes
    {
        private readonly IEnumerable<IIndexService> _indexServices;

        public RebuildIndexes(IEnumerable<IIndexService> indexServices)
        {
            _indexServices = indexServices;
        }

        [FunctionName("RebuildIndexes")]
        public void Run([TimerTrigger("0 0 6 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            foreach (var indexService in _indexServices)
            {
                indexService.CreateIndex();
                indexService.PopulateIndex();
            }
        }
    }
}