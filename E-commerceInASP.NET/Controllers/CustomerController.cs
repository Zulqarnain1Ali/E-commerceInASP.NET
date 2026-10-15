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
            var featuredProducts = context.tbl_product
                                  .OrderByDescending(p => p.product_id)
                                  .Take(8)
                                  .ToList();

            return View(featuredProducts);
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
            if (customer != null && customer.Customer_Password == customer_password)
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
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Please fill in all required fields properly.";
                return View(customer);
            }
            try
            {
                context.tbl_customer.Add(customer);
                context.SaveChanges();
                return RedirectToAction("customerLogin");
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "An error occurred while saving your details. Please check your information and try again.";
                return View(customer);
            }
        }


        public IActionResult customerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("Index");
        }


        public IActionResult customerProfile()
        {
            string customerIdStr = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                return RedirectToAction("customerLogin");
            }
            int customerId = int.Parse(customerIdStr);

            var customer = context.tbl_customer.FirstOrDefault(c => c.Customer_id == customerId);
            if (customer == null)
            {
                return RedirectToAction("customerLogin");
            }

            return View(customer);
        }



        public IActionResult customerEdit()
        {
            string customerIdStr = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                return RedirectToAction("customerLogin");
            }

            int customerId = int.Parse(customerIdStr);
            var customer = context.tbl_customer.FirstOrDefault(c => c.Customer_id == customerId);
            if (customer == null)
            {
                return RedirectToAction("customerLogin");
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult customerEdit(Customer updatedCustomer, IFormFile? customerImageFile)
        {
            var existingCustomer = context.tbl_customer.FirstOrDefault(c => c.Customer_id == updatedCustomer.Customer_id);
            if (existingCustomer == null)
            {
                return RedirectToAction("customerLogin");
            }

            //image
            if (customerImageFile != null && customerImageFile.Length > 0)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/customer");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(customerImageFile.FileName);
                string filePath = Path.Combine(folderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customerImageFile.CopyTo(fileStream);
                }

                existingCustomer.Customer_Image = uniqueFileName;
            }

            // Update editable fields
            existingCustomer.Customer_Name = updatedCustomer.Customer_Name;
            existingCustomer.Customer_Email = updatedCustomer.Customer_Email;
            existingCustomer.Customer_Password = updatedCustomer.Customer_Password;
            existingCustomer.Customer_Phone = updatedCustomer.Customer_Phone;
            existingCustomer.Customer_Gender = updatedCustomer.Customer_Gender;
            existingCustomer.Customer_Country = updatedCustomer.Customer_Country;
            existingCustomer.Customer_City = updatedCustomer.Customer_City;
            existingCustomer.Customer_Address = updatedCustomer.Customer_Address;

            context.SaveChanges();

            return RedirectToAction("customerProfile");
        }



        // GET: Customer/feedback
        public IActionResult feedback()
        {
            var model = new Feedback();

            string customerIdStr = HttpContext.Session.GetString("customerSession");
            if (!string.IsNullOrEmpty(customerIdStr))
            {
                int customerId = int.Parse(customerIdStr);
                var customer = context.tbl_customer.FirstOrDefault(c => c.Customer_id == customerId);
                if (customer != null)
                {
                    model.user_name = customer.Customer_Name;
                    model.user_email = customer.Customer_Email;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult feedback(Feedback feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback.user_name) ||
                string.IsNullOrWhiteSpace(feedback.user_email) ||
                string.IsNullOrWhiteSpace(feedback.user_message))
            {
                ViewBag.ErrorMessage = "Please fill in all fields before submitting.";
                return View(feedback);
            }

            try
            {
                context.tbl_feedback.Add(feedback);
                context.SaveChanges();

                TempData["message"] = "Thank you for your feedback!";
                //ViewBag.Success = "Your feedback has been received. Thank you for reaching out!";
                //return View(new Feedback());
                return RedirectToAction("feedback");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while sending your feedback. Please try again.";
                return View(feedback);
            }
        }



        public IActionResult productDetails(int id)
        {
            var product = context.tbl_product.FirstOrDefault(p => p.product_id == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult allProducts()
        {
            var allProducts = context.tbl_product
                                     .OrderByDescending(p => p.product_id)
                                     .ToList();

            return View(allProducts);
        }


        public IActionResult addToCart()
        {
            return View();
        }




        //cart

//        using Microsoft.EntityFrameworkCore;

//// GET: Customer/cart
//public IActionResult cart()
//    {
//        string customerIdStr = HttpContext.Session.GetString("customerSession");
//        if (string.IsNullOrEmpty(customerIdStr))
//        {
//            return RedirectToAction("customerLogin");
//        }

//        int customerId = int.Parse(customerIdStr);

//        // Fetch all active cart items for the logged-in customer (Cart_Status == true or 1)
//        var cartItems = context.tbl_cart
//                               .Include(c => c.Product)
//                               .Where(c => c.Customer_Id == customerId && c.Cart_Status == true)
//                               .ToList();

//        return View(cartItems);
//    }

//    // POST: Customer/addToCart
//    [HttpPost]
//    public IActionResult addToCart(int productId, int quantity = 1)
//    {
//        string customerIdStr = HttpContext.Session.GetString("customerSession");
//        if (string.IsNullOrEmpty(customerIdStr))
//        {
//            return RedirectToAction("customerLogin");
//        }

//        int customerId = int.Parse(customerIdStr);

//        // Check if the customer already has this active item in cart
//        var existingCartItem = context.tbl_cart.FirstOrDefault(c =>
//            c.Customer_Id == customerId &&
//            c.Product_Id == productId &&
//            c.Cart_Status == true);

//        if (existingCartItem != null)
//        {
//            existingCartItem.Product_Quantity += quantity;
//            context.tbl_cart.Update(existingCartItem);
//        }
//        else
//        {
//            var newCart = new Cart
//            {
//                Customer_Id = customerId,
//                Product_Id = productId,
//                Product_Quantity = quantity,
//                Cart_Status = true // Active item
//            };
//            context.tbl_cart.Add(newCart);
//        }

//        context.SaveChanges();
//        TempData["CartMessage"] = "Item added to cart successfully!";
//        return RedirectToAction("cart");
//    }

//    // GET: Customer/removeFromCart/5
//    public IActionResult removeFromCart(int id)
//    {
//        var cartItem = context.tbl_cart.Find(id);
//        if (cartItem != null)
//        {
//            context.tbl_cart.Remove(cartItem);
//            context.SaveChanges();
//        }
//        return RedirectToAction("cart");
//    }



}
}