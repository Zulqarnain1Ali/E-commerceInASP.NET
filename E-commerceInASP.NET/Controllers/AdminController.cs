using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Admin_Login(string admin_email, string admin_password)
        {
            var admin = context.tbl_admin.FirstOrDefault(a => a.admin_email == admin_email && a.admin_password == admin_password);

            if (admin != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid Credentials";
            return View();
        }
    }
}
