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
    public string FullName
    {
        get { return $"{FirstName} {MiddleName} {FamilyName}"; }
    }
    [SearchableField(IsSortable = true)]
    public string CombinedName
    {
        get { return $"{FirstName} {FamilyName}"; }
    }
    [SearchableField(IsSortable = true)]
    public string FirstName { get; set; }

    [SimpleField]
    public string MiddleName { get; set; }

    [SearchableField(IsSortable = true)]
    public string FamilyName { get; set; }

    [SimpleField]
    public string Title { get; set; }

    [SearchableField(IsSortable = true)]
    public string AlternativeNames { get; set; }

    [SimpleField]
    public string Gender { get; set; }

    [SimpleField]
    public string Occupation { get; set; }

    [SearchableField(IsSortable = true)]
    public string LastKnownTown { get; set; }

    [SimpleField]
    public string LastKnownAddress { get; set; }

    [SearchableField(IsSortable = true)]
    public string LastKnownPostcode { get; set; }

    [SimpleField]
    public string? AddressWithheld { get; set; }

    [SimpleField]
    public string DateOfBirth { get; set; }

    [SimpleField]
    public string CaseName { get; set; }

    [SimpleField(IsFilterable = true)]
    public string Court { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public string CourtNumber { get; set; }

    [SimpleField]
    public string CaseYear { get; set; }

    [SimpleField(IsFilterable = true)]
    public string InsolvencyType { get; set; }

    [SimpleField(IsSortable = true)]
    public DateTime StartDate { get; set; }

    [SimpleField]
    public DateTime EndDate { get; set; }

    [SimpleField]
    public String InsolvencyDate { get; set; }


    [SimpleField]
    public DateTime NotificationDate { get; set; }


    [SimpleField(IsFilterable = true)]
    public string CaseStatus { get; set; }

    [SimpleField]
    public string CaseDescription { get; set; }

    [SearchableField(IsSortable = true)]
    public string TradingName { get; set; }

    [SimpleField]
    public string TradingAddress { get; set; }

    [SimpleField]
    public string TradingPostcode { get; set; }

    [SimpleField]
    public string PractitionerName { get; set; }

    [SimpleField]
    public string PractitionerFirmName { get; set; }
    [SimpleField]
    public string PractitionerAddress { get; set; }

    [SimpleField]
    public string PractitionerPostcode { get; set; }

    [SimpleField]
    public string PractitionerTelephone { get; set; }

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

    [SearchableField(IsSortable = true)]
    public string InsolvencyTradeName { get; set; }

    [SimpleField]
    public string InsolvencyTradeNameAddress { get; set; }
}