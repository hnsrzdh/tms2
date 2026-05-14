using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.Models.ViewModels;

namespace TMS.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        private const string DensityPropertyName = "چگالی / دانسیته";
        private const string LoadTypePropertyName = "نوع بار";
        private const string CargoNaturePropertyName = "ماهیت بار";
        private const string UnitPropertyName = "واحد اندازه‌گیری";
        private const string DefaultDensityValue = "2000";

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.Products
                .Include(x => x.Properties)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                q = q.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Type != null && x.Type.Contains(search)) ||
                    x.Properties.Any(p =>
                        p.Name.Contains(search) ||
                        (p.Value != null && p.Value.Contains(search))));
            }

            var totalItems = await q.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var items = await q
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var item in items)
            {
                item.Properties = item.Properties
                    .OrderBy(x => x.DisplayOrder)
                    .ThenBy(x => x.Id)
                    .ToList();
            }

            var vm = new ProductIndexVm
            {
                Items = items,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new ProductFormVm
            {
                Properties = BuildDefaultProperties()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormVm model)
        {
            NormalizeProductVm(model);
            ValidateProperties(model);

            if (!ModelState.IsValid)
                return View(model);

            var name = NormalizeText(model.Name);

            var existing = await _db.Products.FirstOrDefaultAsync(x => x.Name == name);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Name), "محصولی با این نام قبلاً ثبت شده است.");
                return View(model);
            }

            var properties = BuildCleanProperties(model.Properties);

            var entity = new Product
            {
                Name = name,
                Type = GetPropertyValue(properties, LoadTypePropertyName),
                Properties = properties
            };

            _db.Products.Add(entity);

            await _db.SaveChangesAsync();
            TempData["Ok"] = "محصول با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _db.Products
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            var properties = entity.Properties
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Id)
                .Select(x => new ProductPropertyFormVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            EnsureDefaultPropertyRows(properties, entity.Type);

            return View(new ProductFormVm
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                Properties = properties
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ProductFormVm model)
        {
            if (id != model.Id) return NotFound();

            NormalizeProductVm(model);
            ValidateProperties(model);

            var entity = await _db.Products
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var name = NormalizeText(model.Name);

            var duplicate = await _db.Products.FirstOrDefaultAsync(x => x.Id != id && x.Name == name);
            if (duplicate != null)
            {
                ModelState.AddModelError(nameof(model.Name), "محصول دیگری با این نام قبلاً ثبت شده است.");
                return View(model);
            }

            var properties = BuildCleanProperties(model.Properties);

            entity.Name = name;
            entity.Type = GetPropertyValue(properties, LoadTypePropertyName);

            var existingProperties = entity.Properties.ToList();
            _db.Set<ProductProperty>().RemoveRange(existingProperties);

            foreach (var property in properties)
            {
                entity.Properties.Add(property);
            }

            await _db.SaveChangesAsync();
            TempData["Ok"] = "ویرایش محصول با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.Products
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _db.Products.Remove(entity);
                await _db.SaveChangesAsync();
                TempData["Ok"] = "محصول حذف شد.";
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
                var name = NormalizeText(ws.Cell(row, 1).GetString());

                if (string.IsNullOrWhiteSpace(name))
                {
                    skipped++;
                    continue;
                }

                var density = NormalizeNullableText(ws.Cell(row, 2).GetString()) ?? DefaultDensityValue;
                var loadType = NormalizeNullableText(ws.Cell(row, 3).GetString()) ?? "تمیز";
                var cargoNature = NormalizeNullableText(ws.Cell(row, 4).GetString()) ?? "مایع";
                var unit = NormalizeNullableText(ws.Cell(row, 5).GetString()) ?? "لیتر";

                var existing = await _db.Products
                    .Include(x => x.Properties)
                    .FirstOrDefaultAsync(x => x.Name == name);

                var properties = new List<ProductProperty>
                {
                    new() { Name = DensityPropertyName, Value = density, DisplayOrder = 1 },
                    new() { Name = LoadTypePropertyName, Value = loadType, DisplayOrder = 2 },
                    new() { Name = CargoNaturePropertyName, Value = cargoNature, DisplayOrder = 3 },
                    new() { Name = UnitPropertyName, Value = unit, DisplayOrder = 4 }
                };

                if (existing == null)
                {
                    _db.Products.Add(new Product
                    {
                        Name = name,
                        Type = loadType,
                        Properties = properties
                    });

                    created++;
                }
                else
                {
                    existing.Type = loadType;

                    _db.Set<ProductProperty>().RemoveRange(existing.Properties);
                    foreach (var property in properties)
                    {
                        existing.Properties.Add(property);
                    }

                    updated++;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Ok"] = $"ایمپورت انجام شد. جدید: {created} ، به‌روزرسانی: {updated} ، ردشده: {skipped}";
            return RedirectToAction(nameof(Index));
        }

        private static void NormalizeProductVm(ProductFormVm model)
        {
            model.Name = NormalizeText(model.Name);

            for (var i = 0; i < model.Properties.Count; i++)
            {
                model.Properties[i].Name = NormalizeNullableText(model.Properties[i].Name);
                model.Properties[i].Value = NormalizeNullableText(model.Properties[i].Value);
                model.Properties[i].DisplayOrder = i + 1;
            }
        }

        private void ValidateProperties(ProductFormVm model)
        {
            var rows = model.Properties
                .Select((item, index) => new { Item = item, Index = index })
                .Where(x => !string.IsNullOrWhiteSpace(x.Item.Name) || !string.IsNullOrWhiteSpace(x.Item.Value))
                .ToList();

            foreach (var row in rows.Where(x => string.IsNullOrWhiteSpace(x.Item.Name)))
            {
                ModelState.AddModelError($"Properties[{row.Index}].Name", "برای هر مقدار خصوصیت، نام خصوصیت هم باید وارد شود.");
            }

            var duplicateGroups = rows
                .Where(x => !string.IsNullOrWhiteSpace(x.Item.Name))
                .GroupBy(x => x.Item.Name!.Trim())
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateGroups)
            {
                foreach (var row in group)
                {
                    ModelState.AddModelError($"Properties[{row.Index}].Name", "نام خصوصیت نباید تکراری باشد.");
                }
            }
        }

        private static List<ProductProperty> BuildCleanProperties(List<ProductPropertyFormVm> properties)
        {
            return properties
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Value))
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Select((x, index) => new ProductProperty
                {
                    Name = NormalizeText(x.Name),
                    Value = NormalizeNullableText(x.Value),
                    DisplayOrder = index + 1
                })
                .ToList();
        }

        private List<ProductPropertyFormVm> BuildDefaultProperties()
        {
            return new List<ProductPropertyFormVm>
            {
                new() { Name = DensityPropertyName, Value = DefaultDensityValue, DisplayOrder = 1 },
                new() { Name = LoadTypePropertyName, Value = "تمیز", DisplayOrder = 2 },
                new() { Name = CargoNaturePropertyName, Value = "مایع", DisplayOrder = 3 },
                new() { Name = UnitPropertyName, Value = "لیتر", DisplayOrder = 4 }
            };
        }

        private void EnsureDefaultPropertyRows(List<ProductPropertyFormVm> properties, string? oldType)
        {
            EnsurePropertyRow(properties, DensityPropertyName, DefaultDensityValue, 1);
            EnsurePropertyRow(properties, LoadTypePropertyName, oldType ?? "تمیز", 2);
            EnsurePropertyRow(properties, CargoNaturePropertyName, "مایع", 3);
            EnsurePropertyRow(properties, UnitPropertyName, "لیتر", 4);

            var ordered = properties
                .OrderBy(x => IsDefaultPropertyName(x.Name) ? GetDefaultOrder(x.Name) : 1000 + x.DisplayOrder)
                .ThenBy(x => x.DisplayOrder)
                .ToList();

            properties.Clear();
            properties.AddRange(ordered);

            for (var i = 0; i < properties.Count; i++)
                properties[i].DisplayOrder = i + 1;
        }

        private static void EnsurePropertyRow(List<ProductPropertyFormVm> properties, string name, string? defaultValue, int order)
        {
            if (properties.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
                return;

            properties.Add(new ProductPropertyFormVm
            {
                Name = name,
                Value = defaultValue,
                DisplayOrder = order
            });
        }

        private static bool IsDefaultPropertyName(string? name)
        {
            return name == DensityPropertyName ||
                   name == LoadTypePropertyName ||
                   name == CargoNaturePropertyName ||
                   name == UnitPropertyName;
        }

        private static int GetDefaultOrder(string? name)
        {
            return name switch
            {
                DensityPropertyName => 1,
                LoadTypePropertyName => 2,
                CargoNaturePropertyName => 3,
                UnitPropertyName => 4,
                _ => 1000
            };
        }

        private static string? GetPropertyValue(List<ProductProperty> properties, string propertyName)
        {
            return properties
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                ?.Value;
        }

        private static string NormalizeText(string? value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static string? NormalizeNullableText(string? value)
        {
            var result = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }
    }
}
