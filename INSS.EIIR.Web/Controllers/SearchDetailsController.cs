using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class SearchDetailsController : Controller
    {
        private readonly ICaseService _caseService;

        public SearchDetailsController(ICaseService caseService)
        {
            _caseService = caseService;
        }
        [HttpGet("SearchDetails/{caseNo}/{inivNo}")]
        public async Task<IActionResult> Index(int caseNo, int inivNo = 1)
        {
            var caseDetails = await _caseService.GetCaseAsync(caseNo, inivNo);

            return View(caseDetails);
        }
    }
}