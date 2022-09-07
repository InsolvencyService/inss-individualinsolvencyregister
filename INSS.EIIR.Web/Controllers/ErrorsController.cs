using INSS.EIIR.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace INSS.EIIR.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public new IActionResult NotFound()
        {
            return View();
        }
    }
}