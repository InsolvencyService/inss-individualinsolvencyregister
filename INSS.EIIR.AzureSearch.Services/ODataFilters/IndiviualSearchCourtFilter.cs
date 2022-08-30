using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public class IndividualSearchCourtFilter : BaseFilter, IIndiviualSearchFilter
{
    protected override string FilterODataString => "Court eq '{0}'";

    public IndividualSearchCourtFilter(ISearchCleaningService searchCleaningService) 
        : base(searchCleaningService)
    {
    }

    public string ApplyFilter(IndividualSearchModel searchModel)
    {
        return ApplyFilter(searchModel.Courts);
    }
}