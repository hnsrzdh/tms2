using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class LoadingScheduleIndexViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }

        public int TotalItems { get; set; }
        public int TotalLoadedItems { get; set; }
        public int TotalPlannedItems { get; set; }
        public int TotalOverdueItems { get; set; }
        public int TotalTractors { get; set; }
        public int TotalHavalehs { get; set; }
        public decimal TotalAssignedAmount { get; set; }
        public decimal TotalLoadedAmount { get; set; }

        public List<LoadingScheduleDayViewModel> Days { get; set; } = new();
        public List<LoadingScheduleItemViewModel> Items { get; set; } = new();
    }

    public class LoadingScheduleDayViewModel
    {
        public DateTime Date { get; set; }
        public bool IsToday { get; set; }
        public int TotalItems { get; set; }
        public int LoadedItems { get; set; }
        public int PlannedItems { get; set; }
        public int OverdueItems { get; set; }
        public int TractorCount { get; set; }
        public int HavalehCount { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public List<LoadingScheduleGroupViewModel> Groups { get; set; } = new();
    }

    public class LoadingScheduleGroupViewModel
    {
        public DateTime Date { get; set; }
        public long HavalehId { get; set; }
        public string? HavalehNumber { get; set; }
        public long SubHavalehId { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }
        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public int TotalTrucks { get; set; }
        public int LoadedTrucks { get; set; }
        public int PlannedTrucks { get; set; }
        public int OverdueTrucks { get; set; }
        public decimal AssignedAmount { get; set; }
        public decimal LoadedAmount { get; set; }
        public List<LoadingScheduleItemViewModel> Items { get; set; } = new();
    }

    public class LoadingScheduleItemViewModel
    {
        public long AssignmentId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public bool IsToday { get; set; }
        public bool IsActualLoaded { get; set; }
        public bool IsPlanned { get; set; }
        public bool IsOverdue { get; set; }

        public long HavalehId { get; set; }
        public string? HavalehNumber { get; set; }
        public long SubHavalehId { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }

        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }

        public int TractorId { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? TractorType { get; set; }
        public decimal? TractorCapacity { get; set; }
        public string? TractorCapacityUnit { get; set; }

        public int? DriverProfileId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }

        public AssignmentStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public string ScheduleTypeDisplay { get; set; } = "";
        public string ScheduleTypeBadgeClass { get; set; } = "bg-secondary";

        public DateTime AssignmentDate { get; set; }
        public DateTime? LoadingStartDate { get; set; }
        public DateTime? LoadingEndDate { get; set; }
        public DateTime? ArrivalAtOriginDate { get; set; }
        public DateTime? SubHavalehStartDate { get; set; }
        public DateTime? SubHavalehEndDate { get; set; }

        public decimal? AssignedCargoAmount { get; set; }
        public decimal? LoadedAmount { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public bool IsTruckCapacityFull { get; set; }
    }
}
