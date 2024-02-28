using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.Web.Services;

public interface IIndividualSearch
{
    Task<SearchResults> GetIndividualsAsync(string searchTerm, int page);
}