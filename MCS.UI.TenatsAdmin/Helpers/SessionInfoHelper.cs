using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using MCS.Common;
using MCS.UI.TenantsAdmin.Models;

namespace MCS.UI.TenantsAdmin.Helpers
{
    public class SessionInfoHelper
    {
        public static ApplicationUserVM CurrentUser
        {
            get
            {
                return TryGetUserFromSession() as ApplicationUserVM;
            }
        }

        public static void SetLoggedInUserInSession(ApplicationUserVM userDTO)
        {
            HttpContext context = HttpContext.Current;

            context.User = new GenericPrincipal(new GenericIdentity(userDTO.UserName), null);
            context.Session[Constants.LoggedInUserKey] = userDTO;
        }

        public static string CultureShortName
        {
            get
            {
                if (HttpContext.Current.Session[Constants.Languages.Arabic] == null)
                {
                    return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                }

                return Convert.ToString(HttpContext.Current.Session[Constants.Languages.Arabic]);
            }
            set
            {
                HttpContext.Current.Session[Constants.Languages.Arabic] = value;
            }
        }

        public static string AccessToken
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    ApplicationUserVM userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as ApplicationUserVM;

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
                    ApplicationUserVM userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as ApplicationUserVM;

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

        private static ApplicationUserVM TryGetUserFromSession()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.Session != null)
                return (ApplicationUserVM)HttpContext.Current.Session[Constants.LoggedInUserKey];

            return null;
        }
    }
}