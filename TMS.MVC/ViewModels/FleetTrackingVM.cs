using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class FleetTrackingIndexViewModel
    {
        public List<FleetTrackingItemViewModel> Items { get; set; } = new();
        public List<FleetTrackingMapItemViewModel> MapItems { get; set; } = new();

        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public string? LocationFilter { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => ((Page - 1) * PageSize) + 1;

        public int ActiveAssignmentsCount { get; set; }
        public int ActiveTractorsCount { get; set; }
        public int ActiveDriversCount { get; set; }
        public int WithLocationCount { get; set; }
        public int WithoutLocationCount { get; set; }
        public int StaleLocationCount { get; set; }
        public int AssignedCount { get; set; }
        public int ArrivedAtOriginCount { get; set; }
        public int LoadingCount { get; set; }
        public int LoadedCount { get; set; }
        public int InTransitCount { get; set; }
        public int ArrivedAtDestinationCount { get; set; }
        public int UnloadingCount { get; set; }
        public int CancellationRequestedCount { get; set; }

        public decimal TotalAssignedCargoAmount { get; set; }
        public decimal TotalLoadedAmount { get; set; }
        public decimal TotalUnloadedAmount { get; set; }
    }

    public class FleetTrackingItemViewModel
    {
        public long AssignmentId { get; set; }
        public int TractorId { get; set; }
        public string? TractorPlateNumber { get; set; }
        public string? TractorType { get; set; }
        public decimal? TractorCapacity { get; set; }
        public string? CapacityUnit { get; set; }

        public int? DriverProfileId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }

        public long HavalehId { get; set; }
        public string? HavalehNumber { get; set; }
        public long SubHavalehId { get; set; }
        public string? SubHavalehTitle { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }

        public string? OriginPlaceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? RouteSummary { get; set; }

        public DateTime AssignmentDate { get; set; }
        public DateTime? ArrivalAtOriginDate { get; set; }
        public DateTime? LoadingStartDate { get; set; }
        public DateTime? LoadingEndDate { get; set; }
        public DateTime? ArrivalAtDestinationDate { get; set; }
        public DateTime? UnloadingStartDate { get; set; }
        public DateTime? UnloadingEndDate { get; set; }

        public AssignmentStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "-";
        public string StatusBadgeClass { get; set; } = "bg-secondary";

        public decimal? AssignedCargoAmount { get; set; }
        public decimal? LoadedAmount { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public bool IsTruckCapacityFull { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? LastLocationTime { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public string? LastLocationNote { get; set; }
        public bool HasLocation => Latitude.HasValue && Longitude.HasValue;
        public bool IsLocationStale { get; set; }
        public int? LastLocationAgeMinutes { get; set; }
    }

    public class FleetTrackingMapItemViewModel
    {
        public long AssignmentId { get; set; }
        public int TractorId { get; set; }
        public string TractorPlateNumber { get; set; } = "-";
        public string DriverName { get; set; } = "-";
        public string HavalehNumber { get; set; } = "-";
        public string StatusDisplay { get; set; } = "-";
        public string OriginPlaceName { get; set; } = "-";
        public string DestinationPlaceName { get; set; } = "-";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime? LastLocationTime { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public bool IsLocationStale { get; set; }
    }
}
