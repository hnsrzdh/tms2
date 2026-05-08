using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.Models.ViewModels
{
    public class ProductIndexVm
    {
        public List<Product> Items { get; set; } = new();

        public string? Search { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int TotalPages => PageSize <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public class ProductFormVm
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام محصول الزامی است")]
        [StringLength(300)]
        [Display(Name = "نام محصول")]
        public string Name { get; set; } = string.Empty;

        // برای سازگاری با کدهای قبلی نگه داشته شده است.
        // در فرم جدید نمایش داده نمی‌شود.
        [StringLength(300)]
        [Display(Name = "نوع محصول")]
        public string? Type { get; set; }

        public List<ProductPropertyFormVm> Properties { get; set; } = new();
    }

    public class ProductPropertyFormVm
    {
        public long Id { get; set; }

        [StringLength(200)]
        [Display(Name = "نام خصوصیت")]
        public string? Name { get; set; }

        [StringLength(500)]
        [Display(Name = "مقدار")]
        public string? Value { get; set; }

        public int DisplayOrder { get; set; }
    }
}
