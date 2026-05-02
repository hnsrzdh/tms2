using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;

namespace TMS.MVC.Infrastructure.Permissions;

public interface IPermissionAccessService
{
    Task<bool> HasPermissionAsync(string userId, IEnumerable<string> roleNames, string permissionKey);
}

public class PermissionAccessService : IPermissionAccessService
{
    private readonly ApplicationDbContext _context;

    public PermissionAccessService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(string userId, IEnumerable<string> roleNames, string permissionKey)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(permissionKey))
            return false;

        var permission = await _context.PermissionDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Key == permissionKey);

        if (permission == null)
            return false;

        var hasDirectUserPermission = await _context.UserPermissions
            .AsNoTracking()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.PermissionDefinitionId == permission.Id &&
                x.IsGranted);

        if (hasDirectUserPermission)
            return true;

        var roles = roleNames
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        if (!roles.Any())
            return false;

        var hasRolePermission = await _context.RolePermissions
            .AsNoTracking()
            .AnyAsync(x =>
                roles.Contains(x.RoleName) &&
                x.PermissionDefinitionId == permission.Id &&
                x.IsGranted);

        return hasRolePermission;
    }
}
