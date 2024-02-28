using INSS.EIIR.Models.Home;
using INSS.EIIR.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/home/privacy")]
        public IActionResult Privacy()
        {
            var contentViewModel = new StaticContent()
            {
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(),
            };

            return View(contentViewModel);
        }

        [Route("/home/terms-and-conditions")]
        public IActionResult TermsAndConditions()
        {
            var contentViewModel = new StaticContent()
            {
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(),
            };

            return View(contentViewModel);
        }
        [Route("/home/accessibility-statement")]
        public IActionResult Accessibility()
        {
            var contentViewModel = new StaticContent()
            {
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(),
            };

            return View(contentViewModel);
        }
    }
}