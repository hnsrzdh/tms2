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

        // نگه داشته شده برای سازگاری با بخش‌های قبلی سیستم.
        // مقدار آن در کنترلر از خصوصیت «نوع بار» پر می‌شود.
        [StringLength(300)]
        [Display(Name = "نوع")]
        public string? Type { get; set; }

        public ICollection<ProductProperty> Properties { get; set; } = new List<ProductProperty>();
    }

    public class ProductProperty
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [Required(ErrorMessage = "نام خصوصیت الزامی است")]
        [StringLength(200)]
        [Display(Name = "نام خصوصیت")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "مقدار")]
        public string? Value { get; set; }

        public int DisplayOrder { get; set; }
    }
}
