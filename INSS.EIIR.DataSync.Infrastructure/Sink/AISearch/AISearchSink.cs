using Azure.Core;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using INSS.EIIR.AzureSearch.IndexMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.IndexModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public class AISearchSink : IDataSink<InsolventIndividualRegisterModel>
    {
        public const string SEARCH_INDEX_BASE_NAME = "eiir-individuals";

        private ILogger<AISearchSink> _logger;
        private List<InsolventIndividualRegisterModel> _batch = new List<InsolventIndividualRegisterModel>();
        private SearchIndexClient _indexClient;
        private SearchClient _searchClient;
        private AISearchSinkOptions _options;
        private int _batchLimit;

        private ISetIndexMapService _indexMapSetter;

        private string _newSearchIndex;

        public AISearchSink(AISearchSinkOptions options, ISetIndexMapService indexMapSetter, ILogger<AISearchSink> logger) 
        {
            _options = options;
            _indexMapSetter = indexMapSetter;
            _indexClient = new SearchIndexClient(new Uri(options.AISearchEndpoint), new Azure.AzureKeyCredential(options.AISearchKey));
            _logger = logger;
        }

        public async Task Start() 
        {
            _newSearchIndex = await GetNewIndexName();           
            await CreateNewIndex(_newSearchIndex);

            _searchClient = new SearchClient(new Uri(_options.AISearchEndpoint), _newSearchIndex, new Azure.AzureKeyCredential(_options.AISearchKey), new SearchClientOptions()
            {
                Retry =
    {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            });

            _logger.LogInformation($"Created index {_newSearchIndex}");
        }

        public async Task<DataSinkResponse> Sink(InsolventIndividualRegisterModel model)
        {
            _batch.Add(model);

            if (_batch.Count >= _batchLimit) 
            {
                var mapped = _options.Mapper.Map<List<InsolventIndividualRegisterModel>, List<IndividualSearch>>(_batch);
                var response = await _searchClient.MergeOrUploadDocumentsAsync(mapped);
                _batch = new List<InsolventIndividualRegisterModel>();
            }

            return new DataSinkResponse() { IsError = false };
        }

        public async Task<SinkCompleteResponse> Complete()
        {
            await _indexMapSetter.SetIndexName(_newSearchIndex);

            _logger.LogInformation($"Swapped alias to {_newSearchIndex}");

            return new SinkCompleteResponse() { IsError = false };
        }

        private async Task<string> GetNewIndexName()
        {            
            var todaysIndexName = $"{SEARCH_INDEX_BASE_NAME}-{DateTime.Today.ToString("dd-MM-yyyy")}";
            var todaysIndexAttempt = $"{todaysIndexName}-1"; 

            var indexNames = _indexClient.GetIndexNamesAsync();

            if (await indexNames.AnyAsync(n => n.StartsWith(todaysIndexName)))
            {
                var todaysLastIndex = await indexNames.Where(i => i.StartsWith(todaysIndexName)).OrderBy(x => x).LastAsync();
                int attemptNumber = Convert.ToInt32(todaysLastIndex.Substring(todaysLastIndex.LastIndexOf('-') + 1));
                todaysIndexAttempt = $"{todaysIndexName}-{attemptNumber + 1}";
            }

            return todaysIndexAttempt;
        }

        private async Task CreateNewIndex(string indexName)
        {
            var fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(IndividualSearch));

            var definition = new SearchIndex(indexName, searchFields);

            try
            {
                await _indexClient.CreateIndexAsync(definition);
            }
            catch (Exception ex)
            {
                throw new FailedToCreateIndexException($"Failed to create index {_newSearchIndex}", ex);
            }
        }

        private async Task DeleteIndexes()
        {
            // delete all todays indexes bar the one we just made.
            // get all indexes, order them, remove the one we just made, delete everything past index 5
        }
    }
}
