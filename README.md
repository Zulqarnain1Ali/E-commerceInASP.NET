# Mobile Store — Full-Stack E-Commerce Web Application

A responsive, minimalist full-stack e-commerce web platform built with **ASP.NET Core MVC** and **Entity Framework Core**. The application features separated architectures for a public customer-facing storefront and a dedicated administrative operations portal with cookie-based authentication, real-time cart logic, catalog management, and media uploads.

---

### Project Screenshots

| Customer Storefront | Admin Dashboard |
| :---: | :---: |
| ![Storefront Home](screenshots/storefront-home.png) | ![Admin Dashboard](screenshots/admin-dashboard.png) |

| Product Details & Cart | Order & Catalog Operations |
| :---: | :---: |
| ![Cart & Details](screenshots/cart-details.png) | ![Admin Management](screenshots/admin-management.png) |

---

### Key Features

#### 1. Customer Storefront
- **Dynamic Catalog**: Category filtering and real-time inventory inspection.
- **Cart Management**: AJAX-enabled cart additions, quantity modifiers, and price totals calculation.
- **Order Pipeline**: Integrated checkout flow tracking orders across defined lifecycle statuses (`0: In-Cart`, `1: Pending`, `2: Shipped`, `3: Delivered`).
- **Support & Inquiries**: Dedicated customer feedback submission forms and FAQ reference modules.

#### 2. Admin Management Portal
- **Operational Dashboard**: Live database metrics summarizing total products, registered accounts, active orders, and pending feedback.
- **Decoupled Architecture**: Isolated administrative layout (`_AdminLayout.cshtml`) complete with sidebar navigation, active route indicators, and dedicated management controls.
- **Cookie Authentication**: Secure authentication with persistent login sessions (`rememberMe`) and claims-based identity resolution.
- **Product & Category CRUD**: Full lifecycle management with multipart file uploads for product assets and local storage synchronization.
- **Profile Configuration**: Live administrative avatar preview and credentials management.

---

### Tech Stack & Architecture

- **Framework**: ASP.NET Core MVC (.NET 10)
- **Database Engine**: Microsoft SQL Server
- **ORM**: Entity Framework Core (Code-First Migrations)
- **Frontend / Styling**: Razor Views (`.cshtml`), Bootstrap 5, Bootstrap Icons, Custom CSS
- **Authentication**: Cookie Authentication (`Microsoft.AspNetCore.Authentication.Cookies`)
- **Asset Handling**: Physical file persistence in `wwwroot/images/`

---


## Database Schema Overview

The entity data layer configured via `MyContext` manages the following operational tables:

| Entity | Primary Key | Key Responsibilities |
| :--- | :--- | :--- |
| **tbl_admin** | `admin_id` | Stores administrator credentials, names, and profile media. |
| **tbl_customer** | `customer_id` | Stores customer contact details, accounts, and session data. |
| **tbl_product** | `product_id` | Stores product details, pricing, image references, and category foreign keys. |
| **tbl_category** | `Category_id` | Groups catalog items for filtering and storefront navigation. |
| **tbl_cart** | `cart_id` | Manages active cart items, ordered quantities, and fulfillment stages (0 to 3). |
| **tbl_feedback** | `feedback_id` | Captures customer inquiries, support messages, and timestamps. |
| **tbl_faq** | `faq_id` | Stores general store policies and customer help data. |

---

### Solution Structure
```text
E-commerceInASP.NET/
├── Controllers/
│   ├── AdminController.cs          # Dashboard, catalog, orders, and authentication logic
│   └── CustomerController.cs       # Storefront, cart operations, feedback, and catalog views
├── Migrations/                     # EF Core database migration snapshots
├── Models/
│   ├── Admin.cs                    # Administrator accounts & authentication fields
│   ├── Cart.cs                     # Shopping cart items & order status tracking
│   ├── Category.cs                 # Catalog product categories
│   ├── Customer.cs                 # Registered customer accounts
│   ├── ErrorViewModel.cs           # Error handling models
│   ├── FAQ.cs                      # Help & store inquiries
│   ├── Feedback.cs                 # Customer support inquiries & notes
│   ├── MyContext.cs                # Entity Framework database context
│   └── Product.cs                  # Product catalog records & media references
├── Views/
│   ├── Admin/                      # Administrative views (Dashboard, Products, Profile, etc.)
│   ├── Customer/                   # Storefront views (Index, ViewCart, Checkout, Details)
│   └── Shared/
│       ├── _AdminLayout.cshtml     # Dedicated admin portal layout & sidebar
│       ├── _Layout.cshtml          # Public customer storefront layout
│       ├── _ViewImports.cshtml     # Shared tag helpers & namespaces
│       └── _ViewStart.cshtml       # Layout initialization
├── wwwroot/                        # Static assets (CSS, JS, vendor libs, uploaded images)
├── appsettings.json                # Connection strings & configuration settings
└── Program.cs                      # App pipeline, dependency injection, and auth services
```

---


## Getting Started

### Prerequisites
* .NET SDK 8.0 or 9.0
* Microsoft SQL Server (or LocalDB / Express)
* Visual Studio 2022 or Visual Studio Code

### 1. Clone the Repository
```bash
git clone [https://github.com/your-username/E-commerceInASP.NET.git](https://github.com/your-username/E-commerceInASP.NET.git)
cd E-commerceInASP.NET
```
---

### 2. Configure the Connection String
Open appsettings.json and adjust the connection string to target your local SQL Server instance:
```bash
"ConnectionStrings": {
  "MyConnection": "Server=localhost\\MSSQLSERVER01;Database=ECommerce;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
---

### 3. Apply Database Migrations
Run the following command using the .NET CLI or the Package Manager Console to generate the database schema:
```bash
# Using .NET CLI
dotnet ef database update

# Or via Package Manager Console in Visual Studio
Update-Database
```
---

### 4. Run the Project
Navigate to https://localhost:7284 (or your assigned local port) in your browser.
```bash
dotnet run
```

---

#### Default Credentials (Seed / Initial Setup)
For initial administrative access, insert an administrative record into tbl_admin using SQL Server Management Studio (SSMS) or your database terminal:
```bash
INSERT INTO tbl_admin (admin_name, admin_email, admin_password, admin_image)
VALUES ('Store Admin', 'admin@store.com', 'Admin@123', '/images/default-avatar.png');
```




Thanks for viewing!
