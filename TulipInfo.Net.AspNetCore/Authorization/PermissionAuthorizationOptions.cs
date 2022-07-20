namespace TulipInfo.Net.AspNetCore
{
    public class PermissionAuthorizationOptions
    {
        public const string DefaultAdministratorRoleName = "admin";
        public const string DefaultPermissionClaimType = "permissions";

        public string AdministratorRoleName { get; set; } = DefaultAdministratorRoleName;
        public string PermissionClaimType { get; set; } = DefaultPermissionClaimType;
    }
}
