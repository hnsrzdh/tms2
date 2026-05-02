using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class TractorAssignment
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "زیرحواله")]
        public long SubHavalehId { get; set; }
        public SubHavaleh SubHavaleh { get; set; } = null!;

        [Required]
        [Display(Name = "کشنده")]
        public int TractorId { get; set; }
        public Tractor Tractor { get; set; } = null!;

        [Display(Name = "راننده")]
        public int? DriverProfileId { get; set; }
        public DriverProfile? DriverProfile { get; set; }

        [Display(Name = "تاریخ تخصیص")]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "توضیحات")]
        public string? Notes { get; set; }

        [Display(Name = "وضعیت سفر")]
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Assigned;

        [Display(Name = "تاریخ رسیدن به مبدا")]
        public DateTime? ArrivalAtOriginDate { get; set; }

        [Display(Name = "تأیید رسیدن به مبدا")]
        public bool IsArrivalAtOriginConfirmed { get; set; }

        [Display(Name = "تأییدکننده رسیدن به مبدا")]
        public string? ArrivalAtOriginConfirmedBy { get; set; }

        [Display(Name = "تاریخ شروع بارگیری")]
        public DateTime? LoadingStartDate { get; set; }

        [Display(Name = "تاریخ پایان بارگیری")]
        public DateTime? LoadingEndDate { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار بارگیری شده")]
        public decimal? LoadedAmount { get; set; }

        [Display(Name = "تأیید بارگیری")]
        public bool IsLoadingConfirmed { get; set; }

        [Display(Name = "تأییدکننده بارگیری")]
        public string? LoadingConfirmedBy { get; set; }

        public virtual ICollection<LoadingDocument> LoadingDocuments { get; set; } = new List<LoadingDocument>();

        [Display(Name = "تاریخ رسیدن به مقصد")]
        public DateTime? ArrivalAtDestinationDate { get; set; }

        [Display(Name = "تأیید رسیدن به مقصد")]
        public bool IsArrivalAtDestinationConfirmed { get; set; }

        [Display(Name = "تأییدکننده رسیدن به مقصد")]
        public string? ArrivalAtDestinationConfirmedBy { get; set; }

        [Display(Name = "تاریخ شروع تخلیه")]
        public DateTime? UnloadingStartDate { get; set; }

        [Display(Name = "تاریخ پایان تخلیه")]
        public DateTime? UnloadingEndDate { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار تخلیه شده")]
        public decimal? UnloadedAmount { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار کسری")]
        public decimal? ShortageAmount { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار کسری مشمول جریمه")]
        public decimal? ChargeableShortageAmount { get; set; }

        [Display(Name = "تأیید تخلیه")]
        public bool IsUnloadingConfirmed { get; set; }

        [Display(Name = "تأییدکننده تخلیه")]
        public string? UnloadingConfirmedBy { get; set; }

        public virtual ICollection<UnloadingDocument> UnloadingDocuments { get; set; } = new List<UnloadingDocument>();

        [Display(Name = "تاریخ لغو")]
        public DateTime? CancellationDate { get; set; }

        [Display(Name = "لغو توسط")]
        public string? CancelledBy { get; set; }

        [Display(Name = "دلیل لغو")]
        [StringLength(1000)]
        public string? CancellationReason { get; set; }

        [Display(Name = "درخواست لغو توسط راننده")]
        public bool IsCancellationRequestedByDriver { get; set; }

        [Display(Name = "تاریخ درخواست لغو")]
        public DateTime? CancellationRequestDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "کرایه نهایی")]
        public decimal? FinalFare { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "جریمه کسری")]
        public decimal? ShortagePenalty { get; set; }

        [Display(Name = "روزهای تاخیر بارگیری")]
        public int? LoadingDelayDays { get; set; }

        [Display(Name = "روزهای تاخیر تخلیه")]
        public int? DeliveryDelayDays { get; set; }

        [Display(Name = "کل روزهای تاخیر")]
        public int? TotalDelayDays { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "جریمه تأخیر")]
        public decimal? DelayPenalty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "مبلغ قابل پرداخت")]
        public decimal? PayableAmount { get; set; }

        [StringLength(30)]
        [Display(Name = "واحد پولی مبنای محاسبات")]
        public string? FinancialBaseCurrency { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "نرخ تبدیل کرایه راننده به ارز مبنا")]
        public decimal? DriverPriceExchangeRateToBase { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "نرخ تبدیل انعام راننده به ارز مبنا")]
        public decimal? DriverTipExchangeRateToBase { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "نرخ تبدیل حق توقف راننده به ارز مبنا")]
        public decimal? DriverStopFeeExchangeRateToBase { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "نرخ تبدیل جریمه تأخیر به ارز مبنا")]
        public decimal? DelayPenaltyExchangeRateToBase { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "نرخ تبدیل جریمه کسری به ارز مبنا")]
        public decimal? ShortagePenaltyExchangeRateToBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "مبلغ محاسبه‌شده سیستم")]
        public decimal? FinancialCalculatedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "مبلغ دستی اپراتور")]
        public decimal? FinancialManualAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "مبلغ نهایی تاییدشده")]
        public decimal? FinancialApprovedAmount { get; set; }

        [Display(Name = "تایید مالی شده")]
        public bool IsFinancialApproved { get; set; }

        [StringLength(500)]
        [Display(Name = "توضیح تغییر مبلغ توسط اپراتور")]
        public string? FinancialAdjustmentNote { get; set; }

        [StringLength(100)]
        [Display(Name = "تاییدکننده مالی")]
        public string? FinancialApprovedBy { get; set; }

        [Display(Name = "تاریخ تایید مالی")]
        public DateTime? FinancialApprovedDate { get; set; }

        [Display(Name = "تسویه شده")]
        public bool IsSettled { get; set; }

        [Display(Name = "پرداخت به")]
        [StringLength(50)]
        public string? SettledTo { get; set; }

        [Display(Name = "پرداخت توسط")]
        public string? SettledBy { get; set; }

        [Display(Name = "تاریخ تسویه")]
        public DateTime? SettledDate { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }

    public enum AssignmentStatus
    {
        [Display(Name = "تخصیص داده شده")]
        Assigned,
        [Display(Name = "رسیده به مبدا")]
        ArrivedAtOrigin,
        [Display(Name = "در حال بارگیری")]
        Loading,
        [Display(Name = "بارگیری شده")]
        Loaded,
        [Display(Name = "در راه")]
        InTransit,
        [Display(Name = "رسیده به مقصد")]
        ArrivedAtDestination,
        [Display(Name = "در حال تخلیه")]
        Unloading,
        [Display(Name = "تخلیه شده")]
        Unloaded,
        [Display(Name = "تکمیل شده")]
        Completed,
        [Display(Name = "درخواست لغو")]
        CancellationRequested,
        [Display(Name = "لغو شده")]
        Cancelled
    }

    public class LoadingDocument
    {
        public long Id { get; set; }
        [Required]
        public long TractorAssignmentId { get; set; }
        public TractorAssignment TractorAssignment { get; set; } = null!;
        [Required]
        [StringLength(200)]
        [Display(Name = "عنوان مدرک")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        [Display(Name = "مسیر فایل")]
        public string FilePath { get; set; } = string.Empty;
        [StringLength(100)]
        [Display(Name = "نوع مدرک")]
        public string? DocumentType { get; set; }
        [Display(Name = "تاریخ آپلود")]
        public DateTime UploadDate { get; set; } = DateTime.Now;
        [Display(Name = "آپلودکننده")]
        public string? UploadedBy { get; set; }
        [Display(Name = "تأیید شده")]
        public bool IsApproved { get; set; }
        [Display(Name = "تأییدکننده")]
        public string? ApprovedBy { get; set; }
        [Display(Name = "تاریخ تأیید")]
        public DateTime? ApprovalDate { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات رد")]
        public string? RejectionNote { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات")]
        public string? Notes { get; set; }
    }

    public class UnloadingDocument
    {
        public long Id { get; set; }
        [Required]
        public long TractorAssignmentId { get; set; }
        public TractorAssignment TractorAssignment { get; set; } = null!;
        [Required]
        [StringLength(200)]
        [Display(Name = "عنوان مدرک")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        [Display(Name = "مسیر فایل")]
        public string FilePath { get; set; } = string.Empty;
        [StringLength(100)]
        [Display(Name = "نوع مدرک")]
        public string? DocumentType { get; set; }
        [Display(Name = "تاریخ آپلود")]
        public DateTime UploadDate { get; set; } = DateTime.Now;
        [Display(Name = "آپلودکننده")]
        public string? UploadedBy { get; set; }
        [Display(Name = "تأیید شده")]
        public bool IsApproved { get; set; }
        [Display(Name = "تأییدکننده")]
        public string? ApprovedBy { get; set; }
        [Display(Name = "تاریخ تأیید")]
        public DateTime? ApprovalDate { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات رد")]
        public string? RejectionNote { get; set; }
        [StringLength(500)]
        [Display(Name = "توضیحات")]
        public string? Notes { get; set; }
    }

    public class LocationTracking
    {
        public long Id { get; set; }
        [Required]
        public long TractorAssignmentId { get; set; }
        public TractorAssignment TractorAssignment { get; set; } = null!;
        [Display(Name = "عرض جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal Latitude { get; set; }
        [Display(Name = "طول جغرافیایی")]
        [Column(TypeName = "decimal(10,7)")]
        public decimal Longitude { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [StringLength(500)]
        [Display(Name = "توضیحات")]
        public string? Notes { get; set; }
        [Display(Name = "سرعت")]
        public decimal? Speed { get; set; }
        [Display(Name = "جهت")]
        public decimal? Heading { get; set; }
    }

    public class ChatMessage
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "تخصیص کشنده")]
        public long TractorAssignmentId { get; set; }
        public TractorAssignment TractorAssignment { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Display(Name = "فرستنده")]
        public string SenderId { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        [Display(Name = "نام فرستنده")]
        public string SenderName { get; set; } = string.Empty;
        [StringLength(50)]
        [Display(Name = "نقش فرستنده")]
        public string SenderRole { get; set; } = string.Empty;
        [StringLength(2000)]
        [Display(Name = "متن پیام")]
        public string? Message { get; set; }
        [StringLength(500)]
        [Display(Name = "مسیر فایل")]
        public string? FilePath { get; set; }
        [StringLength(200)]
        [Display(Name = "نام فایل")]
        public string? FileName { get; set; }
        [StringLength(100)]
        [Display(Name = "نوع فایل")]
        public string? FileContentType { get; set; }
        [Display(Name = "حجم فایل")]
        public long? FileSize { get; set; }
        [Display(Name = "تاریخ ارسال")]
        public DateTime SentDate { get; set; } = DateTime.Now;
        [Display(Name = "خوانده شده")]
        public bool IsRead { get; set; }
        [Display(Name = "تاریخ خوانده شدن")]
        public DateTime? ReadDate { get; set; }
    }
}
