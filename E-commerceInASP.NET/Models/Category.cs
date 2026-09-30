using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Category
    {
        [Key]
        public int Key { get; set; }
        public string Category_Name { get; set; }
    }
}
