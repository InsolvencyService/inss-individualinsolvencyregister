using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models.SubscriberModels;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace INSS.EIIR.Web.Areas.Admin.Controllers
{

    public class SubscriberController : Controller
    {
        private readonly ISubscriberDataProvider _subscriberDataProvider;

        public SubscriberController(ISubscriberDataProvider subscriberDataProvider)
        {
            _subscriberDataProvider = subscriberDataProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Area(AreaNames.Admin)]
        [Route(AreaNames.Admin + "/subscriber/{subscriberId}", Name = "SubscriberProfile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Profile(int subscriberId)
        {
            var subscriber = await _subscriberDataProvider.GetSubscriberByIdAsync($"{subscriberId}");

            return View(subscriber);
        }

        [Area(AreaNames.Admin)]
        [HttpGet(AreaNames.Admin + "/subscriber/{subscriberId}/change-profile")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeProfile(int subscriberId)
        {
            var subscriber = await _subscriberDataProvider.GetSubscriberByIdAsync($"{subscriberId}");

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

                await _subscriberDataProvider.UpdateSubscriberAsync($"{subscriber.SubscriberId}", createUpdateSubscriber);

                return RedirectToRoute("SubscriberProfile", new { subscriberId = subscriber.SubscriberId });

            }

            return View(subscriber);
        }
    }
}

