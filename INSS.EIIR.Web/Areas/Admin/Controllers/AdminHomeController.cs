using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [Route(AreaNames.Admin + "/AdminHome")]
    public class AdminHomeController : Controller
    {
        [Authorize(Roles = Role.Admin)]
        public IActionResult AdminHome()
        {
            return View();
        }
    }
}