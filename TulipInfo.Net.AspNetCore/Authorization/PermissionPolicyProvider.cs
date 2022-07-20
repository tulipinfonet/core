using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        const string DefaultAuthenticationScheme = "Bearer";

        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
        private IAuthenticationSchemeProvider SchemeProvider { get; }
        public PermissionPolicyProvider(IAuthenticationSchemeProvider schemeProvider, IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies it should fall back to an alternate provider.
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            SchemeProvider= schemeProvider;
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            string defaultScheme = await GetDefaultAuthenticationScheme();
            return new AuthorizationPolicyBuilder(defaultScheme).RequireAuthenticatedUser().Build();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
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

                string defaultScheme = await GetDefaultAuthenticationScheme();
                var policy = new AuthorizationPolicyBuilder(defaultScheme);
                policy.AddRequirements(new PermissionRequirement(permissions,checkAll));
                return policy.Build();
            }

            return await BackupPolicyProvider.GetPolicyAsync(policyName);
        }

        private async Task<string> GetDefaultAuthenticationScheme()
        {
            var scheme = await SchemeProvider.GetDefaultAuthenticateSchemeAsync();
            if (scheme != null)
            {
                return scheme.Name;
            }
            else
            {
                return DefaultAuthenticationScheme;
            }
        }
    }
}
