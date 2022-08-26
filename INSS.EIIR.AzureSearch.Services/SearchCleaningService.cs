using System.Text;

namespace INSS.EIIR.AzureSearch.Services;

public class SearchCleaningService
{
    private const string SearchEscapeCharacter = @"\";

    private readonly IEnumerable<char> _luceneSpecialCharacters = new HashSet<char> { '+', '-', '&', '|', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '?', ':', ';', '/', '`', '<', '>', '#', '%', '@', '=', '\\' };

    public string EscapeSearchSpecialCharacters(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return term;
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
            return term;
        }

        return term.Replace("'", "''");
    }
}