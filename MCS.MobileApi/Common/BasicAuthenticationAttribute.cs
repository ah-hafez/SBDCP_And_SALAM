using MobileApi.Models;
using MobileApi.UtilityClasses;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MobileApi.Common
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                //TODO: remove the hardcoded of action name 
                bool skipAuthorization = (actionContext.ActionDescriptor.ActionName.ToLower() == "login" || actionContext.ActionDescriptor.ActionName.ToLower() == "checkurl");

                if (!skipAuthorization && !IsUserAuthorized(actionContext))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                base.OnAuthorization(actionContext);
            }
            catch (SecurityTokenExpiredException ex)
            {
                DataResult result = new DataResult();
                result.Code = MessageCode.SessionTokenTimedOut;
                //result.Description = MessageResources.GetResourceText(ResourceText.SessionTokenTimedOut, languageAbbreviation);

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, result);
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        private bool IsUserAuthorized(HttpActionContext actionContext)
        {
            string token = GetTokenFromHeader(actionContext);

            if (!string.IsNullOrEmpty(token))
            {
                AuthenticationModule authenticationModule = new AuthenticationModule();

                JwtSecurityToken userPayloadToken = authenticationModule.GenerateUserClaimFromJWT(token);

                if (userPayloadToken != null)
                {
                    AuthenticationIdentity identity = authenticationModule.PopulateUserIdentity(userPayloadToken);
                    string[] roles = { "All" };
                    GenericPrincipal genericPrincipal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = genericPrincipal;
                    AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

                    if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.UserName))
                    {
                        authenticationIdentity.UserId = identity.UserId;
                        authenticationIdentity.UserName = identity.UserName;
                    }

                    return true;
                }
            }

            return false;
        }

        private string GetTokenFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            AuthenticationHeaderValue authenticationHeaderValue = actionContext.Request.Headers.Authorization;

            if (authenticationHeaderValue != null)
            {
                requestToken = authenticationHeaderValue.Scheme;
            }

            return requestToken;
        }
    }
}