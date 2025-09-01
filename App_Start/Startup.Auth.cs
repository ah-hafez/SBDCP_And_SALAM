using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using MCS.Framework.Security;

namespace MCS.UI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            };

            app.UseCookieAuthentication(cookieOptions);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseMixedAuth(cookieOptions);
        }
    }
}