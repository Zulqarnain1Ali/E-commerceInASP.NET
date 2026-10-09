using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceInASP.NET.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MyContext context;

        public CustomerController(MyContext context)
        {
            this.context = context;
        }

        // 1. Storefront catalog
        public IActionResult Index()
        {
            var products = context.tbl_product.Include(p => p.Category).ToList();
            return View(products);
        }

        // 2. Login GET
        [HttpGet]
        public IActionResult customerLogin()
        {
            // If already logged in, redirect to catalog
            if (HttpContext.Session.GetString("customer_session") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // 3. Login POST
        [HttpPost]
        public IActionResult customerLogin(string customer_email, string customer_password)
        {
            var customer = context.tbl_customer
                .FirstOrDefault(c => c.customer_email == customer_email && c.customer_password == customer_password);

            if (customer != null)
            {
                // Store customer ID and name in session
                HttpContext.Session.SetString("customer_session", customer.customer_id.ToString());
                HttpContext.Session.SetString("customer_name", customer.customer_name);

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }

        // 4. Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_session");
            HttpContext.Session.Remove("customer_name");
            return RedirectToAction("customerLogin");
        }
    }
}using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceInASP.NET.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MyContext context;

        public CustomerController(MyContext context)
        {
            this.context = context;
        }

        // 1. Storefront catalog
        public IActionResult Index()
        {
            var products = context.tbl_product.Include(p => p.Category).ToList();
            return View(products);
        }

        // 2. Login GET
        [HttpGet]
        public IActionResult customerLogin()
        {
            // If already logged in, redirect to catalog
            if (HttpContext.Session.GetString("customer_session") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // 3. Login POST
        [HttpPost]
        public IActionResult customerLogin(string customer_email, string customer_password)
        {
            var customer = context.tbl_customer
                .FirstOrDefault(c => c.customer_email == customer_email && c.customer_password == customer_password);

            if (customer != null)
            {
                // Store customer ID and name in session
                HttpContext.Session.SetString("customer_session", customer.customer_id.ToString());
                HttpContext.Session.SetString("customer_name", customer.customer_name);

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }

        // 4. Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_session");
            HttpContext.Session.Remove("customer_name");
            return RedirectToAction("customerLogin");
        }
    }
}