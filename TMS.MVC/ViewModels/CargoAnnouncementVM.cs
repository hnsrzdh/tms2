using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class CargoAnnouncementIndexItemViewModel
    {
        public long Id { get; set; }
        public string CustomerCompanyName { get; set; } = string.Empty;
        public string? ContactPersonName { get; set; }
        public string ContactMobile { get; set; } = string.Empty;
        public string? AnnouncementType { get; set; }
        public bool RequiresFleetEntryPermit { get; set; }
        public string OriginPlaceName { get; set; } = string.Empty;
        public string DestinationPlaceName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductAmount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime? LoadingStartDate { get; set; }
        public DateTime? LoadingEndDate { get; set; }
        public CargoAnnouncementStatus Status { get; set; }
        public string StatusDisplay { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? CreatedByDisplayName { get; set; }
        public string? OperatorNote { get; set; }
        public string? CreatedHavalehNumber { get; set; }
    }

    public class CargoAnnouncementIndexViewModel
    {
        public List<CargoAnnouncementIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class CargoAnnouncementFormViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "نام شرکت / صاحب کالا الزامی است.")]
        [Display(Name = "نام شرکت / صاحب کالا")]
        public string CustomerCompanyName { get; set; } = string.Empty;

        [Display(Name = "نام شخص رابط")]
        public string? ContactPersonName { get; set; }

        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [Display(Name = "موبایل")]
        public string ContactMobile { get; set; } = string.Empty;

        [Display(Name = "تلفن ثابت")]
        public string? ContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
        [Display(Name = "ایمیل")]
        public string? ContactEmail { get; set; }

        [Display(Name = "نوع درخواست / نوع حواله")]
        public string? AnnouncementType { get; set; }

        [Display(Name = "نیازمند اخذ مجوز ورود ناوگان")]
        public bool RequiresFleetEntryPermit { get; set; }

        [Required(ErrorMessage = "مبدا الزامی است.")]
        [Display(Name = "مبدا")]
        public string OriginPlaceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "مقصد الزامی است.")]
        [Display(Name = "مقصد")]
        public string DestinationPlaceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام محصول الزامی است.")]
        [Display(Name = "محصول")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "مقدار بار الزامی است.")]
        [Range(0.001, double.MaxValue, ErrorMessage = "مقدار بار باید بزرگتر از صفر باشد.")]
        [Display(Name = "مقدار بار")]
        public decimal? ProductAmount { get; set; }

        [Required(ErrorMessage = "واحد الزامی است.")]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = "کیلوگرم";

        [Display(Name = "شروع بازه بارگیری")]
        public DateTime? LoadingStartDate { get; set; }

        [Display(Name = "پایان بازه بارگیری")]
        public DateTime? LoadingEndDate { get; set; }

        [Display(Name = "جریمه کسری به ازای هر واحد")]
        public decimal? ShortagePenaltyPerUnit { get; set; }

        [Display(Name = "توضیحات مشتری")]
        public string? CustomerNotes { get; set; }
    }

    public class CargoAnnouncementDetailsViewModel
    {
        public CargoAnnouncement Entity { get; set; } = null!;
        public string StatusDisplay { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = string.Empty;
    }
}
