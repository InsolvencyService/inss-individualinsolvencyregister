using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    public class ErrorsIssuesController : Controller
    {
        private readonly IErrorIssuesService _errorIssuesService;

        public ErrorsIssuesController(IErrorIssuesService errorIssuesService)
        {
            _errorIssuesService = errorIssuesService;
        }

        [HttpGet("/errors-or-issues")]
        public async Task<IActionResult> Index(int page = 1, string insolvencyType = "", string organisation = "", string status = "All")
        {
            var feedback = new FeedbackViewModel();

            var parameters = CreateParameters(page, insolvencyType, organisation, status);

            feedback.FeedbackWithPaging = await _errorIssuesService.GetFeedbackAsync(parameters);

            feedback.Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList();

            return View(feedback);
        }

        private static FeedbackBody CreateParameters(int page, string insolvencyType, string organisation, string status)
        {
            var parameters = new FeedbackBody
            {
                Filters = new FeedbackFilterModel
                {
                    InsolvencyType = insolvencyType,
                    Organisation = organisation,
                    Status = status
                },
                PagingModel = new PagingParameters
                {
                    PageNumber = page,
                    PageSize = 10
                }
            };

            return parameters;
        }
    }
}