using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.Models.ViewModels;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    public class TransportAgreementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportAgreementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1)
                page = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = _context.TransportAgreements.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.CargoOwnerName.Contains(search) ||
                    x.Origin.Contains(search) ||
                    x.Destination.Contains(search) ||
                    x.ProductName.Contains(search) ||
                    x.Status.Contains(search) ||
                    x.Unit.Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TransportAgreementIndexItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CargoOwnerName = x.CargoOwnerName,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    ProductName = x.ProductName,
                    Amount = x.Amount,
                    Unit = x.Unit,
                    Status = x.Status
                })
                .ToListAsync();

            var vm = new TransportAgreementIndexViewModel
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
        public IActionResult Create()
        {
            var vm = new TransportAgreementUpsertViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportAgreementUpsertViewModel vm)
        {
            NormalizeUpsertViewModel(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = new TransportAgreement
            {
                Title = vm.Title,
                CargoOwnerName = vm.CargoOwnerName,
                Origin = vm.Origin,
                Destination = vm.Destination,
                ProductName = vm.ProductName,
                Amount = vm.Amount!.Value,
                Unit = vm.Unit,
                Status = vm.Status
            };

            _context.TransportAgreements.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "تفاهم‌نامه حمل با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.TransportAgreements.FindAsync(id);
            if (entity == null)
                return NotFound();

            var vm = new TransportAgreementUpsertViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                CargoOwnerName = entity.CargoOwnerName,
                Origin = entity.Origin,
                Destination = entity.Destination,
                ProductName = entity.ProductName,
                Amount = entity.Amount,
                Unit = entity.Unit,
                Status = entity.Status
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransportAgreementUpsertViewModel vm)
        {
            NormalizeUpsertViewModel(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = await _context.TransportAgreements.FindAsync(vm.Id);
            if (entity == null)
                return NotFound();

            entity.Title = vm.Title;
            entity.CargoOwnerName = vm.CargoOwnerName;
            entity.Origin = vm.Origin;
            entity.Destination = vm.Destination;
            entity.ProductName = vm.ProductName;
            entity.Amount = vm.Amount!.Value;
            entity.Unit = vm.Unit;
            entity.Status = vm.Status;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "تفاهم‌نامه حمل با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int pageSize = 10, string? search = null)
        {
            var entity = await _context.TransportAgreements.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.TransportAgreements.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "تفاهم‌نامه حمل با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index), new { page, pageSize, search });
        }

        [HttpGet]
        public async Task<IActionResult> SearchCities(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            term = term.Trim();

            var items = await _context.Cities
                .Where(x =>
                    x.Name.Contains(term) ||
                    x.ProvinceName.Contains(term) ||
                    x.CountryName.Contains(term))
                .OrderBy(x => x.CountryName)
                .ThenBy(x => x.ProvinceName)
                .ThenBy(x => x.Name)
                .Take(30)
                .Select(x => new
                {
                    name = x.Name,
                    provinceName = x.ProvinceName,
                    countryName = x.CountryName
                })
                .ToListAsync();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            term = term.Trim();

            var items = await _context.Products
                .Where(x => x.Name.Contains(term))
                .OrderBy(x => x.Name)
                .Take(30)
                .Select(x => new
                {
                    name = x.Name
                })
                .ToListAsync();

            return Json(items);
        }

        private static void NormalizeUpsertViewModel(TransportAgreementUpsertViewModel vm)
        {
            vm.Title = (vm.Title ?? string.Empty).Trim();
            vm.CargoOwnerName = (vm.CargoOwnerName ?? string.Empty).Trim();
            vm.Origin = (vm.Origin ?? string.Empty).Trim();
            vm.Destination = (vm.Destination ?? string.Empty).Trim();
            vm.ProductName = (vm.ProductName ?? string.Empty).Trim();
            vm.Unit = (vm.Unit ?? string.Empty).Trim();
            vm.Status = (vm.Status ?? string.Empty).Trim();
        }
    }
}