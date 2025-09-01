using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace MCS.UI.Helpers
{
    public class ADHelper
    {
        public static string GetUserPhoneNo(string userName)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    using (UserPrincipal up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, userName))
                    {
                        DirectoryEntry userObj = (DirectoryEntry)up.GetUnderlyingObject();
                        if (userObj.Properties.Contains("mobile"))
                        {
                            return userObj.Properties["mobile"].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public static string GetUserEmail(string userName)
        {
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    using (UserPrincipal up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, userName))
                    {
                        DirectoryEntry userObj = (DirectoryEntry)up.GetUnderlyingObject();
                        if (userObj.Properties.Contains("email"))
                        {
                            return userObj.Properties["email"].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }
    }
}