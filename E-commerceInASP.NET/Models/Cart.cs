using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceInASP.NET.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("products")]
        public int Product_Id { get; set; }

        [ForeignKey("customers")]
        public int Customer_Id { get; set; }

        public int Product_Quantity { get; set; }
        public int Total_Amount { get; set; }
        public int Cart_Status { get; set; }

        public virtual Product? products { get; set; }
        public virtual Customer? customers { get; set; }
    }
}