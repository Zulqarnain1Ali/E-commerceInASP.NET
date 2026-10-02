using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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
                //cookies code
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.admin_email),
                    new Claim("AdminId", admin.admin_id.ToString())
                };
                //var claimsIdentity = new ClaimIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                }; 

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                //cookies code ended
                return RedirectToAction("Index");
            
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies"); // Clears the authentication cookie
            return RedirectToAction("Admin_Login");     // Sends them back to the login page
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var adminIdString = User.FindFirst("AdminId")?.Value;
            if (adminIdString == null) return RedirectToAction("Admin_Login");

            int adminId = int.Parse(adminIdString);
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_id == adminId);

            return View(admin);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(Admin model)
        {
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_id == model.admin_id);
            if (admin != null)
            {
                admin.admin_email = model.admin_email;
                context.SaveChanges();
            }
            ViewBag.Message = "Profile updated successfully";
            return RedirectToAction("Index");
        }

    }
}
