using System;
using System.Threading.Tasks;
using MCS.Framework.Logging;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MCS.UI.Startup))]
namespace MCS.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);

            }
            catch (Exception ex)
            {
                Logger.WriteException(ex);
            }

            app.MapSignalR();
        }
    }
}
