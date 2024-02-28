using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SubscriberController : Controller
    {

        public IActionResult Register()
        {
            return View();
        }
    }
}