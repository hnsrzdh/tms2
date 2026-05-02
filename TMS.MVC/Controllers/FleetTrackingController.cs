using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class FleetTrackingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static readonly AssignmentStatus[] ActiveStatuses =
        {
            AssignmentStatus.Assigned,
            AssignmentStatus.ArrivedAtOrigin,
            AssignmentStatus.Loading,
            AssignmentStatus.Loaded,
            AssignmentStatus.InTransit,
            AssignmentStatus.ArrivedAtDestination,
            AssignmentStatus.Unloading,
            AssignmentStatus.CancellationRequested
        };

        public FleetTrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? statusFilter, string? locationFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            search = search?.Trim();
            statusFilter = statusFilter?.Trim();
            locationFilter = locationFilter?.Trim();

            var activeAssignments = await _context.TractorAssignments
                .AsNoTracking()
                .Include(x => x.Tractor)
                .Include(x => x.DriverProfile)
                    .ThenInclude(x => x.ApplicationUser)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.OriginPlace)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.Product)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.DestinationPlace)
                .Where(x => ActiveStatuses.Contains(x.Status))
                .OrderByDescending(x => x.AssignmentDate)
                .ToListAsync();

            var activeAssignmentIds = activeAssignments.Select(x => x.Id).ToList();

            var latestLocations = await _context.LocationTrackings
                .AsNoTracking()
                .Where(x => activeAssignmentIds.Contains(x.TractorAssignmentId))
                .GroupBy(x => x.TractorAssignmentId)
                .Select(g => g.OrderByDescending(x => x.Timestamp).FirstOrDefault())
                .ToListAsync();

            var latestLocationByAssignmentId = latestLocations
                .Where(x => x != null)
                .ToDictionary(x => x!.TractorAssignmentId, x => x!);

            var now = DateTime.Now;
            var staleAfterMinutes = 30;

            var allItems = activeAssignments.Select(a =>
            {
                latestLocationByAssignmentId.TryGetValue(a.Id, out var location);
                var lastAgeMinutes = location == null ? (int?)null : Math.Max(0, (int)(now - location.Timestamp).TotalMinutes);

                return new FleetTrackingItemViewModel
                {
                    AssignmentId = a.Id,
                    TractorId = a.TractorId,
                    TractorPlateNumber = a.Tractor?.PolicePlateNumber,
                    TractorType = a.Tractor?.TractorType,
                    TractorCapacity = a.Tractor?.MaxLoadCapacity,
                    CapacityUnit = a.Tractor?.CapacityUnit,

                    DriverProfileId = a.DriverProfileId,
                    DriverName = GetDriverName(a.DriverProfile),
                    DriverPhoneNumber = a.DriverProfile?.ApplicationUser?.PhoneNumber,

                    HavalehId = a.SubHavaleh?.HavalehId ?? 0,
                    HavalehNumber = a.SubHavaleh?.Havaleh?.HavalehNumber,
                    SubHavalehId = a.SubHavalehId,
                    SubHavalehTitle = a.SubHavaleh?.Title,
                    ProductName = a.SubHavaleh?.Havaleh?.Product?.Name,
                    Unit = a.SubHavaleh?.Havaleh?.Unit,

                    OriginPlaceName = a.SubHavaleh?.Havaleh?.OriginPlace?.Name,
                    DestinationPlaceName = a.SubHavaleh?.DestinationPlace?.Name,
                    RouteSummary = BuildRouteSummary(a.SubHavaleh),

                    AssignmentDate = a.AssignmentDate,
                    ArrivalAtOriginDate = a.ArrivalAtOriginDate,
                    LoadingStartDate = a.LoadingStartDate,
                    LoadingEndDate = a.LoadingEndDate,
                    ArrivalAtDestinationDate = a.ArrivalAtDestinationDate,
                    UnloadingStartDate = a.UnloadingStartDate,
                    UnloadingEndDate = a.UnloadingEndDate,

                    Status = a.Status,
                    StatusDisplay = GetStatusDisplay(a.Status),
                    StatusBadgeClass = GetStatusBadgeClass(a.Status),

                    AssignedCargoAmount = a.AssignedCargoAmount,
                    LoadedAmount = a.LoadedAmount,
                    UnloadedAmount = a.UnloadedAmount,
                    IsTruckCapacityFull = a.IsTruckCapacityFull,

                    Latitude = location?.Latitude,
                    Longitude = location?.Longitude,
                    LastLocationTime = location?.Timestamp,
                    Speed = location?.Speed,
                    Heading = location?.Heading,
                    LastLocationNote = location?.Notes,
                    IsLocationStale = lastAgeMinutes.HasValue && lastAgeMinutes.Value > staleAfterMinutes,
                    LastLocationAgeMinutes = lastAgeMinutes
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                allItems = allItems.Where(x =>
                    Contains(x.TractorPlateNumber, search) ||
                    Contains(x.DriverName, search) ||
                    Contains(x.DriverPhoneNumber, search) ||
                    Contains(x.HavalehNumber, search) ||
                    Contains(x.SubHavalehTitle, search) ||
                    Contains(x.ProductName, search) ||
                    Contains(x.OriginPlaceName, search) ||
                    Contains(x.DestinationPlaceName, search)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse<AssignmentStatus>(statusFilter, out var selectedStatus))
            {
                allItems = allItems.Where(x => x.Status == selectedStatus).ToList();
            }

            if (!string.IsNullOrWhiteSpace(locationFilter))
            {
                allItems = locationFilter switch
                {
                    "with-location" => allItems.Where(x => x.HasLocation).ToList(),
                    "without-location" => allItems.Where(x => !x.HasLocation).ToList(),
                    "stale-location" => allItems.Where(x => x.IsLocationStale).ToList(),
                    _ => allItems
                };
            }

            var totalItems = allItems.Count;
            var items = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mapItems = allItems
                .Where(x => x.HasLocation)
                .Select(x => new FleetTrackingMapItemViewModel
                {
                    AssignmentId = x.AssignmentId,
                    TractorId = x.TractorId,
                    TractorPlateNumber = x.TractorPlateNumber ?? "-",
                    DriverName = x.DriverName ?? "-",
                    HavalehNumber = x.HavalehNumber ?? "-",
                    StatusDisplay = x.StatusDisplay,
                    OriginPlaceName = x.OriginPlaceName ?? "-",
                    DestinationPlaceName = x.DestinationPlaceName ?? "-",
                    Latitude = x.Latitude!.Value,
                    Longitude = x.Longitude!.Value,
                    LastLocationTime = x.LastLocationTime,
                    Speed = x.Speed,
                    Heading = x.Heading,
                    IsLocationStale = x.IsLocationStale
                })
                .ToList();

            var model = new FleetTrackingIndexViewModel
            {
                Items = items,
                MapItems = mapItems,
                Search = search,
                StatusFilter = statusFilter,
                LocationFilter = locationFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,

                ActiveAssignmentsCount = allItems.Count,
                ActiveTractorsCount = allItems.Select(x => x.TractorId).Distinct().Count(),
                ActiveDriversCount = allItems.Where(x => x.DriverProfileId.HasValue).Select(x => x.DriverProfileId!.Value).Distinct().Count(),
                WithLocationCount = allItems.Count(x => x.HasLocation),
                WithoutLocationCount = allItems.Count(x => !x.HasLocation),
                StaleLocationCount = allItems.Count(x => x.IsLocationStale),
                AssignedCount = allItems.Count(x => x.Status == AssignmentStatus.Assigned),
                ArrivedAtOriginCount = allItems.Count(x => x.Status == AssignmentStatus.ArrivedAtOrigin),
                LoadingCount = allItems.Count(x => x.Status == AssignmentStatus.Loading),
                LoadedCount = allItems.Count(x => x.Status == AssignmentStatus.Loaded),
                InTransitCount = allItems.Count(x => x.Status == AssignmentStatus.InTransit),
                ArrivedAtDestinationCount = allItems.Count(x => x.Status == AssignmentStatus.ArrivedAtDestination),
                UnloadingCount = allItems.Count(x => x.Status == AssignmentStatus.Unloading),
                CancellationRequestedCount = allItems.Count(x => x.Status == AssignmentStatus.CancellationRequested),
                TotalAssignedCargoAmount = allItems.Sum(x => x.AssignedCargoAmount ?? 0),
                TotalLoadedAmount = allItems.Sum(x => x.LoadedAmount ?? 0),
                TotalUnloadedAmount = allItems.Sum(x => x.UnloadedAmount ?? 0)
            };

            return View(model);
        }

        private static string? GetDriverName(DriverProfile? driver)
        {
            if (driver?.ApplicationUser == null) return null;

            var name = $"{driver.ApplicationUser.FirstName} {driver.ApplicationUser.LastName}".Trim();
            return !string.IsNullOrWhiteSpace(name)
                ? name
                : driver.ApplicationUser.Email;
        }

        private static bool Contains(string? source, string term)
        {
            return !string.IsNullOrWhiteSpace(source) &&
                   source.Contains(term, StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildRouteSummary(SubHavaleh? subHavaleh)
        {
            if (subHavaleh == null) return "-";

            var origin = subHavaleh.Havaleh?.OriginPlace?.Name;
            var destination = subHavaleh.DestinationPlace?.Name;

            if (string.IsNullOrWhiteSpace(origin) && string.IsNullOrWhiteSpace(destination))
                return "-";

            if (string.IsNullOrWhiteSpace(origin)) return destination ?? "-";
            if (string.IsNullOrWhiteSpace(destination)) return origin;

            return $"{origin} ← {destination}";
        }

        private static string GetStatusDisplay(AssignmentStatus status)
        {
            return status switch
            {
                AssignmentStatus.Assigned => "تخصیص داده شده",
                AssignmentStatus.ArrivedAtOrigin => "رسیده به مبدا",
                AssignmentStatus.Loading => "در حال بارگیری",
                AssignmentStatus.Loaded => "بارگیری شده",
                AssignmentStatus.InTransit => "در راه",
                AssignmentStatus.ArrivedAtDestination => "رسیده به مقصد",
                AssignmentStatus.Unloading => "در حال تخلیه",
                AssignmentStatus.Unloaded => "تخلیه شده",
                AssignmentStatus.Completed => "تکمیل شده",
                AssignmentStatus.CancellationRequested => "درخواست لغو",
                AssignmentStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetStatusBadgeClass(AssignmentStatus status)
        {
            return status switch
            {
                AssignmentStatus.Assigned => "bg-secondary",
                AssignmentStatus.ArrivedAtOrigin => "bg-info",
                AssignmentStatus.Loading => "bg-warning",
                AssignmentStatus.Loaded => "bg-primary",
                AssignmentStatus.InTransit => "bg-primary",
                AssignmentStatus.ArrivedAtDestination => "bg-info",
                AssignmentStatus.Unloading => "bg-warning",
                AssignmentStatus.CancellationRequested => "bg-warning text-dark",
                _ => "bg-secondary"
            };
        }
    }
}
