using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class DriverWalletWithdrawalCreateVm
    {
        public int DriverProfileId { get; set; }
        public string? DriverName { get; set; }
        public string? NationalId { get; set; }
        public decimal WalletBalance { get; set; }

        [Required(ErrorMessage = "مبلغ برداشت الزامی است.")]
        [Range(1, double.MaxValue, ErrorMessage = "مبلغ برداشت باید بزرگتر از صفر باشد.")]
        [Display(Name = "مبلغ درخواست برداشت")]
        public decimal Amount { get; set; }

        [MaxLength(1000, ErrorMessage = "توضیح نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
        [Display(Name = "توضیح درخواست")]
        public string? RequestNote { get; set; }
    }

    public class DriverWalletWithdrawalIndexVm
    {
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => ((Page - 1) * PageSize) + 1;
        public List<DriverWalletWithdrawalItemVm> Items { get; set; } = new();
    }
    public class DriverWalletTransactionVm
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public decimal Amount { get; set; }
        public string Description { get; set; } = "";
        public long ReferenceId { get; set; }
        public bool IsWithdraw { get; set; }
    }
    public class DriverWalletWithdrawalItemVm
    {
        public long Id { get; set; }
        public int DriverProfileId { get; set; }
        public string DriverName { get; set; } = "-";
        public string? NationalId { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal Amount { get; set; }
        public string? RequestNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DriverWalletWithdrawalRequestStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "-";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public decimal? PaidAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? PaidBy { get; set; }
        public string? PaymentReceiptNote { get; set; }
        public string? RejectionNote { get; set; }
    }
}
