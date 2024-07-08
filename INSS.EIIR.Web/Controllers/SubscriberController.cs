using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SubscriberController : Controller
    {

        public IActionResult Register()
        {
            return View();
        }




        [HttpGet("iirsubscriberlogin.asp")]
        [HttpPost("iirsubscriberlogin.asp")]
        [HttpGet("iirsubscriberprofile.asp")]
        [HttpPost("iirsubscriberprofile.asp")]
        public IActionResult OldSubscriber()
        {
            Response.StatusCode = 400;
            return View("OldLoginPage");
        }
    }
}