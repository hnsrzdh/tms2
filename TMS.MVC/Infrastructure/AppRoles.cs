namespace TMS.MVC.Infrastructure;

public static class AppRoles
{
    public const string SystemAdmin = "SystemAdmin";
    public const string OperationsManager = "OperationsManager";
    public const string FinanceManager = "FinanceManager";
    public const string OperationsUser = "OperationsUser";
    public const string FinanceUser = "FinanceUser";
    public const string CargoOwner = "CargoOwner";
    public const string TransportContractor = "TransportContractor";
    public const string TankerOwner = "TankerOwner";
    public const string Driver = "Driver";
    public const string User = "User";

    public static readonly string[] All =
    {
        SystemAdmin, OperationsManager, FinanceManager,
        OperationsUser, FinanceUser,
        CargoOwner, TransportContractor, TankerOwner,
        Driver, User
    };
}