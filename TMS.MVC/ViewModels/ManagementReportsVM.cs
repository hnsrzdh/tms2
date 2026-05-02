using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class ManagementReportVm
    {
        public string Title { get; set; } = "";
        public string PeriodTitle { get; set; } = "";
        public string ReportMode { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? JalaliYear { get; set; }
        public int? JalaliMonth { get; set; }

        public string? Search { get; set; }

        public ReportSummaryVm Summary { get; set; } = new();
        public List<ReportStatusItemVm> HavalehStatusItems { get; set; } = new();
        public List<ReportStatusItemVm> AssignmentStatusItems { get; set; } = new();
        public List<ReportDailyTrendVm> DailyTrends { get; set; } = new();

        public List<ReportHavalehItemVm> Havalehs { get; set; } = new();
        public List<ReportSubHavalehItemVm> SubHavalehs { get; set; } = new();
        public List<ReportAssignmentItemVm> Assignments { get; set; } = new();
        public List<ReportDriverItemVm> Drivers { get; set; } = new();
        public List<ReportTractorItemVm> Tractors { get; set; } = new();
        public List<ReportCargoRequestItemVm> CargoRequests { get; set; } = new();
        public List<ReportCargoAnnouncementItemVm> CargoAnnouncements { get; set; } = new();

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalHavalehs { get; set; }
        public int TotalSubHavalehs { get; set; }
        public int TotalAssignments { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalTractors { get; set; }
        public int TotalCargoRequests { get; set; }
        public int TotalCargoAnnouncements { get; set; }
    }

    public class ReportSummaryVm
    {
        public int HavalehCount { get; set; }
        public int SubHavalehCount { get; set; }
        public int AssignmentCount { get; set; }
        public int ActiveAssignmentCount { get; set; }
        public int CompletedAssignmentCount { get; set; }

        public int DriverCount { get; set; }
        public int BlockedDriverCount { get; set; }

        public int TractorCount { get; set; }
        public int ActiveTractorCount { get; set; }

        public int CargoRequestCount { get; set; }
        public int PendingCargoRequestCount { get; set; }

        public int CargoAnnouncementCount { get; set; }
        public int PendingCargoAnnouncementCount { get; set; }

        public decimal TotalHavalehAmount { get; set; }
        public decimal TotalSubHavalehAmount { get; set; }
        public decimal TotalAssignedAmount { get; set; }
        public decimal TotalLoadedAmount { get; set; }
        public decimal TotalUnloadedAmount { get; set; }
        public decimal TotalDriverPaidAmount { get; set; }
        public decimal TotalTractorPaidAmount { get; set; }
    }

    public class ReportStatusItemVm
    {
        public string Label { get; set; } = "";
        public int Count { get; set; }
        public string BadgeClass { get; set; } = "bg-secondary";
    }

    public class ReportDailyTrendVm
    {
        public DateTime Date { get; set; }
        public string DateDisplay { get; set; } = "";
        public int HavalehCount { get; set; }
        public int SubHavalehCount { get; set; }
        public int AssignmentCount { get; set; }
        public int CargoRequestCount { get; set; }
        public int CargoAnnouncementCount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
    }

    public class ReportHavalehItemVm
    {
        public long Id { get; set; }
        public string? HavalehNumber { get; set; }
        public string? ProductName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? GoodsOwnerName { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? AllowedLoadingDate { get; set; }
        public decimal? ProductAmount { get; set; }
        public string? Unit { get; set; }
        public int SubHavalehCount { get; set; }
        public decimal SubHavalehAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
    }

    public class ReportSubHavalehItemVm
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? HavalehNumber { get; set; }
        public string? ProductName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? RequestedCargoAmount { get; set; }
        public string? Unit { get; set; }
        public int AssignmentCount { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
    }

    public class ReportAssignmentItemVm
    {
        public long Id { get; set; }
        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? DriverName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public AssignmentStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public DateTime AssignmentDate { get; set; }
        public DateTime? LoadingEndDate { get; set; }
        public DateTime? UnloadingEndDate { get; set; }
        public decimal? AssignedCargoAmount { get; set; }
        public decimal? LoadedAmount { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public string? Unit { get; set; }
    }

    public class ReportDriverItemVm
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
        public decimal? WalletBalance { get; set; }
        public int AssignmentCount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class ReportTractorItemVm
    {
        public int Id { get; set; }
        public string? PolicePlateNumber { get; set; }
        public string? TractorType { get; set; }
        public string? Status { get; set; }
        public decimal? MaxLoadCapacity { get; set; }
        public string? CapacityUnit { get; set; }
        public decimal? WalletBalance { get; set; }
        public int AssignmentCount { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class ReportCargoRequestItemVm
    {
        public long Id { get; set; }
        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? RequesterName { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? StatusDisplay { get; set; }
        public string? StatusBadgeClass { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RequestedLoadingDate { get; set; }
        public decimal RequestedCargoAmount { get; set; }
        public string? Unit { get; set; }
    }

    public class ReportCargoAnnouncementItemVm
    {
        public long Id { get; set; }
        public string? TrackingCode { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? ProductName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RequestedLoadingDate { get; set; }
        public decimal? CargoAmount { get; set; }
        public string? Unit { get; set; }
        public string? StatusDisplay { get; set; }
        public string? StatusBadgeClass { get; set; }
    }
}
