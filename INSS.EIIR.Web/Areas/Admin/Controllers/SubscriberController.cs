
﻿using INSS.EIIR.Interfaces.Web.Services;
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
        private readonly ISubscriberService _subscriberService;
        private List<BreadcrumbLink> _breadcrumbs;

        public SubscriberController(ISubscriberSearch subscriberSearch, ISubscriberService subscriberService)
        {
            _subscriberSearch = subscriberSearch;
            _subscriberService = subscriberService;

            _breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs().ToList();
        }

        [HttpGet(AreaNames.Admin + "/Subscribers/{page?}/{active?}")]
        public async Task<IActionResult> Index(int page = 1, string active = "true")
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


        [Area(AreaNames.Admin)]
        [Route(AreaNames.Admin + "/subscriber/{subscriberId}", Name = "SubscriberProfile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Profile(int subscriberId)
        {
            var subscriber = await _subscriberService.GetSubscriberByIdAsync($"{subscriberId}");

            return View(subscriber);
        }

        [Area(AreaNames.Admin)]
        [HttpGet(AreaNames.Admin + "/subscriber/{subscriberId}/change-profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeProfile(int subscriberId)
        {
            var subscriber = await _subscriberService.GetSubscriberByIdAsync($"{subscriberId}");

            var subscriberProfile = new SubscriberProfile
            {
                SubscriberId = subscriberId,
                OrganisationName = subscriber.OrganisationName,
                OrganisationType = subscriber.SubscriberDetails.OrganisationType,
                ContactForename = subscriber.SubscriberDetails.ContactForename,
                ContactSurname = subscriber.SubscriberDetails.ContactSurname,
                ContactAddress1 = subscriber.SubscriberDetails.ContactAddress1,
                ContactAddress2 = subscriber.SubscriberDetails.ContactAddress2,
                ContactCity = subscriber.SubscriberDetails.ContactCity,
                ContactPostcode = subscriber.SubscriberDetails.ContactPostcode,
                ContactEmail = subscriber.SubscriberDetails.ContactEmail,
                ContactTelephone = subscriber.SubscriberDetails.ContactTelephone,
                ApplicationDay = subscriber.SubscriberDetails.ApplicationDate.Day,
                ApplicationMonth = subscriber.SubscriberDetails.ApplicationDate.Month,
                ApplicationYear = subscriber.SubscriberDetails.ApplicationDate.Year,
                SubscribedFromDay = subscriber.SubscribedFrom?.Day,
                SubscribedFromMonth = subscriber.SubscribedFrom?.Month,
                SubscribedFromYear = subscriber.SubscribedFrom?.Year,
                SubscribedToDay = subscriber.SubscribedTo?.Day,
                SubscribedToMonth = subscriber.SubscribedTo?.Month,
                SubscribedToYear = subscriber.SubscribedTo?.Year,
                EmailAddress1 = subscriber.EmailContacts.Select(e => e.EmailAddress).FirstOrDefault(),
                EmailAddress2 = subscriber.EmailContacts.Skip(1).Select(e => e.EmailAddress).FirstOrDefault(),
                EmailAddress3 = subscriber.EmailContacts.Skip(2).Select(e => e.EmailAddress).FirstOrDefault(),
                AccountActive = subscriber.AccountActive,
                Breadcrumbs = _breadcrumbs
            };

            return View(subscriberProfile);
        }

        [Area(AreaNames.Admin)]
        [HttpPost(AreaNames.Admin + "/subscriber/change-profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeProfile(SubscriberProfile subscriber)
        {
            if (ModelState.IsValid)
            {
               
                var createUpdateSubscriber = new CreateUpdateSubscriber
                {
                    AccountActive = subscriber.AccountActive,
                    ApplicationDate = subscriber.ApplicationDate,
                    ContactAddress1 = subscriber.ContactAddress1,
                    ContactAddress2 = subscriber.ContactAddress2,
                    ContactCity = subscriber.ContactCity,
                    ContactEmail = subscriber.ContactEmail,
                    ContactForename = subscriber.ContactForename,
                    ContactPostcode = subscriber.ContactPostcode,
                    ContactSurname = subscriber.ContactSurname,
                    ContactTelephone = subscriber.ContactTelephone,
                    EmailAddresses = subscriber.EmailAddresses.ToList(),
                    OrganisationName = subscriber.OrganisationName,
                    OrganisationType = subscriber.OrganisationType,
                    SubscribedFrom = subscriber.SubscribedFrom,
                    SubscribedTo = subscriber.SubscribedTo
                };

                await _subscriberService.UpdateSubscriberAsync($"{subscriber.SubscriberId}", createUpdateSubscriber);

                return RedirectToRoute("SubscriberProfile", new { subscriberId = subscriber.SubscriberId });

            }

            return View(subscriber);
        }
    }
}