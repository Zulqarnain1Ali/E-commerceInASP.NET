using Microsoft.AspNetCore.Mvc;

namespace E_commerceInASP.NET.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
