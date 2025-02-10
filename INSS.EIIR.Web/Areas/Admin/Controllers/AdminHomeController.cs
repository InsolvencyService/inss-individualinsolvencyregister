using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Controllers;
using INSS.EIIR.Web.Services;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    [Route(AreaNames.Admin + "/admin-area")]
    public class AdminHomeController : Controller
    {
        private readonly IBanner _bannerService;

        public AdminHomeController(IBanner bannerService)
        {
            _bannerService = bannerService;

        }

        [Authorize(Roles = Role.Admin)]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Index()
        {

            var banner = await _bannerService.GetBannerAsync();

            var adminHomeViewModel = new AdminHomeViewModel() { BannerText = banner.Text };

            return View(adminHomeViewModel);
        }
    }
}