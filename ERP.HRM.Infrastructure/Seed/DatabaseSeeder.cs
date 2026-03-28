using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
            UserManager<User> userManager,
            ILogger logger)
        {
            logger.LogInformation("Starting database seeding...");

            foreach (var role in RoleConstants.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    logger.LogInformation("Creating role: {RoleName}", role);
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
                else
                {
                    logger.LogInformation("Role '{RoleName}' already exists", role);
                }
            }

            const string adminUsername = "admin";
            const string adminPassword = "Admin@123456";

            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                logger.LogInformation("Creating admin user: {Username}", adminUsername);
                adminUser = new User
                {
                    UserName = adminUsername,
                    Email = "admin@erp.com",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    logger.LogInformation("Admin user '{Username}' created successfully", adminUsername);
                    await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
                    logger.LogInformation("Admin role assigned to user '{Username}'", adminUsername);
                }
                else
                {
                    logger.LogError("Failed to create admin user. Errors: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Admin user '{Username}' already exists", adminUsername);
            }

            logger.LogInformation("Database seeding completed");
        }
    }
}
