using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Infrastructure;
using TMS.MVC.Models;
using TMS.MVC.Models.ViewModels;

[Authorize]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x =>
                x.Email!.Contains(search) ||
                (x.FirstName != null && x.FirstName.Contains(search)) ||
                (x.LastName != null && x.LastName.Contains(search)));
        }

        var totalItems = await query.CountAsync();

        var users = await query
            .OrderBy(x => x.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = new List<UserIndexItemViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var rolesTitles =await BuildRoleStringAsync(roles.OrderBy(x => x).ToList());
            items.Add(new UserIndexItemViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Roles = rolesTitles
            });
        }

        var vm = new UserIndexViewModel
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
    public async Task<IActionResult> Create()
    {
        var vm = new UserCreateViewModel
        {
            AvailableRoles = await BuildRoleSelectionItemsAsync()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel vm)
    {
        vm.AvailableRoles = await BuildRoleSelectionItemsAsync(vm.SelectedRoles);

        if (!ModelState.IsValid)
            return View(vm);

        var email = vm.Email.Trim().ToLower();

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(vm.Email), "این ایمیل قبلا ثبت شده است.");
            return View(vm);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = email.ToUpper(),
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(vm);
        }
        if (!vm.SelectedRoles.Contains(AppRoles.User))
        {
            vm.SelectedRoles.Add(AppRoles.User);
        }
        if (vm.SelectedRoles.Any())
        {
            var validRoles = AppRoles.All.Intersect(vm.SelectedRoles).ToList();
            if (validRoles.Any())
                await _userManager.AddToRolesAsync(user, validRoles);
        }

        TempData["SuccessMessage"] = "کاربر با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);

        var vm = new UserEditViewModel
        {
            Id = user.Id,
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            SelectedRoles = userRoles.ToList(),
            AvailableRoles = await BuildRoleSelectionItemsAsync(userRoles.ToList())
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditViewModel vm)
    {
        vm.AvailableRoles = await BuildRoleSelectionItemsAsync(vm.SelectedRoles);

        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.FindByIdAsync(vm.Id);
        if (user == null)
            return NotFound();

        user.FirstName = vm.FirstName;
        user.LastName = vm.LastName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(vm);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = AppRoles.All.Intersect(vm.SelectedRoles).ToList();

        var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(selectedRoles).ToList();

        if (rolesToRemove.Any())
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        if (rolesToAdd.Any())
            await _userManager.AddToRolesAsync(user, rolesToAdd);

        TempData["SuccessMessage"] = "اطلاعات کاربر با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(string id, int page = 1, int pageSize = 10, string? search = null)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        user.IsActive = !user.IsActive;
        await _userManager.UpdateAsync(user);

        TempData["SuccessMessage"] = user.IsActive
            ? "کاربر فعال شد."
            : "کاربر غیرفعال شد.";

        return RedirectToAction(nameof(Index), new { page, pageSize, search });
    }
    [HttpGet]
    public async Task<IActionResult> ChangePassword(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var vm = new ChangePasswordViewModel
        {
            UserId = user.Id,
            Email = user.Email ?? ""
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.FindByIdAsync(vm.UserId);
        if (user == null)
            return NotFound();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, vm.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(vm);
        }

        TempData["SuccessMessage"] = "رمز عبور کاربر با موفقیت تغییر یافت.";
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Permissions(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var allPermissions = await _context.PermissionDefinitions
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Title)
            .ToListAsync();

        var userPermissionIds = await _context.UserPermissions
            .Where(x => x.UserId == id && x.IsGranted)
            .Select(x => x.PermissionDefinitionId)
            .ToListAsync();

        var vm = new UserPermissionsEditViewModel
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Categories = allPermissions
                .GroupBy(x => x.Category ?? "عمومی")
                .Select(g => new UserPermissionCategoryVm
                {
                    Category = g.Key,
                    Items = g.Select(p => new UserPermissionItemVm
                    {
                        PermissionDefinitionId = p.Id,
                        Key = p.Key,
                        Title = p.Title,
                        Selected = userPermissionIds.Contains(p.Id)
                    }).ToList()
                }).ToList()
        };

        return View(vm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Permissions(UserPermissionsEditViewModel vm)
    {
        var user = await _userManager.FindByIdAsync(vm.UserId);
        if (user == null)
            return NotFound();

        var selectedPermissionIds = vm.Categories
            .SelectMany(c => c.Items)
            .Where(x => x.Selected)
            .Select(x => x.PermissionDefinitionId)
            .Distinct()
            .ToList();

        var existing = await _context.UserPermissions
            .Where(x => x.UserId == vm.UserId)
            .ToListAsync();

        _context.UserPermissions.RemoveRange(existing);

        var newItems = selectedPermissionIds.Select(pid => new UserPermission
        {
            UserId = vm.UserId,
            PermissionDefinitionId = pid,
            IsGranted = true
        });

        await _context.UserPermissions.AddRangeAsync(newItems);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "دسترسی‌های کاربر ذخیره شد.";
        return RedirectToAction(nameof(Index));
    }
    private Task<List<RoleSelectionItemVm>> BuildRoleSelectionItemsAsync(IEnumerable<string>? selectedRoles = null)
    {
        selectedRoles ??= Enumerable.Empty<string>();
        var selectedSet = selectedRoles.ToHashSet();

        var roleTitles = new Dictionary<string, string>
        {
            [AppRoles.SystemAdmin] = "ادمین سیستم",
            [AppRoles.OperationsManager] = "مدیر عملیات",
            [AppRoles.FinanceManager] = "مدیر مالی",
            [AppRoles.OperationsUser] = "کاربر عملیات",
            [AppRoles.FinanceUser] = "کاربر مالی",
            [AppRoles.CargoOwner] = "صاحب کالا",
            [AppRoles.TransportContractor] = "پیمانکار حمل",
            [AppRoles.TankerOwner] = "مالک تانکر",
            [AppRoles.Driver] = "راننده",
            [AppRoles.User] = "کاربر"
        };

        var list = AppRoles.All
            .Select(x => new RoleSelectionItemVm
            {
                Name = x,
                Title = roleTitles.ContainsKey(x) ? roleTitles[x] : x,
                Selected = selectedSet.Contains(x)
            })
            .ToList();

        return Task.FromResult(list);
    }
    private Task<List<string>> BuildRoleStringAsync(IEnumerable<string>? selectedRoles = null)
    {
        selectedRoles ??= Enumerable.Empty<string>();
        var selectedSet = selectedRoles.ToList();

        var roleTitles = new Dictionary<string, string>
        {
            [AppRoles.SystemAdmin] = "ادمین سیستم",
            [AppRoles.OperationsManager] = "مدیر عملیات",
            [AppRoles.FinanceManager] = "مدیر مالی",
            [AppRoles.OperationsUser] = "کاربر عملیات",
            [AppRoles.FinanceUser] = "کاربر مالی",
            [AppRoles.CargoOwner] = "صاحب کالا",
            [AppRoles.TransportContractor] = "پیمانکار حمل",
            [AppRoles.TankerOwner] = "مالک تانکر",
            [AppRoles.Driver] = "راننده",
            [AppRoles.User] = "کاربر"
        };
        var list = new List<string>();
        for (int i = 0; i < selectedSet.Count; i++)
        {
            list.Add(roleTitles.ContainsKey(selectedSet[i]) ? roleTitles[selectedSet[i]] : selectedSet[i]);
        }

        return Task.FromResult(list);
    }




}