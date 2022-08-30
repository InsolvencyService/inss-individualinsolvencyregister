using System.Text;

namespace INSS.EIIR.AzureSearch.Services.ODataFilters;

public abstract class BaseFilter
{
    protected virtual string FilterODataString => string.Empty;

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

        return stringBuilder.ToString();
    }
}