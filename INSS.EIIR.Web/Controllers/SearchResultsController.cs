using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchResultsController : Controller
    {
        private readonly IIndividualSearch _individualSearch;

        private readonly List<BreadcrumbLink> _breadcrumbs;

        public SearchResultsController(IIndividualSearch individualSearch)
        {
            _individualSearch = individualSearch;

            _breadcrumbs =  BreadcrumbBuilder.BuildBreadcrumbs(showSearch: true).ToList();
        }

        [HttpGet("SearchResults/{searchTerm}/{page?}")]
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            var searchResults = await _individualSearch.GetIndividualsAsync(searchTerm, page);

            searchResults.Paging.RootUrl = "SearchResults";
            searchResults.Paging.SearchTerm = searchTerm;

            var viewModel = new IndividualListViewModel
            {
                Breadcrumbs = _breadcrumbs,
                SearchResults = searchResults
            };

            return View(viewModel);
        }
    }
}