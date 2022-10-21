namespace INSS.EIIR.Models.SearchModels;

public class SearchResults
{
    public SearchResults()
    {
        Results = new List<SearchResult>();
    }

    public PagingModel Paging { get; set; }

    public List<SearchResult> Results { get; set; }
}