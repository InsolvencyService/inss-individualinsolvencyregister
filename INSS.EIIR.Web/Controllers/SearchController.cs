using INSS.EIIR.Models.Helpers;
using INSS.EIIR.Models.Home;
using INSS.EIIR.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet("search/{error?}")]
        public IActionResult Index(bool? error)
        {
            var contentViewModel = new StaticContent
            {
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs()
            };

            if (error ?? false)
            {
                ModelState.AddModelError("InvalidSearch", string.Empty);
            }
            
            return View(contentViewModel);
        }

        [HttpPost("search/{searchTerm?}")]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index", new { error = true });
            }

            return RedirectToAction("Index", "SearchResults", new { searchTerm = searchTerm.Base64Encode() });
        }
    }
}