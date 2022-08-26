using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public class IndiviualSearchCourtNameFilter : BaseFilter, IIndiviualSearchFilter
{
    protected override string FilterODataString => "CourtName eq '{0}'";

    public string ApplyFilter(IndividualSearchModel searchModel)
    {
        return ApplyFilter(searchModel.CourtNames);
    }
}