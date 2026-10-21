# E-Commerce Web Application in ASP.NET Core MVC

A full-stack e-commerce web platform built with ASP.NET Core MVC and Entity Framework Core. The system features separate customer storefront workflows and admin management panels, incorporating dynamic shopping carts, order status tracking, session-based authentication, and feedback management.

---

## Key Features

### Customer Storefront
* **Product Catalog & Browsing**: Browse items categorized with real-time product details and image previews.
* **Shopping Cart Management**: Add products to cart, increment/decrement quantities inline, and remove items with dynamic subtotal recalculation.
* **Auto-Filled Checkout**: Session-authenticated customers enjoy streamlined checkout with pre-populated name and contact details.
* **Cash on Delivery (COD) Flow**: Transparent order placement with dedicated confirmation screens.
* **Customer Feedback**: Submit product and service inquiries directly via the integrated contact module.

### Admin Dashboard
* **Product & Category CRUD**: Full management of store inventory, image paths, and category hierarchies.
* **Cart & Order Lifecycle Management**: Track user activity and transition order statuses via forward/revert controls:
  * `0`: In Cart (Active browsing)
  * `1`: Order Placed / Pending
  * `2`: Processed / Shipped
  * `3`: Delivered / Closed
* **Business Metrics & Analytics**: Real-time summary cards displaying active carts, pending orders, completed deliveries, and total store revenue.
* **Customer & Feedback Moderation**: Review registered accounts and read incoming customer feedback.

---

## Tech Stack

* **Framework**: ASP.NET Core (.NET 8 / .NET 9) MVC
* **ORM / Database Access**: Entity Framework Core (Code-First / DbContext)
* **Database**: Microsoft SQL Server / LocalDB
* **Frontend**: Razor Views (CSHTML), Bootstrap 5, HTML5, CSS3, JavaScript
* **State Management**: ASP.NET Core Session State

---

## Project Architecture

```text
E_commerceInASP.NET/
├── Controllers/
│   ├── AdminController.cs          # Admin inventory, cart & order moderation
│   └── CustomerController.cs       # Customer store, cart, checkout & feedback
├── Models/
│   ├── Cart.cs                     # Cart item model & status flags
│   ├── Category.cs                 # Product categories
│   ├── Customer.cs                 # Registered customer accounts
│   ├── Feedback.cs                 # Customer feedback submissions
│   ├── MyContext.cs                # EF Core DbContext
│   └── Product.cs                  # Store items and pricing details
├── Views/
│   ├── Admin/                      # Back-office views (fetchCart, CRUD screens)
│   ├── Customer/                   # Storefront views (viewCart, checkout, orderSuccess)
│   └── Shared/                     # Navigation layouts and partials
├── wwwroot/                        # Static assets (CSS, JS, product images)
└── appsettings.json                # Database connection strings and configs
