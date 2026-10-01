using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Admin
    {
        [Key]
        public int admin_id { get; set; }

        [Display(Name = "Name")]
        public string admin_name { get; set; }

        [Display(Name = "Email")]
        public string admin_email { get; set; }

        [Display(Name = "Password")]
        public string admin_password { get; set; }

        [Display(Name = "Profile Image")]
        public string admin_image { get; set; }
    }
}
