using System.Text;
using INSS.EIIR.Interfaces.AzureSearch;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public abstract class BaseFilter
{
    private readonly ISearchCleaningService _searchCleaningService;
    protected virtual string FilterODataString => string.Empty;

    protected BaseFilter(ISearchCleaningService searchCleaningService)
    {
        _searchCleaningService = searchCleaningService;
    }

    public string ApplyFilter(IList<string> searchFilter)
    {
        if (!(searchFilter?.Any() ?? false) || searchFilter.All(string.IsNullOrWhiteSpace))
        {
            return string.Empty;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("(");
        foreach (var filter in searchFilter)
        {
            if (stringBuilder.Length > 1)
            {
                stringBuilder.Append(" or ");
            }

            stringBuilder.AppendFormat(FilterODataString, filter);
        }

        stringBuilder.Append(")");

        return _searchCleaningService.EscapeFilterSpecialCharacters(stringBuilder.ToString());
    }
}