using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Web.Services;

public class IndividualSearch : IIndividualSearch
{
    private const string ApiUrl = "IndividualSearch";

    private readonly IClientService _clientService;

    public IndividualSearch(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<SearchResults> GetIndividualsAsync(string searchTerm, int page)
    {
        var searchModel = new IndividualSearchModel
        {
            SearchTerm = searchTerm,
            Page = page
        };

        var result = await _clientService.PostAsync<IndividualSearchModel, SearchResults> (ApiUrl, searchModel);

        return result;
    }
}