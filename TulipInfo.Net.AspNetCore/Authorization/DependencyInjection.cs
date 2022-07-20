using TulipInfo.Net.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionAuthorizationDependencyInjection
    {
        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
        {
            return services.AddPermissionAuthorization((options) =>
            {

            });
        }

        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services,Action<PermissionAuthorizationOptions> setupAction)
        {
            services.Configure(setupAction);
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            return services;
        }
    }
}
