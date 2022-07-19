using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Identity !=null && context.User.Identity.IsAuthenticated)
            {
                string[] permissions = requirement.AllowedPermissions;
                if (requirement.CheckAll)
                {
                    if(context.User.HasAllPermissions(permissions))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if(context.User.HasAnyPermissions(permissions))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
