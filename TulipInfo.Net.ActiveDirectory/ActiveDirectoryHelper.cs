using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace TulipInfo.Net.ActiveDirectory
{
    public static class ActiveDirectoryHelper
    {
        public static bool ValidateUser(string domainName, string userName, string password)
        {
            bool isValid = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName))
            {
                isValid = pc.ValidateCredentials(userName, password);
            }
            return isValid;
        }
    }
}
