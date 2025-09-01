using System;
using System.Linq;
using System.Web;
using MCS.Common;
using MCS.UI.Areas.User.Models.Shared;
using System.Security.Principal;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.Common.ApiControllerResults;

namespace MCS.UI
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

        public static void SetLoggedInUserInSession(UserVM userVM)
        {
            HttpContext context = HttpContext.Current;
            context.User = new GenericPrincipal(new GenericIdentity(userVM.UserName), null);
            context.Session[Constants.LoggedInUserKey] = userVM;
        }

        public static string CultureShortName
        {
            get
            {
                if (HttpContext.Current.Session[Constants.CultureNameKey] == null)
                {
                    return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                }

                return Convert.ToString(HttpContext.Current.Session[Constants.CultureNameKey]);
            }
            set
            {
                HttpContext.Current.Session[Constants.CultureNameKey] = value;
            }
        }

        public static int OrgUnitId
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserVM userVM = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserVM;

                    if (userVM != null)
                    {
                        if (userVM.UserOrgUnits != null)
                        {
                            UserOrgUnitVM userOrgUnitVM =
                                userVM.UserOrgUnits.Where(u => u.IsSelected == true).FirstOrDefault();

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
        public static bool IsVIPUser
        {
            get
            {
                return IsVIPUser;
            }
        }
        public static bool IsManager
        {
            get
            {
                return SessionInfo.CurrentUser.UserOrgUnits.Any(o => o.Id == SessionInfo.OrgUnitId && o.ManagerId == SessionInfo.CurrentUser.Id);
            }
        }

        public static UserOrgUnitDTO OrgUnitInfo
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserVM userVM = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserVM;
                    if (userVM != null)
                    {
                        if (userVM.UserOrgUnits != null)
                        {
                            UserOrgUnitVM userOrgUnitVM = userVM.UserOrgUnits.Where(u => u.IsSelected == true).FirstOrDefault();
                            if (userOrgUnitVM != null)
                            {
                                return UserOrgUnitMapper.Map(userOrgUnitVM);
                            }
                        }
                    }
                }
                return null;
            }
        }

        public static string AccessToken
        {
            get
            {
                if (HttpContext.Current.Session[Constants.LoggedInUserKey] != null)
                {
                    UserVM userVM = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserVM;

                    if (userVM != null)
                    {
                        return userVM.AccessToken;
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
                    UserVM userVM = HttpContext.Current.Session[Constants.LoggedInUserKey] as UserVM;

                    if (userVM != null)
                    {
                        return userVM.SessionId;
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

        private static UserVM TryGetUserFromSession()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.Session != null)
                return (UserVM)HttpContext.Current.Session[Constants.LoggedInUserKey];

            return null;
        }
    }
}