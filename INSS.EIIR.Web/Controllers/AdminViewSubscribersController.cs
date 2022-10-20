using INSS.EIIR.Interfaces.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class AdminViewSubscribersController : Controller
    {
        public IActionResult ViewSubscibers()
        {
            return View();
        }

    }
}