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
    [Authorize]
    public class OpenSubHavalehsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpenSubHavalehsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var today = DateTime.Today;

            var query = _context.SubHavalehs
                .AsNoTracking()
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.Product)
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.DestinationPlace)
                .Include(x => x.IntermediatePlaces)
                    .ThenInclude(x => x.Place)
                .Where(x =>
                    x.StartDate.HasValue &&
                    x.EndDate.HasValue &&
                    x.StartDate.Value.Date <= today &&
                    x.EndDate.Value.Date >= today &&
                    (x.RequestedCargoAmount ?? 0) > 0)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    (x.Title ?? "").Contains(search) ||
                    x.Havaleh.HavalehNumber.Contains(search) ||
                    (x.Havaleh.Product != null && x.Havaleh.Product.Name.Contains(search)) ||
                    (x.Havaleh.OriginPlace != null && x.Havaleh.OriginPlace.Name.Contains(search)) ||
                    (x.DestinationPlace != null && x.DestinationPlace.Name.Contains(search)));
            }

            var allItems = await query
                .OrderBy(x => x.EndDate)
                .ThenByDescending(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.HavalehId,
                    x.Title,
                    x.StartDate,
                    x.EndDate,
                    x.RequestedCargoAmount,
                    x.DriverPricePer1000Unit,
                    x.DriverPriceCurrency,
                    HavalehNumber = x.Havaleh.HavalehNumber,
                    Unit = x.Havaleh.Unit,
                    ProductName = x.Havaleh.Product != null ? x.Havaleh.Product.Name : null,
                    OriginPlaceName = x.Havaleh.OriginPlace != null ? x.Havaleh.OriginPlace.Name : null,
                    DestinationPlaceName = x.DestinationPlace != null ? x.DestinationPlace.Name : null,
                    RoutePlaces = x.IntermediatePlaces
                        .OrderBy(p => p.SortOrder)
                        .Select(p => p.Place.Name)
                        .ToList(),
                    AssignedAmount = x.TractorAssignments
                        .Where(a => a.Status != AssignmentStatus.Cancelled)
                        .Sum(a => a.AssignedCargoAmount ?? 0),
                    LoadedAmount = x.TractorAssignments
                        .Where(a => a.Status != AssignmentStatus.Cancelled)
                        .Sum(a => a.LoadedAmount ?? 0),
                    UnloadedAmount = x.TractorAssignments
                        .Where(a => a.Status != AssignmentStatus.Cancelled)
                        .Sum(a => a.UnloadedAmount ?? 0),
                    PendingRequestedAmount = _context.SubHavalehAssignmentRequests
                        .Where(r => r.SubHavalehId == x.Id && r.Status == AssignmentRequestStatus.Pending)
                        .Sum(r => (decimal?)r.RequestedCargoAmount) ?? 0
                })
                .ToListAsync();

            var filtered = allItems
                .Select(x => new OpenSubHavalehIndexItemViewModel
                {
                    Id = x.Id,
                    HavalehId = x.HavalehId,
                    HavalehNumber = x.HavalehNumber,
                    Title = x.Title,
                    ProductName = x.ProductName,
                    OriginPlaceName = x.OriginPlaceName,
                    DestinationPlaceName = x.DestinationPlaceName,
                    RouteSummary = x.RoutePlaces.Any() ? string.Join(" ، ", x.RoutePlaces) : "بدون شهر میانی",
                    RequestedCargoAmount = x.RequestedCargoAmount ?? 0,
                    AssignedAmount = x.AssignedAmount,
                    LoadedAmount = x.LoadedAmount,
                    UnloadedAmount = x.UnloadedAmount,
                    PendingRequestedAmount = x.PendingRequestedAmount,
                    RemainingAssignableAmount = Math.Max(0, (x.RequestedCargoAmount ?? 0) - x.AssignedAmount - x.PendingRequestedAmount),
                    Unit = x.Unit,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DriverPricePer1000Unit = x.DriverPricePer1000Unit,
                    DriverPriceCurrency = x.DriverPriceCurrency
                })
                .Where(x => x.RemainingAssignableAmount > 0)
                .ToList();

            var totalItems = filtered.Count;
            var items = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new OpenSubHavalehIndexViewModel
            {
                Items = items,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Request(long subHavalehId)
        {
            var vm = await BuildRequestVmAsync(subHavalehId, new AssignmentRequestCreateViewModel
            {
                SubHavalehId = subHavalehId,
                RequestedLoadingDate = DateTime.Today,
                IsTruckCapacityFull = true
            });

            if (vm == null)
                return NotFound();

            if (vm.RemainingAssignableAmount <= 0)
            {
                TempData["Err"] = "برای این ریزحواله مقدار قابل درخواست باقی نمانده است.";
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Request(AssignmentRequestCreateViewModel vm)
        {
            var filledVm = await BuildRequestVmAsync(vm.SubHavalehId, vm);
            if (filledVm == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(filledVm);

            var today = DateTime.Today;
            if (!filledVm.StartDate.HasValue || !filledVm.EndDate.HasValue ||
                filledVm.StartDate.Value.Date > today || filledVm.EndDate.Value.Date < today)
            {
                ModelState.AddModelError(string.Empty, "مهلت درخواست برای این ریزحواله فعال نیست.");
                return View(filledVm);
            }

            if (!vm.RequestedLoadingDate.HasValue)
            {
                ModelState.AddModelError(nameof(vm.RequestedLoadingDate), "تاریخ مراجعه برای بارگیری الزامی است.");
                return View(filledVm);
            }

            if (vm.RequestedLoadingDate.Value.Date < filledVm.StartDate.Value.Date ||
                vm.RequestedLoadingDate.Value.Date > filledVm.EndDate.Value.Date)
            {
                ModelState.AddModelError(nameof(vm.RequestedLoadingDate), "تاریخ مراجعه باید در بازه شروع و پایان بارگیری ریزحواله باشد.");
                return View(filledVm);
            }

            var requestedAmount = vm.RequestedCargoAmount ?? 0;
            if (requestedAmount <= 0)
            {
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount), "مقدار پیشنهادی باید بزرگتر از صفر باشد.");
                return View(filledVm);
            }

            if (requestedAmount > filledVm.RemainingAssignableAmount)
            {
                ModelState.AddModelError(nameof(vm.RequestedCargoAmount), $"مقدار پیشنهادی بیشتر از باقیمانده قابل درخواست است. باقیمانده: {filledVm.RemainingAssignableAmount:N3} {filledVm.Unit}");
                return View(filledVm);
            }

            var tractor = await _context.Tractors.FirstOrDefaultAsync(x => x.Id == vm.TractorId);
            if (tractor == null)
            {
                ModelState.AddModelError(nameof(vm.TractorId), "کشنده انتخاب شده معتبر نیست.");
                return View(filledVm);
            }

            if (!CanUserUseTractor(tractor))
            {
                ModelState.AddModelError(nameof(vm.TractorId), "شما مجاز به ثبت درخواست با این کشنده نیستید.");
                return View(filledVm);
            }

            var capacityError = await ValidateTractorCapacityAsync(vm.TractorId, requestedAmount);
            if (!string.IsNullOrWhiteSpace(capacityError))
            {
                ModelState.AddModelError(nameof(vm.TractorId), capacityError);
                return View(filledVm);
            }

            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Challenge();

            var driverProfileId = await _context.DriverProfiles
                .Where(x => x.ApplicationUserId == currentUserId)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync();

            var hasPendingSameRequest = await _context.SubHavalehAssignmentRequests.AnyAsync(x =>
                x.SubHavalehId == vm.SubHavalehId &&
                x.TractorId == vm.TractorId &&
                x.RequesterUserId == currentUserId &&
                x.Status == AssignmentRequestStatus.Pending);

            if (hasPendingSameRequest)
            {
                ModelState.AddModelError(string.Empty, "برای این کشنده و این ریزحواله قبلاً یک درخواست در انتظار بررسی ثبت کرده‌اید.");
                return View(filledVm);
            }

            var request = new SubHavalehAssignmentRequest
            {
                SubHavalehId = vm.SubHavalehId,
                TractorId = vm.TractorId,
                RequesterUserId = currentUserId,
                DriverProfileId = driverProfileId,
                RequestedCargoAmount = requestedAmount,
                IsTruckCapacityFull = vm.IsTruckCapacityFull,
                RequestedLoadingDate = vm.RequestedLoadingDate.Value.Date,
                RequesterNote = string.IsNullOrWhiteSpace(vm.RequesterNote) ? null : vm.RequesterNote.Trim(),
                Status = AssignmentRequestStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.SubHavalehAssignmentRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "درخواست تخصیص با موفقیت ثبت شد و در انتظار تایید اپراتور است.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchRequestTractors(string? term)
        {
            term = (term ?? string.Empty).Trim();

            var query = _context.Tractors
                .AsNoTracking()
                .Where(t => t.Status == "فعال")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term) && term.Length >= 2)
                query = query.Where(t => t.PolicePlateNumber.Contains(term));

            var currentUserId = _userManager.GetUserId(User);
            var isOperator = User.IsInRole(AppRoles.SystemAdmin) || User.IsInRole(AppRoles.OperationsManager) || User.IsInRole(AppRoles.OperationsUser);
            var isOwner = User.IsInRole(AppRoles.TankerOwner);

            if (!isOperator && isOwner && !string.IsNullOrWhiteSpace(currentUserId))
                query = query.Where(t => t.OwnerApplicationUserId == currentUserId);

            var activeAssignments = await _context.TractorAssignments
                .AsNoTracking()
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                .Where(x =>
                    x.Status != AssignmentStatus.Completed &&
                    x.Status != AssignmentStatus.Cancelled &&
                    x.Status != AssignmentStatus.Unloaded)
                .ToListAsync();

            var pendingRequests = await _context.SubHavalehAssignmentRequests
                .AsNoTracking()
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                .Where(x => x.Status == AssignmentRequestStatus.Pending)
                .ToListAsync();

            var tractors = await query
                .OrderBy(t => t.PolicePlateNumber)
                .Take(50)
                .ToListAsync();

            var result = tractors.Select(t =>
            {
                var active = activeAssignments.Where(a => a.TractorId == t.Id).ToList();
                var pending = pendingRequests.Where(r => r.TractorId == t.Id).ToList();
                var used = active.Sum(a => a.AssignedCargoAmount ?? 0) + pending.Sum(r => r.RequestedCargoAmount);
                var hasFull = active.Any(a => a.IsTruckCapacityFull);
                var capacity = t.MaxLoadCapacity;
                var free = capacity.HasValue ? capacity.Value - used : (decimal?)null;

                var usedParts = new List<string>();
                usedParts.AddRange(active.Select(a => $"{a.AssignedCargoAmount ?? 0:N3} {t.CapacityUnit} در حواله {a.SubHavaleh.Havaleh.HavalehNumber}"));
                usedParts.AddRange(pending.Select(r => $"{r.RequestedCargoAmount:N3} {t.CapacityUnit} در درخواست حواله {r.SubHavaleh.Havaleh.HavalehNumber}"));

                return new TractorSearchResultViewModel
                {
                    Id = t.Id,
                    PolicePlateNumber = t.PolicePlateNumber,
                    TractorType = t.TractorType,
                    Capacity = t.MaxLoadCapacity,
                    CapacityUnit = t.CapacityUnit,
                    UsedCapacity = used,
                    FreeCapacity = free,
                    HasFullCapacityActiveAssignment = hasFull,
                    UsedCapacityDescription = usedParts.Any() ? string.Join(" | ", usedParts) : "-"
                };
            })
            .Where(x => !x.HasFullCapacityActiveAssignment)
            .Where(x => !x.FreeCapacity.HasValue || x.FreeCapacity.Value > 0)
            .ToList();

            return Json(result);
        }

        private async Task<AssignmentRequestCreateViewModel?> BuildRequestVmAsync(long subHavalehId, AssignmentRequestCreateViewModel vm)
        {
            var item = await _context.SubHavalehs
                .AsNoTracking()
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.Product)
                .Include(x => x.Havaleh)
                    .ThenInclude(x => x.OriginPlace)
                .Include(x => x.DestinationPlace)
                .Where(x => x.Id == subHavalehId)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.StartDate,
                    x.EndDate,
                    x.RequestedCargoAmount,
                    HavalehNumber = x.Havaleh.HavalehNumber,
                    Unit = x.Havaleh.Unit,
                    ProductName = x.Havaleh.Product != null ? x.Havaleh.Product.Name : null,
                    OriginPlaceName = x.Havaleh.OriginPlace != null ? x.Havaleh.OriginPlace.Name : null,
                    DestinationPlaceName = x.DestinationPlace != null ? x.DestinationPlace.Name : null,
                    AssignedAmount = x.TractorAssignments
                        .Where(a => a.Status != AssignmentStatus.Cancelled)
                        .Sum(a => a.AssignedCargoAmount ?? 0),
                    PendingRequestedAmount = _context.SubHavalehAssignmentRequests
                        .Where(r => r.SubHavalehId == x.Id && r.Status == AssignmentRequestStatus.Pending)
                        .Sum(r => (decimal?)r.RequestedCargoAmount) ?? 0
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return null;

            vm.SubHavalehId = item.Id;
            vm.HavalehNumber = item.HavalehNumber;
            vm.SubHavalehTitle = item.Title;
            vm.OriginPlaceName = item.OriginPlaceName;
            vm.DestinationPlaceName = item.DestinationPlaceName;
            vm.ProductName = item.ProductName;
            vm.Unit = item.Unit;
            vm.RequestedCargoAmountTotal = item.RequestedCargoAmount ?? 0;
            vm.AssignedAmount = item.AssignedAmount;
            vm.PendingRequestedAmount = item.PendingRequestedAmount;
            vm.RemainingAssignableAmount = Math.Max(0, (item.RequestedCargoAmount ?? 0) - item.AssignedAmount - item.PendingRequestedAmount);
            vm.StartDate = item.StartDate;
            vm.EndDate = item.EndDate;
            return vm;
        }

        private bool CanUserUseTractor(Tractor tractor)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isOperator = User.IsInRole(AppRoles.SystemAdmin) || User.IsInRole(AppRoles.OperationsManager) || User.IsInRole(AppRoles.OperationsUser);
            if (isOperator)
                return true;

            if (User.IsInRole(AppRoles.TankerOwner))
                return !string.IsNullOrWhiteSpace(currentUserId) && tractor.OwnerApplicationUserId == currentUserId;

            return true;
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
                .Where(x => x.TractorId == tractorId && x.Status == AssignmentRequestStatus.Pending)
                .ToListAsync();

            var usedCapacity = activeAssignments.Sum(x => x.AssignedCargoAmount ?? 0) + pendingRequests.Sum(x => x.RequestedCargoAmount);
            var maxCapacity = tractor.MaxLoadCapacity ?? 0;

            if (maxCapacity > 0 && usedCapacity + requestedAmount > maxCapacity)
            {
                var free = maxCapacity - usedCapacity;
                return $"ظرفیت کشنده کافی نیست. ظرفیت آزاد فعلی: {free:N3} {tractor.CapacityUnit}";
            }

            return null;
        }
    }
}
