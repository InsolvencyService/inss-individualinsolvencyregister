using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public class IndividualSearchCourtNameFilter : BaseFilter, IIndiviualSearchFilter
{
    protected override string FilterODataString => "CourtName eq '{0}'";

    public IndividualSearchCourtNameFilter(ISearchCleaningService searchCleaningService)
        : base(searchCleaningService)
    {
    }

    public string ApplyFilter(IndividualSearchModel searchModel)
    {
        return ApplyFilter(searchModel.CourtNames);
    }
}