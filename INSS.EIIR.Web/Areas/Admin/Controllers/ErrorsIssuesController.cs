using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Frameworks;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    public class ErrorsIssuesController : Controller
    {
        private readonly IErrorIssuesService _errorIssuesService;
        private readonly IConfiguration _config;

        public ErrorsIssuesController(IConfiguration config, IErrorIssuesService errorIssuesService)
        {
            _errorIssuesService = errorIssuesService;
            _config = config;
        }

        [HttpGet(AreaNames.Admin + "/errors-or-issues/{page?}/{insolvencyType?}/{organisation?}/{status?}")]
        public async Task<IActionResult> Index(int page = 1, string insolvencyType = "A", int organisation = 0, int status = 0)
        {
            var feedback = new FeedbackViewModel
            {
                InsolvencyTypeFilter = insolvencyType,
                OrganisationFilter = organisation,
                StatusFilter = status
            };

            var parameters = CreateParameters(page, insolvencyType, organisation, status);

            feedback.FeedbackWithPaging = await _errorIssuesService.GetFeedbackAsync(parameters);
            feedback.FeedbackWithPaging.Paging.RootUrl = "admin/errors-or-issues";
            feedback.FeedbackWithPaging.Paging.Parameters = $"{insolvencyType}/{organisation}/{status}";

            feedback.Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList();

            return View(feedback);
        }

        [HttpGet(AreaNames.Admin + "/errors-or-issues/update-status/{feedbackId}/{viewed}/{insolvencyType?}/{organisation?}/{status?}")]
        public async Task<IActionResult> UpdateStatus(int feedbackId, bool viewed, string insolvencyType = "A", int organisation = 0, int status = 0)
        {
            await _errorIssuesService.UpdateStatusAsync(feedbackId, viewed);

            return RedirectToAction("Index", new { page = 1, insolvencyType, organisation, status });
        }

        private FeedbackBody CreateParameters(int page, string insolvencyType, int organisation, int status)
        {
            var parameters = new FeedbackBody
            {
                Filters = new FeedbackFilterModel
                {
                    InsolvencyType = insolvencyType == "A" ? string.Empty : insolvencyType,
                    Organisation = organisation == 0 ? string.Empty : FeedbackFilters.OrganisationFilters[organisation],
                    Status = status == 1 ? "Unviewed" : FeedbackFilters.StatusFilters[status],
                    SoftDeleteViewedRecordsAfterDays = this.GetSoftDeleteDays()

                },
                PagingModel = new PagingParameters
                {
                    PageNumber = page,
                    PageSize = 10
                }
            };

            return parameters;
        }

        private int GetSoftDeleteDays()
        {
            var setting = _config.GetValue<object>("SoftDeleteViewedRecordsAfterDays", 30);

            int value = 30;

            if (int.TryParse(setting.ToString(), out int result))
            { 
                value = result;
            }

            return value;
        }
    }
}