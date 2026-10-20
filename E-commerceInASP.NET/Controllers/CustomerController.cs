using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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




        [HttpPost]
        public IActionResult addToCart(int Prod_id, int quantity = 1)
        {
            string isLogin = HttpContext.Session.GetString("customerSession");

            if (string.IsNullOrEmpty(isLogin))
            {
                return Json(new { success = false, redirectUrl = Url.Action("customerLogin") });
            }

            int customerId = int.Parse(isLogin);
            int qtyToAdd = quantity < 1 ? 1 : quantity;

            var existingItem = context.tbl_cart
                .FirstOrDefault(c => c.Product_Id == Prod_id && c.Customer_Id == customerId && c.Cart_Status == 0);

            if (existingItem != null)
            {
                existingItem.Product_Quantity += qtyToAdd;
            }
            else
            {
                var cart = new Cart
                {
                    Product_Id = Prod_id,
                    Customer_Id = customerId,
                    Product_Quantity = qtyToAdd,
                    Cart_Status = 0 // 0 = Active cart item
                };
                context.tbl_cart.Add(cart);
            }

            context.SaveChanges();

            return Json(new { success = true, message = "Added to Cart!" });
        }



        public IActionResult viewCart()
        {
            string customerId = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("customerLogin");
            }

            List<Category> category = context.tbl_category.ToList();
            ViewData["category"] = category;

            int parsedCustomerId = int.Parse(customerId);

            var cart = context.tbl_cart
                              .Where(c => c.Customer_Id == int.Parse(customerId) && c.Cart_Status == 0)
                              .Include(c => c.products)
                              .ToList();

            return View(cart);
        }


        [HttpPost]
        public IActionResult deleteCart(int id)
        {
            string customerId = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("customerLogin");
            }

            var cartItem = context.tbl_cart.Find(id);
            if (cartItem != null)
            {
                context.tbl_cart.Remove(cartItem);
                context.SaveChanges();
                TempData["CartMessage"] = "Item removed from your cart.";
            }

            return RedirectToAction("viewCart");
        }

        [HttpPost]
        public IActionResult updateCartQuantity(int cartId, int quantity)
        {
            string customerId = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("customerLogin");
            }

            if (quantity <= 0)
            {
                return RedirectToAction("deleteCart", new { id = cartId });
            }

            var cartItem = context.tbl_cart.Find(cartId);
            if (cartItem != null && cartItem.Customer_Id == int.Parse(customerId))
            {
                cartItem.Product_Quantity = quantity;
                context.tbl_cart.Update(cartItem);
                context.SaveChanges();
            }

            return RedirectToAction("viewCart");
        }





        public IActionResult checkout()
        {
            string customerId = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("customerLogin");
            }

            var activeCart = context.tbl_cart
                                    .Where(c => c.Customer_Id == int.Parse(customerId) && c.Cart_Status == 0)
                                    .Include(c => c.products)
                                    .ToList();

            if (!activeCart.Any())
            {
                return RedirectToAction("viewCart");
            }

            return View(activeCart);
        }
        //cart
        // video 15, 10min
    }
}