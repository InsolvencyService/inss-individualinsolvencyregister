using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Area(AreaNames.Admin)]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberDataProvider _subscriberDataProvider;

        public SubscriberController(ISubscriberDataProvider subscriberDataProvider)
        {
            _subscriberDataProvider = subscriberDataProvider;
        }

        [HttpGet("Subscriber/{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var subscribers = await _subscriberDataProvider.GetSubscribersAsync(new PagingParameters
            {
                PageSize = 10,
                PageNumber = page
            });

            subscribers.Paging.RootUrl = "Subscriber";

            return View(subscribers);
        }

        [HttpPost("Subscriber/{subscriberId}")]
        public async Task<IActionResult> Profile(int subscriberId)
        {
            var subscriber = await _subscriberDataProvider.GetSubscriberByIdAsync($"{subscriberId}");

            return View(subscriber);
        }
    }
}