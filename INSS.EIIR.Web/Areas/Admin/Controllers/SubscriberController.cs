using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly ISubscriberService _subscriberService;

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Area(AreaNames.Admin)]
        [Route(AreaNames.Admin + "/subscriber/{subscriberId}")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Profile(int subscriberId)
        {
            var subscriber = _subscriberService.GetSubscriber(subscriberId);

            return View();
        }
    }
}

