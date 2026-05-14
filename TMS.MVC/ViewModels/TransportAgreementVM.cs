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
        public int DocumentCount { get; set; }
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

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "وارد کردن نام صاحب کالا الزامی است.")]
        [Display(Name = "نام صاحب کالا")]
        public string CargoOwnerName { get; set; } = "";

        [Required(ErrorMessage = "وارد کردن مبدا الزامی است.")]
        [Display(Name = "مبدا")]
        public string Origin { get; set; } = "";

        [Display(Name = "مقصد")]
        public string? Destination { get; set; }

        [Required(ErrorMessage = "وارد کردن محصول الزامی است.")]
        [Display(Name = "محصول")]
        public string ProductName { get; set; } = "";

        [Required(ErrorMessage = "وارد کردن مقدار الزامی است.")]
        [Display(Name = "مقدار")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "انتخاب واحد الزامی است.")]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = "";

        [Display(Name = "وضعیت")]
        public string? Status { get; set; }
    }

    public class TransportAgreementDocumentsViewModel
    {
        public int TransportAgreementId { get; set; }
        public string TransportAgreementTitle { get; set; } = "";
        public string CargoOwnerName { get; set; } = "";
        public string RouteText { get; set; } = "";
        public List<TransportAgreementDocumentItemViewModel> Documents { get; set; } = new();
    }

    public class TransportAgreementDocumentItemViewModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = "";
        public string OriginalFileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
