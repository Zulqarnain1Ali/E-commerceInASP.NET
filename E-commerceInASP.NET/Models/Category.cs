using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }
        public string Category_Name { get; set; }

        public List<Product> Products { get; set; } // Navigation collection
    }
}
