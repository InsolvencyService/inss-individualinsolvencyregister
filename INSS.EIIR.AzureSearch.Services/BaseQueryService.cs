using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using INSS.EIIR.Interfaces.AzureSearch;

namespace INSS.EIIR.AzureSearch.Services;

public abstract class BaseQueryService
{
    private readonly SearchIndexClient _indexClient;
    private readonly ISearchTermFormattingService _searchTermFormattingService;

    protected abstract string IndexName { get; }

    protected BaseQueryService(
        SearchIndexClient indexClient,
        ISearchTermFormattingService searchTermFormattingService)
    {
        _indexClient = indexClient;
        _searchTermFormattingService = searchTermFormattingService;
    }

    protected SearchOptions GetDefaultParameters()
    {
        return new SearchOptions
        {
            QueryType = SearchQueryType.Full,
            SearchMode = SearchMode.Any,
            IncludeTotalCount = true,
            Size = 10000
        };
    }

    protected string FormatSearchTerm(string searchTerm)
    {
        return _searchTermFormattingService.FormatSearchTerm(searchTerm);
    }

    public async Task<SearchResults<T>> SearchIndexAsync<T>(string searchTerm, SearchOptions options)
    {
        var searchClient = _indexClient.GetSearchClient(IndexName);
        var result = await searchClient.SearchAsync<T>(searchTerm, options);

        return result;
    }

    public async Task<Response<T>> GetAsync<T>(string key)
    {
        var searchClient = _indexClient.GetSearchClient(IndexName);
        return await searchClient.GetDocumentAsync<T>(key);
    }
}