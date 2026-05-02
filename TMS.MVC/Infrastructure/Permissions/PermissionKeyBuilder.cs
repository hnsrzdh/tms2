namespace TMS.MVC.Infrastructure.Permissions;

public static class PermissionKeyBuilder
{
    public static string Build(string controllerName, string actionName)
    {
        controllerName = (controllerName ?? string.Empty).Trim();
        actionName = (actionName ?? string.Empty).Trim();
        return $"{controllerName}.{actionName}";
    }
}
