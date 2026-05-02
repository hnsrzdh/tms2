using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class OpenSubHavalehIndexViewModel
    {
        public List<OpenSubHavalehIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class OpenSubHavalehIndexItemViewModel
    {
        public long Id { get; set; }
        public long HavalehId { get; set; }
        public string HavalehNumber { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? ProductName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? RouteSummary { get; set; }
        public decimal RequestedCargoAmount { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
        public decimal PendingRequestedAmount { get; set; }
        public decimal RemainingAssignableAmount { get; set; }
        public string? Unit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DriverPricePer1000Unit { get; set; }
        public string? DriverPriceCurrency { get; set; }
        public bool CanRequest => RemainingAssignableAmount > 0;
    }

    public class AssignmentRequestCreateViewModel
    {
        [Required]
        public long SubHavalehId { get; set; }

        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }
        public decimal RequestedCargoAmountTotal { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal PendingRequestedAmount { get; set; }
        public decimal RemainingAssignableAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "انتخاب کشنده الزامی است.")]
        [Display(Name = "کشنده")]
        public int TractorId { get; set; }

        [Display(Name = "پلاک کشنده")]
        public string? TractorPlateNumber { get; set; }

        [Required(ErrorMessage = "مقدار پیشنهادی برای حمل الزامی است.")]
        [Range(0.001, double.MaxValue, ErrorMessage = "مقدار پیشنهادی باید بزرگتر از صفر باشد.")]
        [Display(Name = "مقدار پیشنهادی برای حمل")]
        public decimal? RequestedCargoAmount { get; set; }

        [Display(Name = "ظرفیت کشنده کامل پر می‌شود")]
        public bool IsTruckCapacityFull { get; set; } = true;

        [Required(ErrorMessage = "تاریخ مراجعه برای بارگیری الزامی است.")]
        [Display(Name = "تاریخ مراجعه برای بارگیری")]
        public DateTime? RequestedLoadingDate { get; set; }

        [StringLength(1000)]
        [Display(Name = "توضیحات")]
        public string? RequesterNote { get; set; }
    }

    public class AssignmentRequestIndexViewModel
    {
        public List<AssignmentRequestIndexItemViewModel> Items { get; set; } = new();
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
    }

    public class AssignmentRequestIndexItemViewModel
    {
        public long Id { get; set; }
        public long SubHavalehId { get; set; }
        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? RequesterName { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public decimal RequestedCargoAmount { get; set; }
        public string? Unit { get; set; }
        public DateTime RequestedLoadingDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public AssignmentRequestStatus Status { get; set; }
        public string StatusDisplay { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public bool IsTruckCapacityFull { get; set; }
        public string? RequesterNote { get; set; }
        public string? OperatorNote { get; set; }
    }
}
