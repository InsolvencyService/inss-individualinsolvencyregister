using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.BannerModels;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{


    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    public class BannerMessageController : Controller
    {

        private readonly List<BreadcrumbLink> _breadcrumbs;
        private readonly IBanner _bannerService;

        public BannerMessageController(IBanner bannerService)
        {
            _bannerService = bannerService;
            _breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList();
        }

        [Area(AreaNames.Admin)]
        [HttpGet(AreaNames.Admin + "/bannermessage")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Index()
        {
            var banner = await _bannerService.GetBannerAsync();

            var viewModel = new ModifyBannerMessageViewModel
            {
                BannerText = banner.Text,
                Breadcrumbs = _breadcrumbs
            };
            return View(viewModel);
        }


        [Area(AreaNames.Admin)]
        [HttpPost(AreaNames.Admin + "/bannermessage")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateBanner(ModifyBannerMessageViewModel banner)
        {

            var newbanner = await _bannerService.SetBannerAsync(new Banner() { Text = banner.BannerText });

            return RedirectToAction("Index", "AdminHome");
        }
    }
}
