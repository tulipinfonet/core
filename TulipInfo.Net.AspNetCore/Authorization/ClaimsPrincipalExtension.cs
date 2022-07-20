using System.Security.Claims;

namespace TulipInfo.Net.AspNetCore.Authorization
{
    public static class ClaimsPrincipalExtension
    {
        public static bool HasRole(this ClaimsPrincipal pri, string role)
        {
            if (pri != null && pri.Identity != null && pri.Identity.IsAuthenticated)
            {
                string[] currentRoles = GetRoles(pri.Claims);
                return currentRoles.Any(r => r.Equals(role, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }

        public static bool HasAllPermissions(this ClaimsPrincipal pri, string[] permissions,
            string adminRoleName = PermissionAuthorizationOptions.DefaultAdministratorRoleName,
            string permissionClaimType = PermissionAuthorizationOptions.DefaultPermissionClaimType)
        {
            if (pri != null && pri.Identity != null && pri.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrWhiteSpace(adminRoleName) && pri.HasRole(adminRoleName))
                {
                    return true;
                }

                string[] currentPermissions = GetPermissions(pri.Claims, permissionClaimType);
                return HasAll(currentPermissions, permissions);
            }
            return false;
        }

        public static bool HasAnyPermissions(this ClaimsPrincipal pri, string[] permissions,
            string adminRoleName = PermissionAuthorizationOptions.DefaultAdministratorRoleName,
            string permissionClaimType = PermissionAuthorizationOptions.DefaultPermissionClaimType)
        {
            if (pri != null && pri.Identity != null && pri.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrWhiteSpace(adminRoleName) && pri.HasRole(adminRoleName))
                {
                    return true;
                }

                string[] currentPermissions = GetPermissions(pri.Claims, permissionClaimType);
                return HasAny(currentPermissions, permissions);
            }
            return false;
        }

        private static string[] GetPermissions(IEnumerable<Claim> claims,
            string permissionClaimType = PermissionAuthorizationOptions.DefaultPermissionClaimType)
        {
            return GetArray(claims, permissionClaimType);
        }

        private static string[] GetRoles(IEnumerable<Claim> claims)
        {
            return GetArray(claims, ClaimTypes.Role);
        }

        private static string[] GetArray(IEnumerable<Claim> claims, string claimType)
        {
            return claims.Where(c => c.Type == claimType && !string.IsNullOrWhiteSpace(c.Value))
                .Select(c => c.Value).ToArray();
        }

        private static bool HasAll(string[] currentValues, string[] incomeValues)
        {
            bool hasAll = true;
            if (incomeValues.Any(p =>
                currentValues.Any(cp => cp.Equals(p, StringComparison.InvariantCultureIgnoreCase)) == false))
            {
                hasAll = false;
            }

            return hasAll;
        }

        private static bool HasAny(string[] currentValues, string[] incomeValues)
        {
            if (incomeValues.Any(p =>
                currentValues.Any(cp => cp.Equals(p, StringComparison.InvariantCultureIgnoreCase))))
            {
                return true;
            }
            return false;
        }

    }
}
