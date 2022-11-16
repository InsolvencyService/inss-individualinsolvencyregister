using System.Diagnostics.CodeAnalysis;
using Azure.Search.Documents.Indexes;

namespace INSS.EIIR.Models.IndexModels;

[ExcludeFromCodeCoverage]
public class IndividualSearch
{
    [SearchableField(IsSortable = true, IsKey = true)]
    public string CaseNumber { get; set; }

    [SearchableField(IsSortable = true)]
    public string IndividualNumber { get; set; }

    [SearchableField(IsSortable = true)]
    public string FirstName { get; set; }

    [SearchableField(IsSortable = true)]
    public string FamilyName { get; set; }

    [SearchableField(IsSortable = true)]
    public string AlternativeNames { get; set; }

    [SearchableField(IsSortable = true)]
    public string CompanyName { get; set; }

    [SimpleField]
    public string LastKnownLocality { get; set; }

    [SearchableField]
    public string LastKnownPostcode { get; set; }

}