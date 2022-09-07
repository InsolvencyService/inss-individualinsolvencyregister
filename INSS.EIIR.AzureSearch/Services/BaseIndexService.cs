using Azure.Search.Documents.Indexes.Models;
using System.Diagnostics.CodeAnalysis;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.Interfaces.SearchIndexer;
using Azure.Search.Documents.Models;
using INSS.EIIR.Models.IndexModels;
using Azure;

namespace INSS.EIIR.AzureSearch.Services;

public abstract class BaseIndexService<T> : IIndexService
{
    private readonly SearchIndexClient _searchClient;
    
    protected abstract string IndexName { get; }

    protected BaseIndexService(
        SearchIndexClient searchClient)
    {
        _searchClient = searchClient;
    }

    [ExcludeFromCodeCoverage]
    public async Task CreateIndexAsync()
    {

        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(T));

        var definition = new SearchIndex(IndexName, searchFields);

        try
        {
            await _searchClient.CreateOrUpdateIndexAsync(definition);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to create index {IndexName}");
        }
    }

    [ExcludeFromCodeCoverage]
    public async Task DeleteIndexAsync()
    {
        try
        {
            var index = await _searchClient.GetIndexAsync(IndexName);

            if (index != null)
            {
                await _searchClient.DeleteIndexAsync(index);
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to delete index {IndexName}");
        }
    }

    public abstract Task PopulateIndexAsync();

    [ExcludeFromCodeCoverage]
    protected async Task IndexBatchAsync(int page, IEnumerable<IndividualSearch> data)
    {
        try
        {
            var uploaderClient = _searchClient.GetSearchClient(IndexName);

            IndexDocumentsResult result = await uploaderClient.MergeOrUploadDocumentsAsync(data);

            Thread.Sleep(2000);
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to index page: {page}");
        }
    }
}