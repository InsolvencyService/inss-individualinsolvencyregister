using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.SubscriberModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Helper;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberSearch _subscriberSearch;
        private List<BreadcrumbLink> _breadcrumbs;

        public SubscriberController(ISubscriberSearch subscriberSearch)
        {
            _subscriberSearch = subscriberSearch;

            _breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs().ToList();
        }

        [HttpGet(AreaNames.Admin + "/Subscribers/{page?}/{active?}")]
        public async Task<IActionResult> Index(int page = 1, string active = null)
        {
            var paging = new PagingParameters
            {
                PageSize = 10,
                PageNumber = page
            };

            var subscribers = active switch
            {
                "true" => await _subscriberSearch.GetActiveSubscribersAsync(paging),
                "false" => await _subscriberSearch.GetInActiveSubscribersAsync(paging),
                _ => await _subscriberSearch.GetSubscribersAsync(paging)
            };

            subscribers.Paging.RootUrl = "Admin/Subscribers";
            subscribers.Paging.Parameters = active ?? string.Empty;

            var viewModel = new SubscriberListViewModel
            {
                SubscriberWithPaging = subscribers,
                Breadcrumbs = _breadcrumbs
            };

            return View(viewModel);
        }

        [HttpGet(AreaNames.Admin + "/Subscriber/{subscriberId}")]
        public async Task<IActionResult> Profile(int subscriberId)
        {
            var subscriber = await _subscriberSearch.GetSubscriberByIdAsync(subscriberId);

            return View(subscriber);
        }
    }
}