using AutoMapper;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using INSS.EIIR.AzureSearch.Services.Constants;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace INSS.EIIR.AzureSearch.Services;

public class IndividualSearchIndexService : BaseIndexService<IndividualSearch>
{
    protected override string IndexName => SearchIndexes.EiirIndividuals;
 
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

    public override async Task DeleteIndexAsync(ILogger logger)
    {
        try
        {
            var index = await _searchClient.GetIndexAsync(IndexName);

            if (index != null)
            {
                index.Value.ScoringProfiles.Clear();
                await _searchClient.DeleteIndexAsync(index);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete index {IndexName}", IndexName);
        }
    }

    public override async Task PopulateIndexAsync(ILogger logger)
    {
        logger.LogDebug("Get Data from GetIndividualSearchData()");
        var data = _searchDataProvider.GetIndividualSearchData();

        logger.LogDebug("Map Data");
        var indexData = _mapper.Map<IEnumerable<CaseResult>, IEnumerable<IndividualSearch>>(data).ToList();

        var pages = indexData.Count / PageSize;

        if (indexData.Count % PageSize != 0)
        {
            pages += 1;
        }

        logger.LogDebug("Loop through pages and index batch");
        for (var i = 0; i < pages; i++)
        {
            var dataBatch = indexData
                .Skip(i * PageSize)
                .Take(PageSize);

            await IndexBatchAsync(i, dataBatch, logger);
        }
    }

}