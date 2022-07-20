using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TulipInfo.Net.AspNetCore.Authorization;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        PermissionAuthorizationOptions _options;
        public PermissionHandler(IOptions<PermissionAuthorizationOptions> options)
        {
            _options = options.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity !=null && context.User.Identity.IsAuthenticated)
            {
                string[] permissions = requirement.AllowedPermissions;
                if (requirement.CheckAll)
                {
                    if(context.User.HasAllPermissions(permissions,_options.AdministratorRoleName,_options.PermissionClaimType))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if(context.User.HasAnyPermissions(permissions, _options.AdministratorRoleName, _options.PermissionClaimType))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
