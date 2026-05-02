using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class DriverWalletWithdrawalRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverWalletWithdrawalRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int driverProfileId)
        {
            var driver = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == driverProfileId);

            if (driver == null)
                return NotFound();

            var model = new DriverWalletWithdrawalCreateVm
            {
                DriverProfileId = driver.Id,
                DriverName = GetDriverName(driver),
                NationalId = driver.ApplicationUser?.NationalId,
                WalletBalance = driver.WalletBalance ?? 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DriverWalletWithdrawalCreateVm model)
        {
            var driver = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == model.DriverProfileId);

            if (driver == null)
                return NotFound();

            model.DriverName = GetDriverName(driver);
            model.NationalId = driver.ApplicationUser?.NationalId;
            model.WalletBalance = driver.WalletBalance ?? 0;

            if (!ModelState.IsValid)
                return View(model);

            if (model.Amount <= 0)
            {
                ModelState.AddModelError(nameof(model.Amount), "مبلغ برداشت باید بزرگتر از صفر باشد.");
                return View(model);
            }

            if (model.Amount > model.WalletBalance)
            {
                ModelState.AddModelError(nameof(model.Amount), $"مبلغ درخواست برداشت نمی‌تواند بیشتر از موجودی کیف پول باشد. موجودی فعلی: {model.WalletBalance:N0} ریال");
                return View(model);
            }

            var hasPendingRequest = await _context.DriverWalletWithdrawalRequests
                .AnyAsync(x => x.DriverProfileId == model.DriverProfileId && x.Status == DriverWalletWithdrawalRequestStatus.Pending);

            if (hasPendingRequest)
            {
                ModelState.AddModelError(string.Empty, "این راننده یک درخواست برداشت در انتظار بررسی دارد. ابتدا باید درخواست قبلی تعیین تکلیف شود.");
                return View(model);
            }

            var request = new DriverWalletWithdrawalRequest
            {
                DriverProfileId = model.DriverProfileId,
                Amount = model.Amount,
                RequestNote = model.RequestNote?.Trim(),
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name,
                Status = DriverWalletWithdrawalRequestStatus.Pending
            };

            _context.DriverWalletWithdrawalRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست برداشت از کیف پول با موفقیت ثبت شد و در انتظار بررسی مالی است.";
            return RedirectToAction("Details", "Drivers", new { id = model.DriverProfileId });
        }

        [Authorize(Roles = "SystemAdmin,FinanceManager,FinanceUser")]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.DriverWalletWithdrawalRequests
                .Include(x => x.DriverProfile)
                    .ThenInclude(x => x.ApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<DriverWalletWithdrawalRequestStatus>(statusFilter, out var status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.DriverProfile.ApplicationUser.FirstName.Contains(search) ||
                    x.DriverProfile.ApplicationUser.LastName.Contains(search) ||
                    (x.DriverProfile.ApplicationUser.FirstName + " " + x.DriverProfile.ApplicationUser.LastName).Contains(search) ||
                    (x.DriverProfile.ApplicationUser.NationalId != null && x.DriverProfile.ApplicationUser.NationalId.Contains(search)) ||
                    (x.DriverProfile.ApplicationUser.PhoneNumber != null && x.DriverProfile.ApplicationUser.PhoneNumber.Contains(search)) ||
                    (x.DriverProfile.ApplicationUser.Email != null && x.DriverProfile.ApplicationUser.Email.Contains(search)));
            }

            var totalItems = await query.CountAsync();

            var requests = await query
                .OrderBy(x => x.Status == DriverWalletWithdrawalRequestStatus.Pending ? 0 : 1)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new DriverWalletWithdrawalIndexVm
            {
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = requests.Select(x => new DriverWalletWithdrawalItemVm
                {
                    Id = x.Id,
                    DriverProfileId = x.DriverProfileId,
                    DriverName = GetDriverName(x.DriverProfile),
                    NationalId = x.DriverProfile.ApplicationUser?.NationalId,
                    PhoneNumber = x.DriverProfile.ApplicationUser?.PhoneNumber,
                    WalletBalance = x.DriverProfile.WalletBalance ?? 0,
                    Amount = x.Amount,
                    RequestNote = x.RequestNote,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status,
                    StatusDisplay = GetStatusDisplay(x.Status),
                    StatusBadgeClass = GetStatusBadgeClass(x.Status),
                    PaidAmount = x.PaidAmount,
                    PaidAt = x.PaidAt,
                    PaidBy = x.PaidBy,
                    PaymentReceiptNote = x.PaymentReceiptNote,
                    RejectionNote = x.RejectionNote
                }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "SystemAdmin,FinanceManager,FinanceUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(long id, decimal paidAmount, string? paymentReceiptNote)
        {
            var request = await _context.DriverWalletWithdrawalRequests
                .Include(x => x.DriverProfile)
                    .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return NotFound();

            if (request.Status != DriverWalletWithdrawalRequestStatus.Pending)
            {
                TempData["Err"] = "فقط درخواست‌های در انتظار بررسی قابل پرداخت هستند.";
                return RedirectToAction(nameof(Index));
            }

            if (paidAmount <= 0)
            {
                TempData["Err"] = "مبلغ پرداخت باید بزرگتر از صفر باشد.";
                return RedirectToAction(nameof(Index));
            }

            if (paidAmount > request.Amount)
            {
                TempData["Err"] = "مبلغ پرداخت نمی‌تواند بیشتر از مبلغ درخواست باشد.";
                return RedirectToAction(nameof(Index));
            }

            var currentBalance = request.DriverProfile.WalletBalance ?? 0;
            if (paidAmount > currentBalance)
            {
                TempData["Err"] = $"موجودی کیف پول راننده کافی نیست. موجودی فعلی: {currentBalance:N0} ریال";
                return RedirectToAction(nameof(Index));
            }

            request.Status = DriverWalletWithdrawalRequestStatus.Paid;
            request.PaidAmount = paidAmount;
            request.PaidAt = DateTime.Now;
            request.PaidBy = User.Identity?.Name;
            request.PaymentReceiptNote = paymentReceiptNote?.Trim();

            request.DriverProfile.WalletBalance = currentBalance - paidAmount;

            await _context.SaveChangesAsync();

            TempData["Ok"] = $"پرداخت برداشت با موفقیت ثبت شد و مبلغ {paidAmount:N0} ریال از کیف پول راننده کسر شد.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "SystemAdmin,FinanceManager,FinanceUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(long id, string? rejectionNote)
        {
            var request = await _context.DriverWalletWithdrawalRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            if (request.Status != DriverWalletWithdrawalRequestStatus.Pending)
            {
                TempData["Err"] = "فقط درخواست‌های در انتظار بررسی قابل رد هستند.";
                return RedirectToAction(nameof(Index));
            }

            request.Status = DriverWalletWithdrawalRequestStatus.Rejected;
            request.RejectedAt = DateTime.Now;
            request.RejectedBy = User.Identity?.Name;
            request.RejectionNote = rejectionNote?.Trim();

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست برداشت رد شد.";
            return RedirectToAction(nameof(Index));
        }

        private static string GetDriverName(DriverProfile driver)
        {
            if (driver.ApplicationUser == null)
                return "-";

            var name = $"{driver.ApplicationUser.FirstName} {driver.ApplicationUser.LastName}".Trim();
            return !string.IsNullOrWhiteSpace(name) ? name : driver.ApplicationUser.Email ?? "-";
        }

        private static string GetStatusDisplay(DriverWalletWithdrawalRequestStatus status)
        {
            return status switch
            {
                DriverWalletWithdrawalRequestStatus.Pending => "در انتظار بررسی",
                DriverWalletWithdrawalRequestStatus.Paid => "پرداخت شده",
                DriverWalletWithdrawalRequestStatus.Rejected => "رد شده",
                DriverWalletWithdrawalRequestStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetStatusBadgeClass(DriverWalletWithdrawalRequestStatus status)
        {
            return status switch
            {
                DriverWalletWithdrawalRequestStatus.Pending => "bg-warning",
                DriverWalletWithdrawalRequestStatus.Paid => "bg-success",
                DriverWalletWithdrawalRequestStatus.Rejected => "bg-danger",
                DriverWalletWithdrawalRequestStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
