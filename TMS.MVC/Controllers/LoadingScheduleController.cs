using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class LoadingScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoadingScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? search, string? statusFilter)
        {
            var today = DateTime.Today;
            var from = (startDate ?? today.AddDays(-3)).Date;
            var to = (endDate ?? today.AddDays(7)).Date;

            if (to < from)
                (from, to) = (to, from);

            var relevantStatuses = GetRelevantStatuses();

            var query = _context.TractorAssignments
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
                .Where(x => relevantStatuses.Contains(x.Status))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<AssignmentStatus>(statusFilter, out var parsedStatus))
            {
                query = query.Where(x => x.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Tractor.PolicePlateNumber.Contains(search) ||
                    x.SubHavaleh.Havaleh.HavalehNumber.Contains(search) ||
                    x.SubHavaleh.Title.Contains(search) ||
                    x.SubHavaleh.Havaleh.Product.Name.Contains(search) ||
                    x.SubHavaleh.Havaleh.OriginPlace.Name.Contains(search) ||
                    x.SubHavaleh.DestinationPlace.Name.Contains(search) ||
                    (
                        x.DriverProfile != null &&
                        (
                            x.DriverProfile.ApplicationUser.FirstName.Contains(search) ||
                            x.DriverProfile.ApplicationUser.LastName.Contains(search) ||
                            (x.DriverProfile.ApplicationUser.FirstName + " " + x.DriverProfile.ApplicationUser.LastName).Contains(search) ||
                            x.DriverProfile.ApplicationUser.PhoneNumber.Contains(search)
                        )
                    )
                );
            }

            var assignments = await query
                .OrderBy(x => x.LoadingEndDate ?? x.LoadingStartDate ?? x.SubHavaleh.StartDate ?? x.AssignmentDate)
                .ToListAsync();

            var items = assignments
                .Select(x => BuildItem(x, from, to, today))
                .Where(x => x != null)
                .Cast<LoadingScheduleItemViewModel>()
                .Where(x => x.ScheduleDate.Date >= from && x.ScheduleDate.Date <= to)
                .OrderBy(x => x.ScheduleDate)
                .ThenBy(x => x.HavalehNumber)
                .ThenBy(x => x.SubHavalehTitle)
                .ThenBy(x => x.TractorPlateNumber)
                .ToList();

            var days = new List<LoadingScheduleDayViewModel>();
            for (var day = from; day <= to; day = day.AddDays(1))
            {
                var dayItems = items.Where(x => x.ScheduleDate.Date == day).ToList();

                var groups = dayItems
                    .GroupBy(x => new
                    {
                        x.HavalehId,
                        x.HavalehNumber,
                        x.SubHavalehId,
                        x.SubHavalehTitle,
                        x.ProductName,
                        x.Unit,
                        x.OriginPlaceName,
                        x.DestinationPlaceName
                    })
                    .Select(g => new LoadingScheduleGroupViewModel
                    {
                        Date = day,
                        HavalehId = g.Key.HavalehId,
                        HavalehNumber = g.Key.HavalehNumber,
                        SubHavalehId = g.Key.SubHavalehId,
                        SubHavalehTitle = g.Key.SubHavalehTitle,
                        ProductName = g.Key.ProductName,
                        Unit = g.Key.Unit,
                        OriginPlaceName = g.Key.OriginPlaceName,
                        DestinationPlaceName = g.Key.DestinationPlaceName,
                        TotalTrucks = g.Select(x => x.TractorId).Distinct().Count(),
                        LoadedTrucks = g.Count(x => x.IsActualLoaded),
                        PlannedTrucks = g.Count(x => x.IsPlanned),
                        OverdueTrucks = g.Count(x => x.IsOverdue),
                        AssignedAmount = g.Sum(x => x.AssignedCargoAmount ?? 0),
                        LoadedAmount = g.Sum(x => x.LoadedAmount ?? 0),
                        Items = g.OrderBy(x => x.TractorPlateNumber).ToList()
                    })
                    .OrderBy(x => x.HavalehNumber)
                    .ThenBy(x => x.SubHavalehTitle)
                    .ToList();

                days.Add(new LoadingScheduleDayViewModel
                {
                    Date = day,
                    IsToday = day == today,
                    TotalItems = dayItems.Count,
                    LoadedItems = dayItems.Count(x => x.IsActualLoaded),
                    PlannedItems = dayItems.Count(x => x.IsPlanned),
                    OverdueItems = dayItems.Count(x => x.IsOverdue),
                    TractorCount = dayItems.Select(x => x.TractorId).Distinct().Count(),
                    HavalehCount = dayItems.Select(x => x.HavalehId).Distinct().Count(),
                    AssignedAmount = dayItems.Sum(x => x.AssignedCargoAmount ?? 0),
                    LoadedAmount = dayItems.Sum(x => x.LoadedAmount ?? 0),
                    Groups = groups
                });
            }

            var model = new LoadingScheduleIndexViewModel
            {
                StartDate = from,
                EndDate = to,
                Search = search,
                StatusFilter = statusFilter,
                TotalItems = items.Count,
                TotalLoadedItems = items.Count(x => x.IsActualLoaded),
                TotalPlannedItems = items.Count(x => x.IsPlanned),
                TotalOverdueItems = items.Count(x => x.IsOverdue),
                TotalTractors = items.Select(x => x.TractorId).Distinct().Count(),
                TotalHavalehs = items.Select(x => x.HavalehId).Distinct().Count(),
                TotalAssignedAmount = items.Sum(x => x.AssignedCargoAmount ?? 0),
                TotalLoadedAmount = items.Sum(x => x.LoadedAmount ?? 0),
                Items = items,
                Days = days
            };

            return View(model);
        }

        private static LoadingScheduleItemViewModel? BuildItem(TractorAssignment assignment, DateTime from, DateTime to, DateTime today)
        {
            var actualLoadingDate = (assignment.LoadingEndDate ?? assignment.LoadingStartDate)?.Date;
            var plannedStart = assignment.SubHavaleh?.StartDate?.Date;
            var plannedEnd = assignment.SubHavaleh?.EndDate?.Date ?? plannedStart;

            var isActualLoaded = actualLoadingDate.HasValue;
            DateTime? scheduleDate = null;

            if (isActualLoaded)
            {
                scheduleDate = actualLoadingDate.Value;
            }
            else if (plannedStart.HasValue)
            {
                var pStart = plannedStart.Value;
                var pEnd = plannedEnd ?? pStart;

                if (today >= pStart && today <= pEnd)
                    scheduleDate = today;
                else if (pStart >= from && pStart <= to)
                    scheduleDate = pStart;
                else if (pEnd >= from && pEnd <= to)
                    scheduleDate = pEnd;
                else if (pStart <= to && pEnd >= from)
                    scheduleDate = pStart < from ? from : pStart;
            }
            else
            {
                scheduleDate = assignment.AssignmentDate.Date;
            }

            if (!scheduleDate.HasValue)
                return null;

            var isPlanned = !isActualLoaded;
            var isOverdue = isPlanned && scheduleDate.Value.Date < today;

            return new LoadingScheduleItemViewModel
            {
                AssignmentId = assignment.Id,
                ScheduleDate = scheduleDate.Value.Date,
                IsToday = scheduleDate.Value.Date == today,
                IsActualLoaded = isActualLoaded,
                IsPlanned = isPlanned,
                IsOverdue = isOverdue,

                HavalehId = assignment.SubHavaleh?.HavalehId ?? 0,
                HavalehNumber = assignment.SubHavaleh?.Havaleh?.HavalehNumber,
                SubHavalehId = assignment.SubHavalehId,
                SubHavalehTitle = assignment.SubHavaleh?.Title,
                ProductName = assignment.SubHavaleh?.Havaleh?.Product?.Name,
                Unit = assignment.SubHavaleh?.Havaleh?.Unit,

                OriginPlaceName = assignment.SubHavaleh?.Havaleh?.OriginPlace?.Name,
                DestinationPlaceName = assignment.SubHavaleh?.DestinationPlace?.Name,

                TractorId = assignment.TractorId,
                TractorPlateNumber = assignment.Tractor?.PolicePlateNumber,
                TractorType = assignment.Tractor?.TractorType,
                TractorCapacity = assignment.Tractor?.MaxLoadCapacity,
                TractorCapacityUnit = assignment.Tractor?.CapacityUnit,

                DriverProfileId = assignment.DriverProfileId,
                DriverName = assignment.DriverProfile?.ApplicationUser == null
                    ? "-"
                    : GetUserDisplayName(
                        assignment.DriverProfile.ApplicationUser.FirstName,
                        assignment.DriverProfile.ApplicationUser.LastName,
                        assignment.DriverProfile.ApplicationUser.Email),
                DriverPhoneNumber = assignment.DriverProfile?.ApplicationUser?.PhoneNumber,

                Status = assignment.Status,
                StatusDisplay = GetStatusDisplay(assignment.Status),
                StatusBadgeClass = GetStatusBadgeClass(assignment.Status),
                ScheduleTypeDisplay = isActualLoaded ? "بارگیری شده" : isOverdue ? "عقب‌افتاده" : "برنامه بارگیری",
                ScheduleTypeBadgeClass = isActualLoaded ? "bg-success" : isOverdue ? "bg-danger" : "bg-primary",

                AssignmentDate = assignment.AssignmentDate,
                LoadingStartDate = assignment.LoadingStartDate,
                LoadingEndDate = assignment.LoadingEndDate,
                ArrivalAtOriginDate = assignment.ArrivalAtOriginDate,
                SubHavalehStartDate = assignment.SubHavaleh?.StartDate,
                SubHavalehEndDate = assignment.SubHavaleh?.EndDate,

                AssignedCargoAmount = assignment.AssignedCargoAmount,
                LoadedAmount = assignment.LoadedAmount,
                UnloadedAmount = assignment.UnloadedAmount,
                IsTruckCapacityFull = assignment.IsTruckCapacityFull
            };
        }

        private static List<AssignmentStatus> GetRelevantStatuses()
        {
            return new List<AssignmentStatus>
            {
                AssignmentStatus.Assigned,
                AssignmentStatus.ArrivedAtOrigin,
                AssignmentStatus.Loading,
                AssignmentStatus.Loaded,
                AssignmentStatus.InTransit,
                AssignmentStatus.ArrivedAtDestination,
                AssignmentStatus.Unloading,
                AssignmentStatus.Unloaded,
                AssignmentStatus.Completed,
                AssignmentStatus.CancellationRequested
            };
        }

        private static string GetUserDisplayName(string? firstName, string? lastName, string? email)
        {
            var name = $"{firstName ?? ""} {lastName ?? ""}".Trim();
            return string.IsNullOrWhiteSpace(name) ? email ?? "-" : name;
        }

        private static string GetStatusDisplay(AssignmentStatus status)
        {
            return status switch
            {
                AssignmentStatus.Assigned => "تخصیص داده شده",
                AssignmentStatus.ArrivedAtOrigin => "رسیده به مبدا",
                AssignmentStatus.Loading => "در حال بارگیری",
                AssignmentStatus.Loaded => "بارگیری شده",
                AssignmentStatus.InTransit => "در مسیر",
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
                AssignmentStatus.Unloaded => "bg-success",
                AssignmentStatus.Completed => "bg-success",
                AssignmentStatus.CancellationRequested => "bg-danger",
                AssignmentStatus.Cancelled => "bg-danger",
                _ => "bg-secondary"
            };
        }
    }
}
