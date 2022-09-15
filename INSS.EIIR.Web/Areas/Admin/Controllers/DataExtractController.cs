using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [Route(AreaNames.Admin + "/DataExtract")]
    public class DataExtractController : Controller
    {
        [Authorize(Roles = Role.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}