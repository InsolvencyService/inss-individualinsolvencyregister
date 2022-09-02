using System.Text;
using INSS.EIIR.Interfaces.AzureSearch;

namespace INSS.EIIR.AzureSearch.Services;

public class SearchCleaningService : ISearchCleaningService
{
    private const string SearchEscapeCharacter = @"\";

    private readonly IEnumerable<char> _luceneSpecialCharacters = new HashSet<char> { '+', '-', '&', '|', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '?', ':', ';', '/', '`', '<', '>', '#', '%', '@', '=', '\\' };

    public string EscapeSearchSpecialCharacters(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return string.Empty;
        }

        var stringBuilder = new StringBuilder();

        foreach (var character in term)
        {
            if (_luceneSpecialCharacters.Contains(character))
            {
                stringBuilder.Append(SearchEscapeCharacter);
            }

            stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }

    public string EscapeFilterSpecialCharacters(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return string.Empty;
        }

        return term.Replace("'", "''");
    }
}