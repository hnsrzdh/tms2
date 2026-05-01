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

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Type != null && x.Type.Contains(search)));
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
            return View(new ProductFormVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var name = NormalizeText(model.Name);
            var title = NormalizeNullableText(model.Type);

            var existing = await _db.Products.FirstOrDefaultAsync(x => x.Name == name);
            if (existing != null)
            {
                ModelState.AddModelError("", "محصولی با این نام قبلاً ثبت شده است.");
                return View(model);
            }

            _db.Products.Add(new Product
            {
                Name = name,
                Type = title
            });

            await _db.SaveChangesAsync();
            TempData["Ok"] = "محصول با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _db.Products.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new ProductFormVm
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ProductFormVm model)
        {
            if (id != model.Id) return NotFound();

            var entity = await _db.Products.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var name = NormalizeText(model.Name);
            var title = NormalizeNullableText(model.Type);

            var duplicate = await _db.Products.FirstOrDefaultAsync(x => x.Id != id && x.Name == name);
            if (duplicate != null)
            {
                ModelState.AddModelError("", "محصول دیگری با این نام قبلاً ثبت شده است.");
                return View(model);
            }

            entity.Name = name;
            entity.Type = title;

            await _db.SaveChangesAsync();
            TempData["Ok"] = "ویرایش محصول با موفقیت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.Products.FindAsync(id);
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
                var title = NormalizeNullableText(ws.Cell(row, 2).GetString());

                if (string.IsNullOrWhiteSpace(name))
                {
                    skipped++;
                    continue;
                }

                var existing = await _db.Products.FirstOrDefaultAsync(x => x.Name == name);

                if (existing == null)
                {
                    _db.Products.Add(new Product
                    {
                        Name = name,
                        Type = title
                    });
                    created++;
                }
                else
                {
                    existing.Type = title;
                    updated++;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Ok"] = $"ایمپورت انجام شد. جدید: {created} ، به‌روزرسانی: {updated} ، ردشده: {skipped}";
            return RedirectToAction(nameof(Index));
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