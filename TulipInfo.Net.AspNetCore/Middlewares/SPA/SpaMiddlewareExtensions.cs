using TulipInfo.Net.AspNetCore;
using System.Globalization;

namespace Microsoft.AspNetCore.Builder
{
    public static class SpaMiddlewareExtensions
    {
        public static IApplicationBuilder UseSpa(
            this IApplicationBuilder builder)
        {
            return builder.UseSpa(new SpaMiddlewareOptions());
        }

        public static IApplicationBuilder UseSpa(
            this IApplicationBuilder builder, SpaMiddlewareOptions options)
        {
            return builder.UseMiddleware<SpaMiddleware>(options)
                .UseStaticFiles();
        }
    }
}
