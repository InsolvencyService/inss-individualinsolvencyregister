using INSS.EIIR.Interfaces.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class ExtractPageController : Controller
    {
        private readonly IExtractService _extractService;

        public ExtractPageController(IExtractService extractService)
        {
            _extractService = extractService;
        }

        [HttpGet("/{subscriberId}/downloads/latest")]
        public async Task<IActionResult> Index(string subscriberId)
        {
            var result = await _extractService.GetLatestExtractAsync(subscriberId);

            return result;
        }
    }
}