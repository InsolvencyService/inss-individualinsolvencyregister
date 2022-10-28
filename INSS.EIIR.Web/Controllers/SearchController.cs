using INSS.EIIR.Models.Home;
using INSS.EIIR.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            var contentViewModel = new StaticContent
            {
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs()
            };

            return View(contentViewModel);
        }

        [HttpPost("Search/{searchTerm}")]
        public IActionResult Search(string searchTerm)
        {
            return RedirectToAction("Index", "SearchResults", new { searchTerm });
        }
    }
}