using System.Threading.Tasks;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GrabNReadApp.Web.Middlewares
{
    public class SeedRolesAndAdminMiddleware
    {
        private readonly RequestDelegate next;

        public SeedRolesAndAdminMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            GrabNReadAppContext context,
            UserManager<GrabNReadAppUser> userManager,
            SignInManager<GrabNReadAppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            bool hasAdminRole = await roleManager.RoleExistsAsync("Admin");
            if (!hasAdminRole)
            {
                var role = new IdentityRole { Name = "Admin" };
                await roleManager.CreateAsync(role);

                var user = new GrabNReadAppUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    FirstName = "Administrator",
                    LastName = "Administrator",
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
            
            await this.next(httpContext);
        }
    }
}
