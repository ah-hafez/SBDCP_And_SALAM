using MCS.DTO;
using MCS.Framework.Security;
using MCS.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MCS.Common;
using System.Security.Principal;

namespace MCS.UI.TenantsAdmin
{
    public class SessionInfo
    {
        public static UserDTO CurrentUser
        {
            get
            {
                return TryGetUserFromSession() as UserDTO;
            }
        }

        public static void SetLoggedInUserInSession(UserDTO userDTO)
        {
            HttpContext context = HttpContext.Current;

            context.User = new GenericPrincipal(new GenericIdentity(userDTO.UserName), null);
            context.Session[Constants.LoggedInUserKey] = userDTO;
        }

        public static string CultureShortName
        {
            get
            {
                if (HttpContext.Current.Session[Constants.CultureNameKey] == null)
                {
                    return "ar";
                }

                return Convert.ToString(HttpContext.Current.Session[Constants.CultureNameKey]);
            }
            set
            {
                HttpContext.Current.Session[Constants.CultureNameKey] = value;
            }
        }

        public static int OrgUnit
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        if (userDTO.UserOrgUnits != null)
                        {
                            UserOrgUnitDTO userOrgUnitDTO =
                                userDTO.UserOrgUnits.Where(u => u.IsSelected == true).FirstOrDefault();

                            if (userOrgUnitDTO != null)
                            {
                                return userOrgUnitDTO.Id;
                            }
                        }
                    }
                }

                return -1;
            }
        }

        public static string AccessToken
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        return userDTO.AccessToken;
                    }
                }

                return string.Empty;
            }
        }

        public static string SessionId
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        return userDTO.SessionId;
                    }
                }

                return string.Empty;
            }
        }

        public static void SetObjectInSession(object obj, string sessionKey)
        {
            HttpContext.Current.Session[sessionKey] = obj;
        }

        public static object GetObjectFromSession(string sessionKey)
        {
            if (HttpContext.Current.Session[sessionKey] == null)
            {
                return null;
            }

            return HttpContext.Current.Session[sessionKey];
        }

        private static UserDTO TryGetUserFromSession()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.Session != null)
                return (UserDTO)HttpContext.Current.Session[Constants.LoggedInUserKey];

            return null;
        }
    }

}