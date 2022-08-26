using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public class IndiviualSearchCourtFilter : BaseFilter, IIndiviualSearchFilter
{
    protected override string FilterODataString => "Court eq '{0}'";

    public string ApplyFilter(IndividualSearchModel searchModel)
    {
        return ApplyFilter(searchModel.Courts);
    }
}