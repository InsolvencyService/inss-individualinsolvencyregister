using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public new IActionResult NotFound()
        {
            return View();
        }
    }
}