using System.Web.Mvc;
using System.Web.Routing;

namespace MCS.UI
{
    public class CustomAuthorize : AuthorizeAttribute
    {
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