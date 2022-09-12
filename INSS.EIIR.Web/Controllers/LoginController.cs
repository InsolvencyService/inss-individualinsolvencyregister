using System.Security.Claims;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Web.Constants;
using INSS.EIIR.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public LoginController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(User user)
        {
            var validUser = _authenticationProvider.AdminAccountIsValid(user.UserName, user.Password);

            if (validUser)
            {
                await Authenticate(user , Roles.Admin);

                return RedirectToAction("Index", "DataExtract", new { area = AreaNames.Admin });
            }

            return View("Index", user);
        }

        private async Task Authenticate(User user, string role)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, role),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
