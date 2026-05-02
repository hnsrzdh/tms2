using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class TractorWalletWithdrawalRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TractorWalletWithdrawalRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int tractorId)
        {
            var tractor = await _context.Tractors
                .Include(x => x.OwnerApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == tractorId);

            if (tractor == null)
                return NotFound();

            var model = new TractorWalletWithdrawalCreateVm
            {
                TractorId = tractor.Id,
                PolicePlateNumber = tractor.PolicePlateNumber,
                OwnerName = GetOwnerName(tractor),
                OwnerNationalId = tractor.OwnerApplicationUser?.NationalId,
                WalletBalance = tractor.WalletBalance ?? 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TractorWalletWithdrawalCreateVm model)
        {
            var tractor = await _context.Tractors
                .Include(x => x.OwnerApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == model.TractorId);

            if (tractor == null)
                return NotFound();

            model.PolicePlateNumber = tractor.PolicePlateNumber;
            model.OwnerName = GetOwnerName(tractor);
            model.OwnerNationalId = tractor.OwnerApplicationUser?.NationalId;
            model.WalletBalance = tractor.WalletBalance ?? 0;

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

            var hasPendingRequest = await _context.TractorWalletWithdrawalRequests
                .AnyAsync(x => x.TractorId == model.TractorId && x.Status == TractorWalletWithdrawalRequestStatus.Pending);

            if (hasPendingRequest)
            {
                ModelState.AddModelError(string.Empty, "این کشنده یک درخواست برداشت در انتظار بررسی دارد. ابتدا باید درخواست قبلی تعیین تکلیف شود.");
                return View(model);
            }

            var request = new TractorWalletWithdrawalRequest
            {
                TractorId = model.TractorId,
                Amount = model.Amount,
                RequestNote = model.RequestNote?.Trim(),
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name,
                Status = TractorWalletWithdrawalRequestStatus.Pending
            };

            _context.TractorWalletWithdrawalRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست برداشت از کیف پول کشنده با موفقیت ثبت شد و در انتظار بررسی مالی است.";
            return RedirectToAction("Details", "Tractors", new { id = model.TractorId });
        }

        [Authorize(Roles = "SystemAdmin,FinanceManager,FinanceUser")]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.TractorWalletWithdrawalRequests
                .Include(x => x.Tractor)
                    .ThenInclude(x => x.OwnerApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<TractorWalletWithdrawalRequestStatus>(statusFilter, out var status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.Tractor.PolicePlateNumber.Contains(search) ||
                    (x.Tractor.TractorSmartCardNumber != null && x.Tractor.TractorSmartCardNumber.Contains(search)) ||
                    (x.Tractor.OwnerApplicationUser != null &&
                        (
                            x.Tractor.OwnerApplicationUser.FirstName.Contains(search) ||
                            x.Tractor.OwnerApplicationUser.LastName.Contains(search) ||
                            (x.Tractor.OwnerApplicationUser.FirstName + " " + x.Tractor.OwnerApplicationUser.LastName).Contains(search) ||
                            (x.Tractor.OwnerApplicationUser.NationalId != null && x.Tractor.OwnerApplicationUser.NationalId.Contains(search)) ||
                            (x.Tractor.OwnerApplicationUser.PhoneNumber != null && x.Tractor.OwnerApplicationUser.PhoneNumber.Contains(search)) ||
                            (x.Tractor.OwnerApplicationUser.Email != null && x.Tractor.OwnerApplicationUser.Email.Contains(search))
                        )));
            }

            var totalItems = await query.CountAsync();

            var requests = await query
                .OrderBy(x => x.Status == TractorWalletWithdrawalRequestStatus.Pending ? 0 : 1)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new TractorWalletWithdrawalIndexVm
            {
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = requests.Select(x => new TractorWalletWithdrawalItemVm
                {
                    Id = x.Id,
                    TractorId = x.TractorId,
                    PolicePlateNumber = x.Tractor.PolicePlateNumber,
                    OwnerName = GetOwnerName(x.Tractor),
                    OwnerNationalId = x.Tractor.OwnerApplicationUser?.NationalId,
                    OwnerPhoneNumber = x.Tractor.OwnerApplicationUser?.PhoneNumber,
                    WalletBalance = x.Tractor.WalletBalance ?? 0,
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
            var request = await _context.TractorWalletWithdrawalRequests
                .Include(x => x.Tractor)
                    .ThenInclude(x => x.OwnerApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return NotFound();

            if (request.Status != TractorWalletWithdrawalRequestStatus.Pending)
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

            var currentBalance = request.Tractor.WalletBalance ?? 0;
            if (paidAmount > currentBalance)
            {
                TempData["Err"] = $"موجودی کیف پول کشنده کافی نیست. موجودی فعلی: {currentBalance:N0} ریال";
                return RedirectToAction(nameof(Index));
            }

            request.Status = TractorWalletWithdrawalRequestStatus.Paid;
            request.PaidAmount = paidAmount;
            request.PaidAt = DateTime.Now;
            request.PaidBy = User.Identity?.Name;
            request.PaymentReceiptNote = paymentReceiptNote?.Trim();

            request.Tractor.WalletBalance = currentBalance - paidAmount;

            await _context.SaveChangesAsync();

            TempData["Ok"] = $"پرداخت برداشت با موفقیت ثبت شد و مبلغ {paidAmount:N0} ریال از کیف پول کشنده کسر شد.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "SystemAdmin,FinanceManager,FinanceUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(long id, string? rejectionNote)
        {
            var request = await _context.TractorWalletWithdrawalRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            if (request.Status != TractorWalletWithdrawalRequestStatus.Pending)
            {
                TempData["Err"] = "فقط درخواست‌های در انتظار بررسی قابل رد هستند.";
                return RedirectToAction(nameof(Index));
            }

            request.Status = TractorWalletWithdrawalRequestStatus.Rejected;
            request.RejectedAt = DateTime.Now;
            request.RejectedBy = User.Identity?.Name;
            request.RejectionNote = rejectionNote?.Trim();

            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست برداشت کشنده رد شد.";
            return RedirectToAction(nameof(Index));
        }

        private static string GetOwnerName(Tractor tractor)
        {
            if (tractor.OwnerApplicationUser == null)
                return "-";

            var name = $"{tractor.OwnerApplicationUser.FirstName} {tractor.OwnerApplicationUser.LastName}".Trim();
            return !string.IsNullOrWhiteSpace(name) ? name : tractor.OwnerApplicationUser.Email ?? "-";
        }

        private static string GetStatusDisplay(TractorWalletWithdrawalRequestStatus status)
        {
            return status switch
            {
                TractorWalletWithdrawalRequestStatus.Pending => "در انتظار بررسی",
                TractorWalletWithdrawalRequestStatus.Paid => "پرداخت شده",
                TractorWalletWithdrawalRequestStatus.Rejected => "رد شده",
                TractorWalletWithdrawalRequestStatus.Cancelled => "لغو شده",
                _ => status.ToString()
            };
        }

        private static string GetStatusBadgeClass(TractorWalletWithdrawalRequestStatus status)
        {
            return status switch
            {
                TractorWalletWithdrawalRequestStatus.Pending => "bg-warning",
                TractorWalletWithdrawalRequestStatus.Paid => "bg-success",
                TractorWalletWithdrawalRequestStatus.Rejected => "bg-danger",
                TractorWalletWithdrawalRequestStatus.Cancelled => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
