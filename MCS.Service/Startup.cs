using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using MCS.Business.ASPNETIdentity;

[assembly: OwinStartup(typeof(MCS.Service.Startup))]
namespace MCS.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.CreatePerOwinContext<CustomUserManager>(CustomUserManager.Create);
            app.CreatePerOwinContext<CustomSignInManager>(CustomSignInManager.Create);

            var config = new HubConfiguration();
            config.EnableJSONP = true;
            app.MapSignalR(config);
        }
    }
}