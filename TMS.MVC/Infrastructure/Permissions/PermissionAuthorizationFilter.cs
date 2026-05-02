using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Authorization;
using TMS.MVC.Models;

namespace TMS.MVC.Infrastructure.Permissions;

public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPermissionAccessService _permissionAccessService;

    public PermissionAuthorizationFilter(
        UserManager<ApplicationUser> userManager,
        IPermissionAccessService permissionAccessService)
    {
        _userManager = userManager;
        _permissionAccessService = permissionAccessService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (ShouldSkip(context))
            return;

        if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
            return;

        if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var user = await _userManager.GetUserAsync(context.HttpContext.User);
        if (user == null || !user.IsActive)
        {
            context.Result = new ForbidResult();
            return;
        }

        var roles = await _userManager.GetRolesAsync(user);

        // ادمین سیستم همیشه دسترسی کامل دارد تا هیچ‌وقت از مدیریت Permissionها قفل نشود.
        if (roles.Contains("SystemAdmin"))
            return;

        var permissionKey = PermissionKeyBuilder.Build(descriptor.ControllerName, descriptor.ActionName);
        var hasPermission = await _permissionAccessService.HasPermissionAsync(user.Id, roles, permissionKey);

        if (!hasPermission)
            context.Result = new ForbidResult();
    }

    private static bool ShouldSkip(AuthorizationFilterContext context)
    {
        if (context.Filters.Any(x => x is IAllowAnonymousFilter))
            return true;

        if (context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            return true;

        if (context.ActionDescriptor.EndpointMetadata.Any(x => x is SkipPermissionAttribute))
            return true;

        if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
        {
            var controllerHasSkip = descriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(SkipPermissionAttribute), true)
                .Any();

            var actionHasSkip = descriptor.MethodInfo
                .GetCustomAttributes(typeof(SkipPermissionAttribute), true)
                .Any();

            if (controllerHasSkip || actionHasSkip)
                return true;
        }

        return false;
    }
}
