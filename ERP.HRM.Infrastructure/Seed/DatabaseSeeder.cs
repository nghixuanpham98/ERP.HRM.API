using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<User> userManager)
        {
            foreach (var role in RoleConstants.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }

            const string adminUsername = "admin";
            const string adminPassword = "Admin@123456";

            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUsername,
                    Email = "admin@erp.com",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }
    }
}
