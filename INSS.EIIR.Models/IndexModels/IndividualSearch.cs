using System.Diagnostics.CodeAnalysis;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.CaseModels;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace INSS.EIIR.Models.IndexModels;

[ExcludeFromCodeCoverage]
public class IndividualSearch
{

    [SimpleField(IsKey = true)]
    public string Case_Indiv_No
    {
        get 
        {
            return $"{CaseNumber}_{IndividualNumber}";
        }    
    }

    [SimpleField]
    public string CaseNumber { get; set; }

    [SimpleField]
    public string IndividualNumber { get; set; }

    /// <summary>
    /// Contains searchablefields which are not part of Forenames, Surnames or Tradenames
    /// </summary>
    [SearchableField]
    public string GlobalSearchField
    {
        get
        {
            string globalSearchField = $"{CaseNumber} {IndividualNumber}" +
                                        $" {(LastKnownTown == Common.NoLastKnownTown ? "" : LastKnownTown)}" +
                                        $" {(LastKnownPostcode == Common.NoLastKnownPostCode ? "" : LastKnownPostcode)}";

            return string.Join(" ", globalSearchField.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }

    [SearchableField]
    public string TradeNamesSearchField
    {
        get
        {
            return $"{string.Join(" ", TradingNames.Split(",", StringSplitOptions.RemoveEmptyEntries))}";
        }
    }

    /// <summary>
    /// Contains lastnames from Family name and any surname from alternative names with no duplicates which would biase results
    /// </summary>
    [SearchableField]
    public string LastNamesSearchField
    {
        get
        {
            //Combine surname with alternatnames (which is comma seperated list of "lastname firstname"
            var lastnameFirstnamesString = $"{FamilyName?.Trim()},{(AlternativeNames == Common.NoOtherNames ? "" : AlternativeNames)}";

            var lastnameFirstnames = lastnameFirstnamesString.Split(",", StringSplitOptions.RemoveEmptyEntries);

            //Get the first non empty element from each lastnamefirstname => lastname
            var lastnames = lastnameFirstnames.Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries)).Select(n => n.Count() != 0 ? n.First(f => !string.IsNullOrEmpty(f)) : null);

            //Select distinct values which are not null/empty
            return lastnames.Where(s => !string.IsNullOrEmpty(s)).Count() == 0 ? null : string.Join(" ", lastnames.Where(s => !string.IsNullOrEmpty(s)).Distinct(StringComparer.CurrentCultureIgnoreCase));
        }
    }

    /// <summary>
    /// Contains names from Firstname and any firstnames from alternative names with no duplicates which would biase results
    /// </summary>
    [SearchableField]
    public string ForeNamesSearchField
    {
        get
        {
            //Get alternatnames (which is comma seperated list of "lastname firstname"
            var altLastnameFirstnamesString = $"{(AlternativeNames == Common.NoOtherNames ? "" : AlternativeNames)}";

            var altLastnameFirstnamesArray = altLastnameFirstnamesString.Split(",", StringSplitOptions.RemoveEmptyEntries);

            //Get the 2+ non empty element from each lastnamefirstname => forenames
            var altFirstnames = altLastnameFirstnamesArray.Select(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries)).Select(n => n.Count() > 1 ? string.Join(" ", n.Skip(1)) : "");

            //Combine Firstnames with alternative first names
            var combinedFirstnames = string.IsNullOrWhiteSpace(FirstName) ? new string[] { } : FirstName.Trim().Split(" ").Select(s=>s);
            combinedFirstnames = combinedFirstnames.Concat(altFirstnames.Where(x => !string.IsNullOrWhiteSpace(x)).Select(a => a.Split(" ")).SelectMany(x => x));

            //Select distinct values which are not null/empty
            return combinedFirstnames?.Any() != true ? null : string.Join(" ", combinedFirstnames.Distinct(StringComparer.CurrentCultureIgnoreCase).Where(s => !string.IsNullOrEmpty(s)));
        }
    }


    [SimpleField]
    public string FirstName { get; set; }

    [SimpleField]
    public string FamilyName { get; set; }

    [SimpleField]
    public string Title { get; set; }

    [SimpleField]
    public string AlternativeNames { get; set; }

    [SimpleField]
    public string Gender { get; set; }

    [SimpleField]
    public string Occupation { get; set; }

    [SimpleField]
    public string LastKnownTown { get; set; }

    [SimpleField]
    public string LastKnownAddress { get; set; }

    [SimpleField]
    public string LastKnownPostcode { get; set; }

    [SimpleField]
    public string? AddressWithheld { get; set; }

    [SimpleField]
    public string DateOfBirth { get; set; }

    [SimpleField]
    public string CaseName { get; set; }

    [SimpleField(IsFilterable = true)]
    public string Court { get; set; }

    [SimpleField(IsFilterable = true)]
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

    [SimpleField]
    public string TradingData { get; set; }


    /// <summary>
    /// Returns any TradingNames in TradingData XML or and empty string
    /// </summary>
    public string TradingNames {

        get 
        {
            if (TradingData == Common.NoTradingNames || string.IsNullOrEmpty(TradingData)) return "";

            Trading trading; 

            try
            {
                var serializer = new XmlSerializer(typeof(Trading));

                using (TextReader reader = new StringReader(TradingData))
                {
                    trading = (Trading)serializer.Deserialize(reader);
                }

            }
            catch (InvalidOperationException ex) 
            {
                if (ex.InnerException is XmlException)
                    return "";

                throw;           
            }
            
            return string.Join(",",  trading.TradingDetails.Select(td => td.TradingName).ToArray());
        }

    }

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

    /// <summary>
    /// Whether the individual had a previous Interim Restrictions Order for their current Restrictions Order
    /// Practically only applies to BROs
    /// </summary>
    [SimpleField]
    public Boolean HasPrevInterimRestrictionsOrder { get; set; }

    /// <summary>
    /// The start date of their previous Interim Restrictions Order
    /// </summary>
    [SimpleField]
    public DateTime? PrevInterimRestrictionsOrderStartDate { get; set; }

    /// <summary>
    /// The end date of their previous Interim Restrictions Order
    /// </summary>
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