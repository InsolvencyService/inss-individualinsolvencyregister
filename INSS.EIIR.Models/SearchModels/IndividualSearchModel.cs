namespace INSS.EIIR.Models.SearchModels;

public class IndividualSearchModel
{
    public IndividualSearchModel()
    {
        Courts = Enumerable.Empty<string>().ToList();
        CourtNames = Enumerable.Empty<string>().ToList();
        InsolvencyTypes = Enumerable.Empty<string>().ToList();
        InsolvencyServiceOffices = Enumerable.Empty<string>().ToList();
    }

    public int Page { get; set; }
    
    //searchable fields
    public string SearchTerm { get; set; }

    //Filterable Fields
    public List<string> Courts { get; set; }

    public List<string> CourtNames { get; set; }

    public List<string> InsolvencyTypes { get; set; }

    public List<string> InsolvencyServiceOffices { get; set; }
}