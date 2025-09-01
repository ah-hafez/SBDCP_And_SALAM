using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace MCS.Service.Helpers
{
    public class ADHelper
    {
        public static bool AuthenticateUserPassword(string userName, string password)
        {
            try
            {
                if (userName.StartsWith(".\\"))
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Machine))
                    {
                        return pc.ValidateCredentials(userName, password);
                    }
                }
                else
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                    {
                        return pc.ValidateCredentials(userName, password);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}