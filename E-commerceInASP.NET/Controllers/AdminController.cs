using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceInASP.NET.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext context;

        public AdminController(MyContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin_Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Admin_Login(string admin_email, string admin_password, bool rememberMe)
        {
            if(string.IsNullOrEmpty(admin_email) || string.IsNullOrEmpty(admin_password))
            {
                ModelState.AddModelError("", "Email and password cant be empty.");
                return View();
            }
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_email == admin_email && a.admin_password == admin_password);

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.admin_email),
                    new Claim("AdminId", admin.admin_id.ToString())
                };
                var claimsIdentity = new ClaimIdentity(claims, CookieAuthenticationDefaults, AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index");

            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }
    }
}
