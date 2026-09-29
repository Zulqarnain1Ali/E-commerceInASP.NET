using Microsoft.EntityFrameworkCore;

namespace E_commerceInASP.NET.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options)
        {
             
        }
        public DbSet<Admin> tbl_admin { get; set; }
    }
}
