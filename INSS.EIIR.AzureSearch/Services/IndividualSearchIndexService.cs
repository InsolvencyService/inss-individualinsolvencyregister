using AutoMapper;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services.Constants;
using INSS.EIIR.Interfaces.SearchIndexer;
using INSS.EIIR.Models;
using INSS.EIIR.Models.IndexModels;

namespace INSS.EIIR.AzureSearch.Services;

public class IndividualSearchIndexService : BaseIndexService<IndividualSearch>
{
    protected override string IndexName => SearchIndexes.IndividualSearch;

    private const int PageSize = 2000;

    private readonly IMapper _mapper;
    private readonly ISearchDataProvider _searchDataProvider;

    public IndividualSearchIndexService(
        SearchIndexClient searchClient,
        IMapper mapper,
        ISearchDataProvider searchDataProvider)
        : base(searchClient)
    {
        _mapper = mapper;
        _searchDataProvider = searchDataProvider;
    }

    public override async Task PopulateIndexAsync()
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
}