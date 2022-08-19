using System;
using INSS.EIIR.Interfaces.SearchIndexer;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.SearchIndexer.Functions
{
    public class RebuildIndexes
    {
        private readonly IIndexService _indexService;

        public RebuildIndexes(IIndexService indexService)
        {
            _indexService = indexService;
        }

        [FunctionName("RebuildIndexes")]
        public void Run([TimerTrigger("0 0 6 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            //_indexService.CreateIndex<>();
        }
    }
}