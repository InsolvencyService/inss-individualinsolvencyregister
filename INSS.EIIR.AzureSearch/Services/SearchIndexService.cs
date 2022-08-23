using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using INSS.EIIR.AzureSearch.IndexModels;
using INSS.EIIR.Interfaces.SearchIndexer;
using INSS.EIIR.Models;

namespace INSS.EIIR.AzureSearch.Services;

public class SearchIndexService : IIndexService
{
    private const string IndexName = "individual_search";
    private const int PageSize = 2000;

    private readonly SearchIndexClient _searchClient;
    private readonly IMapper _mapper;
    private readonly ISearchDataProvider _searchDataProvider;

    public SearchIndexService(
        SearchIndexClient searchClient,
        IMapper mapper,
        ISearchDataProvider searchDataProvider)
    {
        _searchClient = searchClient;
        _mapper = mapper;
        _searchDataProvider = searchDataProvider;
    }

    [ExcludeFromCodeCoverage]
    public void CreateIndex()
    {
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(IndividualSearch));

        var definition = new SearchIndex(IndexName, searchFields);

        _searchClient.CreateOrUpdateIndex(definition);
    }

    public async Task PopulateIndexAsync()
    {
        var data = _searchDataProvider.GetIndividualSearchData();
        
        var indexData = _mapper.Map<IEnumerable<SearchResult>, IEnumerable<IndividualSearch>>(data).ToList();

        var pages = indexData.Count / PageSize;

        if (indexData.Count % PageSize != 0)
        {
            pages += 1;
        }

        for (var i = 0; i < pages; i++)
        {
            var dataBatch = indexData
                .Skip(i * PageSize)
                .Take(PageSize);

            await IndexBatchAsync(i, dataBatch);
        }
    }

    [ExcludeFromCodeCoverage]
    private async Task IndexBatchAsync(int page, IEnumerable<IndividualSearch> data)
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