
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
        private readonly List<BreadcrumbLink> _breadcrumbs;

        public SubscriberController(ISubscriberSearch subscriberSearch, ISubscriberService subscriberService)
        {
            _subscriberSearch = subscriberSearch;
            _subscriberService = subscriberService;

            _breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList();
        }

        [HttpGet(AreaNames.Admin + "/subscribers/{page?}/{active?}")]
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

            subscribers.Paging.RootUrl = "admin/subscribers";
            subscribers.Paging.Parameters = active ?? string.Empty;

            var viewModel = new SubscriberListViewModel
            {
                SubscriberWithPaging = subscribers,
                Breadcrumbs = _breadcrumbs
            };

            return View(viewModel);
        }


        [Area(AreaNames.Admin)]
        [Route(AreaNames.Admin + "/subscriber/{subscriberId}/{page}/{active}", Name = "SubscriberProfile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Profile(int subscriberId, int page, string active)
        {
            var subscriber = await _subscriberService.GetSubscriberByIdAsync($"{subscriberId}");

            var parameters = new SubscriberParameters
            {
                SubscriberId = subscriberId,
                Page = page,
                Active = active
            };
            subscriber.SubscriberParameters = parameters;

            subscriber.Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true, showSubscriberList: true, subscriberParameters: parameters).ToList();

            return View(subscriber);
        }

        [Area(AreaNames.Admin)]
        [HttpGet(AreaNames.Admin + "/subscriber/add-subscriber")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult AddProfile()
        {
            var subscriberProfile = new SubscriberProfile
            {
                AccountActive = "Y",
                Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList()
            };

            ViewBag.Header = "Add new subscriber";
            ViewBag.Title = "Add subscriber";

            return View("ChangeProfile", subscriberProfile);
        }

        [Area(AreaNames.Admin)]
        [HttpPost(AreaNames.Admin + "/subscriber/add-subscriber")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AddProfile(SubscriberProfile subscriber)
        {
            ValidateDates(subscriber);

            ValidateEmails(subscriber);

            if (ModelState.IsValid)
            {
                var createUpdateSubscriber = CreateUpdateSubscriber(subscriber);

                await _subscriberService.CreateSubscriberAsync(createUpdateSubscriber);

                return RedirectToAction("Index", "AdminHome");
            }

            subscriber.Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true).ToList();

            ViewBag.Header = "Add new subscriber";
            ViewBag.Title = "Add subscriber";

            GetSortedErrors();

            return View("ChangeProfile", subscriber);
        }

        [Area(AreaNames.Admin)]
        [HttpGet(AreaNames.Admin + "/subscriber/{subscriberId}/{page}/{active}/change-profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeProfile(int subscriberId, int page, string active)
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
                ApplicationDay = subscriber.SubscriberDetails.ApplicationDate.Day.ToString(),
                ApplicationMonth = subscriber.SubscriberDetails.ApplicationDate.Month.ToString(),
                ApplicationYear = subscriber.SubscriberDetails.ApplicationDate.Year.ToString(),
                SubscribedFromDay = subscriber.SubscribedFrom?.Day.ToString(),
                SubscribedFromMonth = subscriber.SubscribedFrom?.Month.ToString(),
                SubscribedFromYear = subscriber.SubscribedFrom?.Year.ToString(),
                SubscribedToDay = subscriber.SubscribedTo?.Day.ToString(),
                SubscribedToMonth = subscriber.SubscribedTo?.Month.ToString(),
                SubscribedToYear = subscriber.SubscribedTo?.Year.ToString(),
                EmailAddress1 = subscriber.EmailContacts.Select(e => e.EmailAddress).FirstOrDefault(),
                EmailAddress2 = subscriber.EmailContacts.Skip(1).Select(e => e.EmailAddress).FirstOrDefault(),
                EmailAddress3 = subscriber.EmailContacts.Skip(2).Select(e => e.EmailAddress).FirstOrDefault(),
                AccountActive = subscriber.AccountActive,
            };

            var parameters = new SubscriberParameters
            {
                SubscriberId = subscriberId,
                Page = page,
                Active = active
            };

            subscriberProfile.SubscriberParameters = parameters;

            subscriberProfile.Breadcrumbs =
                BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true, showSubscriberList: true, showSubscriber:true, subscriberParameters: parameters).ToList();

            ViewBag.Header = "Update subscriber details";
            ViewBag.Title = "Update subscriber details";

            return View(subscriberProfile);
        }


        [Area(AreaNames.Admin)]
        [HttpPost(AreaNames.Admin + "/subscriber/change-profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeProfile(SubscriberProfile subscriber)
        {
            ValidateDates(subscriber);

            ValidateEmails(subscriber);

            if (ModelState.IsValid)
            {
               
                var createUpdateSubscriber = CreateUpdateSubscriber(subscriber);

                await _subscriberService.UpdateSubscriberAsync($"{subscriber.SubscriberId}", createUpdateSubscriber);

                return RedirectToAction("Profile", new
                {
                    subscriberId = subscriber.SubscriberId,
                    page = subscriber.SubscriberParameters.Page,
                    active = subscriber.SubscriberParameters.Active
                });
            }

            GetSortedErrors();

            subscriber.Breadcrumbs = BreadcrumbBuilder.BuildBreadcrumbs(isAdmin: true, showSubscriberList: true, showSubscriber: true, subscriberParameters: subscriber.SubscriberParameters).ToList();

            ViewBag.Header = "Update subscriber details";
            ViewBag.Title = "Update subscriber details";

            return View(subscriber);
        }

        private static CreateUpdateSubscriber CreateUpdateSubscriber(SubscriberProfile subscriber)
        {
            var createUpdateSubscriber = new CreateUpdateSubscriber
            {
                AccountActive = subscriber.AccountActive,
                ApplicationDate = subscriber.ApplicationDate,
                ContactAddress1 = subscriber.ContactAddress1 ?? string.Empty,
                ContactAddress2 = subscriber.ContactAddress2 ?? string.Empty,
                ContactCity = subscriber.ContactCity ?? string.Empty,
                ContactEmail = subscriber.ContactEmail ?? string.Empty,
                ContactForename = subscriber.ContactForename ?? string.Empty,
                ContactPostcode = subscriber.ContactPostcode ?? string.Empty,
                ContactSurname = subscriber.ContactSurname ?? string.Empty,
                ContactTelephone = subscriber.ContactTelephone ?? string.Empty,
                EmailAddresses = subscriber.EmailAddresses.ToList(),
                OrganisationName = subscriber.OrganisationName ?? string.Empty,
                OrganisationType = subscriber.OrganisationType,
                SubscribedFrom = subscriber.SubscribedFrom,
                SubscribedTo = subscriber.SubscribedTo
            };
            return createUpdateSubscriber;
        }

        private void ValidateDates(SubscriberProfile subscriber)
        {
            if (!string.IsNullOrEmpty(subscriber.ApplicationDay) && !string.IsNullOrEmpty(subscriber.ApplicationMonth) &&
                !string.IsNullOrEmpty(subscriber.ApplicationYear))
            {
                var isDate =
                    DateTime.TryParse(
                        $"{subscriber.ApplicationYear}-{subscriber.ApplicationMonth}-{subscriber.ApplicationDay}",
                        out _);

                if (!isDate)
                {
                    ModelState.AddModelError($"{nameof(subscriber.ApplicationDate)}",
                        "The application submitted date must be a real date");
                }
            }

            var validFrom = ValidateDate(subscriber.SubscribedFromDay, subscriber.SubscribedFromMonth,
                subscriber.SubscribedFromYear, nameof(subscriber.SubscribedFrom), "subscription start date");
            var validTo = ValidateDate(subscriber.SubscribedToDay, subscriber.SubscribedToMonth, subscriber.SubscribedToYear,
                nameof(subscriber.SubscribedTo), "subscription end date");

            if (validFrom && validTo && subscriber.SubscribedTo < subscriber.SubscribedFrom)
            {
                ModelState.AddModelError($"{nameof(subscriber.SubscribedTo)}Date",
                    "Enter a subscription end date which is the same as or after the subscription start date");
            }
        }

        private void ValidateEmails(SubscriberProfile subscriber)
        {
            if ((subscriber.EmailAddress1 == subscriber.EmailAddress2 && !string.IsNullOrEmpty(subscriber.EmailAddress2)) ||
                (subscriber.EmailAddress1 == subscriber.EmailAddress3 && !string.IsNullOrEmpty(subscriber.EmailAddress3)) ||
                (subscriber.EmailAddress2 == subscriber.EmailAddress3 && !string.IsNullOrEmpty(subscriber.EmailAddress2) && !string.IsNullOrEmpty(subscriber.EmailAddress3)))
            {
                ModelState.AddModelError("EmailAddresses",
                    "The email address entered for each data extract email address must be unique");
            }
        }

        private bool ValidateDate(string day, string month, string year, string fieldName, string messageName)
        {
            if (!string.IsNullOrEmpty(day) || !string.IsNullOrEmpty(month) || !string.IsNullOrEmpty(year))
            {
                if (string.IsNullOrEmpty(day))
                {
                    ModelState.AddModelError($"{fieldName}Day", $"The {messageName} must include a day");
                }

                if (string.IsNullOrEmpty(month))
                {
                    ModelState.AddModelError($"{fieldName}Month", $"The {messageName} must include a month");
                }

                if (string.IsNullOrEmpty(year))
                {
                    ModelState.AddModelError($"{fieldName}Year", $"The {messageName} must include a year");
                }
            }

            if (!string.IsNullOrEmpty(day) && !string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
            {
                var isDate = DateTime.TryParse($"{year}-{month}-{day}", out _);

                if (!isDate)
                {
                    ModelState.AddModelError($"{fieldName}Date", $"The {messageName} must be a real date");

                    return false;
                }

                return true;
            }

            return false;
        }

        private void GetSortedErrors()
        {
            if (ModelState.ErrorCount > 0)
            {
                ViewBag.SortedErrors = ModelState
                    .Select(m => new
                    {
                        Key = m.Key,
                        Order = ValidationOrder.SubscriberFieldValidationOrder.IndexOf(m.Key),
                        Error = m.Value
                    })
                    .SelectMany(m => m.Error.Errors.Select(e => new
                    {
                        m.Key,
                        m.Order,
                        e.ErrorMessage
                    }))
                    .OrderBy(m => m.Order);
            }
        }
    }
}