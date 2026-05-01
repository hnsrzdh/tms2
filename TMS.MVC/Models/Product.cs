using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "نام محصول")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "نوع")]
        public string? Type { get; set; }
    }
}