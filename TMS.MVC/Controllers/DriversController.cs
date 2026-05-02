using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    (x.ApplicationUser.FirstName ?? "").Contains(search) ||
                    (x.ApplicationUser.LastName ?? "").Contains(search) ||
                    (x.ApplicationUser.Email ?? "").Contains(search) ||
                    (x.ApplicationUser.NationalId ?? "").Contains(search) ||
                    (x.SmartCardNumber ?? "").Contains(search) ||
                    (x.DrivingLicenseNumber ?? "").Contains(search) ||
                    (x.BlockDescription ?? "").Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.ApplicationUser.FirstName)
                .ThenBy(x => x.ApplicationUser.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DriverIndexItemViewModel
                {
                    Id = x.Id,
                    FullName = (((x.ApplicationUser.FirstName ?? "") + " " + (x.ApplicationUser.LastName ?? "")).Trim() != "")
                        ? ((x.ApplicationUser.FirstName ?? "") + " " + (x.ApplicationUser.LastName ?? "")).Trim()
                        : (x.ApplicationUser.Email ?? ""),
                    NationalId = x.ApplicationUser.NationalId,
                    SmartCardNumber = x.SmartCardNumber,
                    DrivingLicenseNumber = x.DrivingLicenseNumber,
                    IsBlocked = x.IsBlocked
                })
                .ToListAsync();

            return View(new DriverIndexViewModel
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
            return View(new DriverUpsertViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DriverUpsertViewModel vm)
        {
            NormalizeDriverVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == vm.ApplicationUserId);
            if (user == null)
            {
                ModelState.AddModelError(nameof(vm.ApplicationUserId), "کاربر انتخاب‌شده یافت نشد.");
                return View(vm);
            }
            user.NationalId = vm.NationalId;

            var alreadyExists = await _context.DriverProfiles.AnyAsync(x => x.ApplicationUserId == vm.ApplicationUserId);
            if (alreadyExists)
            {
                ModelState.AddModelError(nameof(vm.ApplicationUserId), "برای این کاربر قبلاً راننده تعریف شده است.");
                return View(vm);
            }

            var entity = new DriverProfile
            {
                ApplicationUserId = vm.ApplicationUserId,
                SmartCardNumber = vm.SmartCardNumber,
                DrivingLicenseNumber = vm.DrivingLicenseNumber,
                IsBlocked = vm.IsBlocked,
                BlockDescription = vm.BlockDescription
            };

            _context.DriverProfiles.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "راننده با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            var vm = new DriverUpsertViewModel
            {
                Id = entity.Id,
                ApplicationUserId = entity.ApplicationUserId,
                UserDisplayName = entity.ApplicationUser == null
                    ? null
                    : $"{entity.ApplicationUser.FirstName} {entity.ApplicationUser.LastName} - {entity.ApplicationUser.Email}".Trim(),
                NationalId = entity.ApplicationUser?.NationalId,
                SmartCardNumber = entity.SmartCardNumber,
                DrivingLicenseNumber = entity.DrivingLicenseNumber,
                IsBlocked = entity.IsBlocked,
                BlockDescription = entity.BlockDescription
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DriverUpsertViewModel vm)
        {
            NormalizeDriverVm(vm);

            if (!ModelState.IsValid)
                return View(vm);

            var entity = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (entity == null)
                return NotFound();

            if (entity.ApplicationUser != null)
            {
                entity.ApplicationUser.NationalId = vm.NationalId;
            }

            entity.SmartCardNumber = vm.SmartCardNumber;
            entity.DrivingLicenseNumber = vm.DrivingLicenseNumber;
            entity.IsBlocked = vm.IsBlocked;
            entity.BlockDescription = vm.BlockDescription;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "راننده با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.DriverProfiles
                .Include(x => x.BankAccounts)
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            _context.DriverProfiles.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Ok"] = "راننده با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _context.DriverProfiles
                .Include(x => x.ApplicationUser)
                .Include(x => x.BankAccounts)
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound();

            var assignments = await _context.TractorAssignments
                .Include(x => x.Tractor)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.Havaleh)
                        .ThenInclude(x => x.OriginPlace)
                .Include(x => x.SubHavaleh)
                    .ThenInclude(x => x.DestinationPlace)
                .Where(x => x.DriverProfileId == id)
                .OrderByDescending(x => x.AssignmentDate)
                .ToListAsync();

            var totalAssignments = assignments.Count;

            var activeAssignments = assignments.Count(x =>
                x.Status != AssignmentStatus.Completed &&
                x.Status != AssignmentStatus.Cancelled &&
                x.Status != AssignmentStatus.Unloaded);

            var completedAssignments = assignments.Count(x =>
                x.Status == AssignmentStatus.Completed ||
                x.Status == AssignmentStatus.Unloaded);

            var cancelledAssignments = assignments.Count(x =>
                x.Status == AssignmentStatus.Cancelled);

            var totalAssignedAmount = assignments
                .Where(x => x.Status != AssignmentStatus.Cancelled)
                .Sum(x => x.AssignedCargoAmount ?? 0);

            var totalLoadedAmount = assignments
                .Where(x => x.Status != AssignmentStatus.Cancelled)
                .Sum(x => x.LoadedAmount ?? 0);

            var totalUnloadedAmount = assignments
                .Where(x => x.Status != AssignmentStatus.Cancelled)
                .Sum(x => x.UnloadedAmount ?? 0);

            var totalPaidAmount = assignments
                .Where(x => x.IsSettled && x.SettledTo == "Driver")
                .Sum(x => x.PayableAmount ?? x.FinalFare ?? x.FinancialApprovedAmount ?? 0);

            var depositTransactions = assignments
                .Where(x => x.IsSettled && x.SettledTo == "Driver")
                .Select(x => new DriverWalletTransactionVm
                {
                    Date = x.SettledDate ?? x.AssignmentDate,
                    Type = "واریز به کیف پول",
                    Amount = x.PayableAmount ?? x.FinalFare ?? x.FinancialApprovedAmount ?? 0,
                    Description = $"تسویه حمل حواله {x.SubHavaleh.Havaleh.HavalehNumber}",
                    ReferenceId = x.Id,
                    IsWithdraw = false
                })
                .ToList();

            var withdrawalTransactions = await _context.DriverWalletWithdrawalRequests
                .Where(x => x.DriverProfileId == id &&
                            x.Status == DriverWalletWithdrawalRequestStatus.Paid)
                .Select(x => new DriverWalletTransactionVm
                {
                    Date = x.PaidAt ?? x.RejectedAt ?? x.CreatedAt,
                    Type = "برداشت از کیف پول",
                    Amount = -x.Amount,
                    Description = string.IsNullOrWhiteSpace(x.PaymentReceiptNote)
                        ? "برداشت پرداخت‌شده از کیف پول"
                        : x.PaymentReceiptNote,
                    ReferenceId = x.Id,
                    IsWithdraw = true
                })
                .ToListAsync();

            var walletTransactions = depositTransactions
                .Concat(withdrawalTransactions)
                .OrderByDescending(x => x.Date)
                .ToList();

            ViewBag.Assignments = assignments;
            ViewBag.WalletTransactions = walletTransactions;

            ViewBag.TotalAssignments = totalAssignments;
            ViewBag.ActiveAssignments = activeAssignments;
            ViewBag.CompletedAssignments = completedAssignments;
            ViewBag.CancelledAssignments = cancelledAssignments;
            ViewBag.TotalAssignedAmount = totalAssignedAmount;
            ViewBag.TotalLoadedAmount = totalLoadedAmount;
            ViewBag.TotalUnloadedAmount = totalUnloadedAmount;
            ViewBag.TotalPaidAmount = totalPaidAmount;

            return View(entity);
        }

        [HttpGet]
        public IActionResult AddBankAccount(int driverProfileId)
            => View(new DriverBankAccountUpsertViewModel { DriverProfileId = driverProfileId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBankAccount(DriverBankAccountUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.IsDefault)
            {
                var currentDefaults = await _context.DriverBankAccounts
                    .Where(x => x.DriverProfileId == vm.DriverProfileId && x.IsDefault)
                    .ToListAsync();

                foreach (var item in currentDefaults)
                    item.IsDefault = false;
            }

            _context.DriverBankAccounts.Add(new DriverBankAccount
            {
                DriverProfileId = vm.DriverProfileId,
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
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpGet]
        public async Task<IActionResult> EditBankAccount(int id)
        {
            var entity = await _context.DriverBankAccounts.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new DriverBankAccountUpsertViewModel
            {
                Id = entity.Id,
                DriverProfileId = entity.DriverProfileId,
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
        public async Task<IActionResult> EditBankAccount(DriverBankAccountUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.DriverBankAccounts.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            if (vm.IsDefault)
            {
                var currentDefaults = await _context.DriverBankAccounts
                    .Where(x => x.DriverProfileId == vm.DriverProfileId && x.Id != vm.Id && x.IsDefault)
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
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBankAccount(int id, int driverProfileId)
        {
            var entity = await _context.DriverBankAccounts.FindAsync(id);
            if (entity != null)
            {
                _context.DriverBankAccounts.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "اطلاعات بانکی حذف شد.";
            return RedirectToAction(nameof(Details), new { id = driverProfileId });
        }

        [HttpGet]
        public IActionResult AddContact(int driverProfileId)
            => View(new DriverContactUpsertViewModel { DriverProfileId = driverProfileId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(DriverContactUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _context.DriverContacts.Add(new DriverContact
            {
                DriverProfileId = vm.DriverProfileId,
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
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(int id)
        {
            var entity = await _context.DriverContacts.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new DriverContactUpsertViewModel
            {
                Id = entity.Id,
                DriverProfileId = entity.DriverProfileId,
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
        public async Task<IActionResult> EditContact(DriverContactUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.DriverContacts.FindAsync(vm.Id);
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
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id, int driverProfileId)
        {
            var entity = await _context.DriverContacts.FindAsync(id);
            if (entity != null)
            {
                _context.DriverContacts.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "راه ارتباطی حذف شد.";
            return RedirectToAction(nameof(Details), new { id = driverProfileId });
        }

        [HttpGet]
        public IActionResult AddAddress(int driverProfileId)
            => View(new DriverAddressUpsertViewModel { DriverProfileId = driverProfileId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(DriverAddressUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _context.DriverAddresses.Add(new DriverAddress
            {
                DriverProfileId = vm.DriverProfileId,
                Title = vm.Title,
                AddressText = vm.AddressText
            });

            await _context.SaveChangesAsync();
            TempData["Ok"] = "آدرس اضافه شد.";
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress(int id)
        {
            var entity = await _context.DriverAddresses.FindAsync(id);
            if (entity == null) return NotFound();

            return View(new DriverAddressUpsertViewModel
            {
                Id = entity.Id,
                DriverProfileId = entity.DriverProfileId,
                Title = entity.Title,
                AddressText = entity.AddressText
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(DriverAddressUpsertViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.DriverAddresses.FindAsync(vm.Id);
            if (entity == null) return NotFound();

            entity.Title = vm.Title;
            entity.AddressText = vm.AddressText;

            await _context.SaveChangesAsync();
            TempData["Ok"] = "آدرس ویرایش شد.";
            return RedirectToAction(nameof(Details), new { id = vm.DriverProfileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id, int driverProfileId)
        {
            var entity = await _context.DriverAddresses.FindAsync(id);
            if (entity != null)
            {
                _context.DriverAddresses.Remove(entity);
                await _context.SaveChangesAsync();
            }

            TempData["Ok"] = "آدرس حذف شد.";
            return RedirectToAction(nameof(Details), new { id = driverProfileId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string? term)
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

        private static void NormalizeDriverVm(DriverUpsertViewModel vm)
        {
            vm.ApplicationUserId = (vm.ApplicationUserId ?? "").Trim();
            vm.UserDisplayName = string.IsNullOrWhiteSpace(vm.UserDisplayName) ? null : vm.UserDisplayName.Trim();
            vm.NationalId = string.IsNullOrWhiteSpace(vm.NationalId) ? null : vm.NationalId.Trim();
            vm.SmartCardNumber = string.IsNullOrWhiteSpace(vm.SmartCardNumber) ? null : vm.SmartCardNumber.Trim();
            vm.DrivingLicenseNumber = string.IsNullOrWhiteSpace(vm.DrivingLicenseNumber) ? null : vm.DrivingLicenseNumber.Trim();
            vm.BlockDescription = string.IsNullOrWhiteSpace(vm.BlockDescription) ? null : vm.BlockDescription.Trim();
        }
    }
}