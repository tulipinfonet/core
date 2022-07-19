using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TulipInfo.Net.AspNetCore
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] AllowedPermissions { get; }
        public bool CheckAll { get; set; }
        public PermissionRequirement(string[] allowedPermissions, bool checkAll)
        {
            this.AllowedPermissions = allowedPermissions;
        }
    }
}
