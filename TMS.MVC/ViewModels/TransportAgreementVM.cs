using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.ViewModels
{
    public class TransportAgreementIndexItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string CargoOwnerName { get; set; } = "";
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
        public string ProductName { get; set; } = "";
        public decimal Amount { get; set; }
        public string Unit { get; set; } = "";
        public string Status { get; set; } = "";
    }
    public class TransportAgreementIndexViewModel
    {
        public List<TransportAgreementIndexItemViewModel> Items { get; set; } = new();

        public string? Search { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

public class TransportAgreementUpsertViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = "";

        [Required]
        [Display(Name = "نام صاحب کالا")]
        public string CargoOwnerName { get; set; } = "";

        [Required]
        [Display(Name = "مبدا")]
        public string Origin { get; set; } = "";

        [Required]
        [Display(Name = "مقصد")]
        public string Destination { get; set; } = "";

        [Required]
        [Display(Name = "محصول")]
        public string ProductName { get; set; } = "";

        [Required]
        [Display(Name = "مقدار")]
        public decimal? Amount { get; set; }

        [Required]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = "";

        [Required]
        [Display(Name = "وضعیت")]
        public string Status { get; set; } = "";
    }

}
