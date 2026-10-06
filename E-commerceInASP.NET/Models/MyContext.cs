using Microsoft.EntityFrameworkCore;

namespace E_commerceInASP.NET.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options)
        {
             
        }
        public DbSet<Admin> tbl_admin { get; set; }
        public DbSet<Customer> tbl_customer { get; set; }
        public DbSet<Category> tbl_category { get; set; }
        public DbSet<Cart> tbl_cart { get; set; }
        public DbSet<Feedback> tbl_feedback { get; set; }
        public DbSet<Product> tbl_product { get; set; }
        public DbSet<FAQ> tbl_faq { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    object hasForeignKey = modelBuilder.Entity<Product>()
        //        .HasOne( p => p.Category)
        //        .WithMany( c => c.Products)
        //        .HasForeignKey(p => p.Category_id);
        //}


    }
}
