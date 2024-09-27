﻿using Azure.Core;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using INSS.EIIR.AzureSearch.IndexMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.IndexModels;
using Microsoft.Extensions.Logging;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public class AISearchSink : IDataSink<InsolventIndividualRegisterModel>
    {
        public const string SEARCH_INDEX_BASE_NAME = "eiir-individuals";

        private readonly ILogger<AISearchSink> _logger;
        private readonly SearchIndexClient _indexClient;
        private readonly AISearchSinkOptions _options;
        private readonly int _batchLimit;
        private readonly ISetIndexMapService _indexMapSetter;

        private SearchClient? _searchClient;
        private string? _newSearchIndex;
        private List<InsolventIndividualRegisterModel> _batch = new List<InsolventIndividualRegisterModel>();

        public AISearchSink(AISearchSinkOptions options, ISetIndexMapService indexMapSetter, ILogger<AISearchSink> logger) 
        {
            _options = options;
            _indexMapSetter = indexMapSetter;
            _indexClient = new SearchIndexClient(new Uri(options.AISearchEndpoint), new Azure.AzureKeyCredential(options.AISearchKey), new SearchClientOptions()
            {
                Retry =
    {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            });

            _logger = logger;
        }

        public async Task Start() 
        {
            _newSearchIndex = await IndexNameHelper.GetNewIndexName(_indexClient.GetIndexNamesAsync());
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
            if (_newSearchIndex != null)
            {
                await _indexMapSetter.SetIndexName(_newSearchIndex);
            }
            else throw new ArgumentNullException("newSearchIndex cannot be null.");

            _logger.LogInformation($"Swapped alias to {_newSearchIndex}");

            await DeleteIndexes();

            _logger.LogInformation("Deleted old indexes");

            return new SinkCompleteResponse() { IsError = false };
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
            var indexNames = _indexClient.GetIndexNamesAsync();
            var nameList = await indexNames.ToListAsync();
                        
            var deleteList = IndexNameHelper.GetIndexNamesToDelete(nameList, _newSearchIndex);

            foreach (var name in deleteList)
            {
                await _indexClient.DeleteIndexAsync(name);
            }
        }
    }
}
