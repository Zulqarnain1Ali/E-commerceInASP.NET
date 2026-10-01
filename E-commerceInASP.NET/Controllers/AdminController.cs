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
            var adminList = context.tbl_admin.ToList();
            return View(adminList);
        }
    }
}
