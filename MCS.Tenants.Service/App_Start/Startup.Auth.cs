using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using MCS.Business.ASPNETIdentity;
using MCS.Tenants.Service.Providers;

[assembly: OwinStartup(typeof(MCS.Tenants.Service.Startup))]
namespace MCS.Tenants.Service
{
    public partial class Startup
    {
        private static OAuthAuthorizationServerOptions _oAuthOptions { get; set; }
        private static string _publicClientId { get; set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(MultiTenantCustomIdentityDbContext.Create);
            app.CreatePerOwinContext<MultiTenantCustomUserManager>(MultiTenantCustomUserManager.Create);
            app.CreatePerOwinContext<MultiTenantCustomSignInManager>(MultiTenantCustomSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            _publicClientId = "self";
            _oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(_publicClientId),

                #if DEBUG
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(60),
                #else
                    AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                #endif
                #if DEBUG
                //TODO NEVER ENABLE THIS IN PRODUCTION
                AllowInsecureHttp = true,
                #else
                    AllowInsecureHttp = false,
                #endif
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(_oAuthOptions);
        }
    }
}