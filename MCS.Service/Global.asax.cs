using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MCS.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Logger.SetLogWriter(new LogWriterFactory().Create());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
