namespace Eventures.Extensions
{
    using Eventures.Middlewares;
    using Microsoft.AspNetCore.Builder;

    public static class SeedRolesAndAdminMiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedRolesAndAdminMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedRolesAndAdminMiddleware>();
        }

    }
}
