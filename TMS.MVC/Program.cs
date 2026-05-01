using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Infrastructure;
using TMS.MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole(AppRoles.SystemAdmin));
});
builder.Services.AddHttpClient("Nominatim", client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("TMS-MVC/1.0 (contact: alinsr157@gmail.com)");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    // Ensure HavalehReservationRequests table exists (create if missing) to avoid runtime SQL errors
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var ensureTableSql = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HavalehReservationRequests')
BEGIN
    CREATE TABLE [dbo].[HavalehReservationRequests]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [HavalehId] BIGINT NOT NULL,
        [RequesterUserId] NVARCHAR(450) NOT NULL,
        [RequesterRole] NVARCHAR(50) NOT NULL,
        [ReservedDay] DATETIME2 NOT NULL,
        [Status] NVARCHAR(30) NOT NULL DEFAULT('Pending'),
        [CreatedAt] DATETIME2 NOT NULL DEFAULT(GETUTCDATE())
    );
END";

    await db.Database.ExecuteSqlRawAsync(ensureTableSql);
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}


app.UseAuthentication();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
