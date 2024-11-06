using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
// Stripe: besk-hcwu-cqub-khtj-cfvq
// pk_test_51QEfHKG49cs2m96oj7OFeu4vsXpX5qEdCUZOMpsLrNvE2TuohpnLBkB0uUNlv5MF3GAvaKUkp6Trb1LE6ABRxJEl00F2k9VvhM
// Add services to the container.
// Google Maps JavaScript API key: AIzaSyBdkv4tRPCXtSDaqbRpQLQ6QjER5zIAagg
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<TaskUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// builder.Services.AddHttpsRedirection(options =>
// {
//     options.HttpsPort = 7227;
// });

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

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey=builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var roleName  
 in new[] { "Admin", "ViceAdmin", "Inactive_Poster", "Poster", "Inactive_Taker", "Taker" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

app.Run();
