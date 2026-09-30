using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        public int Product_Quantity { get; set; }
        public Boolean Cart_Status { get; set; }
        public int Product_Id { get; set; }
        public int Customer_Id { get; set; }

    }
}
