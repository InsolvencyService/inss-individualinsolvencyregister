using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.SearchModels;

[ExcludeFromCodeCoverage]
public class SearchResult
{
    public string indvidualForenames { get; set; }
    public string indvidualSurname { get; set; }
    public string indvidualAlias { get; set; }

    public string companyName { get; set; }

    public string indvidualTown { get; set; }

    public string indvidualPostcode { get; set; }

    public string caseNo { get; set; }

    public string indivNo { get; set; }

}