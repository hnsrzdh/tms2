using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class SystemDashboardViewModel
    {
        public DateTime Today { get; set; } = DateTime.Today;
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }

        public DashboardSummaryVm Summary { get; set; } = new();

        public List<DashboardStatusItemVm> AssignmentStatusItems { get; set; } = new();
        public List<DashboardStatusItemVm> CargoRequestStatusItems { get; set; } = new();
        public List<DashboardStatusItemVm> CargoAnnouncementStatusItems { get; set; } = new();
        public List<DashboardStatusItemVm> TicketStatusItems { get; set; } = new();

        public List<DashboardDailyTrendVm> DailyTrends { get; set; } = new();

        public List<DashboardFleetItemVm> ActiveFleetItems { get; set; } = new();
        public List<DashboardAssignmentItemVm> LatestAssignments { get; set; } = new();
        public List<DashboardCargoRequestItemVm> LatestCargoRequests { get; set; } = new();
        public List<DashboardCargoAnnouncementItemVm> LatestCargoAnnouncements { get; set; } = new();
        public List<DashboardTicketItemVm> LatestTickets { get; set; } = new();
        public List<DashboardDocumentAlertVm> DocumentAlerts { get; set; } = new();
    }

    public class DashboardSummaryVm
    {
        public int HavalehCount { get; set; }
        public int SubHavalehCount { get; set; }

        public int ActiveAssignmentCount { get; set; }
        public int TodayAssignmentCount { get; set; }
        public int TodayLoadingCount { get; set; }
        public int TodayUnloadingCount { get; set; }

        public int TractorCount { get; set; }
        public int ActiveTractorCount { get; set; }
        public int TractorWithActiveTripCount { get; set; }

        public int DriverCount { get; set; }
        public int BlockedDriverCount { get; set; }

        public int PendingCargoRequestCount { get; set; }
        public int PendingCargoAnnouncementCount { get; set; }
        public int OpenTicketCount { get; set; }
        public int UrgentTicketCount { get; set; }

        public decimal TotalHavalehAmount { get; set; }
        public decimal TotalSubHavalehAmount { get; set; }
        public decimal TotalAssignedAmount { get; set; }
        public decimal TotalLoadedAmount { get; set; }
        public decimal TotalUnloadedAmount { get; set; }

        public decimal DriverWalletBalanceTotal { get; set; }
        public decimal TractorWalletBalanceTotal { get; set; }
        public decimal PendingDriverWithdrawalAmount { get; set; }
    }

    public class DashboardStatusItemVm
    {
        public string Label { get; set; } = "";
        public int Count { get; set; }
        public string BadgeClass { get; set; } = "bg-secondary";
    }

    public class DashboardDailyTrendVm
    {
        public DateTime Date { get; set; }
        public string DateDisplay { get; set; } = "";
        public int Assignments { get; set; }
        public int Loading { get; set; }
        public int Unloading { get; set; }
        public int CargoRequests { get; set; }
        public int CargoAnnouncements { get; set; }
        public int Tickets { get; set; }
        public decimal LoadedAmount { get; set; }
        public decimal UnloadedAmount { get; set; }
    }

    public class DashboardFleetItemVm
    {
        public long AssignmentId { get; set; }
        public int TractorId { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? TractorType { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public AssignmentStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public DateTime AssignmentDate { get; set; }
        public decimal? AssignedCargoAmount { get; set; }
        public decimal? LoadedAmount { get; set; }
        public string? Unit { get; set; }
        public bool HasLocation { get; set; }
        public DateTime? LatestLocationAt { get; set; }
    }

    public class DashboardAssignmentItemVm
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
        public decimal? AssignedCargoAmount { get; set; }
        public decimal? LoadedAmount { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public string? Unit { get; set; }
    }

    public class DashboardCargoRequestItemVm
    {
        public long Id { get; set; }
        public string? HavalehNumber { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public DateTime CreatedAt { get; set; }
        public DateTime RequestedLoadingDate { get; set; }
        public decimal RequestedCargoAmount { get; set; }
        public string? Unit { get; set; }
    }

    public class DashboardCargoAnnouncementItemVm
    {
        public long Id { get; set; }
        public string TrackingCode { get; set; } = "";
        public string? CustomerCompanyName { get; set; }
        public string? ContactMobile { get; set; }
        public string? ProductName { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LoadingStartDate { get; set; }
        public decimal ProductAmount { get; set; }
        public string? Unit { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
    }

    public class DashboardTicketItemVm
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public string PriorityDisplay { get; set; } = "";
        public string PriorityBadgeClass { get; set; } = "bg-secondary";
        public string? CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DashboardDocumentAlertVm
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Subtitle { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? DaysLeft { get; set; }
        public string BadgeClass { get; set; } = "bg-warning";
        public string? DetailsUrl { get; set; }
    }
}
