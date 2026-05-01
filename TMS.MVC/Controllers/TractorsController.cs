using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    public class TractorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TractorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Tractors
                .Include(x => x.OwnerApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.PolicePlateNumber.Contains(search) ||
                    (x.TractorSmartCardNumber ?? "").Contains(search) ||
                    (x.Status ?? "").Contains(search) ||
                    (x.FileNumber ?? "").Contains(search) ||
                    ((x.OwnerApplicationUser != null ? x.OwnerApplicationUser.FirstName : "") ?? "").Contains(search) ||
                    ((x.OwnerApplicationUser != null ? x.OwnerApplicationUser.LastName : "") ?? "").Contains(search) ||
                    ((x.OwnerApplicationUser != null ? x.OwnerApplicationUser.Email : "") ?? "").Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.PolicePlateNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TractorIndexItemViewModel
                {
                    Id = x.Id,
                    PolicePlateNumber = x.PolicePlateNumber,
                    TractorSmartCardNumber = x.TractorSmartCardNumber,
                    Status = x.Status,
                    MaxLoadCapacity = x.MaxLoadCapacity,
                    CapacityUnit = x.CapacityUnit,
                    TechnicalInspectionExpireDate = x.TechnicalInspectionExpireDate,
                    ThirdPartyInsuranceExpireDate = x.ThirdPartyInsuranceExpireDate,
                    OwnerName = x.OwnerApplicationUser == null
                        ? ""
                        : (
                            (((x.OwnerApplicationUser.FirstName ?? "") + " " + (x.OwnerApplicationUser.LastName ?? "")).Trim() != "")
                                ? ((x.OwnerApplicationUser.FirstName ?? "") + " " + (x.OwnerApplicationUser.LastName ?? "")).Trim()
                                : (x.OwnerApplicationUser.Email ?? "")
                          )
                })
                .ToListAsync();

            return View(new TractorIndexViewModel
            {
                Items = items,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new TractorUpsertViewModel
            {
                Status = "فعال"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TractorUpsertViewModel vm)
        {
            NormalizeTractorVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var policePlateNumber = BuildPolicePlateNumber(vm);

            var entity = new Tractor
            {
                PolicePlateNumber = policePlateNumber,
                OwnerApplicationUserId = string.IsNullOrWhiteSpace(vm.OwnerApplicationUserId) ? null : vm.OwnerApplicationUserId,
                NationalId = vm.NationalId,
                TractorSmartCardNumber = vm.TractorSmartCardNumber,
                FileNumber = vm.FileNumber,
                Status = vm.Status,
                TractorIdentifier = vm.TractorIdentifier,
                MaxLoadCapacity = vm.MaxLoadCapacity,
                TechnicalInspectionExpireDate = vm.TechnicalInspectionExpireDate,
                ThirdPartyInsuranceExpireDate = vm.ThirdPartyInsuranceExpireDate,
                ProductionYear = vm.ProductionYear,
                AxleCount = vm.AxleCount,
                SystemName = vm.SystemName,
                TractorType = vm.TractorType,
                CapacityUnit = vm.CapacityUnit,
                TransitPlateNumber = vm.TransitPlateNumber
            };

            _context.Tractors.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "کشنده با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.Tractors
                .Include(x => x.OwnerApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            var vm = new TractorUpsertViewModel
            {
                Id = entity.Id,
                NationalId = entity.NationalId,
                TractorSmartCardNumber = entity.TractorSmartCardNumber,
                FileNumber = entity.FileNumber,
                Status = entity.Status,
                TractorIdentifier = entity.TractorIdentifier,
                MaxLoadCapacity = entity.MaxLoadCapacity,
                TechnicalInspectionExpireDate = entity.TechnicalInspectionExpireDate,
                ThirdPartyInsuranceExpireDate = entity.ThirdPartyInsuranceExpireDate,
                ProductionYear = entity.ProductionYear,
                AxleCount = entity.AxleCount,
                SystemName = entity.SystemName,
                TractorType = entity.TractorType,
                CapacityUnit = entity.CapacityUnit,
                TransitPlateNumber = entity.TransitPlateNumber,
                OwnerApplicationUserId = entity.OwnerApplicationUserId,
                OwnerUserDisplayName = entity.OwnerApplicationUser == null
                    ? null
                    : $"{entity.OwnerApplicationUser.FirstName} {entity.OwnerApplicationUser.LastName} - {entity.OwnerApplicationUser.Email}".Trim()
            };

            SplitPolicePlate(entity.PolicePlateNumber, vm);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Tractors
                .Include(x => x.BankAccounts)
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            _context.Tractors.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "کشنده با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TractorUpsertViewModel vm)
        {
            NormalizeTractorVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = await _context.Tractors
                .FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (entity == null)
                return NotFound();

            entity.PolicePlateNumber = BuildPolicePlateNumber(vm);
            entity.OwnerApplicationUserId = string.IsNullOrWhiteSpace(vm.OwnerApplicationUserId) ? null : vm.OwnerApplicationUserId;
            entity.NationalId = vm.NationalId;
            entity.TractorSmartCardNumber = vm.TractorSmartCardNumber;
            entity.FileNumber = vm.FileNumber;
            entity.Status = vm.Status;
            entity.TractorIdentifier = vm.TractorIdentifier;
            entity.MaxLoadCapacity = vm.MaxLoadCapacity;
            entity.TechnicalInspectionExpireDate = vm.TechnicalInspectionExpireDate;
            entity.ThirdPartyInsuranceExpireDate = vm.ThirdPartyInsuranceExpireDate;
            entity.ProductionYear = vm.ProductionYear;
            entity.AxleCount = vm.AxleCount;
            entity.SystemName = vm.SystemName;
            entity.TractorType = vm.TractorType;
            entity.CapacityUnit = vm.CapacityUnit;
            entity.TransitPlateNumber = vm.TransitPlateNumber;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "کشنده با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _context.Tractors
                .Include(x => x.OwnerApplicationUser)
                .Include(x => x.BankAccounts)
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [HttpGet]
        public IActionResult AddBankAccount(int tractorId)
            => View(new TractorBankAccountUpsertViewModel { TractorId = tractorId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBankAccount(TractorBankAccountUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.IsDefault)
            {
                var currentDefaults = await _context.TractorBankAccounts
                    .Where(x => x.TractorId == vm.TractorId && x.IsDefault)
                    .ToListAsync();

                foreach (var item in currentDefaults)
                    item.IsDefault = false;
            }

            _context.TractorBankAccounts.Add(new TractorBankAccount
            {
                TractorId = vm.TractorId,
                AccountOwnerName = vm.AccountOwnerName,
                BankName = vm.BankName,
                AccountNumber = vm.AccountNumber,
                CardNumber = vm.CardNumber,
                ShebaNumber = vm.ShebaNumber,
                IsDefault = vm.IsDefault,
                Description = vm.Description
            });

            await _context.SaveChangesAsync();
            TempData["Ok"] = "اطلاعات بانکی اضافه شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditBankAccount(int id)
        {
            var entity = await _context.TractorBankAccounts.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new TractorBankAccountUpsertViewModel
            {
                Id = entity.Id,
                TractorId = entity.TractorId,
                AccountOwnerName = entity.AccountOwnerName,
                BankName = entity.BankName,
                AccountNumber = entity.AccountNumber,
                CardNumber = entity.CardNumber,
                ShebaNumber = entity.ShebaNumber,
                IsDefault = entity.IsDefault,
                Description = entity.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBankAccount(TractorBankAccountUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.TractorBankAccounts.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            if (vm.IsDefault)
            {
                var currentDefaults = await _context.TractorBankAccounts
                    .Where(x => x.TractorId == vm.TractorId && x.Id != vm.Id && x.IsDefault)
                    .ToListAsync();

                foreach (var item in currentDefaults)
                    item.IsDefault = false;
            }

            entity.AccountOwnerName = vm.AccountOwnerName;
            entity.BankName = vm.BankName;
            entity.AccountNumber = vm.AccountNumber;
            entity.CardNumber = vm.CardNumber;
            entity.ShebaNumber = vm.ShebaNumber;
            entity.IsDefault = vm.IsDefault;
            entity.Description = vm.Description;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "اطلاعات بانکی ویرایش شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBankAccount(int id, int tractorId)
        {
            var entity = await _context.TractorBankAccounts.FindAsync(id);
            if (entity != null)
            {
                _context.TractorBankAccounts.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "اطلاعات بانکی حذف شد.";
            return RedirectToAction(nameof(Details), new { id = tractorId });
        }

        [HttpGet]
        public IActionResult AddContact(int tractorId)
            => View(new TractorContactUpsertViewModel { TractorId = tractorId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(TractorContactUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _context.TractorContacts.Add(new TractorContact
            {
                TractorId = vm.TractorId,
                Title = vm.Title,
                ContactValue = vm.ContactValue,
                HasSms = vm.HasSms,
                HasWhatsApp = vm.HasWhatsApp,
                IsFax = vm.IsFax,
                IsPhone = vm.IsPhone,
                IsEmail = vm.IsEmail
            });

            await _context.SaveChangesAsync();
            TempData["Ok"] = "راه ارتباطی اضافه شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(int id)
        {
            var entity = await _context.TractorContacts.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new TractorContactUpsertViewModel
            {
                Id = entity.Id,
                TractorId = entity.TractorId,
                Title = entity.Title,
                ContactValue = entity.ContactValue,
                HasSms = entity.HasSms,
                HasWhatsApp = entity.HasWhatsApp,
                IsFax = entity.IsFax,
                IsPhone = entity.IsPhone,
                IsEmail = entity.IsEmail
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(TractorContactUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.TractorContacts.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            entity.Title = vm.Title;
            entity.ContactValue = vm.ContactValue;
            entity.HasSms = vm.HasSms;
            entity.HasWhatsApp = vm.HasWhatsApp;
            entity.IsFax = vm.IsFax;
            entity.IsPhone = vm.IsPhone;
            entity.IsEmail = vm.IsEmail;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "راه ارتباطی ویرایش شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id, int tractorId)
        {
            var entity = await _context.TractorContacts.FindAsync(id);
            if (entity != null)
            {
                _context.TractorContacts.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "شماره تماس حذف شد.";
            return RedirectToAction(nameof(Details), new { id = tractorId });
        }

        [HttpGet]
        public IActionResult AddAddress(int tractorId)
            => View(new TractorAddressUpsertViewModel { TractorId = tractorId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(TractorAddressUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _context.TractorAddresses.Add(new TractorAddress
            {
                TractorId = vm.TractorId,
                Title = vm.Title,
                AddressText = vm.AddressText
            });

            await _context.SaveChangesAsync();
            TempData["Ok"] = "آدرس اضافه شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress(int id)
        {
            var entity = await _context.TractorAddresses.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new TractorAddressUpsertViewModel
            {
                Id = entity.Id,
                TractorId = entity.TractorId,
                Title = entity.Title,
                AddressText = entity.AddressText
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(TractorAddressUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.TractorAddresses.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            entity.Title = vm.Title;
            entity.AddressText = vm.AddressText;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "آدرس ویرایش شد.";
            return RedirectToAction(nameof(Details), new { id = vm.TractorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id, int tractorId)
        {
            var entity = await _context.TractorAddresses.FindAsync(id);
            if (entity != null)
            {
                _context.TractorAddresses.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "آدرس حذف شد.";
            return RedirectToAction(nameof(Details), new { id = tractorId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchOwners(string? term)
        {
            term = (term ?? "").Trim();
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var items = await _context.Users
                .Where(x =>
                    (x.FirstName ?? "").Contains(term) ||
                    (x.LastName ?? "").Contains(term) ||
                    (x.Email ?? "").Contains(term) ||
                    (x.NationalId ?? "").Contains(term))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Take(30)
                .Select(x => new
                {
                    applicationUserId = x.Id,
                    fullName = ((x.FirstName ?? "") + " " + (x.LastName ?? "")).Trim(),
                    email = x.Email,
                    nationalId = x.NationalId
                })
                .ToListAsync();

            return Json(items);
        }

        private static void NormalizeTractorVm(TractorUpsertViewModel vm)
        {
            vm.PlatePartLeft2 = (vm.PlatePartLeft2 ?? "").Trim();
            vm.PlateLetter = (vm.PlateLetter ?? "").Trim();
            vm.PlatePartMiddle3 = (vm.PlatePartMiddle3 ?? "").Trim();
            vm.PlatePartRight2 = (vm.PlatePartRight2 ?? "").Trim();

            vm.OwnerApplicationUserId = string.IsNullOrWhiteSpace(vm.OwnerApplicationUserId) ? null : vm.OwnerApplicationUserId.Trim();
            vm.OwnerUserDisplayName = string.IsNullOrWhiteSpace(vm.OwnerUserDisplayName) ? null : vm.OwnerUserDisplayName.Trim();
            vm.NationalId = string.IsNullOrWhiteSpace(vm.NationalId) ? null : vm.NationalId.Trim();
            vm.TractorSmartCardNumber = string.IsNullOrWhiteSpace(vm.TractorSmartCardNumber) ? null : vm.TractorSmartCardNumber.Trim();
            vm.FileNumber = string.IsNullOrWhiteSpace(vm.FileNumber) ? null : vm.FileNumber.Trim();
            vm.Status = string.IsNullOrWhiteSpace(vm.Status) ? null : vm.Status.Trim();
            vm.TractorIdentifier = string.IsNullOrWhiteSpace(vm.TractorIdentifier) ? null : vm.TractorIdentifier.Trim();
            vm.SystemName = string.IsNullOrWhiteSpace(vm.SystemName) ? null : vm.SystemName.Trim();
            vm.TractorType = string.IsNullOrWhiteSpace(vm.TractorType) ? null : vm.TractorType.Trim();
            vm.TransitPlateNumber = string.IsNullOrWhiteSpace(vm.TransitPlateNumber) ? null : vm.TransitPlateNumber.Trim();
        }

        private static string BuildPolicePlateNumber(TractorUpsertViewModel vm)
        {
            var left2 = (vm.PlatePartLeft2 ?? "").Trim();
            var letter = (vm.PlateLetter ?? "").Trim();
            var middle3 = (vm.PlatePartMiddle3 ?? "").Trim();
            var right2 = (vm.PlatePartRight2 ?? "").Trim();

            return $"ایران {right2} - {middle3} {letter} {left2}";
        }

        private static void SplitPolicePlate(string? plate, TractorUpsertViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return;

            var parts = plate.Split('-', StringSplitOptions.TrimEntries);
            if (parts.Length == 2)
            {
                vm.PlatePartRight2 = parts[0].Replace("ایران ", "").Trim();
                var pleft = parts[1].Trim().Split(' ');
                if (pleft.Length == 3)
                {
                    vm.PlatePartMiddle3 = pleft[0].Trim();
                    vm.PlateLetter = pleft[1].Trim();
                    vm.PlatePartLeft2 = pleft[2].Trim();
                }
            }
        }
    }
}