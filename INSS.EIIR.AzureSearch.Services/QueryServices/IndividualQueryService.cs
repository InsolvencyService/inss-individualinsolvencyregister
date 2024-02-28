using System.Text;
using AutoMapper;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services.Constants;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.QueryServices;

public class IndividualQueryService : BaseQueryService, IIndividualQueryService
{
    private readonly IEnumerable<IIndiviualSearchFilter> _filters;

    private const int PageSize = 10;

    protected override string IndexName => SearchIndexes.EiirIndividuals;

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

    public async Task<SearchResults> SearchIndexAsync(IndividualSearchModel searchModel)
    {
        var options = GetDefaultSearchOptions();

        options.Filter = GetFilter(searchModel);

        var result = (await SearchIndexAsync<IndividualSearch, SearchResult>(searchModel.SearchTerm, options)).ToList();

        var results = new SearchResults
        {
            
            Results = result.Skip((searchModel.Page - 1) * PageSize).Take(PageSize).ToList(),
            Paging = new PagingModel(result.Count, searchModel.Page)
            {
                TotalPages = (int)Math.Ceiling((double)((decimal)result.Count / PageSize))
            }
        };

        return results;
    }

    public async Task<CaseResult> SearchDetailIndexAsync(CaseRequest caseModel)
    {
        var options = GetDefaultSearchOptions();

        var result = (await SearchIndexDetailAsync<IndividualSearch, CaseResult>(caseModel, options));

        return result;
    }

    public async Task<CaseResult> GetAsync(IndividualSearch individualSearch)
    {
        var result = await GetAsync<IndividualSearch, CaseResult>(individualSearch.CaseNumber);

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