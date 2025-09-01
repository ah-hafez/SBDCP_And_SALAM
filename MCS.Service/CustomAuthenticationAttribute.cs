using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using MCS.Framework.Web;

namespace MCS.Service
{
    public class CustomAuthenticationAttribute : System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpContext context = HttpContext.Current;

            if (context == null &&
                !context.User.Identity.IsAuthenticated &&
                HttpContext.Current.Session[UserContext.LoggedInUserSessionVariable] == null)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnAuthorization(actionContext);
        }
    }
}