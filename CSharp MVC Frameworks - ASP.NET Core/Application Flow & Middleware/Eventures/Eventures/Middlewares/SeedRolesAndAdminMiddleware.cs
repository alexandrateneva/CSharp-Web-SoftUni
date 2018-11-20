namespace Eventures.Middlewares
{
    using System;
    using System.Threading.Tasks;
    using Eventures.Data;
    using Eventures.Loggers;
    using Eventures.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    public class SeedRolesAndAdminMiddleware
    {
        private readonly RequestDelegate next;

        public SeedRolesAndAdminMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        
        public async Task InvokeAsync(
            HttpContext httpContext,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            bool hasAdminRole = await roleManager.RoleExistsAsync("Admin");
            if (!hasAdminRole)
            {
                var role = new IdentityRole { Name = "Admin" };
                await roleManager.CreateAsync(role);

                var user = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    FirstName = "Administrator",
                    LastName = "Administrator",
                    UniqueCitizenNumber = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(user, "Admin123456");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            bool hasUserRole = await roleManager.RoleExistsAsync("User");
            if (!hasUserRole)
            {
                var role = new IdentityRole { Name = "User" };
                await roleManager.CreateAsync(role);
            }

            loggerFactory.AddColoredConsoleLogger(c =>
            {
                c.LogLevel = LogLevel.Information;
                c.Color = ConsoleColor.Yellow;
                c.Message = "Roles and admin user were added to the database.";
            });

            await this.next(httpContext);
        }
    }
}
