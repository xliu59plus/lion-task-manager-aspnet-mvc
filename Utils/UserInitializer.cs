using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace LionTaskManagementApp.Utils
{
    public static class UserInitializer
    {
        public static async Task Initialize(UserManager<TaskUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles
            var roles = new[] { "Admin", "ViceAdmin", "Inactive_Poster", "Poster", "Inactive_Taker", "Taker" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user
            var adminEmail = "admin@mgmt.com";
            var adminPassword = "admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new TaskUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // Log errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }
        }
    }
}
