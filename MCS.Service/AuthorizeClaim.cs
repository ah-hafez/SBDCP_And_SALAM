using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using System;
using System.Security.Claims;
using MCS.Common;

namespace MCS.Service
{
    [CustomAuthenticationAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HubAuthorization : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return base.AuthorizeHubConnection(hubDescriptor, request);
        }
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
            // check the authenticated user principal from environment
            var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
            var principal = environment["server.User"] as ClaimsPrincipal;
            if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
            {

                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.TenantId]))
                {
                    string tenantId = System.Web.HttpContext.Current.Request.QueryString.Get(Constants.TenantId);
                    if (!string.IsNullOrEmpty(tenantId))
                    {
                        System.Web.HttpContext.Current.Request.Headers.Add(Constants.TenantId, tenantId);
                    } 
                }
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.TenantDatabaseName]))
                {
                    string tenantDatabaseName = System.Web.HttpContext.Current.Request.QueryString.Get(Constants.TenantDatabaseName);
                    if (!string.IsNullOrEmpty(tenantDatabaseName))
                    {
                        System.Web.HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, tenantDatabaseName);
                    }
                }
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers[Constants.UserId]))
                {
                    Claim idClaim = principal.FindFirst(Constants.UserId);
                    if (idClaim != null)
                    {
                        System.Web.HttpContext.Current.Request.Headers.Add(Constants.UserId, idClaim.Value);
                    } 
                }
                // create a new HubCallerContext instance with the principal generated from token
                // and replace the current context so that in hubs we can retrieve current user identity
                hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new ServerRequest(environment), connectionId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}