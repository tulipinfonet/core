using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                string strPermission = Policy.Substring(POLICY_PREFIX.Length);
                string[] permissions = strPermission.Split(POLICY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                return permissions;
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
