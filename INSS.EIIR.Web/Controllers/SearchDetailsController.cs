using INSS.EIIR.Interfaces.Web.Services;
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

        [HttpGet("case-details/{caseNo}/{indivNo}")]
        public async Task<IActionResult> Index(int caseNo, int indivNo = 1)
        {
            var caseDetails = await _caseService.GetCaseAsync(caseNo, indivNo);

            return View(caseDetails);
        }
    }
}