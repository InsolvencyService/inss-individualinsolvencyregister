namespace INSS.EIIR.Models.SearchModels;

public class IndividualSearchModel
{
    public string FirstName { get; set; }

    public string Surname { get; set; }

    public List<string> Courts { get; set; }

    public List<string> CourtNames { get; set; }
}