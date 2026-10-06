using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Price { get; set; }
        public string Product_Image { get; set; }
        public string Product_Description { get; set; }

        public int Cat_Id { get; set; }
    }
}
