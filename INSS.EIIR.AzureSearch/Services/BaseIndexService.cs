using Azure.Search.Documents.Indexes.Models;
using System.Diagnostics.CodeAnalysis;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using INSS.EIIR.Models.IndexModels;
using Microsoft.Extensions.Logging;
using INSS.EIIR.Interfaces.AzureSearch;

namespace INSS.EIIR.AzureSearch.Services;

public abstract class BaseIndexService<T> : IIndexService
{
    protected readonly SearchIndexClient _searchClient;
    
    protected abstract string IndexName { get; }

    protected BaseIndexService(
        SearchIndexClient searchClient)
    {
        _searchClient = searchClient;
    }

    [ExcludeFromCodeCoverage]
    public async Task CreateIndexAsync(ILogger logger)
    {

        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(T));

        var definition = new SearchIndex(IndexName, searchFields);

        try
        {
            await _searchClient.CreateOrUpdateIndexAsync(definition);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to create index {IndexName}");
        }
    }

    [ExcludeFromCodeCoverage]
    public virtual async Task DeleteIndexAsync(ILogger logger)
    {
        try
        {
            var index = await _searchClient.GetIndexAsync(IndexName);

            if (index != null)
            {
                await _searchClient.DeleteIndexAsync(index);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to delete index {IndexName}");
        }
    }

    public abstract Task PopulateIndexAsync(ILogger logger);

    public abstract Task UploadSynonymMapAsync(ILogger logger);
    
    [ExcludeFromCodeCoverage]
    protected async Task IndexBatchAsync(int page, IEnumerable<IndividualSearch> data, ILogger logger)
    {
        try
        {
            var uploaderClient = _searchClient.GetSearchClient(IndexName);

            IndexDocumentsResult result = await uploaderClient.MergeOrUploadDocumentsAsync(data);

            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to index page: {page}");
        }
    }
}