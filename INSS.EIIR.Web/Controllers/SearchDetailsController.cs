using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
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

        [HttpGet("case-details/{page}/{fromAdmin}/{caseNo?}/{indivNo?}/{searchTerm?}/{insolvencyType?}/{organisation?}/{status?}")]
        public async Task<IActionResult> Index(
            int page,
            bool fromAdmin = false,
            int caseNo = 0,
            int indivNo = 0,
            string searchTerm = "",
            string insolvencyType = "A",
            int organisation = 0,
            int status = 0)
        {
            var caseDetails = await _caseService.GetCaseAsync(caseNo, indivNo);

            List<BreadcrumbLink> breadcrumbs;

            if (fromAdmin)
            {
                breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(
                    isAdmin: true,
                    showErrorList: true,
                    errorListParameters: new ErrorListParameters
                    {
                        Page = page,
                        InsolvencyType = insolvencyType,
                        Organisation = organisation,
                        Status = status
                    }).ToList();
            }
            else
            {
                breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(
                    showSearch: true,
                    showSearchList: true,
                    searchParameters: new SearchParameters
                    {
                        Page = page,
                        SearchTerm = searchTerm
                    }).ToList();
            }

            var viewModel = new CaseDetailsViewModel
            {
                Breadcrumbs = breadcrumbs,
                CaseDetails = caseDetails
            };

            if (fromAdmin)
            {
                viewModel.ErrorListParameters = new ErrorListParameters
                {
                    Page = page,
                    InsolvencyType = insolvencyType,
                    Organisation = organisation,
                    Status = status,
                    CaseNo = caseNo,
                    IndivNo = indivNo
                };
            }
            else
            {
                viewModel.SearchParameters = new SearchParameters
                {
                    Page = page,
                    SearchTerm = searchTerm,
                    CaseNo = caseNo,
                    IndivNo = indivNo
                };
            }

            return View(viewModel);
        }
    }
}