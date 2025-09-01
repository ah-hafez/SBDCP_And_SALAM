using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Framework.Web;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Web;
using System.Diagnostics;
using System.Web.Http;
using System.Web.SessionState;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MCS.IntegrationServices
{
    public class CustomHttpApplication : HttpApplicationBase
    {
        public CustomHttpApplication()
        {
        }

        protected static void Application_Error(Object sender, EventArgs e)
        {
        }

        public override void HttpApplicationBase_BeginRequest(Object sender, EventArgs e)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ar-JO");

            CultureInfo cultureInfo = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();

            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            if (!IoC.IsInitialized)
                InitializeContainer(this);

            base.HttpApplicationBase_BeginRequest(sender, e);
        }

        //CobaltServer svr;
        public override void Application_Start(Object sender, EventArgs e)
        {
            // var port = int.Parse( ConfigurationManager.AppSettings["Port"]);
            // var docsPath = ConfigurationManager.AppSettings["DocsPath"];
            //var host = ConfigurationManager.AppSettings["Host"];
            // Process.Start("cmd");
            // svr = new CobaltServer(docsPath, host, port);
            // svr.Start();


            Logger.SetLogWriter(new LogWriterFactory().Create());

            CultureInfo cultureInfo = new CultureInfo("ar-JO");

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            MvcHandler.DisableMvcResponseHeader = true;
            InitializeContainer(this);

            AreaRegistration.RegisterAllAreas();
            // RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
          
            base.Application_Start(sender, e);
        }
        protected void Application_EndRequest()
        {


            Response.Headers.Remove("Server");
        }
        public override void Application_End(Object sender, EventArgs e)
        {

           // svr.Stop();
            IoC.Reset();

            base.Application_End(sender, e);
        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void InitializeContainer(CustomHttpApplication self)
        {
            if (IoC.IsInitialized)
                return;

            self.CreateContainer();
        }

        private void CreateContainer()
        {
            //IoC.Container = Bootstrapper.Initialize();
        }

    }
}