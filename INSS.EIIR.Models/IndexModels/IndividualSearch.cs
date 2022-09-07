using System.Diagnostics.CodeAnalysis;
using Azure.Search.Documents.Indexes;

namespace INSS.EIIR.Models.IndexModels;

[ExcludeFromCodeCoverage]
public class IndividualSearch
{
    [SearchableField(IsSortable = true, IsKey = true)]
    public string CaseNumber { get; set; }

    [SearchableField(IsSortable = true, IsKey = true)]
    public string IndividualNumber { get; set; }

    [SearchableField(IsSortable = true)]
    public string FirstName { get; set; }

    [SearchableField]
    public string MiddleName { get; set; }

    [SearchableField(IsSortable = true)]
    public string FamilyName { get; set; }

    [SearchableField(IsSortable = true)]
    public string AlternativeNames { get; set; }

    [SimpleField]
    public string Gender { get; set; }

    [SimpleField]
    public string Occupation { get; set; }

    [SimpleField]
    public string LastKnownAddress { get; set; }

    [SearchableField]
    public string LastKnownPostcode { get; set; }

    [SimpleField]
    public DateTime DateOfBirth { get; set; }

    [SimpleField]
    public string CaseName { get; set; }

    [SimpleField(IsFilterable = true)]
    public string Court { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public int CourtNumber { get; set; }

    [SimpleField(IsFilterable = true)]
    public string InsolvencyType { get; set; }

    [SimpleField(IsSortable = true)]
    public DateTime StartDate { get; set; }

    [SimpleField]
    public DateTime EndDate { get; set; }

    [SimpleField(IsFilterable = true)]
    public string CaseStatus { get; set; }

    [SimpleField]
    public string CaseDescription { get; set; }

    [SearchableField(IsSortable = true)]
    public string TradingName { get; set; }

    [SimpleField]
    public string TradingAddress { get; set; }

    [SearchableField]
    public string TradingPostcode { get; set; }

    [SimpleField(IsFilterable = true)]
    public string InsolvencyServiceOffice { get; set; }

    [SimpleField]
    public string InsolvencyServiceContact { get; set; }

    [SimpleField]
    public string InsolvencyServiceAddress { get; set; }

    [SimpleField]
    public string InsolvencyServicePostcode { get; set; }

    [SimpleField]
    public string InsolvencyServiceTelephone { get; set; }
}