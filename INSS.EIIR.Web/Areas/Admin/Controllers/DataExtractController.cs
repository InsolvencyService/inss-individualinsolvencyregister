using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [Route(AreaNames.Admin + "/dataExtract")]
    public class DataExtractController : Controller
    {
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}