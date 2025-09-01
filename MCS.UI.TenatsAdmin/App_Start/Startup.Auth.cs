using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using MCS.Framework.Security;

namespace MCS.UI.TenantsAdmin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
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