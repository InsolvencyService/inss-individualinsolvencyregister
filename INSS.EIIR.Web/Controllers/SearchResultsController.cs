using INSS.EIIR.Interfaces.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchResultsController : Controller
    {
        private readonly IIndividualSearch _individualSearch;

        public SearchResultsController(IIndividualSearch individualSearch)
        {
            _individualSearch = individualSearch;
        }

        [HttpGet("SearchResults/{searchTerm}/{page?}")]
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            var searchResults = await _individualSearch.GetIndividualsAsync(searchTerm, page);

            searchResults.Paging.RootUrl = "SearchResults";
            searchResults.Paging.SearchTerm = searchTerm;

            return View(searchResults);
        }
    }
}