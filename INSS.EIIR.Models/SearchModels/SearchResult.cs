using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.SearchModels;

[ExcludeFromCodeCoverage]
public class SearchResult
{
    public string individualForenames { get; set; }
    public string individualSurname { get; set; }
    public string individualAlias { get; set; }

    public string companyName { get; set; }

    public string individualTown { get; set; }

    public string individualPostcode { get; set; }

    public string caseNo { get; set; }

    public string indivNo { get; set; }

}