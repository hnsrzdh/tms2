using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public enum DriverWalletWithdrawalRequestStatus
    {
        Pending = 0,
        Paid = 1,
        Rejected = 2,
        Cancelled = 3
    }

    public class DriverWalletWithdrawalRequest
    {
        public long Id { get; set; }

        [Required]
        public int DriverProfileId { get; set; }
        public DriverProfile DriverProfile { get; set; } = null!;

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "مبلغ درخواست برداشت")]
        public decimal Amount { get; set; }

        [MaxLength(1000)]
        [Display(Name = "توضیح درخواست‌دهنده")]
        public string? RequestNote { get; set; }

        [Display(Name = "تاریخ ثبت درخواست")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(256)]
        public string? CreatedBy { get; set; }

        public DriverWalletWithdrawalRequestStatus Status { get; set; } = DriverWalletWithdrawalRequestStatus.Pending;

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "مبلغ پرداخت‌شده")]
        public decimal? PaidAmount { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PaidAt { get; set; }

        [MaxLength(256)]
        [Display(Name = "پرداخت‌شده توسط")]
        public string? PaidBy { get; set; }

        [MaxLength(2000)]
        [Display(Name = "توضیحات رسید / تراکنش")]
        public string? PaymentReceiptNote { get; set; }

        [Display(Name = "تاریخ رد")]
        public DateTime? RejectedAt { get; set; }

        [MaxLength(256)]
        [Display(Name = "رد شده توسط")]
        public string? RejectedBy { get; set; }

        [MaxLength(2000)]
        [Display(Name = "علت رد")]
        public string? RejectionNote { get; set; }
    }
}
