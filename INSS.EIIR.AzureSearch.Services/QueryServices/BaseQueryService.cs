using AutoMapper;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Helpers;

namespace INSS.EIIR.AzureSearch.Services.QueryServices;

public abstract class BaseQueryService
{
    private readonly IMapper _mapper;
    private readonly SearchIndexClient _indexClient;
    private readonly ISearchTermFormattingService _searchTermFormattingService;
    private readonly ISearchCleaningService _searchCleaningService;

    protected const string Concatenation = " and ";

    protected abstract string IndexName { get; }

    protected BaseQueryService(
        IMapper mapper,
        SearchIndexClient indexClient,
        ISearchTermFormattingService searchTermFormattingService,
        ISearchCleaningService searchCleaningService)
    {
        _mapper = mapper;
        _indexClient = indexClient;
        _searchTermFormattingService = searchTermFormattingService;
        _searchCleaningService = searchCleaningService;
    }

    protected SearchOptions GetDefaultSearchOptions()
    {
        return new SearchOptions
        {
            QueryType = SearchQueryType.Full,
            SearchMode = SearchMode.All,
            IncludeTotalCount = true,
            Size = 10000
        };
    }

    protected string FormatSearchTerm(string searchTerm)
    {
        return _searchTermFormattingService.FormatSearchTerm(searchTerm);
    }

    protected string CleanSearchString(string searchTerm)
    {
        return _searchCleaningService.EscapeSearchSpecialCharacters(searchTerm);
    }

    public async Task<IEnumerable<TR>> SearchIndexAsync<T, TR>(string searchTerm, SearchOptions options)
    {

        searchTerm = FormatSearchTerm(CleanSearchString(searchTerm.Base64Decode()));

        var searchClient = _indexClient.GetSearchClient(IndexName);
        var result = await searchClient.SearchAsync<T>(searchTerm, options);

        var mappedResults = result.Value.GetResults().Select(r => r.Document)
            .Select(d => _mapper.Map<T, TR>(d));

        return mappedResults;
    }

    public async Task<TR> SearchIndexDetailAsync<T, TR>(CaseRequest caseModel, SearchOptions options)
    {
        var searchTerm = String.Format("(CaseNumber eq {0}) + (IndividualNumber eq {1})", caseModel.CaseNo, caseModel.IndivNo);
        var searchClient = _indexClient.GetSearchClient(IndexName);
        var result = await searchClient.SearchAsync<T>(searchTerm, options);

        var mappedResults = result.Value.GetResults().Select(r => r.Document)
            .Select(d => _mapper.Map<T, TR>(d));

        return mappedResults.FirstOrDefault();
    }

    public async Task<TR> GetAsync<T, TR>(string key)
    {
        var searchClient = _indexClient.GetSearchClient(IndexName);
        var result = await searchClient.GetDocumentAsync<T>(key);

        var mappedResults = _mapper.Map<T, TR>(result);

        return mappedResults;
    }
}