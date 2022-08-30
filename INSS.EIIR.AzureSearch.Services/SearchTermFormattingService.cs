using INSS.EIIR.Interfaces.AzureSearch;

namespace INSS.EIIR.AzureSearch.Services;

public class SearchTermFormattingService : ISearchTermFormattingService
{
    public string FormatSearchTerm(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return string.Empty;
        }

        if (HasTermWithPrefixedWildCard(searchTerm))
        {
            return ConvertTermToRegularExpression(searchTerm);
        }

        return searchTerm.Trim();
    }

    private static bool HasTermWithPrefixedWildCard(string searchTerm)
    {
        return searchTerm.StartsWith("*") || searchTerm.Contains(" *");
    }

    private static string ConvertTermToRegularExpression(string searchTerm)
    {
        var searchTermWords = GetSearchTermAsListOfWords(searchTerm);

        searchTermWords = searchTermWords.Select(ConvertPrefixWildCardsToRegEx).ToList();

        return string.Join(" ", searchTermWords);
    }

    private static List<string> GetSearchTermAsListOfWords(string searchTerm)
    {
        return searchTerm.Split(' ')
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.Trim()).ToList();
    }

    private static string ConvertPrefixWildCardsToRegEx(string searchWord)
    {
        if (searchWord.StartsWith("*"))
        {
            return $"/{searchWord.Replace("*", ".*")}/";
        }

        return searchWord;
    }
}