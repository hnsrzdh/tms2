using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.ViewModels;

public class UserCreateViewModel
{
    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
    public string Email { get; set; } = null!;

    [Display(Name = "نام")]
    [Required(ErrorMessage = "وارد کردن نام الزامی است.")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن یکسان نیست.")]
    public string ConfirmPassword { get; set; } = null!;

    public List<string> SelectedRoles { get; set; } = new();
    public List<RoleSelectionItemVm> AvailableRoles { get; set; } = new();
}

public class UserIndexItemViewModel
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}
public class UserIndexViewModel
{
    public List<UserIndexItemViewModel> Items { get; set; } = new();

    public string? Search { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public int RowNumberStart => TotalItems == 0 ? 0 : ((Page - 1) * PageSize) + 1;
}

public class UserEditViewModel
{
    public string Id { get; set; } = null!;

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
    public string Email { get; set; } = null!; // فقط نمایش

    [Display(Name = "نام")]
    [Required(ErrorMessage = "وارد کردن نام الزامی است.")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است.")]
    public string? LastName { get; set; }

    public List<string> SelectedRoles { get; set; } = new();
    public List<RoleSelectionItemVm> AvailableRoles { get; set; } = new();
}
public class RoleSelectionItemVm
{
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool Selected { get; set; }
}
public class ChangePasswordViewModel
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "وارد کردن رمز عبور جدید الزامی است.")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور جدید")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "تکرار رمز عبور جدید الزامی است.")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور و تکرار آن یکسان نیست.")]
    [Display(Name = "تکرار رمز عبور جدید")]
    public string ConfirmNewPassword { get; set; } = null!;
}
public class UserPermissionsEditViewModel
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }

    public List<UserPermissionCategoryVm> Categories { get; set; } = new();
}

public class UserPermissionCategoryVm
{
    public string Category { get; set; } = null!;
    public List<UserPermissionItemVm> Items { get; set; } = new();
}

public class UserPermissionItemVm
{
    public int PermissionDefinitionId { get; set; }
    public string Key { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool Selected { get; set; }
}
public class PermissionSyncResultViewModel
{
    public int TotalActions { get; set; }
    public int Added { get; set; }
    public int Updated { get; set; }
}
