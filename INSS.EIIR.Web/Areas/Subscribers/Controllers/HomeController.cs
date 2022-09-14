using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Subscribers.Controllers
{
    [Area(AreaNames.Subscribers)]
    [Route(AreaNames.Subscribers + "/Home")]
    public class HomeController : Controller
    {
        [Authorize(Roles = Role.Subscriber)]
        public IActionResult Index()
        {
            return View();
        }
    }
}