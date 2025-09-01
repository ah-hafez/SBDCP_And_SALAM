using System;
using System.Linq;
using System.Web;
using MCS.Common;
using System.Security.Principal;
using MCS.DTO;

namespace MCS.IntegrationServices
{
    public class SessionInfo
    {
        public static UserVM CurrentUser
        {
            get
            {
                return TryGetUserFromSession() as UserVM;
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
                return "ar";
            }
        }

        public static int OrgUnitId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        if (userDTO.UserOrgUnits != null)
                        {
                            UserOrgUnitDTO userOrgUnitVM =
                                userDTO.UserOrgUnits.Where(u => u.IsSelected == true).FirstOrDefault();

                            if (userOrgUnitVM != null)
                            {
                                return userOrgUnitVM.Id;
                            }
                        }
                    }
                }

                return -1;
            }
        }

        private static string accessToken;
        public static string AccessToken
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        accessToken = userDTO.AccessToken;

                        return accessToken;
                    }
                }

                return accessToken;
            }
            set 
            {
                accessToken = value;
            }
        }

        private static string sessionId;
        public static string SessionId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserDTO userDTO = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserDTO;

                    if (userDTO != null)
                    {
                        sessionId = userDTO.SessionId;

                        return sessionId;
                    }
                }

                return sessionId;
            }
            set
            {
                sessionId = value;
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

        private static UserVM TryGetUserFromSession()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.Session != null)
                return (UserVM)HttpContext.Current.Session[Constants.LoggedInUserKey];

            return null;
        }
    }
}