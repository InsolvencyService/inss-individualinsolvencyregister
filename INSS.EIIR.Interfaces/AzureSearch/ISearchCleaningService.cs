namespace INSS.EIIR.Interfaces.AzureSearch;

public interface ISearchCleaningService
{
    string EscapeSearchSpecialCharacters(string term);

    string EscapeFilterSpecialCharacters(string term);
}