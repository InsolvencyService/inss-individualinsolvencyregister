using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    [Route(AreaNames.Admin + "/admin-area")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}