using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Customer
    {
        [Key]
        public int Customer_id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string Customer_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Customer_Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Customer_Password { get; set; } = string.Empty;

        // Optional fields
        public string? Customer_Phone { get; set; }
        public string? Customer_Gender { get; set; }
        public string? Customer_Image { get; set; }
        public string? Customer_Country { get; set; }
        public string? Customer_City { get; set; }
        public string? Customer_Address { get; set; }
    }
}