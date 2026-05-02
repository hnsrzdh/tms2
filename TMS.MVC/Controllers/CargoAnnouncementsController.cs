using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class CargoAnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CargoAnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? statusFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.CargoAnnouncements.AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<CargoAnnouncementStatus>(statusFilter, out var status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.CustomerCompanyName.Contains(search) ||
                    (x.ContactPersonName ?? "").Contains(search) ||
                    x.ContactMobile.Contains(search) ||
                    (x.AnnouncementType ?? "").Contains(search) ||
                    x.OriginPlaceName.Contains(search) ||
                    x.DestinationPlaceName.Contains(search) ||
                    x.ProductName.Contains(search) ||
                    (x.CreatedByDisplayName ?? "").Contains(search) ||
                    (x.CreatedHavalehNumber ?? "").Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CargoAnnouncementIndexItemViewModel
                {
                    Id = x.Id,
                    CustomerCompanyName = x.CustomerCompanyName,
                    ContactPersonName = x.ContactPersonName,
                    ContactMobile = x.ContactMobile,
                    AnnouncementType = x.AnnouncementType,
                    RequiresFleetEntryPermit = x.RequiresFleetEntryPermit,
                    OriginPlaceName = x.OriginPlaceName,
                    DestinationPlaceName = x.DestinationPlaceName,
                    ProductName = x.ProductName,
                    ProductAmount = x.ProductAmount,
                    Unit = x.Unit,
                    LoadingStartDate = x.LoadingStartDate,
                    LoadingEndDate = x.LoadingEndDate,
                    Status = x.Status,
                    StatusDisplay = GetStatusDisplay(x.Status),
                    StatusBadgeClass = GetStatusBadgeClass(x.Status),
                    CreatedAt = x.CreatedAt,
                    CreatedByDisplayName = x.CreatedByDisplayName,
                    OperatorNote = x.OperatorNote,
                    CreatedHavalehNumber = x.CreatedHavalehNumber
                })
                .ToListAsync();

            return View(new CargoAnnouncementIndexViewModel
            {
                Items = items,
                Search = search,
                StatusFilter = statusFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CargoAnnouncementFormViewModel
            {
                Unit = "کیلوگرم"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CargoAnnouncementFormViewModel vm)
        {
            Normalize(vm);

            if (vm.LoadingStartDate.HasValue && vm.LoadingEndDate.HasValue && vm.LoadingEndDate.Value.Date < vm.LoadingStartDate.Value.Date)
            {
                ModelState.AddModelError(nameof(vm.LoadingEndDate), "پایان بازه بارگیری نمی‌تواند قبل از شروع بازه باشد.");
            }

            if (!ModelState.IsValid)
                return View(vm);

            var entity = new CargoAnnouncement
            {
                CreatedByUserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null,
                CreatedByDisplayName = User.Identity?.Name,
                CustomerCompanyName = vm.CustomerCompanyName,
                ContactPersonName = vm.ContactPersonName,
                ContactMobile = vm.ContactMobile,
                ContactPhone = vm.ContactPhone,
                ContactEmail = vm.ContactEmail,
                AnnouncementType = vm.AnnouncementType,
                RequiresFleetEntryPermit = vm.RequiresFleetEntryPermit,
                OriginPlaceName = vm.OriginPlaceName,
                DestinationPlaceName = vm.DestinationPlaceName,
                ProductName = vm.ProductName,
                ProductAmount = vm.ProductAmount!.Value,
                Unit = vm.Unit,
                LoadingStartDate = vm.LoadingStartDate,
                LoadingEndDate = vm.LoadingEndDate,
                ShortagePenaltyPerUnit = vm.ShortagePenaltyPerUnit,
                CustomerNotes = vm.CustomerNotes,
                Status = CargoAnnouncementStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.CargoAnnouncements.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "اعلام بار شما با موفقیت ثبت شد. اپراتور پس از بررسی با شما تماس خواهد گرفت.";
            return RedirectToAction(nameof(Details), new { id = entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var entity = await _context.CargoAnnouncements.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            return View(new CargoAnnouncementDetailsViewModel
            {
                Entity = entity,
                StatusDisplay = GetStatusDisplay(entity.Status),
                StatusBadgeClass = GetStatusBadgeClass(entity.Status)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkContacted(long id, string? operatorNote)
        {
            var entity = await _context.CargoAnnouncements.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Status = CargoAnnouncementStatus.Contacted;
            entity.OperatorNote = NormalizeNullable(operatorNote);
            entity.ReviewedAt = DateTime.Now;
            entity.ReviewedByUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            entity.ReviewedByDisplayName = User.Identity?.Name;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "وضعیت اعلام بار به تماس گرفته شد تغییر کرد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkConverted(long id, string? createdHavalehNumber, string? operatorNote)
        {
            var entity = await _context.CargoAnnouncements.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Status = CargoAnnouncementStatus.ConvertedToHavaleh;
            entity.CreatedHavalehNumber = NormalizeNullable(createdHavalehNumber);
            entity.OperatorNote = NormalizeNullable(operatorNote);
            entity.ReviewedAt = DateTime.Now;
            entity.ReviewedByUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            entity.ReviewedByDisplayName = User.Identity?.Name;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "اعلام بار به وضعیت تبدیل به حواله شد تغییر کرد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(long id, string? operatorNote)
        {
            var entity = await _context.CargoAnnouncements.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Status = CargoAnnouncementStatus.Rejected;
            entity.OperatorNote = NormalizeNullable(operatorNote);
            entity.ReviewedAt = DateTime.Now;
            entity.ReviewedByUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            entity.ReviewedByDisplayName = User.Identity?.Name;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "اعلام بار رد شد.";
            return RedirectToAction(nameof(Index));
        }

        private static void Normalize(CargoAnnouncementFormViewModel vm)
        {
            vm.CustomerCompanyName = (vm.CustomerCompanyName ?? "").Trim();
            vm.ContactPersonName = NormalizeNullable(vm.ContactPersonName);
            vm.ContactMobile = (vm.ContactMobile ?? "").Trim();
            vm.ContactPhone = NormalizeNullable(vm.ContactPhone);
            vm.ContactEmail = NormalizeNullable(vm.ContactEmail);
            vm.AnnouncementType = NormalizeNullable(vm.AnnouncementType);
            vm.OriginPlaceName = (vm.OriginPlaceName ?? "").Trim();
            vm.DestinationPlaceName = (vm.DestinationPlaceName ?? "").Trim();
            vm.ProductName = (vm.ProductName ?? "").Trim();
            vm.Unit = string.IsNullOrWhiteSpace(vm.Unit) ? "کیلوگرم" : vm.Unit.Trim();
            vm.CustomerNotes = NormalizeNullable(vm.CustomerNotes);
        }

        private static string? NormalizeNullable(string? value)
        {
            value = (value ?? "").Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static string GetStatusDisplay(CargoAnnouncementStatus status)
        {
            return status switch
            {
                CargoAnnouncementStatus.Pending => "در انتظار بررسی",
                CargoAnnouncementStatus.Contacted => "تایید شد",
                CargoAnnouncementStatus.ConvertedToHavaleh => "تبدیل به حواله شد",
                CargoAnnouncementStatus.Rejected => "رد شد",
                CargoAnnouncementStatus.Cancelled => "لغو شد",
                _ => status.ToString()
            };
        }

        private static string GetStatusBadgeClass(CargoAnnouncementStatus status)
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
    }
}
