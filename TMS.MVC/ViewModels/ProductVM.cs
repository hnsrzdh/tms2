using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.Models.ViewModels
{
    public class ProductIndexVm
    {
        public List<Product> Items { get; set; } = new();

        public string? Search { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public class ProductFormVm
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "نام محصول")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "نوع محصول")]
        public string? Type { get; set; }
    }
}