using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public new IActionResult NotFound()
        {
            return View();
        }
    }
}