namespace INSS.EIIR.Models.SearchModels;

public class IndividualSearchModel
{

    //searchable fields
    public string SearchTerm { get; set; }

    //Filterable Fields
    public List<string> Courts { get; set; }

    public List<string> CourtNames { get; set; }

    public List<string> InsolvencyTypes { get; set; }

    public List<string> InsolvencyServiceOffices { get; set; }
}