namespace INSS.EIIR.Models.SearchModels;

public class IndividualSearchModel
{

    //searchable fields
    public string CaseNumber { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string FamilyName { get; set; }

    public string AlternativeNames { get; set; }

    public string LastKnownPostcode { get; set; }

    public string TradingName { get; set; }

    public string TradingPostcode { get; set; }

    //Filterable Fields
    public List<string> Courts { get; set; }

    public List<string> CourtNames { get; set; }

    public List<string> InsolvencyTypes { get; set; }

    public List<string> InsolvencyServiceOffices { get; set; }
}