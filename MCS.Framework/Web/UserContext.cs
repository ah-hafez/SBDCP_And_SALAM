using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MCS.Framework.Security;

namespace MCS.Framework.Web
{
    public class UserContext : IUserContext
    {
        public static readonly string LoggedInUserSessionVariable = "__LoggedInUser";

        public TUser GetLoggedInUser<TUser>() where TUser : IUser
        {
            var user = TryGetUserFromSession();

            return user != null ? (TUser)user : default(TUser);
        }

        private static IUser TryGetUserFromSession()
        {
             HttpContext context = HttpContext.Current;

            if (context != null && context.User != null && context.User.Identity.IsAuthenticated && context.Session != null)
                return (IUser)HttpContext.Current.Session[LoggedInUserSessionVariable];

            return null;
        }

        public static IUser LoggedInUser
        {
            get { return TryGetUserFromSession(); }
        }

        public static void SetLoggedInUserInWebSession(IUser user)
        {
            HttpContext.Current.Session[LoggedInUserSessionVariable] = user;
        }
    }
}
