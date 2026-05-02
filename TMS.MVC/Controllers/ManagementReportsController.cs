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
    public class ManagementReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagementReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Daily(DateTime? date, string? search, int page = 1, int pageSize = 10)
        {
            var selectedDate = date?.Date ?? DateTime.Today;
            return View("Report", await BuildReportAsync(
                title: "گزارش روزانه",
                reportMode: "Daily",
                startDate: selectedDate,
                endDate: selectedDate,
                search: search,
                page: page,
                pageSize: pageSize));
        }

        public async Task<IActionResult> Weekly(DateTime? date, string? search, int page = 1, int pageSize = 10)
        {
            var selectedDate = date?.Date ?? DateTime.Today;
            var start = GetSaturdayOfWeek(selectedDate);
            var end = start.AddDays(6);

            return View("Report", await BuildReportAsync(
                title: "گزارش هفتگی",
                reportMode: "Weekly",
                startDate: start,
                endDate: end,
                search: search,
                page: page,
                pageSize: pageSize));
        }

        public async Task<IActionResult> Monthly(int? year, int? month, string? search, int page = 1, int pageSize = 10)
        {
            var pc = new PersianCalendar();
            var now = DateTime.Today;

            var jy = year ?? pc.GetYear(now);
            var jm = month ?? pc.GetMonth(now);

            if (jm < 1) jm = 1;
            if (jm > 12) jm = 12;

            var daysInMonth = pc.GetDaysInMonth(jy, jm);
            var start = pc.ToDateTime(jy, jm, 1, 0, 0, 0, 0).Date;
            var end = pc.ToDateTime(jy, jm, daysInMonth, 0, 0, 0, 0).Date;

            var model = await BuildReportAsync(
                title: "گزارش ماهیانه",
                reportMode: "Monthly",
                startDate: start,
                endDate: end,
                search: search,
                page: page,
                pageSize: pageSize);

            model.JalaliYear = jy;
            model.JalaliMonth = jm;

            return View("Report", model);
        }

        public async Task<IActionResult> Yearly(int? year, string? search, int page = 1, int pageSize = 10)
        {
            var pc = new PersianCalendar();
            var jy = year ?? pc.GetYear(DateTime.Today);

            var start = pc.ToDateTime(jy, 1, 1, 0, 0, 0, 0).Date;
            var end = pc.ToDateTime(jy, 12, pc.GetDaysInMonth(jy, 12), 0, 0, 0, 0).Date;

            var model = await BuildReportAsync(
                title: "گزارش سالیانه",
                reportMode: "Yearly",
                startDate: start,
                endDate: end,
                search: search,
                page: page,
                pageSize: pageSize);

            model.JalaliYear = jy;

            return View("Report", model);
        }

        private async Task<ManagementReportVm> BuildReportAsync(
            string title,
            string reportMode,
            DateTime startDate,
            DateTime endDate,
            string? search,
            int page,
            int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var from = startDate.Date;
            var to = endDate.Date.AddDays(1);

            var havalehQuery = _context.Havalehs
                .Include(x => x.Product)
                .Include(x => x.OriginPlace)
                .Include(x => x.GoodsOwnerLegalEntity)
                .Include(x => x.SubHavalehs)
                    .ThenInclude(x => x.TractorAssignments)
                .Where(x =>
                    (x.PurchaseDate.HasValue && x.PurchaseDate.Value >= from && x.PurchaseDate.Value < to) ||
                    (x.AllowedLoadingDate.HasValue && x.AllowedLoadingDate.Value >= from && x.AllowedLoadingDate.Value < to))
                .AsQueryable();

            var subHavalehQuery = _context.SubHavalehs
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.Product)
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.DestinationPlace)
                .Include(x => x.TractorAssignments)
                .Where(x =>
                    (x.StartDate.HasValue && x.StartDate.Value >= from && x.StartDate.Value < to) ||
                    (x.EndDate.HasValue && x.EndDate.Value >= from && x.EndDate.Value < to))
                .AsQueryable();

            var assignmentQuery = _context.TractorAssignments
                .Include(x => x.Tractor)
                .Include(x => x.DriverProfile)
                    .ThenInclude(x => x.ApplicationUser)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.OriginPlace)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.DestinationPlace)
                .Where(x =>
                    x.AssignmentDate >= from && x.AssignmentDate < to ||
                    (x.LoadingEndDate.HasValue && x.LoadingEndDate.Value >= from && x.LoadingEndDate.Value < to) ||
                    (x.UnloadingEndDate.HasValue && x.UnloadingEndDate.Value >= from && x.UnloadingEndDate.Value < to))
                .AsQueryable();

            var cargoRequestQuery = _context.SubHavalehAssignmentRequests
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                .Include(x => x.Tractor)
                .Where(x =>
                    x.CreatedAt >= from && x.CreatedAt < to ||
                    x.RequestedLoadingDate >= from && x.RequestedLoadingDate < to)
                .AsQueryable();

            var cargoAnnouncementQuery = _context.CargoAnnouncements
                .Where(x =>
                    x.CreatedAt >= from && x.CreatedAt < to)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                havalehQuery = havalehQuery.Where(x =>
                    x.HavalehNumber.Contains(search) ||
                    x.Product.Name.Contains(search) ||
                    x.OriginPlace.Name.Contains(search) ||
                    x.GoodsOwnerLegalEntity.CompanyName.Contains(search));

                subHavalehQuery = subHavalehQuery.Where(x =>
                    x.Title.Contains(search) ||
                    x.Havaleh.HavalehNumber.Contains(search) ||
                    x.Havaleh.Product.Name.Contains(search) ||
                    x.Havaleh.OriginPlace.Name.Contains(search) ||
                    x.DestinationPlace.Name.Contains(search));

                assignmentQuery = assignmentQuery.Where(x =>
                    x.Tractor.PolicePlateNumber.Contains(search) ||
                    x.SubHavaleh.Havaleh.HavalehNumber.Contains(search) ||
                    x.SubHavaleh.Title.Contains(search) ||
                    x.SubHavaleh.Havaleh.OriginPlace.Name.Contains(search) ||
                    x.SubHavaleh.DestinationPlace.Name.Contains(search) ||
                    (x.DriverProfile != null &&
                     (x.DriverProfile.ApplicationUser.FirstName.Contains(search) ||
                      x.DriverProfile.ApplicationUser.LastName.Contains(search) ||
                      (x.DriverProfile.ApplicationUser.FirstName + " " + x.DriverProfile.ApplicationUser.LastName).Contains(search))));

                cargoRequestQuery = cargoRequestQuery.Where(x =>
                    x.SubHavaleh.Havaleh.HavalehNumber.Contains(search) ||
                    x.SubHavaleh.Title.Contains(search) ||
                    x.Tractor.PolicePlateNumber.Contains(search));

                cargoAnnouncementQuery = cargoAnnouncementQuery.Where(x =>
                    x.CustomerCompanyName.Contains(search) ||
                    x.ContactPhone!.Contains(search) ||
                    x.ProductName.Contains(search) ||
                    x.OriginPlaceName.Contains(search) ||
                    x.DestinationPlaceName.Contains(search));
            }

            var havalehs = await havalehQuery.OrderByDescending(x => x.Id).ToListAsync();
            var subHavalehs = await subHavalehQuery.OrderByDescending(x => x.Id).ToListAsync();
            var assignments = await assignmentQuery.OrderByDescending(x => x.AssignmentDate).ToListAsync();
            var cargoRequests = await cargoRequestQuery.OrderByDescending(x => x.CreatedAt).ToListAsync();
            var cargoAnnouncements = await cargoAnnouncementQuery.OrderByDescending(x => x.CreatedAt).ToListAsync();

            var driverIds = assignments.Where(x => x.DriverProfileId.HasValue).Select(x => x.DriverProfileId!.Value).Distinct().ToList();
            var tractorIds = assignments.Select(x => x.TractorId).Distinct().ToList();

            var drivers = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .Where(x => driverIds.Contains(x.Id))
                .ToListAsync();

            var tractors = await _context.Tractors
                .Where(x => tractorIds.Contains(x.Id))
                .ToListAsync();

            var dailyTrends = BuildDailyTrends(from, endDate, havalehs, subHavalehs, assignments, cargoRequests, cargoAnnouncements);

            var model = new ManagementReportVm
            {
                Title = title,
                ReportMode = reportMode,
                StartDate = from,
                EndDate = endDate.Date,
                Search = search,
                Page = page,
                PageSize = pageSize,
                PeriodTitle = BuildPeriodTitle(reportMode, from, endDate.Date),

                Summary = new ReportSummaryVm
                {
                    HavalehCount = havalehs.Count,
                    SubHavalehCount = subHavalehs.Count,
                    AssignmentCount = assignments.Count,
                    ActiveAssignmentCount = assignments.Count(x => IsActiveAssignment(x.Status)),
                    CompletedAssignmentCount = assignments.Count(x => x.Status == AssignmentStatus.Completed || x.Status == AssignmentStatus.Unloaded),
                    DriverCount = drivers.Count,
                    BlockedDriverCount = drivers.Count(x => x.IsBlocked),
                    TractorCount = tractors.Count,
                    ActiveTractorCount = tractors.Count(x => x.Status == "فعال"),
                    CargoRequestCount = cargoRequests.Count,
                    PendingCargoRequestCount = cargoRequests.Count(x => x.Status.ToString() == "Pending"),
                    CargoAnnouncementCount = cargoAnnouncements.Count,
                    PendingCargoAnnouncementCount = cargoAnnouncements.Count(x => x.Status.ToString() == "Pending"),
                    TotalHavalehAmount = havalehs.Sum(x => x.ProductAmount ?? 0),
                    TotalSubHavalehAmount = subHavalehs.Sum(x => x.RequestedCargoAmount ?? 0),
                    TotalAssignedAmount = assignments.Sum(x => x.AssignedCargoAmount ?? 0),
                    TotalLoadedAmount = assignments.Sum(x => x.LoadedAmount ?? 0),
                    TotalUnloadedAmount = assignments.Sum(x => x.UnloadedAmount ?? 0),
                    TotalDriverPaidAmount = assignments.Where(x => x.IsSettled && x.SettledTo == "Driver").Sum(x => x.PayableAmount ?? x.FinalFare ?? x.FinancialApprovedAmount ?? 0),
                    TotalTractorPaidAmount = assignments.Where(x => x.IsSettled && x.SettledTo == "Tractor").Sum(x => x.PayableAmount ?? x.FinalFare ?? x.FinancialApprovedAmount ?? 0)
                },

                HavalehStatusItems = new List<ReportStatusItemVm>
                {
                    new() { Label = "حواله", Count = havalehs.Count, BadgeClass = "bg-primary" },
                    new() { Label = "ریزحواله", Count = subHavalehs.Count, BadgeClass = "bg-info" },
                    new() { Label = "تخصیص", Count = assignments.Count, BadgeClass = "bg-secondary" },
                    new() { Label = "درخواست حمل", Count = cargoRequests.Count, BadgeClass = "bg-warning" },
                    new() { Label = "اعلام بار", Count = cargoAnnouncements.Count, BadgeClass = "bg-success" }
                },

                AssignmentStatusItems = assignments
                    .GroupBy(x => x.Status)
                    .Select(g => new ReportStatusItemVm
                    {
                        Label = GetStatusDisplay(g.Key),
                        Count = g.Count(),
                        BadgeClass = GetStatusBadgeClass(g.Key)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList(),

                DailyTrends = dailyTrends,

                Havalehs = havalehs
                    .Select(x => new ReportHavalehItemVm
                    {
                        Id = x.Id,
                        HavalehNumber = x.HavalehNumber,
                        ProductName = x.Product?.Name,
                        OriginPlaceName = x.OriginPlace?.Name,
                        GoodsOwnerName = x.GoodsOwnerLegalEntity?.CompanyName,
                        PurchaseDate = x.PurchaseDate,
                        AllowedLoadingDate = x.AllowedLoadingDate,
                        ProductAmount = x.ProductAmount,
                        Unit = x.Unit,
                        SubHavalehCount = x.SubHavalehs?.Count ?? 0,
                        SubHavalehAmount = x.SubHavalehs?.Sum(s => s.RequestedCargoAmount ?? 0) ?? 0,
                        LoadedAmount = x.SubHavalehs?.SelectMany(s => s.TractorAssignments).Sum(a => a.LoadedAmount ?? 0) ?? 0,
                        UnloadedAmount = x.SubHavalehs?.SelectMany(s => s.TractorAssignments).Sum(a => a.UnloadedAmount ?? 0) ?? 0
                    })
                    .ToList(),

                SubHavalehs = subHavalehs
                    .Select(x => new ReportSubHavalehItemVm
                    {
                        Id = x.Id,
                        Title = x.Title,
                        HavalehNumber = x.Havaleh?.HavalehNumber,
                        ProductName = x.Havaleh?.Product?.Name,
                        OriginPlaceName = x.Havaleh?.OriginPlace?.Name,
                        DestinationPlaceName = x.DestinationPlace?.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        RequestedCargoAmount = x.RequestedCargoAmount,
                        Unit = x.Havaleh?.Unit,
                        AssignmentCount = x.TractorAssignments?.Count ?? 0,
                        AssignedAmount = x.TractorAssignments?.Sum(a => a.AssignedCargoAmount ?? 0) ?? 0,
                        LoadedAmount = x.TractorAssignments?.Sum(a => a.LoadedAmount ?? 0) ?? 0,
                        UnloadedAmount = x.TractorAssignments?.Sum(a => a.UnloadedAmount ?? 0) ?? 0
                    })
                    .ToList(),

                Assignments = assignments
                    .Select(x => new ReportAssignmentItemVm
                    {
                        Id = x.Id,
                        HavalehNumber = x.SubHavaleh?.Havaleh?.HavalehNumber,
                        SubHavalehTitle = x.SubHavaleh?.Title,
                        TractorPlateNumber = x.Tractor?.PolicePlateNumber,
                        DriverName = x.DriverProfile?.ApplicationUser == null
                            ? "-"
                            : GetUserDisplayName(x.DriverProfile.ApplicationUser.FirstName, x.DriverProfile.ApplicationUser.LastName, x.DriverProfile.ApplicationUser.Email),
                        OriginPlaceName = x.SubHavaleh?.Havaleh?.OriginPlace?.Name,
                        DestinationPlaceName = x.SubHavaleh?.DestinationPlace?.Name,
                        Status = x.Status,
                        StatusDisplay = GetStatusDisplay(x.Status),
                        StatusBadgeClass = GetStatusBadgeClass(x.Status),
                        AssignmentDate = x.AssignmentDate,
                        LoadingEndDate = x.LoadingEndDate,
                        UnloadingEndDate = x.UnloadingEndDate,
                        AssignedCargoAmount = x.AssignedCargoAmount,
                        LoadedAmount = x.LoadedAmount,
                        UnloadedAmount = x.UnloadedAmount,
                        Unit = x.SubHavaleh?.Havaleh?.Unit
                    })
                    .ToList(),

                Drivers = drivers
                    .Select(d =>
                    {
                        var driverAssignments = assignments.Where(a => a.DriverProfileId == d.Id).ToList();
                        return new ReportDriverItemVm
                        {
                            Id = d.Id,
                            FullName = GetUserDisplayName(d.ApplicationUser?.FirstName, d.ApplicationUser?.LastName, d.ApplicationUser?.Email),
                            NationalId = d.ApplicationUser?.NationalId,
                            PhoneNumber = d.ApplicationUser?.PhoneNumber,
                            IsBlocked = d.IsBlocked,
                            WalletBalance = d.WalletBalance,
                            AssignmentCount = driverAssignments.Count,
                            LoadedAmount = driverAssignments.Sum(a => a.LoadedAmount ?? 0),
                            PaidAmount = driverAssignments.Where(a => a.IsSettled && a.SettledTo == "Driver").Sum(a => a.PayableAmount ?? a.FinalFare ?? a.FinancialApprovedAmount ?? 0)
                        };
                    })
                    .ToList(),

                Tractors = tractors
                    .Select(t =>
                    {
                        var tractorAssignments = assignments.Where(a => a.TractorId == t.Id).ToList();
                        return new ReportTractorItemVm
                        {
                            Id = t.Id,
                            PolicePlateNumber = t.PolicePlateNumber,
                            TractorType = t.TractorType,
                            Status = t.Status,
                            MaxLoadCapacity = t.MaxLoadCapacity,
                            CapacityUnit = t.CapacityUnit,
                            WalletBalance = t.WalletBalance,
                            AssignmentCount = tractorAssignments.Count,
                            LoadedAmount = tractorAssignments.Sum(a => a.LoadedAmount ?? 0),
                            PaidAmount = tractorAssignments.Where(a => a.IsSettled && a.SettledTo == "Tractor").Sum(a => a.PayableAmount ?? a.FinalFare ?? a.FinancialApprovedAmount ?? 0)
                        };
                    })
                    .ToList(),

                CargoRequests = cargoRequests
                    .Select(x => new ReportCargoRequestItemVm
                    {
                        Id = x.Id,
                        HavalehNumber = x.SubHavaleh?.Havaleh?.HavalehNumber,
                        SubHavalehTitle = x.SubHavaleh?.Title,
                        RequesterName = x.RequesterUser?.FullName,
                        TractorPlateNumber = x.Tractor?.PolicePlateNumber,
                        StatusDisplay = GetCargoRequestStatusDisplay(x.Status.ToString()),
                        StatusBadgeClass = GetRequestBadgeClass(x.Status.ToString()),
                        CreatedAt = x.CreatedAt,
                        RequestedLoadingDate = x.RequestedLoadingDate,
                        RequestedCargoAmount = x.RequestedCargoAmount,
                        Unit = x.SubHavaleh?.Havaleh?.Unit
                    })
                    .ToList(),

                CargoAnnouncements = cargoAnnouncements
                    .Select(x => new ReportCargoAnnouncementItemVm
                    {
                        Id = x.Id,
                        TrackingCode = x.Id.ToString(),
                        CustomerName = x.CustomerCompanyName,
                        CustomerPhone = x.ContactPhone,
                        ProductName = x.ProductName,
                        OriginPlaceName = x.OriginPlaceName,
                        DestinationPlaceName = x.DestinationPlaceName,
                        CreatedAt = x.CreatedAt,
                        RequestedLoadingDate = x.LoadingStartDate,
                        CargoAmount = x.ProductAmount,
                        Unit = x.Unit,
                        StatusDisplay = GetCargoAnnouncementStatusDisplay(x.Status.ToString()),
                        StatusBadgeClass = GetRequestBadgeClass(x.Status.ToString())
                    })
                    .ToList()
            };

            model.TotalHavalehs = model.Havalehs.Count;
            model.TotalSubHavalehs = model.SubHavalehs.Count;
            model.TotalAssignments = model.Assignments.Count;
            model.TotalDrivers = model.Drivers.Count;
            model.TotalTractors = model.Tractors.Count;
            model.TotalCargoRequests = model.CargoRequests.Count;
            model.TotalCargoAnnouncements = model.CargoAnnouncements.Count;

            return model;
        }

        private static List<ReportDailyTrendVm> BuildDailyTrends(
            DateTime from,
            DateTime endDate,
            List<Havaleh> havalehs,
            List<SubHavaleh> subHavalehs,
            List<TractorAssignment> assignments,
            List<SubHavalehAssignmentRequest> cargoRequests,
            List<CargoAnnouncement> cargoAnnouncements)
        {
            var result = new List<ReportDailyTrendVm>();

            for (var date = from.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var next = date.AddDays(1);

                result.Add(new ReportDailyTrendVm
                {
                    Date = date,
                    DateDisplay = ToJalali(date),
                    HavalehCount = havalehs.Count(x =>
                        (x.PurchaseDate.HasValue && x.PurchaseDate.Value >= date && x.PurchaseDate.Value < next) ||
                        (x.AllowedLoadingDate.HasValue && x.AllowedLoadingDate.Value >= date && x.AllowedLoadingDate.Value < next)),
                    SubHavalehCount = subHavalehs.Count(x =>
                        (x.StartDate.HasValue && x.StartDate.Value >= date && x.StartDate.Value < next) ||
                        (x.EndDate.HasValue && x.EndDate.Value >= date && x.EndDate.Value < next)),
                    AssignmentCount = assignments.Count(x => x.AssignmentDate >= date && x.AssignmentDate < next),
                    CargoRequestCount = cargoRequests.Count(x => x.CreatedAt >= date && x.CreatedAt < next),
                    CargoAnnouncementCount = cargoAnnouncements.Count(x => x.CreatedAt >= date && x.CreatedAt < next),
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

        private static DateTime GetSaturdayOfWeek(DateTime date)
        {
            var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;
            return date.Date.AddDays(-diff);
        }

        private static string BuildPeriodTitle(string mode, DateTime start, DateTime end)
        {
            return mode switch
            {
                "Daily" => ToJalali(start),
                "Weekly" => $"{ToJalali(start)} تا {ToJalali(end)}",
                "Monthly" => $"{ToJalali(start)} تا {ToJalali(end)}",
                "Yearly" => $"{ToJalali(start)} تا {ToJalali(end)}",
                _ => $"{ToJalali(start)} تا {ToJalali(end)}"
            };
        }

        private static string ToJalali(DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }

        private static bool IsActiveAssignment(AssignmentStatus status)
        {
            return status != AssignmentStatus.Completed &&
                   status != AssignmentStatus.Cancelled &&
                   status != AssignmentStatus.Unloaded;
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

        private static string GetCargoAnnouncementStatusDisplay(string status)
        {
            return status switch
            {
                "Pending" => "در انتظار بررسی",
                "Contacted" => "تماس گرفته شد",
                "ConvertedToHavaleh" => "تبدیل به حواله",
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
                "Contacted" => "bg-info",
                "ConvertedToHavaleh" => "bg-success",
                "Rejected" => "bg-danger",
                "Cancelled" => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
