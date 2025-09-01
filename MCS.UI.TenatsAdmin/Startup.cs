using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using MCS.Business.ASPNETIdentity;

[assembly: OwinStartupAttribute(typeof(MCS.UI.TenantsAdmin.Startup))]
namespace MCS.UI.TenantsAdmin
{
    public partial class Startup
    {
        private static OAuthAuthorizationServerOptions OAuthOptions { get; set; }
        private static string PublicClientId { get; set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}