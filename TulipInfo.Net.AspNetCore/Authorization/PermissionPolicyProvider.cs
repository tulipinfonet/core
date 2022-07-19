using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        //we use Jwt Bear in this project
        const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies it should fall back to an alternate provider.
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(AuthenticationScheme).RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PermissionAuthorizeAttribute.POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                string[] policyValues = policyName.Substring(PermissionAuthorizeAttribute.POLICY_PREFIX.Length)
                    .Split(PermissionAuthorizeAttribute.POLICY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                bool checkAll = Convert.ToBoolean(policyValues[0]);
                string[] permissions = new string[policyValues.Length-1];
                for(int i = 1; i < policyValues.Length; i++)
                {
                    permissions[i - 1] = policyValues[i];
                }

                var policy = new AuthorizationPolicyBuilder(AuthenticationScheme);
                policy.AddRequirements(new PermissionRequirement(permissions,checkAll));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }

            return BackupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
