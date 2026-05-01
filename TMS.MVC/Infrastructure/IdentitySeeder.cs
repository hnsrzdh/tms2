using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Models;

namespace TMS.MVC.Infrastructure;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();


        // 1) Roles
        foreach (var role in AppRoles.All)
        {
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new IdentityRole(role));
        }

        // 2) First admin (این‌ها را با تنظیمات خودت ست کن)
        var adminEmail = "admin@tms.local";
        var adminPass = "Admin123!";

        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var create = await userMgr.CreateAsync(admin, adminPass);
            if (!create.Succeeded) return;
        }

        if (!await userMgr.IsInRoleAsync(admin, AppRoles.SystemAdmin))
            await userMgr.AddToRoleAsync(admin, AppRoles.SystemAdmin);

        // اگر دوست داری همه کاربرها حداقل "User" باشند:
        if (!await userMgr.IsInRoleAsync(admin, AppRoles.User))
            await userMgr.AddToRoleAsync(admin, AppRoles.User);

        // Seed default ticket categories if none exist
        try
        {
            var db = sp.GetRequiredService<TMS.MVC.Data.ApplicationDbContext>();
            if (!await db.TicketCategories.AnyAsync(c => true))
            {
                var defaults = new[] {
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "پشتیبانی فنی", IsActive = true, SortOrder = 10 },
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "درخواست ویژگی", IsActive = true, SortOrder = 20 },
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "گزارش باگ", IsActive = true, SortOrder = 30 },
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "پیشنهادات", IsActive = true, SortOrder = 40 },
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "مسائل مالی", IsActive = true, SortOrder = 50 },
                    new TMS.MVC.Models.Tickets.TicketCategory { Title = "سایر", IsActive = true, SortOrder = 999 }
                };
                db.TicketCategories.AddRange(defaults);
                await db.SaveChangesAsync();
            }
        }
        catch
        {
            // ignore seeding errors to not block app startup
        }
    }
}