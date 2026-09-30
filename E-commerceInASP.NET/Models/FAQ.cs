using System.ComponentModel.DataAnnotations;

namespace E_commerceInASP.NET.Models
{
    public class FAQ
    {
        [Key]
        public int id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
