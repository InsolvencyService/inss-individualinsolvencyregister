using System.Security.Claims;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Authentication;
using INSS.EIIR.Web.Constants;
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

        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return View("Admin");
        }

        [HttpPost("AdminLogin")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLoginAsync(User user)
        {
            var validUser = _authenticationProvider.GetAdminUser(user.UserName, user.Password);

            if (validUser == null)
            {
                return View("Admin", user);
            }

            await Authenticate(validUser);

            return RedirectToAction("Index", "DataExtract", new { area = AreaNames.Admin });

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Admin");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.UserRole)
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