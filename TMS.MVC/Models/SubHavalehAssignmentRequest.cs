using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public class SubHavalehAssignmentRequest
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "ریزحواله")]
        public long SubHavalehId { get; set; }
        public SubHavaleh SubHavaleh { get; set; } = null!;

        [Required]
        [Display(Name = "کشنده")]
        public int TractorId { get; set; }
        public Tractor Tractor { get; set; } = null!;

        [Required]
        [StringLength(450)]
        [Display(Name = "درخواست‌دهنده")]
        public string RequesterUserId { get; set; } = string.Empty;
        public ApplicationUser? RequesterUser { get; set; }

        [Display(Name = "راننده پیشنهادی")]
        public int? DriverProfileId { get; set; }
        public DriverProfile? DriverProfile { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "مقدار پیشنهادی برای حمل")]
        public decimal RequestedCargoAmount { get; set; }

        [Display(Name = "ظرفیت کشنده کامل پر می‌شود")]
        public bool IsTruckCapacityFull { get; set; } = true;

        [Display(Name = "تاریخ مراجعه برای بارگیری")]
        public DateTime RequestedLoadingDate { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات درخواست‌دهنده")]
        public string? RequesterNote { get; set; }

        [Display(Name = "وضعیت درخواست")]
        public AssignmentRequestStatus Status { get; set; } = AssignmentRequestStatus.Pending;

        [Display(Name = "تاریخ ثبت درخواست")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(450)]
        [Display(Name = "بررسی‌کننده")]
        public string? ReviewedByUserId { get; set; }
        public ApplicationUser? ReviewedByUser { get; set; }

        [Display(Name = "تاریخ بررسی")]
        public DateTime? ReviewedAt { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات اپراتور")]
        public string? OperatorNote { get; set; }

        [Display(Name = "تخصیص ایجاد شده")]
        public long? CreatedTractorAssignmentId { get; set; }
        public TractorAssignment? CreatedTractorAssignment { get; set; }
    }

    public enum AssignmentRequestStatus
    {
        [Display(Name = "در انتظار بررسی")]
        Pending = 0,

        [Display(Name = "تایید شده")]
        Approved = 1,

        [Display(Name = "رد شده")]
        Rejected = 2,

        [Display(Name = "لغو شده")]
        Cancelled = 3
    }
}
