using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MCS.Tenants.Service.Startup))]

namespace MCS.Tenants.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Seconde Solution
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureAuth(app);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
