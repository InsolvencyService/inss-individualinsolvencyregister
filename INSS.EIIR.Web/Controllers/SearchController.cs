using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Search/{searchTerm}")]
        public IActionResult Search(string searchTerm)
        {
            return RedirectToAction("Index", "SearchResults", new { searchTerm });
        }
    }
}