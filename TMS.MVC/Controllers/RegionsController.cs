using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.Models.ViewModels;

namespace TMS.MVC.Controllers
{
    public class RegionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RegionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(
            string? search,
            string? countryFilter,
            string? provinceFilter,
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.Cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x =>
                    x.CountryName.Contains(search) ||
                    x.ProvinceName.Contains(search) ||
                    x.Name.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(countryFilter))
                q = q.Where(x => x.CountryName == countryFilter);

            if (!string.IsNullOrWhiteSpace(provinceFilter))
                q = q.Where(x => x.ProvinceName == provinceFilter);

            var totalItems = await q.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            var items = await q
                .OrderBy(x => x.CountryName)
                .ThenBy(x => x.ProvinceName)
                .ThenBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new CityIndexVm
            {
                Items = items,
                Search = search,
                CountryFilter = countryFilter,
                ProvinceFilter = provinceFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,

                Countries = await _db.Cities
                    .Select(x => x.CountryName)
                    .Where(x => x != null && x != "")
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(x => new SelectListItem { Value = x, Text = x })
                    .ToListAsync(),

                Provinces = await _db.Cities
                    .Select(x => x.ProvinceName)
                    .Where(x => x != null && x != "")
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(x => new SelectListItem { Value = x, Text = x })
                    .ToListAsync()
            };

            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            var vm = await BuildFormVm();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityFormVm model)
        {
            if (!ModelState.IsValid)
            {
                await FillFormLists(model);
                return View(model);
            }

            var countryName = NormalizeName(model.CountryName);
            var provinceName = NormalizeName(model.ProvinceName);
            var cityName = NormalizeName(model.Name);

            var existing = await _db.Cities.FirstOrDefaultAsync(x =>
                x.CountryName == countryName &&
                x.ProvinceName == provinceName &&
                x.Name == cityName);

            if (existing != null)
            {
                ModelState.AddModelError("", "این شهر قبلاً ثبت شده است.");
                await FillFormLists(model);
                return View(model);
            }

            _db.Cities.Add(new City
            {
                CountryName = countryName,
                ProvinceName = provinceName,
                Name = cityName,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            });

            await _db.SaveChangesAsync();
            TempData["Ok"] = "ثبت با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null) return NotFound();

            var vm = new CityFormVm
            {
                Id = city.Id,
                CountryName = city.CountryName,
                ProvinceName = city.ProvinceName,
                Name = city.Name,
                Latitude = city.Latitude,
                Longitude = city.Longitude
            };

            await FillFormLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CityFormVm model)
        {
            if (id != model.Id) return NotFound();

            var entity = await _db.Cities.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillFormLists(model);
                return View(model);
            }

            var countryName = NormalizeName(model.CountryName);
            var provinceName = NormalizeName(model.ProvinceName);
            var cityName = NormalizeName(model.Name);

            var duplicate = await _db.Cities.FirstOrDefaultAsync(x =>
                x.Id != id &&
                x.CountryName == countryName &&
                x.ProvinceName == provinceName &&
                x.Name == cityName);

            if (duplicate != null)
            {
                ModelState.AddModelError("", "رکورد تکراری است.");
                await FillFormLists(model);
                return View(model);
            }

            entity.CountryName = countryName;
            entity.ProvinceName = provinceName;
            entity.Name = cityName;
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;

            await _db.SaveChangesAsync();
            TempData["Ok"] = "ویرایش با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.Cities.FindAsync(id);
            if (entity != null)
            {
                _db.Cities.Remove(entity);
                await _db.SaveChangesAsync();
                TempData["Ok"] = "حذف انجام شد.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Err"] = "فایل اکسل انتخاب نشده است.";
                return RedirectToAction(nameof(Index));
            }

            int created = 0;
            int updated = 0;
            int skipped = 0;

            using var stream = excelFile.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var ws = workbook.Worksheet(1);

            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;

            for (int row = 2; row <= lastRow; row++)
            {
                var countryName = NormalizeName(ws.Cell(row, 1).GetString());
                var provinceName = NormalizeName(ws.Cell(row, 2).GetString());
                var cityName = NormalizeName(ws.Cell(row, 3).GetString());

                decimal? latitude = TryGetDecimal(ws.Cell(row, 4).GetString());
                decimal? longitude = TryGetDecimal(ws.Cell(row, 5).GetString());

                if (string.IsNullOrWhiteSpace(countryName) ||
                    string.IsNullOrWhiteSpace(provinceName) ||
                    string.IsNullOrWhiteSpace(cityName))
                {
                    skipped++;
                    continue;
                }

                var existing = await _db.Cities.FirstOrDefaultAsync(x =>
                    x.CountryName == countryName &&
                    x.ProvinceName == provinceName &&
                    x.Name == cityName);

                if (existing == null)
                {
                    _db.Cities.Add(new City
                    {
                        CountryName = countryName,
                        ProvinceName = provinceName,
                        Name = cityName,
                        Latitude = latitude,
                        Longitude = longitude
                    });
                    created++;
                }
                else
                {
                    if (latitude.HasValue) existing.Latitude = latitude;
                    if (longitude.HasValue) existing.Longitude = longitude;
                    updated++;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Ok"] = $"ایمپورت انجام شد. جدید: {created} ، به‌روزرسانی: {updated} ، ردشده: {skipped}";
            return RedirectToAction(nameof(Index));
        }

        private async Task<CityFormVm> BuildFormVm()
        {
            var vm = new CityFormVm();
            await FillFormLists(vm);
            return vm;
        }

        private async Task FillFormLists(CityFormVm vm)
        {
            var countryList = await _db.Cities
                .Where(x => x.CountryName != null && x.CountryName != "")
                .Select(x => x.CountryName)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            var allProvincePairs = await _db.Cities
                .Where(x => x.CountryName != null && x.CountryName != "" &&
                            x.ProvinceName != null && x.ProvinceName != "")
                .Select(x => new
                {
                    x.CountryName,
                    x.ProvinceName
                })
                .Distinct()
                .OrderBy(x => x.CountryName)
                .ThenBy(x => x.ProvinceName)
                .ToListAsync();

            vm.Countries = countryList
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                })
                .ToList();

            var selectedCountry = (vm.CountryName ?? string.Empty).Trim();

            var provinceList = allProvincePairs
                .Where(x => x.CountryName == selectedCountry)
                .Select(x => x.ProvinceName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            vm.Provinces = provinceList
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                })
                .ToList();

            vm.CountryProvinceMap = allProvincePairs
                .GroupBy(x => x.CountryName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ProvinceName).Distinct().OrderBy(x => x).ToList()
                );
        }
        private static string NormalizeName(string? value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static decimal? TryGetDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim().Replace(",", ".");
            if (decimal.TryParse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var result))
            {
                return result;
            }

            return null;
        }
    }
}