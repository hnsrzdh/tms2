using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class CargoAnnouncement
    {
        public long Id { get; set; }

        [StringLength(450)]
        [Display(Name = "کاربر ثبت‌کننده")]
        public string? CreatedByUserId { get; set; }

        [StringLength(200)]
        [Display(Name = "نام ثبت‌کننده")]
        public string? CreatedByDisplayName { get; set; }

        [Required(ErrorMessage = "نام شرکت / صاحب کالا الزامی است.")]
        [StringLength(250)]
        [Display(Name = "نام شرکت / صاحب کالا")]
        public string CustomerCompanyName { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "نام شخص رابط")]
        public string? ContactPersonName { get; set; }

        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [StringLength(30)]
        [Display(Name = "موبایل")]
        public string ContactMobile { get; set; } = string.Empty;

        [StringLength(30)]
        [Display(Name = "تلفن ثابت")]
        public string? ContactPhone { get; set; }

        [StringLength(200)]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
        [Display(Name = "ایمیل")]
        public string? ContactEmail { get; set; }

        [StringLength(100)]
        [Display(Name = "نوع درخواست / نوع حواله")]
        public string? AnnouncementType { get; set; }

        [Display(Name = "نیازمند اخذ مجوز ورود ناوگان")]
        public bool RequiresFleetEntryPermit { get; set; }

        [Required(ErrorMessage = "مبدا الزامی است.")]
        [StringLength(250)]
        [Display(Name = "مبدا")]
        public string OriginPlaceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "مقصد الزامی است.")]
        [StringLength(250)]
        [Display(Name = "مقصد")]
        public string DestinationPlaceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام محصول الزامی است.")]
        [StringLength(250)]
        [Display(Name = "محصول")]
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,3)")]
        [Required(ErrorMessage = "مقدار بار الزامی است.")]
        [Range(0.001, double.MaxValue, ErrorMessage = "مقدار بار باید بزرگتر از صفر باشد.")]
        [Display(Name = "مقدار بار")]
        public decimal ProductAmount { get; set; }

        [Required(ErrorMessage = "واحد الزامی است.")]
        [StringLength(20)]
        [Display(Name = "واحد")]
        public string Unit { get; set; } = "کیلوگرم";

        [Display(Name = "شروع بازه بارگیری")]
        public DateTime? LoadingStartDate { get; set; }

        [Display(Name = "پایان بازه بارگیری")]
        public DateTime? LoadingEndDate { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "جریمه کسری به ازای هر واحد")]
        public decimal? ShortagePenaltyPerUnit { get; set; }

        [StringLength(2000)]
        [Display(Name = "توضیحات مشتری")]
        public string? CustomerNotes { get; set; }

        [Display(Name = "وضعیت")]
        public CargoAnnouncementStatus Status { get; set; } = CargoAnnouncementStatus.Pending;

        [StringLength(2000)]
        [Display(Name = "یادداشت اپراتور")]
        public string? OperatorNote { get; set; }

        [StringLength(450)]
        [Display(Name = "کاربر بررسی‌کننده")]
        public string? ReviewedByUserId { get; set; }

        [StringLength(200)]
        [Display(Name = "نام بررسی‌کننده")]
        public string? ReviewedByDisplayName { get; set; }

        [Display(Name = "تاریخ بررسی")]
        public DateTime? ReviewedAt { get; set; }

        [StringLength(100)]
        [Display(Name = "شماره حواله ایجاد شده")]
        public string? CreatedHavalehNumber { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ آخرین تغییر")]
        public DateTime? UpdatedAt { get; set; }
    }

    public enum CargoAnnouncementStatus
    {
        [Display(Name = "در انتظار بررسی")]
        Pending = 0,

        [Display(Name = "تماس گرفته شد")]
        Contacted = 1,

        [Display(Name = "تبدیل به حواله شد")]
        ConvertedToHavaleh = 2,

        [Display(Name = "رد شد")]
        Rejected = 3,

        [Display(Name = "لغو شد")]
        Cancelled = 4
    }
}
