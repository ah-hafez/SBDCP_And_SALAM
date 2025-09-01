using Microsoft.Owin;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MCS.Common;

namespace MCS.Service.Providers
{
    public class OwinMiddleWareQueryStringExtractor : OwinMiddleware
    {


        public OwinMiddleWareQueryStringExtractor(OwinMiddleware next)
      : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.TenantId]))
            {
                string tenantId = context.Request.Query.Get(Constants.TenantId);
                if (!string.IsNullOrEmpty(tenantId))
                {
                    context.Request.Headers.Add(Constants.TenantId, new string[] { tenantId });
                }
            }

            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.TenantDatabaseName]))
            {
                string tenantDatabaseName = context.Request.Query.Get(Constants.TenantDatabaseName);
                if (!string.IsNullOrEmpty(tenantDatabaseName))
                {
                    context.Request.Headers.Add(Constants.TenantDatabaseName, new string[] { tenantDatabaseName });
                }
            }

            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers["Authorization"]))
            {
                string bearerToken = context.Request.Query.Get("Authorization");
                if (bearerToken != null)
                {
                    var ticket = Startup._oAuthOptions.AccessTokenFormat.Unprotect(bearerToken);
                    if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
                    {
                        string[] authorization = { "Bearer " + bearerToken };
                        context.Request.Headers.Add("Authorization", authorization);
                        IPrincipal principal = new ClaimsPrincipal(ticket.Identity);
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;

                        if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.UserId]))
                        {
                            Claim idClaim = ticket.Identity.FindFirst(Constants.UserId);
                            if (idClaim != null)
                            {
                                System.Web.HttpContext.Current.Request.Headers.Add(Constants.UserId, idClaim.Value);
                            }
                        }
                        context.Request.Environment["server.User"] = new ClaimsPrincipal(ticket.Identity);
                    }
                }
            }

            await Next.Invoke(context);
        }
    }

}