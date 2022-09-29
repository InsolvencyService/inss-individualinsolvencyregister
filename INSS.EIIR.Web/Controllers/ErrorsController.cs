using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("/errors/{statusCode?}")]
        public IActionResult Index(int? statusCode)
        {
            if (statusCode != null)
            {
                switch (statusCode.Value)
                {
                    case 404:
                        return View("NotFound");
                    case 503:
                        return View("Maintenance");
                    case 500.30:
                        return View("Index");
                }
            }

            return View();
        }
    }
}