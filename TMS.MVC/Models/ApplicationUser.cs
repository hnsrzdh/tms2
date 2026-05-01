using Microsoft.AspNetCore.Identity;

namespace TMS.MVC.Models;
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? NationalId {  get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? FullName { get { return FirstName! + " " + LastName!; } }
}

public class PermissionDefinition
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;   // مثلا Users.View
    public string Title { get; set; } = null!; // مثلا مشاهده کاربران
    public string? Category { get; set; }      // مثلا کاربران
}
public class UserPermission
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int PermissionDefinitionId { get; set; }
    public PermissionDefinition PermissionDefinition { get; set; } = null!;

    public bool IsGranted { get; set; } = true;
}
public class RolePermission
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public int PermissionDefinitionId { get; set; }
    public PermissionDefinition PermissionDefinition { get; set; } = null!;

    public bool IsGranted { get; set; } = true;
}