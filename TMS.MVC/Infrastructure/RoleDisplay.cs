namespace TMS.MVC.Infrastructure;

public static class RoleDisplay
{
    public static readonly Dictionary<string, string> Fa = new()
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
        [AppRoles.User] = "کاربر عادی",
    };
}