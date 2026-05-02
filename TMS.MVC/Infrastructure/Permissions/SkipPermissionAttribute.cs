using System;

namespace TMS.MVC.Infrastructure.Permissions;

/// <summary>
/// این Attribute برای اکشن‌ها یا کنترلرهایی است که نباید با سیستم Permission کنترل شوند.
/// مثال: صفحات عمومی، Error، AccessDenied، Logout، یا اکشن‌های داخلی Ajax که خودتان می‌خواهید آزاد باشند.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class SkipPermissionAttribute : Attribute
{
}
