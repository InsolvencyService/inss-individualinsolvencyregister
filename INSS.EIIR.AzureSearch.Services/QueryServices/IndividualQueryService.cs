using System.Text;
using AutoMapper;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services.Constants;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.QueryServices;

public class IndividualQueryService : BaseQueryService, IIndividualQueryService
{
    private readonly IEnumerable<IIndiviualSearchFilter> _filters;
    protected override string IndexName => SearchIndexes.IndividualSearch;

    public IndividualQueryService(
        IMapper mapper,
        SearchIndexClient indexClient,
        ISearchTermFormattingService searchTermFormattingService,
        ISearchCleaningService searchCleaningService,
        IEnumerable<IIndiviualSearchFilter> filters) 
        : base(mapper, indexClient, searchTermFormattingService, searchCleaningService)
    {
        _filters = filters;
    }

    public async Task<IEnumerable<SearchResult>> SearchIndexAsync(IndividualSearchModel searchModel)
    {
        var options = GetDefaultSearchOptions();

        options.Filter = GetFilter(searchModel);

        var result = await SearchIndexAsync<IndividualSearch, SearchResult>(searchModel.SearchTerm, options);

        return result;
    }

    private string GetFilter(IndividualSearchModel searchModel)
    {
        var filterBuilder = new StringBuilder();
        foreach (var filter in _filters)
        {
            var filterString = filter.ApplyFilter(searchModel);

            filterBuilder.Append(filterBuilder.Length == 0 ? filterString : Concatenation + filterString);
        }

        return filterBuilder.ToString();
    }
}