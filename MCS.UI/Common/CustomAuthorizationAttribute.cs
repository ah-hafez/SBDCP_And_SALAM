using System.Web.Mvc;
using System.Web.Routing;

namespace MCS.UI.Common
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
            if (SessionInfo.CurrentUser != null)
            {
                bool hasPermission = false;

                foreach (string permissionCode in _permissionCodes)
                {
                    if (SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                    {
                        hasPermission = true;
                        break;
                    }
                }

                if (!hasPermission)
                {
                    if (filterContext.Controller.TempData.ContainsKey("area"))
                    {
                        filterContext.Controller.TempData.Remove("area");
                    }

                    filterContext.Controller.TempData.Add("area", filterContext.RouteData.DataTokens["area"].ToString());

                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { area = filterContext.RouteData.DataTokens["area"].ToString(), controller = "Error", action = "Unauthorized" }));
                }
            }

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext authorizationContext)
        {
            if (SessionInfo.CurrentUser == null)
            {
                var area = authorizationContext.RouteData.DataTokens["area"];
                if (area == null)
                {
                    area = "User";
                }
                authorizationContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { area = area, controller = "Login", action = "Login", returnUrl = authorizationContext.HttpContext.Request.RawUrl }));
            }
        }
    }
}