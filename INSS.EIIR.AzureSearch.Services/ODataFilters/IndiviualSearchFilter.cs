namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public class IndiviualSearchFilter : BaseFilter
{
    protected override string FilterODataString => "AwardingBodyCode eq '{0}' or AwardingBodyName eq '{0}'";

    public string ApplyFilter()
    {
        return ApplyFilter();
    }
}