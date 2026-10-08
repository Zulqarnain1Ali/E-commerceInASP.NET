using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceInASP.NET.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MyContext context;

        public CustomerController(MyContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
