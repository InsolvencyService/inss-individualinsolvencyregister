namespace INSS.EIIR.Models;

public class PagingModel
{
    public int ResultCount { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public string RootUrl { get; set; }

    public string SearchTerm { get; set; }
}