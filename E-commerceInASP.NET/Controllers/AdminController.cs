using E_commerceInASP.NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            if (string.IsNullOrEmpty(admin_email) || string.IsNullOrEmpty(admin_password))
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

        [HttpPost]
        public IActionResult updateCustomer(Customer customer, IFormFile file, string? newPassword)
        {
            ModelState.Remove("Customer_Password");
            var existingCustomer = context.tbl_customer.FirstOrDefault(a => a.Customer_id == customer.Customer_id);

            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/customer");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                existingCustomer.Customer_Image = "/images/customer/" + fileName;
            }

            existingCustomer.Customer_Name = customer.Customer_Name;
            existingCustomer.Customer_Email = customer.Customer_Email;
            existingCustomer.Customer_Phone = customer.Customer_Phone;
            existingCustomer.Customer_Gender = customer.Customer_Gender;
            existingCustomer.Customer_Country = customer.Customer_Country;
            existingCustomer.Customer_City = customer.Customer_City;
            existingCustomer.Customer_Address = customer.Customer_Address;

            if (!string.IsNullOrEmpty(newPassword))
            {
                existingCustomer.Customer_Password = newPassword;
            }

            context.SaveChanges();
            return RedirectToAction("customerDetails", new { id = customer.Customer_id });
        }

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





        public IActionResult fetchCategory()
        {
            return View(context.tbl_category.ToList());
        }

        public IActionResult addCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addCategory(Category cat)
        {
            // 1. Check if category already exists
            bool categoryExists = context.tbl_category
                .Any(c => c.Category_Name.ToLower() == cat.Category_Name.ToLower());

            if (categoryExists)
            {
                ModelState.AddModelError("Category_Name", "This category already exists!");
                return View(cat);
            }

            // 2. Ensure Category_Name isn't empty or null manually
            if (!string.IsNullOrEmpty(cat.Category_Name))
            {
                context.tbl_category.Add(cat);
                context.SaveChanges();
                return RedirectToAction("fetchCategory");
            }

            // If name is empty, re-display view
            ModelState.AddModelError("Category_Name", "Category name is required.");
            return View(cat);
        }

        public IActionResult updateCategory(int id)
        {
            var category = context.tbl_category.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult updateCategory(Category cat)
        {
            context.tbl_category.Update(cat);
            context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult deleteCategory(int id)
        {
            var category = context.tbl_category.Find(id);
            if (category != null)
            {
                context.tbl_category.Remove(category);
                context.SaveChanges();
            }
            return RedirectToAction("fetchCategory");
        }




        public IActionResult fetchProducts()
        {
            return View(context.tbl_product.Include(p => p.Category).ToList());
        }

        public IActionResult addProduct()
        {
            //ViewBag.Categories = new SelectList(context.tbl_category, "Category_id", "Category_Name");
            List<Category> categories = context.tbl_category.ToList();
            ViewData["category"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product prod, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product_Image");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                prod.Product_Image = "/images/Product_Image/" + fileName;
            }

            context.tbl_product.Add(prod);
            context.SaveChanges();
            return RedirectToAction("fetchProducts");
        }

        public IActionResult productDetails(int id)
        {
            var product = context.tbl_product
        .Include(p => p.Category)
        .FirstOrDefault(p => p.product_id == id);
            return View(context.tbl_product.Find(id));
        }

        public IActionResult updateProduct(int id)
        {
            var product = context.tbl_product.Find(id);

            ViewData["SelectedCategory"] = product.Category_id;
            ViewData["category"] = context.tbl_category.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult updateProduct(Product product, IFormFile file)
        {
            var existingProduct = context.tbl_product.Find(product.product_id);

            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product_Image");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                existingProduct.Product_Image = "/images/Product_Image/" + fileName;
            }

            existingProduct.Product_Name = product.Product_Name;
            existingProduct.Product_Price = product.Product_Price;
            existingProduct.Product_Description = product.Product_Description;
            existingProduct.Category_id = product.Category_id;

            context.SaveChanges();
            return RedirectToAction("productDetails", new { id = product.product_id });
        }

        public IActionResult deleteProduct(int id)
        {
            var product = context.tbl_product.Find(id);

            if (product != null)
            {
                context.tbl_product.Remove(product);
                context.SaveChanges();
                TempData["Message"] = "Product deleted successfully!";
            }

            return RedirectToAction("fetchProducts");
        }



        public IActionResult fetchFeedback()
        {
            var feedbackList = context.tbl_feedback
                              .OrderByDescending(f => f.id)
                              .ToList();

            return View(feedbackList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult deleteFeedback(int id)
        {
            var feedbackItem = context.tbl_feedback.Find(id);

            if (feedbackItem != null)
            {
                context.tbl_feedback.Remove(feedbackItem);
                context.SaveChanges();
                TempData["success"] = "Feedback deleted successfully.";
            }

            return RedirectToAction("fetchfeedback");
        }



        //cart
        public IActionResult fetchCart()
        {
            var cart = context.tbl_cart.Include(c => c.products).Include(c => c.customers).OrderByDescending(c => c.id).ToList();
            return View(cart);
        }

      
        [HttpPost]
        public IActionResult deleteCart(int id)
        {
            var cartItem = context.tbl_cart.Find(id);

            if (cartItem != null)
            {
                context.tbl_cart.Remove(cartItem);
                context.SaveChanges();
                TempData["success"] = "Cart entry removed successfully.";
            }

            return RedirectToAction("fetchCart");
        }


        [HttpPost]
        public IActionResult updateCart(int id)
        {
            var cartItem = context.tbl_cart.Find(id);

            if (cartItem != null)
            {
                if (cartItem.Cart_Status == 1)
                {
                    cartItem.Cart_Status = 2;
                    TempData["success"] = $"Order #{id} marked as Processed / Shipped.";
                }
                else if (cartItem.Cart_Status == 2)
                {
                    cartItem.Cart_Status = 3;
                    TempData["success"] = $"Order #{id} marked as Delivered / Completed.";
                }
                else if (cartItem.Cart_Status == 3)
                {
                    // Optional: Revert back to Processed if clicked again
                    cartItem.Cart_Status = 2;
                    TempData["success"] = $"Order #{id} status reverted to Processed.";
                }

                context.tbl_cart.Update(cartItem);
                context.SaveChanges();
            }

            return RedirectToAction("fetchCart");
        }







        //cart amount calculation remaining

    } 
}