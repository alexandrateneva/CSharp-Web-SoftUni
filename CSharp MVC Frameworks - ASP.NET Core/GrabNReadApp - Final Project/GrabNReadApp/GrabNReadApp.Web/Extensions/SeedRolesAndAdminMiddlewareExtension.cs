using GrabNReadApp.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace GrabNReadApp.Web.Extensions
{
    public static class SeedRolesAndAdminMiddlewareExtension
    {
        public static IApplicationBuilder UseSeedRolesAndAdminMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedRolesAndAdminMiddleware>();
        }
    }
}
