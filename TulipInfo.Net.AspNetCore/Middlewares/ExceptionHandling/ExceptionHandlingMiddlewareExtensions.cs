using TulipInfo.Net.AspNetCore;
using System.Globalization;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
