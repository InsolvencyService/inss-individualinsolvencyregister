using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Home;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IBanner _bannerService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBanner bannerService)
        {
            _logger = logger;
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            var banner = await _bannerService.GetBannerAsync();

            var homePageViewModel = new HomePageViewModel() { BannerText = banner.Text };

            return View(homePageViewModel);
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