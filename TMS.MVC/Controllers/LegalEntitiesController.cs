using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.Models.ViewModels;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    public class LegalEntitiesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LegalEntitiesController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.LegalEntities
                .Include(x => x.Addresses)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x =>
                    x.CompanyName.Contains(search) ||
                    (x.CompanyType != null && x.CompanyType.Contains(search)) ||
                    (x.City != null && x.City.Contains(search)));
            }

            var totalItems = await q.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var items = await q
                .OrderBy(x => x.CompanyName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new LegalEntityIndexRowVm
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    CompanyType = x.CompanyType,
                    City = x.City,
                    FirstAddress = x.Addresses
                        .OrderBy(a => a.Id)
                        .Select(a => a.AddressText)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(new LegalEntityIndexVm
            {
                Items = items,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            });
        }
        public IActionResult Create()
        {
            return View(new LegalEntityFormVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LegalEntityFormVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = new LegalEntity
            {
                CompanyName = model.CompanyName.Trim(),
                CompanyType = model.CompanyType?.Trim(),
                City = model.City?.Trim()
            };

            _db.LegalEntities.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Ok"] = "شرکت با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _db.LegalEntities.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new LegalEntityFormVm
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                CompanyType = entity.CompanyType,
                City = entity.City
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LegalEntityFormVm model)
        {
            if (id != model.Id) return NotFound();

            var entity = await _db.LegalEntities.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            entity.CompanyName = model.CompanyName.Trim();
            entity.CompanyType = model.CompanyType?.Trim();
            entity.City = model.City?.Trim();

            await _db.SaveChangesAsync();
            TempData["Ok"] = "ویرایش شرکت انجام شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(long id)
        {
            var entity = await _db.LegalEntities
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .Include(x => x.BankAccounts)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            return View(new LegalEntityDetailsVm
            {
                Entity = entity
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.LegalEntities.FindAsync(id);
            if (entity != null)
            {
                _db.LegalEntities.Remove(entity);
                await _db.SaveChangesAsync();
                TempData["Ok"] = "شخص حقوقی حذف شد.";
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddContact(long legalEntityId)
        {
            return View(new LegalEntityContact { LegalEntityId = legalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(LegalEntityContact model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.LegalEntityContacts.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.LegalEntityId });
        }

        public async Task<IActionResult> EditContact(long id)
        {
            var item = await _db.LegalEntityContacts.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(long id, LegalEntityContact model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var item = await _db.LegalEntityContacts.FindAsync(id);
            if (item == null) return NotFound();

            item.Title = model.Title;
            item.ContactValue = model.ContactValue;
            item.HasSms = model.HasSms;
            item.HasWhatsApp = model.HasWhatsApp;
            item.IsFax = model.IsFax;
            item.IsPhone = model.IsPhone;
            item.IsEmail = model.IsEmail;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = item.LegalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(long id)
        {
            var item = await _db.LegalEntityContacts.FindAsync(id);
            if (item != null)
            {
                var legalEntityId = item.LegalEntityId;
                _db.LegalEntityContacts.Remove(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = legalEntityId });
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddAddress(long legalEntityId)
        {
            return View(new LegalEntityAddress { LegalEntityId = legalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(LegalEntityAddress model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.LegalEntityAddresses.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.LegalEntityId });
        }

        public async Task<IActionResult> EditAddress(long id)
        {
            var item = await _db.LegalEntityAddresses.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(long id, LegalEntityAddress model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var item = await _db.LegalEntityAddresses.FindAsync(id);
            if (item == null) return NotFound();

            item.Title = model.Title;
            item.AddressText = model.AddressText;
            item.PostalCode = model.PostalCode;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = item.LegalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var item = await _db.LegalEntityAddresses.FindAsync(id);
            if (item != null)
            {
                var legalEntityId = item.LegalEntityId;
                _db.LegalEntityAddresses.Remove(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = legalEntityId });
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddBankAccount(long legalEntityId)
        {
            return View(new LegalEntityBankAccount { LegalEntityId = legalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBankAccount(LegalEntityBankAccount model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.LegalEntityBankAccounts.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.LegalEntityId });
        }

        public async Task<IActionResult> EditBankAccount(long id)
        {
            var item = await _db.LegalEntityBankAccounts.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBankAccount(long id, LegalEntityBankAccount model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var item = await _db.LegalEntityBankAccounts.FindAsync(id);
            if (item == null) return NotFound();

            item.Iban = model.Iban;
            item.AccountHolderName = model.AccountHolderName;
            item.VerifiedName = model.VerifiedName;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = item.LegalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBankAccount(long id)
        {
            var item = await _db.LegalEntityBankAccounts.FindAsync(id);
            if (item != null)
            {
                var legalEntityId = item.LegalEntityId;
                _db.LegalEntityBankAccounts.Remove(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = legalEntityId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}