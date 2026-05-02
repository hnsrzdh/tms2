using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Infrastructure;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize(Roles = AppRoles.SystemAdmin + "," + AppRoles.OperationsManager + "," + AppRoles.OperationsUser)]
    public class AssignmentRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignmentRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.SubHavalehAssignmentRequests
                .AsNoTracking()
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.OriginPlace)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.DestinationPlace)
                .Include(x => x.Tractor)
                .Include(x => x.RequesterUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse<AssignmentRequestStatus>(statusFilter, true, out var status))
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.SubHavaleh.Havaleh.HavalehNumber.Contains(search) ||
                    (x.SubHavaleh.Title ?? "").Contains(search) ||
                    x.Tractor.PolicePlateNumber.Contains(search) ||
                    (x.RequesterUser != null && ((x.RequesterUser.FirstName ?? "") + " " + (x.RequesterUser.LastName ?? "")).Contains(search)) ||
                    (x.RequesterUser != null && (x.RequesterUser.Email ?? "").Contains(search)));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Status == AssignmentRequestStatus.Pending ? 0 : 1)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AssignmentRequestIndexItemViewModel
                {
                    Id = x.Id,
                    SubHavalehId = x.SubHavalehId,
                    HavalehNumber = x.SubHavaleh.Havaleh.HavalehNumber,
                    SubHavalehTitle = x.SubHavaleh.Title,
                    RequesterName = x.RequesterUser == null
                        ? "-"
                        : (((x.RequesterUser.FirstName ?? "") + " " + (x.RequesterUser.LastName ?? "")).Trim() != ""
                            ? ((x.RequesterUser.FirstName ?? "") + " " + (x.RequesterUser.LastName ?? "")).Trim()
                            : x.RequesterUser.Email),
                    TractorPlateNumber = x.Tractor.PolicePlateNumber,
                    OriginPlaceName = x.SubHavaleh.Havaleh.OriginPlace != null ? x.SubHavaleh.Havaleh.OriginPlace.Name : null,
                    DestinationPlaceName = x.SubHavaleh.DestinationPlace != null ? x.SubHavaleh.DestinationPlace.Name : null,
                    RequestedCargoAmount = x.RequestedCargoAmount,
                    Unit = x.SubHavaleh.Havaleh.Unit,
                    RequestedLoadingDate = x.RequestedLoadingDate,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status,
                    StatusDisplay = GetRequestStatusDisplay(x.Status),
                    StatusBadgeClass = GetRequestStatusBadgeClass(x.Status),
                    IsTruckCapacityFull = x.IsTruckCapacityFull,
                    RequesterNote = x.RequesterNote,
                    OperatorNote = x.OperatorNote
                })
                .ToListAsync();

            var vm = new AssignmentRequestIndexViewModel
            {
                Items = items,
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(long id, string? operatorNote)
        {
            var request = await _context.SubHavalehAssignmentRequests
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                .Include(x => x.Tractor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return NotFound();

            if (request.Status != AssignmentRequestStatus.Pending)
            {
                TempData["Err"] = "فقط درخواست‌های در انتظار بررسی قابل تایید هستند.";
                return RedirectToAction(nameof(Index));
            }

            var subAmount = request.SubHavaleh.RequestedCargoAmount ?? 0;
            var currentAssigned = await _context.TractorAssignments
                .Where(x => x.SubHavalehId == request.SubHavalehId && x.Status != AssignmentStatus.Cancelled)
                .SumAsync(x => (decimal?)x.AssignedCargoAmount) ?? 0;

            if (currentAssigned + request.RequestedCargoAmount > subAmount)
            {
                var remaining = subAmount - currentAssigned;
                TempData["Err"] = $"امکان تایید وجود ندارد. مقدار باقیمانده قابل تخصیص: {remaining:N3} {request.SubHavaleh.Havaleh.Unit}";
                return RedirectToAction(nameof(Index));
            }

            var capacityError = await ValidateTractorCapacityAsync(request.TractorId, request.RequestedCargoAmount);
            if (!string.IsNullOrWhiteSpace(capacityError))
            {
                TempData["Err"] = capacityError;
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = _userManager.GetUserId(User);

            var assignment = new TractorAssignment
            {
                SubHavalehId = request.SubHavalehId,
                TractorId = request.TractorId,
                DriverProfileId = request.DriverProfileId,
                AssignmentDate = DateTime.Now,
                AssignedCargoAmount = request.RequestedCargoAmount,
                IsTruckCapacityFull = request.IsTruckCapacityFull,
                Notes = string.IsNullOrWhiteSpace(operatorNote)
                    ? $"ایجاد شده از درخواست تخصیص شماره {request.Id}"
                    : $"ایجاد شده از درخواست تخصیص شماره {request.Id} - {operatorNote.Trim()}",
                Status = AssignmentStatus.Assigned
            };

            _context.TractorAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            request.Status = AssignmentRequestStatus.Approved;
            request.ReviewedAt = DateTime.Now;
            request.ReviewedByUserId = currentUserId;
            request.OperatorNote = string.IsNullOrWhiteSpace(operatorNote) ? null : operatorNote.Trim();
            request.CreatedTractorAssignmentId = assignment.Id;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست تایید شد و تخصیص کشنده برای ریزحواله ایجاد شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(long id, string? operatorNote)
        {
            var request = await _context.SubHavalehAssignmentRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (request == null)
                return NotFound();

            if (request.Status != AssignmentRequestStatus.Pending)
            {
                TempData["Err"] = "فقط درخواست‌های در انتظار بررسی قابل رد هستند.";
                return RedirectToAction(nameof(Index));
            }

            request.Status = AssignmentRequestStatus.Rejected;
            request.ReviewedAt = DateTime.Now;
            request.ReviewedByUserId = _userManager.GetUserId(User);
            request.OperatorNote = string.IsNullOrWhiteSpace(operatorNote) ? null : operatorNote.Trim();

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست رد شد.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string?> ValidateTractorCapacityAsync(int tractorId, decimal requestedAmount)
        {
            var tractor = await _context.Tractors.FirstOrDefaultAsync(x => x.Id == tractorId);
            if (tractor == null)
                return "کشنده انتخاب شده معتبر نیست.";

            var activeAssignments = await _context.TractorAssignments
                .Where(x => x.TractorId == tractorId &&
                            x.Status != AssignmentStatus.Completed &&
                            x.Status != AssignmentStatus.Cancelled &&
                            x.Status != AssignmentStatus.Unloaded)
                .ToListAsync();

            if (activeAssignments.Any(x => x.IsTruckCapacityFull))
                return "این کشنده در یک حمل فعال با ظرفیت کامل ثبت شده و فعلاً قابل استفاده نیست.";

            var pendingRequests = await _context.SubHavalehAssignmentRequests
                .Where(x => x.TractorId == tractorId && x.Status == AssignmentRequestStatus.Pending && x.Id != 0)
                .ToListAsync();

            var usedCapacity = activeAssignments.Sum(x => x.AssignedCargoAmount ?? 0) + pendingRequests.Sum(x => x.RequestedCargoAmount);
            usedCapacity -= requestedAmount; // درخواست جاری قبلاً در pendingRequests حساب شده است.
            if (usedCapacity < 0) usedCapacity = 0;

            var maxCapacity = tractor.MaxLoadCapacity ?? 0;
            if (maxCapacity > 0 && usedCapacity + requestedAmount > maxCapacity)
            {
                var free = maxCapacity - usedCapacity;
                return $"ظرفیت کشنده کافی نیست. ظرفیت آزاد فعلی: {free:N3} {tractor.CapacityUnit}";
            }

            return null;
        }

        private static string GetRequestStatusDisplay(AssignmentRequestStatus status)
        {
            return status switch
            {
                AssignmentRequestStatus.Pending => "در انتظار بررسی",
                AssignmentRequestStatus.Approved => "تایید شده",
                AssignmentRequestStatus.Rejected => "رد شده",
                AssignmentRequestStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetRequestStatusBadgeClass(AssignmentRequestStatus status)
        {
            return status switch
            {
                AssignmentRequestStatus.Pending => "bg-warning",
                AssignmentRequestStatus.Approved => "bg-success",
                AssignmentRequestStatus.Rejected => "bg-danger",
                AssignmentRequestStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
