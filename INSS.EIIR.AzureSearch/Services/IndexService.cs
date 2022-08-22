using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using INSS.EIIR.Interfaces.SearchIndexer;

namespace INSS.EIIR.AzureSearch.Services;

public class IndexService : IIndexService
{
    private readonly SearchIndexClient _searchClient;
    private readonly ISearchDataProvider _searchDataProvider;

    public IndexService(
        SearchIndexClient searchClient,
        ISearchDataProvider searchDataProvider)
    {
        _searchClient = searchClient;
        _searchDataProvider = searchDataProvider;
    }

    public void CreateIndex<T>(string indexName)
    {
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(T));

        var definition = new SearchIndex(indexName, searchFields);

        _searchClient.CreateOrUpdateIndex(definition);
    }
}