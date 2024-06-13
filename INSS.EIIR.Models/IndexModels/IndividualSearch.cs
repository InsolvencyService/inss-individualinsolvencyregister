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
        get
        {
            string fullName = $"{FirstName?.Trim()} {MiddleName?.Trim()} {FamilyName?.Trim()}";
            return string.Join(" ", fullName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }

    [SearchableField(IsSortable = true)]
    public string CombinedName
    {
        get
        {
            string combinedName = $"{FirstName?.Trim()} {FamilyName?.Trim()}";
            return string.Join(" ", combinedName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
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

    /// <summary>
    /// InsolvencyDate contains the date in text form dd/MM/yyyy
    /// At time of writing June 2024 there were no records on the register without an insolvency_date
    /// </summary>
    [SimpleField(IsSortable = true)]
    public String InsolvencyDate { get; set; }

    /// <summary>
    /// Applies to IVAs
    /// </summary>
    [SimpleField]
    public DateTime NotificationDate { get; set; }

    [SimpleField(IsFilterable = true)]
    public string CaseStatus { get; set; }

    [SimpleField]
    public string CaseDescription { get; set; }

    /// <summary>
    /// Contains an XML fragment with following structure
    /// <Trading><TradingDetails><TradingName></TradingName><TradingAddress></TradingAddress></TradingDetails></Trading>
    /// Where
    ///     TradingName contains a value
    ///     TradingAddress contains as value
    ///     There can be mulitple <TradingDetails> elements - i.e muliptle names and addresses
    /// OR
    /// <No //Trading Names Found> //Don't know why but need to include "//" before Trading earlier in this comment line
    /// </summary>
    [SearchableField(IsSortable = true)]
    public string TradingData { get; set; }

    /*          Restriction related fields - Start            */

    [SimpleField]
    public Boolean HasRestrictions { get; set; }

    /// <summary>
    /// Expected values null, Interim Order, Order, Undertaking 
    /// </summary>
    [SimpleField]
    public string RestrictionsType { get; set; }

    [SimpleField]
    public DateTime? RestrictionsStartDate { get; set; }

    [SimpleField]
    public DateTime? RestrictionsEndDate { get; set; }

    //Whether the individual had a previous Interim Restrictions Order for their current Restrictions Order
    //Practically only applies to BROs
    [SimpleField]
    public Boolean HasPrevInterimRestrictionsOrder { get; set; }

    //The start date of their previous Interim Restrictions Order
    [SimpleField]
    public DateTime? PrevInterimRestrictionsOrderStartDate { get; set; }

    //The end date of their previous Interim Restrictions Order
    [SimpleField]
    public DateTime? PrevInterimRestrictionsOrderEndDate { get; set; }

    /*          Restriction related fields - End            */

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
 
    [SimpleField]
    public DateTime? DateOfPreviousOrder { get; set; }
    [SimpleField]
    public string? DeceasedDate { get; set; }

}