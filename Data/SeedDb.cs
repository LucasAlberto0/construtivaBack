
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection; // Added for IServiceProvider

namespace construtivaBack.Data;

public static class SeedDb
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { "Admin", "Coordenador", "Fiscal" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create default admin user
        string adminEmail = "admin@system.com";
        string adminPassword = "Admin@123";

        ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminEmail,
                NomeCompleto = "Administrador do Sistema",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Ensure the existing admin user has the Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
