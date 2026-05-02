using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;

namespace TMS.MVC.Infrastructure.Permissions;

public class PermissionSyncResult
{
    public int TotalActions { get; set; }
    public int Added { get; set; }
    public int Updated { get; set; }
}

public class PermissionSyncService
{
    private readonly ApplicationDbContext _context;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;

    public PermissionSyncService(
        ApplicationDbContext context,
        IActionDescriptorCollectionProvider actionDescriptorProvider)
    {
        _context = context;
        _actionDescriptorProvider = actionDescriptorProvider;
    }

    public async Task<PermissionSyncResult> SyncAsync()
    {
        var actions = _actionDescriptorProvider.ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .Where(ShouldCreatePermissionForAction)
            .Select(x => new PermissionDefinition
            {
                Key = PermissionKeyBuilder.Build(x.ControllerName, x.ActionName),
                Category = GetControllerTitle(x.ControllerName),
                Title = BuildActionTitle(x.ControllerName, x.ActionName)
            })
            .GroupBy(x => x.Key)
            .Select(x => x.First())
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Title)
            .ToList();

        var existing = await _context.PermissionDefinitions.ToListAsync();
        var existingByKey = existing.ToDictionary(x => x.Key, x => x);

        var result = new PermissionSyncResult
        {
            TotalActions = actions.Count
        };

        foreach (var permission in actions)
        {
            if (!existingByKey.TryGetValue(permission.Key, out var current))
            {
                _context.PermissionDefinitions.Add(permission);
                result.Added++;
                continue;
            }

            var changed = false;

            if (current.Title != permission.Title)
            {
                current.Title = permission.Title;
                changed = true;
            }

            if (current.Category != permission.Category)
            {
                current.Category = permission.Category;
                changed = true;
            }

            if (changed)
                result.Updated++;
        }

        await _context.SaveChangesAsync();
        return result;
    }

    private static bool ShouldCreatePermissionForAction(ControllerActionDescriptor descriptor)
    {
        if (descriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            return false;

        if (descriptor.EndpointMetadata.Any(x => x is SkipPermissionAttribute))
            return false;

        if (descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            return false;

        if (descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(SkipPermissionAttribute), true).Any())
            return false;

        if (descriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            return false;

        if (descriptor.MethodInfo.GetCustomAttributes(typeof(SkipPermissionAttribute), true).Any())
            return false;

        if (descriptor.MethodInfo.GetCustomAttributes(typeof(NonActionAttribute), true).Any())
            return false;

        // اکشن‌های خطا و صفحات عمومی را Permission نکن.
        if (descriptor.ControllerName.Equals("Home", StringComparison.OrdinalIgnoreCase) &&
            descriptor.ActionName.Equals("Error", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    private static string BuildActionTitle(string controllerName, string actionName)
    {
        var category = GetControllerTitle(controllerName);
        var actionTitle = GetActionTitle(actionName);
        return $"{actionTitle} - {category}";
    }

    private static string GetActionTitle(string actionName)
    {
        return actionName switch
        {
            "Index" => "مشاهده لیست",
            "Details" => "مشاهده جزئیات",
            "Create" => "ایجاد",
            "Edit" => "ویرایش",
            "Delete" => "حذف",
            "DeleteConfirmed" => "تایید حذف",
            "ChangePassword" => "تغییر رمز عبور",
            "Permissions" => "مدیریت دسترسی‌های کاربر",
            "SyncPermissions" => "همگام‌سازی دسترسی‌ها",
            "Approve" => "تایید",
            "Reject" => "رد",
            "Cancel" => "لغو",
            "Request" => "ثبت درخواست",
            "Pay" => "ثبت پرداخت",
            "MarkAsPaid" => "ثبت پرداخت",
            "AddContact" => "افزودن راه ارتباطی",
            "EditContact" => "ویرایش راه ارتباطی",
            "DeleteContact" => "حذف راه ارتباطی",
            "AddAddress" => "افزودن آدرس",
            "EditAddress" => "ویرایش آدرس",
            "DeleteAddress" => "حذف آدرس",
            "AddBankAccount" => "افزودن حساب بانکی",
            "EditBankAccount" => "ویرایش حساب بانکی",
            "DeleteBankAccount" => "حذف حساب بانکی",
            "AddSubHavaleh" => "افزودن زیرحواله",
            "EditSubHavaleh" => "ویرایش زیرحواله",
            "DeleteSubHavaleh" => "حذف زیرحواله",
            "SubHavalehDetails" => "جزئیات زیرحواله",
            "Assign" => "تخصیص",
            "Loading" => "ثبت بارگیری",
            "Unloading" => "ثبت تخلیه",
            "Complete" => "تکمیل",
            "Settle" => "تسویه",
            "Daily" => "گزارش روزانه",
            "Weekly" => "گزارش هفتگی",
            "Monthly" => "گزارش ماهیانه",
            "Yearly" => "گزارش سالیانه",
            _ => actionName
        };
    }

    private static string GetControllerTitle(string controllerName)
    {
        return controllerName switch
        {
            "Home" => "داشبورد",
            "SystemDashboard" => "داشبورد مدیریت",
            "Users" => "کاربران",
            "LegalEntities" => "اشخاص حقوقی",
            "Products" => "محصولات",
            "Regions" => "مناطق",
            "Places" => "مکان‌ها",
            "TransportAgreements" => "تفاهم‌نامه‌های حمل",
            "Havalehs" => "حواله‌ها",
            "OpenSubHavalehs" => "درخواست‌های حمل",
            "AssignmentRequests" => "درخواست‌های تخصیص",
            "CargoAnnouncements" => "سامانه اعلام بار",
            "Drivers" => "رانندگان",
            "Tractors" => "کشنده‌ها",
            "TractorAssignment" => "تخصیص کشنده",
            "FleetTracking" => "پیگیری ناوگان",
            "LoadingSchedule" => "برنامه زمانی بارگیری",
            "DriverWalletWithdrawalRequests" => "درخواست برداشت رانندگان",
            "TractorWalletWithdrawalRequests" => "درخواست برداشت کشنده‌ها",
            "Tickets" => "تیکت‌های من",
            "TicketOperator" => "مدیریت تیکت‌ها",
            "ManagementReports" => "گزارش‌های مدیریتی",
            _ => controllerName
        };
    }
}
