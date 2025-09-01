using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using MCS.Business.ASPNETIdentity;
using MCS.Service.Providers;

namespace MCS.Service
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions _oAuthOptions { get; set; }
        private static string _publicClientId { get; set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(CustomIdentityDbContext.Create);
            app.CreatePerOwinContext<CustomUserManager>(CustomUserManager.Create);
            app.CreatePerOwinContext<CustomSignInManager>(CustomSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            _publicClientId = "eMorasalatSelf";

            _oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(_publicClientId),

                //to move the expire time span to the config 
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(_oAuthOptions);
            app.Use(typeof(OwinMiddleWareQueryStringExtractor));
        }
    }
}