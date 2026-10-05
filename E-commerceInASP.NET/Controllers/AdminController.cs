using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace E_commerceInASP.NET.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public IActionResult Admin_Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

       
        public IActionResult Profile()
        {
            var adminIdString = User.FindFirst("AdminId")?.Value;
            if (adminIdString == null) return RedirectToAction("Admin_Login");

            int adminId = int.Parse(adminIdString);
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_id == adminId);

            return View(admin);
        }

        [HttpPost]
        public IActionResult Profile(Admin model, IFormFile file)
        {
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_id == model.admin_id);
            if (admin != null)
            {
                //admin.admin_email = model.admin_email;

                // Check if the user uploaded a file
                if (file != null && file.Length > 0)
                {
                    // Generate a unique filename to avoid naming conflicts
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Define the folder path inside wwwroot
                    string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/admin");

                    // Create the directory if it doesn't exist yet
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string filePath = Path.Combine(uploadDir, fileName);

                    // Save the file physically to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Save the relative path string into your database model property
                    admin.admin_image = "/images/admin/" + fileName;
                }

                // Update basic info
                admin.admin_email = model.admin_email;
                admin.admin_name = model.admin_name;

                // Only update the password if the admin typed a new one
                if (!string.IsNullOrEmpty(model.admin_password))
                {
                    // Note: If you are hashing passwords with BCrypt or SHA256, hash it here before saving!
                    admin.admin_password = model.admin_password;
                }

                context.SaveChanges();
            }
            ViewBag.Message = "Profile updated successfully";
            return RedirectToAction("Index");
        }


        public IActionResult fetchCustomer()
        {
            return View(context.tbl_customer.ToList());
        }

        public IActionResult customerDetails(int id)
        {
            var customer = context.tbl_customer.FirstOrDefault(c => c.Customer_id == id);
            return View(customer);
        }

        public IActionResult updateCustomer(int id)
        {   
            return View(context.tbl_customer.Find(id));
        }

        //[HttpPost]
        //public IActionResult updateCustomer(int id)
        //{
        //    return View();
        //}

        public IActionResult deleteCustomer(int id)
        {
            var customer = context.tbl_customer.Find(id);

            if (customer != null)
            {
                context.tbl_customer.Remove(customer);
                context.SaveChanges();
                TempData["Message"] = "Customer deleted successfully!";
            }
            
            return RedirectToAction("fetchCustomer");
        }
    }
}
