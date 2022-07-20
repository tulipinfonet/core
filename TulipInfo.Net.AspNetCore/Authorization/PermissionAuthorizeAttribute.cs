using Microsoft.AspNetCore.Authorization;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "Permission";
        public const string POLICY_SEPARATOR = "$";

        public PermissionAuthorizeAttribute(string permission) => Permissions = new string[] { permission };

        public PermissionAuthorizeAttribute(string[] permissions) => Permissions = permissions;

        public bool CheckAll { get; set; }

        public string[] Permissions
        {
            get
            {
                if(Policy!=null)
                {
                    string strPermission = Policy.Substring(POLICY_PREFIX.Length);
                    string[] permissions = strPermission.Split(POLICY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                    return permissions;
                }
                else
                {
                    return new string[0];
                }
            }
            set
            {
                string strPermission = "";
                if (value != null && value.Length > 0)
                {
                    strPermission = string.Join(POLICY_SEPARATOR, value);
                }
                Policy = $"{POLICY_PREFIX}{POLICY_SEPARATOR}{CheckAll}{POLICY_SEPARATOR}{strPermission}";
            }
        }
    }
}
