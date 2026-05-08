﻿﻿using Microsoft.AspNetCore.Mvc;
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

            var q = _db.LegalEntities.Include(x => x.Addresses).AsQueryable();

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
                    FirstAddress = x.Addresses.OrderBy(a => a.Id).Select(a => a.AddressText).FirstOrDefault()
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

        public IActionResult Create() => View(new LegalEntityFormVm());

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

            return View(new LegalEntityDetailsVm { Entity = entity });
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
            return View(new LegalEntityContactChannelVm { LegalEntityId = legalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(LegalEntityContactChannelVm vm)
        {
            NormalizeLegalEntityContactVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = new LegalEntityContact
            {
                LegalEntityId = vm.LegalEntityId,
                Title = vm.Title,
                SmsNumber = vm.SmsNumber,
                WhatsAppNumber = vm.WhatsAppNumber,
                FaxNumber = vm.FaxNumber,
                PhoneNumber = vm.PhoneNumber,
                EmailAddress = vm.EmailAddress
            };

            _db.LegalEntityContacts.Add(entity);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = vm.LegalEntityId });
        }

        public async Task<IActionResult> EditContact(long id)
        {
            var item = await _db.LegalEntityContacts.FindAsync(id);
            if (item == null) return NotFound();

            return View(new LegalEntityContactChannelVm
            {
                Id = item.Id,
                LegalEntityId = item.LegalEntityId,
                Title = item.Title,
                SmsNumber = item.SmsNumber,
                WhatsAppNumber = item.WhatsAppNumber,
                FaxNumber = item.FaxNumber,
                PhoneNumber = item.PhoneNumber,
                EmailAddress = item.EmailAddress
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(long id, LegalEntityContactChannelVm vm)
        {
            NormalizeLegalEntityContactVm(vm);

            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var item = await _db.LegalEntityContacts.FindAsync(id);
            if (item == null) return NotFound();

            item.Title = vm.Title;
            item.SmsNumber = vm.SmsNumber;
            item.WhatsAppNumber = vm.WhatsAppNumber;
            item.FaxNumber = vm.FaxNumber;
            item.PhoneNumber = vm.PhoneNumber;
            item.EmailAddress = vm.EmailAddress;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = vm.LegalEntityId });
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
            return View(new LegalEntityBankAccountVm { LegalEntityId = legalEntityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBankAccount(LegalEntityBankAccountVm vm)
        {
            NormalizeLegalEntityBankAccountVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            if (vm.IsDefault)
            {
                var currentDefaults = await _db.LegalEntityBankAccounts
                    .Where(x => x.LegalEntityId == vm.LegalEntityId && x.IsDefault)
                    .ToListAsync();

                foreach (var account in currentDefaults)
                    account.IsDefault = false;
            }

            var entity = new LegalEntityBankAccount
            {
                LegalEntityId = vm.LegalEntityId,
                AccountOwnerName = vm.AccountOwnerName,
                AccountHolderName = vm.AccountOwnerName,
                BankName = vm.BankName,
                AccountNumber = vm.AccountNumber,
                CardNumber = vm.CardNumber,
                ShebaNumber = vm.ShebaNumber,
                Iban = vm.ShebaNumber ?? string.Empty,
                IsDefault = vm.IsDefault,
                Description = vm.Description
            };

            _db.LegalEntityBankAccounts.Add(entity);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = vm.LegalEntityId });
        }

        public async Task<IActionResult> EditBankAccount(long id)
        {
            var item = await _db.LegalEntityBankAccounts.FindAsync(id);
            if (item == null) return NotFound();

            return View(new LegalEntityBankAccountVm
            {
                Id = item.Id,
                LegalEntityId = item.LegalEntityId,
                AccountOwnerName = item.AccountOwnerName ?? item.AccountHolderName,
                BankName = item.BankName,
                AccountNumber = item.AccountNumber,
                CardNumber = item.CardNumber,
                ShebaNumber = item.ShebaNumber ?? item.Iban,
                IsDefault = item.IsDefault,
                Description = item.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBankAccount(long id, LegalEntityBankAccountVm vm)
        {
            NormalizeLegalEntityBankAccountVm(vm);

            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var item = await _db.LegalEntityBankAccounts.FindAsync(id);
            if (item == null) return NotFound();

            if (vm.IsDefault)
            {
                var currentDefaults = await _db.LegalEntityBankAccounts
                    .Where(x => x.LegalEntityId == vm.LegalEntityId && x.Id != vm.Id && x.IsDefault)
                    .ToListAsync();

                foreach (var account in currentDefaults)
                    account.IsDefault = false;
            }

            item.AccountOwnerName = vm.AccountOwnerName;
            item.AccountHolderName = vm.AccountOwnerName;
            item.BankName = vm.BankName;
            item.AccountNumber = vm.AccountNumber;
            item.CardNumber = vm.CardNumber;
            item.ShebaNumber = vm.ShebaNumber;
            item.Iban = vm.ShebaNumber ?? string.Empty;
            item.IsDefault = vm.IsDefault;
            item.Description = vm.Description;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = vm.LegalEntityId });
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

        private static void NormalizeLegalEntityContactVm(LegalEntityContactChannelVm vm)
        {
            vm.Title = (vm.Title ?? string.Empty).Trim();
            vm.SmsNumber = NormalizeText(vm.SmsNumber);
            vm.WhatsAppNumber = NormalizeText(vm.WhatsAppNumber);
            vm.FaxNumber = NormalizeText(vm.FaxNumber);
            vm.PhoneNumber = NormalizeText(vm.PhoneNumber);
            vm.EmailAddress = NormalizeText(vm.EmailAddress);
        }

        private static void NormalizeLegalEntityBankAccountVm(LegalEntityBankAccountVm vm)
        {
            vm.AccountOwnerName = NormalizeText(vm.AccountOwnerName);
            vm.BankName = NormalizeText(vm.BankName);
            vm.AccountNumber = NormalizeText(vm.AccountNumber);
            vm.CardNumber = NormalizeText(vm.CardNumber);
            vm.ShebaNumber = NormalizeText(vm.ShebaNumber);
            vm.Description = NormalizeText(vm.Description);
        }

        private static string? NormalizeText(string? value)
        {
            var result = (value ?? string.Empty).Trim();
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }
    }
}
