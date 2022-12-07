using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace INSS.EIIR.Web.Controllers
{
    public class ReportErrorIssueController : Controller
    {
        private readonly IErrorIssuesService _feedbackService;
        private readonly ICaseService _caseService;

        public ReportErrorIssueController(
            IErrorIssuesService feedbackService,
            ICaseService caseService)
        {
            _feedbackService = feedbackService;
            _caseService = caseService;
        }

        [HttpGet("report-an-error/{page}/{fromAdmin}/{caseNo?}/{indivNo?}/{searchTerm?}/{insolvencyType?}/{organisation?}/{status?}")]
        public async Task<IActionResult> Index(int page,
            bool fromAdmin = false,
            int caseNo = 0,
            int indivNo = 0,
            string searchTerm = "",
            string insolvencyType = "A",
            int organisation = 0,
            int status = 0)
        {
            var caseDetails = await _caseService.GetCaseAsync(caseNo, indivNo);

            var model = new CreateFeedbackViewModel
            {
                CaseNo = caseNo,
                IndivNo = indivNo,
                Name = caseDetails.caseName,
                Type = caseDetails.insolvencyType,
                ArrangementDate = DateTime.MinValue, //TODO
                CaseFeedback = new CreateCaseFeedback
                {
                    CaseId = caseDetails.caseNo
                }
            };

            List<BreadcrumbLink> breadcrumbs;

            if (fromAdmin)
            {
                breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(
                    isAdmin: true,
                    showErrorList: true,
                    showSearchDetails: true,
                    errorListParameters: new ErrorListParameters
                    {
                        Page = page,
                        InsolvencyType = insolvencyType,
                        Organisation = organisation,
                        Status = status,
                        CaseNo = caseNo,
                        IndivNo = indivNo
                    }).ToList();
            }
            else
            {
                breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(
                    showSearch: true,
                    showSearchList: true,
                    showSearchDetails: true,
                    searchParameters: new SearchParameters
                    {
                        Page = page,
                        SearchTerm = searchTerm,
                        CaseNo = caseNo,
                        IndivNo = indivNo
                    }).ToList();
            }

            model.Breadcrumbs = breadcrumbs;

            ViewBag.Title = "Report an error or issue";
            ViewBag.Header = "Report an error or issue";

            return View(model);
        }

        [HttpPost("feedback/")]
        public async Task<IActionResult> CreateFeedback([FromForm]CreateFeedbackViewModel feedback)
        {
            feedback.CaseFeedback.FeedbackDate = DateTime.Now;

            if(!string.IsNullOrEmpty(feedback.CaseFeedback.ReporterOrganisation))
            {
                feedback.CaseFeedback.ReporterOrganisation =
                    FeedbackFilters.OrganisationFilters[Convert.ToInt32(feedback.CaseFeedback.ReporterOrganisation)];
            }

            if (ModelState.IsValid)
            {

                await _feedbackService.CreateFeedback(feedback.CaseFeedback);

                return RedirectToAction("Index", "SearchDetails", new { feedback.CaseNo, feedback.IndivNo });
            }

            ViewBag.Title = "Report an error or issue";
            ViewBag.Header = "Report an error or issue";

            return View("Index", feedback);
        }
    }
}