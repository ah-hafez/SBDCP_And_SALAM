using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MCS.Framework.Security;
using MCS.Business;
using MCS.Common;
using MCS.Service.Helpers;

namespace MCS.Service.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId not provided");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IApplicationUser user = null;
            bool isWindowsLogin = bool.Parse(context.OwinContext.Get<string>("isWindowsLogin"));
            if (isWindowsLogin)
            {
                int startIndex = context.UserName.LastIndexOf("\\") + 1;
                int length = context.UserName.Length - startIndex;

                string userName = context.UserName.Substring(startIndex, length);

                if (ADHelper.AuthenticateUserPassword(context.UserName, context.Password))
                {
                    user = await UserManager.UserManagerProvider.FindByNameAsync(userName);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(context.Password))
                {
                    user = await UserManager.UserManagerProvider.FindByNameAsync(context.UserName);
                }
                else
                { 
                    user = await UserManager.UserManagerProvider.FindAsync(context.UserName, context.Password);
                }
            }

            if (user == null)
            {
                context.SetError(StatusCode.UserNameOrPasswordNotCorrect.ToString());
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager.UserManagerProvider,
              OAuthDefaults.AuthenticationType);

            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            oAuthIdentity.AddClaim(new Claim("UserIdentity", user.Id));

            AuthenticationProperties properties = CreateProperties(user.UserName, user.Id);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(properties, oAuthIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string isWindowsLogin = context.Parameters.Get("isWindowsLogin");
            context.OwinContext.Set<string>("isWindowsLogin", isWindowsLogin);
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string userId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "userIdentity", userId }
            };

            return new AuthenticationProperties(data);
        }
    }

}