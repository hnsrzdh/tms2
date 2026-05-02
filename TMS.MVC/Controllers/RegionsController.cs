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
            string? cityFilter,
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.Places.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                q = q.Where(x =>
                    x.CountryName.Contains(search) ||
                    x.ProvinceName.Contains(search) ||
                    x.CityName.Contains(search) ||
                    x.Name.Contains(search) ||
                    (x.Address != null && x.Address.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(countryFilter))
                q = q.Where(x => x.CountryName == countryFilter);

            if (!string.IsNullOrWhiteSpace(provinceFilter))
                q = q.Where(x => x.ProvinceName == provinceFilter);

            if (!string.IsNullOrWhiteSpace(cityFilter))
                q = q.Where(x => x.CityName == cityFilter);

            var totalItems = await q.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            var items = await q
                .OrderBy(x => x.CountryName)
                .ThenBy(x => x.ProvinceName)
                .ThenBy(x => x.CityName)
                .ThenBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new PlaceIndexVm
            {
                Items = items,
                Search = search,
                CountryFilter = countryFilter,
                ProvinceFilter = provinceFilter,
                CityFilter = cityFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Countries = await GetCountryItems(),
                Provinces = await GetProvinceItems(countryFilter),
                Cities = await GetCityItems(countryFilter, provinceFilter)
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new PlaceFormVm
            {
                IsActive = true,
                Latitude = 35.6892m,
                Longitude = 51.3890m
            };

            await FillFormLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceFormVm model)
        {
            if (!ModelState.IsValid)
            {
                await FillFormLists(model);
                return View(model);
            }

            var countryName = NormalizeName(model.CountryName);
            var provinceName = NormalizeName(model.ProvinceName);
            var cityName = NormalizeName(model.CityName);
            var placeName = NormalizeName(model.Name);

            var duplicate = await _db.Places.AnyAsync(x =>
                x.CountryName == countryName &&
                x.ProvinceName == provinceName &&
                x.CityName == cityName &&
                x.Name == placeName);

            if (duplicate)
            {
                ModelState.AddModelError("", "این مکان قبلاً برای این شهر ثبت شده است.");
                await FillFormLists(model);
                return View(model);
            }

            _db.Places.Add(new Place
            {
                CountryName = countryName,
                ProvinceName = provinceName,
                CityName = cityName,
                Name = placeName,
                Address = NormalizeNullable(model.Address),
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                IsActive = model.IsActive
            });

            await _db.SaveChangesAsync();

            TempData["Ok"] = "ثبت مکان با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var place = await _db.Places.FindAsync(id);
            if (place == null) return NotFound();

            var vm = new PlaceFormVm
            {
                Id = place.Id,
                CountryName = place.CountryName,
                ProvinceName = place.ProvinceName,
                CityName = place.CityName,
                Name = place.Name,
                Address = place.Address,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                IsActive = place.IsActive
            };

            await FillFormLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PlaceFormVm model)
        {
            if (id != model.Id) return NotFound();

            var entity = await _db.Places.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillFormLists(model);
                return View(model);
            }

            var countryName = NormalizeName(model.CountryName);
            var provinceName = NormalizeName(model.ProvinceName);
            var cityName = NormalizeName(model.CityName);
            var placeName = NormalizeName(model.Name);

            var duplicate = await _db.Places.AnyAsync(x =>
                x.Id != id &&
                x.CountryName == countryName &&
                x.ProvinceName == provinceName &&
                x.CityName == cityName &&
                x.Name == placeName);

            if (duplicate)
            {
                ModelState.AddModelError("", "این مکان قبلاً برای این شهر ثبت شده است.");
                await FillFormLists(model);
                return View(model);
            }

            entity.CountryName = countryName;
            entity.ProvinceName = provinceName;
            entity.CityName = cityName;
            entity.Name = placeName;
            entity.Address = NormalizeNullable(model.Address);
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;
            entity.IsActive = model.IsActive;

            await _db.SaveChangesAsync();

            TempData["Ok"] = "ویرایش مکان با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.Places.FindAsync(id);

            if (entity != null)
            {
                _db.Places.Remove(entity);
                await _db.SaveChangesAsync();

                TempData["Ok"] = "حذف مکان انجام شد.";
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
                var placeName = NormalizeName(ws.Cell(row, 4).GetString());

                decimal? latitude = TryGetDecimal(ws.Cell(row, 5).GetString());
                decimal? longitude = TryGetDecimal(ws.Cell(row, 6).GetString());

                var address = NormalizeNullable(ws.Cell(row, 7).GetString());

                if (string.IsNullOrWhiteSpace(countryName) ||
                    string.IsNullOrWhiteSpace(provinceName) ||
                    string.IsNullOrWhiteSpace(cityName) ||
                    string.IsNullOrWhiteSpace(placeName))
                {
                    skipped++;
                    continue;
                }

                var existing = await _db.Places.FirstOrDefaultAsync(x =>
                    x.CountryName == countryName &&
                    x.ProvinceName == provinceName &&
                    x.CityName == cityName &&
                    x.Name == placeName);

                if (existing == null)
                {
                    _db.Places.Add(new Place
                    {
                        CountryName = countryName,
                        ProvinceName = provinceName,
                        CityName = cityName,
                        Name = placeName,
                        Latitude = latitude,
                        Longitude = longitude,
                        Address = address,
                        IsActive = true
                    });

                    created++;
                }
                else
                {
                    if (latitude.HasValue) existing.Latitude = latitude;
                    if (longitude.HasValue) existing.Longitude = longitude;
                    if (!string.IsNullOrWhiteSpace(address)) existing.Address = address;

                    existing.IsActive = true;
                    updated++;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Ok"] = $"ایمپورت انجام شد. جدید: {created} ، به‌روزرسانی: {updated} ، ردشده: {skipped}";
            return RedirectToAction(nameof(Index));
        }

        private async Task FillFormLists(PlaceFormVm vm)
        {
            var allPlaces = await _db.Places
                .Select(x => new
                {
                    x.CountryName,
                    x.ProvinceName,
                    x.CityName
                })
                .ToListAsync();

            vm.Countries = allPlaces
                .Where(x => !string.IsNullOrWhiteSpace(x.CountryName))
                .Select(x => x.CountryName)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToList();

            vm.Provinces = allPlaces
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CountryName) &&
                    x.CountryName == vm.CountryName &&
                    !string.IsNullOrWhiteSpace(x.ProvinceName))
                .Select(x => x.ProvinceName)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToList();

            vm.Cities = allPlaces
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CountryName) &&
                    !string.IsNullOrWhiteSpace(x.ProvinceName) &&
                    x.CountryName == vm.CountryName &&
                    x.ProvinceName == vm.ProvinceName &&
                    !string.IsNullOrWhiteSpace(x.CityName))
                .Select(x => x.CityName)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToList();

            vm.CountryProvinceMap = allPlaces
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CountryName) &&
                    !string.IsNullOrWhiteSpace(x.ProvinceName))
                .GroupBy(x => x.CountryName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ProvinceName).Distinct().OrderBy(x => x).ToList()
                );

            vm.ProvinceCityMap = allPlaces
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.ProvinceName) &&
                    !string.IsNullOrWhiteSpace(x.CityName))
                .GroupBy(x => x.ProvinceName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.CityName).Distinct().OrderBy(x => x).ToList()
                );
        }

        private async Task<List<SelectListItem>> GetCountryItems()
        {
            return await _db.Places
                .Select(x => x.CountryName)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> GetProvinceItems(string? countryName)
        {
            var q = _db.Places.AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryName))
                q = q.Where(x => x.CountryName == countryName);

            return await q
                .Select(x => x.ProvinceName)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> GetCityItems(string? countryName, string? provinceName)
        {
            var q = _db.Places.AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryName))
                q = q.Where(x => x.CountryName == countryName);

            if (!string.IsNullOrWhiteSpace(provinceName))
                q = q.Where(x => x.ProvinceName == provinceName);

            return await q
                .Select(x => x.CityName)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new SelectListItem { Value = x, Text = x })
                .ToListAsync();
        }

        private static string NormalizeName(string? value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static string? NormalizeNullable(string? value)
        {
            value = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static decimal? TryGetDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim().Replace(",", ".");

            if (decimal.TryParse(
                    value,
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