using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class Feedback
    {
        [Key]
        public int id { get; set; }

        public string user_name { get; set;}
        public string user_email { get; set; }
        public string user_message { get; set; }
    }
}
