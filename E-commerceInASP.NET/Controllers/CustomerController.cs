using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceInASP.NET.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MyContext context;

        public CustomerController(MyContext _context)
        {
            context = _context;
        }

        // 1. Storefront catalog
        public IActionResult Index()
        {
            var products = context.tbl_product.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult customerLogin()
        {
            if (HttpContext.Session.GetString("customerSession") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult customerLogin(string customer_email, string customer_password)
        {
            var customer = context.tbl_customer.FirstOrDefault(c => c.Customer_Email == customer_email);
            if(customer!=null && customer.Customer_Password== customer_password)
            {
                HttpContext.Session.SetString("customerSession", customer.Customer_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Credentials";
                return View();
            }
        }

        public IActionResult customerRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult customerRegistration(Customer customer)
        {
            context.tbl_customer.Add(customer);
            context.SaveChanges();
            return RedirectToAction("customerLogin");
        }


        // set customer accounts same as we set admin accounts
        // Next: login/registration option should disappear when user is login. video: 10, 6 min



        //// 2. Customer Login GET
        //[HttpGet]
        //public IActionResult customerLogin()
        //{
        //    if (HttpContext.Session.GetString("customer_session") != null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //// 3. Customer Login POST
        //[HttpPost]
        //public IActionResult customerLogin(string customer_email, string customer_password)
        //{
        //    var customer = context.tbl_customer
        //        .FirstOrDefault(c => c.Customer_Email == customer_email && c.Customer_Password == customer_password);

        //    if (customer != null)
        //    {
        //        HttpContext.Session.SetString("customer_session", customer.Customer_id.ToString());
        //        HttpContext.Session.SetString("customer_name", customer.Customer_Name);
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ErrorMessage = "Invalid email or password.";
        //    return View();
        //}

        //// 4. Logout
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Remove("customer_session");
        //    HttpContext.Session.Remove("customer_name");
        //    return RedirectToAction("customerLogin");
        //}


    }
}