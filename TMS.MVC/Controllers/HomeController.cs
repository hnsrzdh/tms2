using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var trendStart = today.AddDays(-6);
            var warningExpireDate = today.AddDays(30);

            var activeStatuses = GetActiveAssignmentStatuses();

            var havalehBase = await _context.Havalehs
                .Include(x => x.SubHavalehs)
                    .ThenInclude(x => x.TractorAssignments)
                .AsNoTracking()
                .ToListAsync();

            var subHavalehBase = await _context.SubHavalehs
                .Include(x => x.TractorAssignments)
                .AsNoTracking()
                .ToListAsync();

            var assignments = await _context.TractorAssignments
                .Include(x => x.Tractor)
                .Include(x => x.DriverProfile)
                    .ThenInclude(x => x.ApplicationUser)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.OriginPlace)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.DestinationPlace)
                .AsNoTracking()
                .ToListAsync();

            var activeAssignments = assignments
                .Where(x => activeStatuses.Contains(x.Status))
                .OrderByDescending(x => x.AssignmentDate)
                .ToList();

            var assignmentIds = activeAssignments.Select(x => x.Id).ToList();

            var latestLocations = await _context.LocationTrackings
                .Where(x => assignmentIds.Contains(x.TractorAssignmentId))
                .AsNoTracking()
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();

            var latestLocationByAssignment = latestLocations
                .GroupBy(x => x.TractorAssignmentId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(l => l.Timestamp).First());

            var tractors = await _context.Tractors
                .AsNoTracking()
                .ToListAsync();

            var drivers = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .AsNoTracking()
                .ToListAsync();

            var cargoRequests = await _context.SubHavalehAssignmentRequests
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                .Include(x => x.Tractor)
                .AsNoTracking()
                .ToListAsync();

            var cargoAnnouncements = await _context.CargoAnnouncements
                .AsNoTracking()
                .ToListAsync();

            var tickets = await _context.Tickets
                .Include(x => x.CreatedByUser)
                .AsNoTracking()
                .ToListAsync();

            var pendingDriverWithdrawals = await _context.DriverWalletWithdrawalRequests
                .Where(x => x.Status == DriverWalletWithdrawalRequestStatus.Pending)
                .AsNoTracking()
                .ToListAsync();

            var dailyTrends = BuildDailyTrends(
                trendStart,
                today,
                assignments,
                cargoRequests,
                cargoAnnouncements,
                tickets);

            var documentAlerts = BuildDocumentAlerts(tractors, today, warningExpireDate);

            var model = new SystemDashboardViewModel
            {
                Today = today,
                WeekStart = GetSaturdayOfWeek(today),
                WeekEnd = GetSaturdayOfWeek(today).AddDays(6),

                Summary = new DashboardSummaryVm
                {
                    HavalehCount = havalehBase.Count,
                    SubHavalehCount = subHavalehBase.Count,

                    ActiveAssignmentCount = activeAssignments.Count,
                    TodayAssignmentCount = assignments.Count(x => x.AssignmentDate >= today && x.AssignmentDate < tomorrow),
                    TodayLoadingCount = assignments.Count(x => x.LoadingEndDate.HasValue && x.LoadingEndDate.Value >= today && x.LoadingEndDate.Value < tomorrow),
                    TodayUnloadingCount = assignments.Count(x => x.UnloadingEndDate.HasValue && x.UnloadingEndDate.Value >= today && x.UnloadingEndDate.Value < tomorrow),

                    TractorCount = tractors.Count,
                    ActiveTractorCount = tractors.Count(x => string.Equals(x.Status, "فعال", StringComparison.OrdinalIgnoreCase)),
                    TractorWithActiveTripCount = activeAssignments.Select(x => x.TractorId).Distinct().Count(),

                    DriverCount = drivers.Count,
                    BlockedDriverCount = drivers.Count(x => x.IsBlocked),

                    PendingCargoRequestCount = cargoRequests.Count(x => x.Status.ToString() == "Pending"),
                    PendingCargoAnnouncementCount = cargoAnnouncements.Count(x => x.Status == CargoAnnouncementStatus.Pending),
                    OpenTicketCount = tickets.Count(x => x.Status != TicketStatus.Closed && x.Status != TicketStatus.Cancelled),
                    UrgentTicketCount = tickets.Count(x => x.Priority == TicketPriority.Urgent && x.Status != TicketStatus.Closed && x.Status != TicketStatus.Cancelled),

                    TotalHavalehAmount = havalehBase.Sum(x => x.ProductAmount ?? 0),
                    TotalSubHavalehAmount = subHavalehBase.Sum(x => x.RequestedCargoAmount ?? 0),
                    TotalAssignedAmount = assignments.Where(x => x.Status != AssignmentStatus.Cancelled).Sum(x => x.AssignedCargoAmount ?? 0),
                    TotalLoadedAmount = assignments.Where(x => x.Status != AssignmentStatus.Cancelled).Sum(x => x.LoadedAmount ?? 0),
                    TotalUnloadedAmount = assignments.Where(x => x.Status != AssignmentStatus.Cancelled).Sum(x => x.UnloadedAmount ?? 0),

                    DriverWalletBalanceTotal = drivers.Sum(x => x.WalletBalance ?? 0),
                    TractorWalletBalanceTotal = tractors.Sum(x => x.WalletBalance ?? 0),
                    PendingDriverWithdrawalAmount = pendingDriverWithdrawals.Sum(x => x.Amount)
                },

                AssignmentStatusItems = assignments
                    .GroupBy(x => x.Status)
                    .Select(x => new DashboardStatusItemVm
                    {
                        Label = GetAssignmentStatusDisplay(x.Key),
                        Count = x.Count(),
                        BadgeClass = GetAssignmentStatusBadgeClass(x.Key)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList(),

                CargoRequestStatusItems = cargoRequests
                    .GroupBy(x => x.Status.ToString())
                    .Select(x => new DashboardStatusItemVm
                    {
                        Label = GetCargoRequestStatusDisplay(x.Key),
                        Count = x.Count(),
                        BadgeClass = GetRequestBadgeClass(x.Key)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList(),

                CargoAnnouncementStatusItems = cargoAnnouncements
                    .GroupBy(x => x.Status)
                    .Select(x => new DashboardStatusItemVm
                    {
                        Label = GetCargoAnnouncementStatusDisplay(x.Key),
                        Count = x.Count(),
                        BadgeClass = GetCargoAnnouncementBadgeClass(x.Key)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList(),

                TicketStatusItems = tickets
                    .GroupBy(x => x.Status)
                    .Select(x => new DashboardStatusItemVm
                    {
                        Label = GetTicketStatusDisplay(x.Key),
                        Count = x.Count(),
                        BadgeClass = GetTicketStatusBadgeClass(x.Key)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList(),

                DailyTrends = dailyTrends,

                ActiveFleetItems = activeAssignments
                    .Take(8)
                    .Select(x =>
                    {
                        latestLocationByAssignment.TryGetValue(x.Id, out var loc);

                        return new DashboardFleetItemVm
                        {
                            AssignmentId = x.Id,
                            TractorId = x.TractorId,
                            TractorPlateNumber = x.Tractor?.PolicePlateNumber,
                            TractorType = x.Tractor?.TractorType,
                            DriverName = x.DriverProfile?.ApplicationUser == null
                                ? "-"
                                : GetUserDisplayName(
                                    x.DriverProfile.ApplicationUser.FirstName,
                                    x.DriverProfile.ApplicationUser.LastName,
                                    x.DriverProfile.ApplicationUser.Email),
                            DriverPhoneNumber = x.DriverProfile?.ApplicationUser?.PhoneNumber,
                            HavalehNumber = x.SubHavaleh?.Havaleh?.HavalehNumber,
                            SubHavalehTitle = x.SubHavaleh?.Title,
                            OriginPlaceName = x.SubHavaleh?.Havaleh?.OriginPlace?.Name,
                            DestinationPlaceName = x.SubHavaleh?.DestinationPlace?.Name,
                            Status = x.Status,
                            StatusDisplay = GetAssignmentStatusDisplay(x.Status),
                            StatusBadgeClass = GetAssignmentStatusBadgeClass(x.Status),
                            AssignmentDate = x.AssignmentDate,
                            AssignedCargoAmount = x.AssignedCargoAmount,
                            LoadedAmount = x.LoadedAmount,
                            Unit = x.SubHavaleh?.Havaleh?.Unit,
                            HasLocation = loc != null,
                            LatestLocationAt = loc?.Timestamp
                        };
                    })
                    .ToList(),

                LatestAssignments = assignments
                    .OrderByDescending(x => x.AssignmentDate)
                    .Take(10)
                    .Select(x => new DashboardAssignmentItemVm
                    {
                        Id = x.Id,
                        HavalehNumber = x.SubHavaleh?.Havaleh?.HavalehNumber,
                        SubHavalehTitle = x.SubHavaleh?.Title,
                        TractorPlateNumber = x.Tractor?.PolicePlateNumber,
                        DriverName = x.DriverProfile?.ApplicationUser == null
                            ? "-"
                            : GetUserDisplayName(
                                x.DriverProfile.ApplicationUser.FirstName,
                                x.DriverProfile.ApplicationUser.LastName,
                                x.DriverProfile.ApplicationUser.Email),
                        OriginPlaceName = x.SubHavaleh?.Havaleh?.OriginPlace?.Name,
                        DestinationPlaceName = x.SubHavaleh?.DestinationPlace?.Name,
                        Status = x.Status,
                        StatusDisplay = GetAssignmentStatusDisplay(x.Status),
                        StatusBadgeClass = GetAssignmentStatusBadgeClass(x.Status),
                        AssignmentDate = x.AssignmentDate,
                        AssignedCargoAmount = x.AssignedCargoAmount,
                        LoadedAmount = x.LoadedAmount,
                        UnloadedAmount = x.UnloadedAmount,
                        Unit = x.SubHavaleh?.Havaleh?.Unit
                    })
                    .ToList(),

                LatestCargoRequests = cargoRequests
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(8)
                    .Select(x => new DashboardCargoRequestItemVm
                    {
                        Id = x.Id,
                        HavalehNumber = x.SubHavaleh?.Havaleh?.HavalehNumber,
                        SubHavalehTitle = x.SubHavaleh?.Title,
                        TractorPlateNumber = x.Tractor?.PolicePlateNumber,
                        StatusDisplay = GetCargoRequestStatusDisplay(x.Status.ToString()),
                        StatusBadgeClass = GetRequestBadgeClass(x.Status.ToString()),
                        CreatedAt = x.CreatedAt,
                        RequestedLoadingDate = x.RequestedLoadingDate,
                        RequestedCargoAmount = x.RequestedCargoAmount,
                        Unit = x.SubHavaleh?.Havaleh?.Unit
                    })
                    .ToList(),

                LatestCargoAnnouncements = cargoAnnouncements
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(8)
                    .Select(x => new DashboardCargoAnnouncementItemVm
                    {
                        Id = x.Id,
                        TrackingCode = $"CA-{x.Id}",
                        CustomerCompanyName = x.CustomerCompanyName,
                        ContactMobile = x.ContactMobile,
                        ProductName = x.ProductName,
                        OriginPlaceName = x.OriginPlaceName,
                        DestinationPlaceName = x.DestinationPlaceName,
                        CreatedAt = x.CreatedAt,
                        LoadingStartDate = x.LoadingStartDate,
                        ProductAmount = x.ProductAmount,
                        Unit = x.Unit,
                        StatusDisplay = GetCargoAnnouncementStatusDisplay(x.Status),
                        StatusBadgeClass = GetCargoAnnouncementBadgeClass(x.Status)
                    })
                    .ToList(),

                LatestTickets = tickets
                    .OrderByDescending(x => x.UpdatedAt)
                    .Take(8)
                    .Select(x => new DashboardTicketItemVm
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Category = x.Category,
                        StatusDisplay = GetTicketStatusDisplay(x.Status),
                        StatusBadgeClass = GetTicketStatusBadgeClass(x.Status),
                        PriorityDisplay = GetTicketPriorityDisplay(x.Priority),
                        PriorityBadgeClass = GetTicketPriorityBadgeClass(x.Priority),
                        CreatedByName = GetUserDisplayName(x.CreatedByUser?.FirstName, x.CreatedByUser?.LastName, x.CreatedByUser?.Email),
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .ToList(),

                DocumentAlerts = documentAlerts
            };

            return View(model);
        }

        private static List<DashboardDailyTrendVm> BuildDailyTrends(
            DateTime start,
            DateTime end,
            List<TractorAssignment> assignments,
            List<SubHavalehAssignmentRequest> cargoRequests,
            List<CargoAnnouncement> cargoAnnouncements,
            List<Ticket> tickets)
        {
            var result = new List<DashboardDailyTrendVm>();

            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                var next = date.AddDays(1);

                result.Add(new DashboardDailyTrendVm
                {
                    Date = date,
                    DateDisplay = ToJalali(date),
                    Assignments = assignments.Count(x => x.AssignmentDate >= date && x.AssignmentDate < next),
                    Loading = assignments.Count(x => x.LoadingEndDate.HasValue && x.LoadingEndDate.Value >= date && x.LoadingEndDate.Value < next),
                    Unloading = assignments.Count(x => x.UnloadingEndDate.HasValue && x.UnloadingEndDate.Value >= date && x.UnloadingEndDate.Value < next),
                    CargoRequests = cargoRequests.Count(x => x.CreatedAt >= date && x.CreatedAt < next),
                    CargoAnnouncements = cargoAnnouncements.Count(x => x.CreatedAt >= date && x.CreatedAt < next),
                    Tickets = tickets.Count(x => x.CreatedAt >= date && x.CreatedAt < next),
                    LoadedAmount = assignments
                        .Where(x => x.LoadingEndDate.HasValue && x.LoadingEndDate.Value >= date && x.LoadingEndDate.Value < next)
                        .Sum(x => x.LoadedAmount ?? 0),
                    UnloadedAmount = assignments
                        .Where(x => x.UnloadingEndDate.HasValue && x.UnloadingEndDate.Value >= date && x.UnloadingEndDate.Value < next)
                        .Sum(x => x.UnloadedAmount ?? 0)
                });
            }

            return result;
        }

        private static List<DashboardDocumentAlertVm> BuildDocumentAlerts(List<Tractor> tractors, DateTime today, DateTime warningDate)
        {
            var alerts = new List<DashboardDocumentAlertVm>();

            foreach (var tractor in tractors)
            {
                if (tractor.TechnicalInspectionExpireDate.HasValue &&
                    tractor.TechnicalInspectionExpireDate.Value.Date <= warningDate)
                {
                    alerts.Add(new DashboardDocumentAlertVm
                    {
                        Type = "معاینه فنی",
                        Title = tractor.PolicePlateNumber,
                        Subtitle = tractor.TractorType,
                        ExpireDate = tractor.TechnicalInspectionExpireDate.Value.Date,
                        DaysLeft = (int)(tractor.TechnicalInspectionExpireDate.Value.Date - today).TotalDays,
                        BadgeClass = tractor.TechnicalInspectionExpireDate.Value.Date < today ? "bg-danger" : "bg-warning",
                        DetailsUrl = $"/Tractors/Details/{tractor.Id}"
                    });
                }

                if (tractor.ThirdPartyInsuranceExpireDate.HasValue &&
                    tractor.ThirdPartyInsuranceExpireDate.Value.Date <= warningDate)
                {
                    alerts.Add(new DashboardDocumentAlertVm
                    {
                        Type = "بیمه شخص ثالث",
                        Title = tractor.PolicePlateNumber,
                        Subtitle = tractor.TractorType,
                        ExpireDate = tractor.ThirdPartyInsuranceExpireDate.Value.Date,
                        DaysLeft = (int)(tractor.ThirdPartyInsuranceExpireDate.Value.Date - today).TotalDays,
                        BadgeClass = tractor.ThirdPartyInsuranceExpireDate.Value.Date < today ? "bg-danger" : "bg-warning",
                        DetailsUrl = $"/Tractors/Details/{tractor.Id}"
                    });
                }
            }

            return alerts
                .OrderBy(x => x.ExpireDate)
                .Take(10)
                .ToList();
        }

        private static List<AssignmentStatus> GetActiveAssignmentStatuses()
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
                AssignmentStatus.CancellationRequested
            };
        }

        private static DateTime GetSaturdayOfWeek(DateTime date)
        {
            var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return date.Date.AddDays(-diff);
        }

        private static string GetUserDisplayName(string? firstName, string? lastName, string? email)
        {
            var name = $"{firstName ?? ""} {lastName ?? ""}".Trim();
            return string.IsNullOrWhiteSpace(name) ? email ?? "-" : name;
        }

        private static string ToJalali(DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }

        private static string GetAssignmentStatusDisplay(AssignmentStatus status)
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

        private static string GetAssignmentStatusBadgeClass(AssignmentStatus status)
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

        private static string GetCargoRequestStatusDisplay(string status)
        {
            return status switch
            {
                "Pending" => "در انتظار بررسی",
                "Approved" => "تایید شده",
                "Rejected" => "رد شده",
                "Cancelled" => "لغو شده",
                _ => status
            };
        }

        private static string GetRequestBadgeClass(string status)
        {
            return status switch
            {
                "Pending" => "bg-warning",
                "Approved" => "bg-success",
                "Rejected" => "bg-danger",
                "Cancelled" => "bg-secondary",
                _ => "bg-secondary"
            };
        }

        private static string GetCargoAnnouncementStatusDisplay(CargoAnnouncementStatus status)
        {
            return status switch
            {
                CargoAnnouncementStatus.Pending => "در انتظار بررسی",
                CargoAnnouncementStatus.Contacted => "تماس گرفته شد",
                CargoAnnouncementStatus.ConvertedToHavaleh => "تبدیل به حواله",
                CargoAnnouncementStatus.Rejected => "رد شده",
                CargoAnnouncementStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetCargoAnnouncementBadgeClass(CargoAnnouncementStatus status)
        {
            return status switch
            {
                CargoAnnouncementStatus.Pending => "bg-warning",
                CargoAnnouncementStatus.Contacted => "bg-info",
                CargoAnnouncementStatus.ConvertedToHavaleh => "bg-success",
                CargoAnnouncementStatus.Rejected => "bg-danger",
                CargoAnnouncementStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }

        private static string GetTicketStatusDisplay(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "باز",
                TicketStatus.WaitingForOperator => "در انتظار اپراتور",
                TicketStatus.WaitingForUser => "در انتظار کاربر",
                TicketStatus.Closed => "بسته شده",
                TicketStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetTicketStatusBadgeClass(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "bg-primary",
                TicketStatus.WaitingForOperator => "bg-warning",
                TicketStatus.WaitingForUser => "bg-info",
                TicketStatus.Closed => "bg-success",
                TicketStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }

        private static string GetTicketPriorityDisplay(TicketPriority priority)
        {
            return priority switch
            {
                TicketPriority.Low => "کم",
                TicketPriority.Normal => "معمولی",
                TicketPriority.High => "زیاد",
                TicketPriority.Urgent => "فوری",
                _ => priority.ToString()
            };
        }

        private static string GetTicketPriorityBadgeClass(TicketPriority priority)
        {
            return priority switch
            {
                TicketPriority.Low => "bg-secondary",
                TicketPriority.Normal => "bg-info",
                TicketPriority.High => "bg-warning",
                TicketPriority.Urgent => "bg-danger",
                _ => "bg-secondary"
            };
        }
    }
}
