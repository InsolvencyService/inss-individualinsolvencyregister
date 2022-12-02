using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("report-an-error/{caseNo}/{indivNo}/")]
        public async Task<IActionResult> Index(int caseNo, int indivNo)
        {
            var caseDetails = await _caseService.GetCaseAsync(caseNo, indivNo);

            var model = new CreateFeedbackViewModel
            {
                CaseNo = caseNo,
                IndivNo = indivNo,
                Name = caseDetails.caseName,
                Type = caseDetails.insolvencyType,
                ArrangementDate = DateTime.MinValue,
                CaseFeedback = new CreateCaseFeedback
                {
                    CaseId = caseDetails.caseNo
                }
            };

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