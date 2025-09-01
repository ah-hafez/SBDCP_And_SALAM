using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MCS.Framework.Web;

namespace MCS.UI.TenantsAdmin
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissionCodes;

        public CustomAuthorizationAttribute(params string[] permissionCodes)
        {
            _permissionCodes = permissionCodes;
        }

        public CustomAuthorizationAttribute()
        {
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            HttpContext context = HttpContext.Current;

            if (HttpContext.Current.Session[UserContext.LoggedInUserSessionVariable] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { area = "", controller = "Error", action = "Unauthorized" }));
            }

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext authorizationContext)
        {

            if (HttpContext.Current.Session[UserContext.LoggedInUserSessionVariable] == null)
            {
                authorizationContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { area = "", controller = "Login", action = "Login", returnUrl = authorizationContext.HttpContext.Request.RawUrl }));
            }

        }
    }
}