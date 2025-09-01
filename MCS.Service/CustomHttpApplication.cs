using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MCS.Framework;
using MCS.Framework.Web;
using System.Collections;

namespace MCS.Service
{
    public class CustomHttpApplication : HttpApplicationBase
    {
        public static Hashtable WordAddInListSession = new Hashtable();
        public CustomHttpApplication()
        {
        }

        protected static void Application_Error(Object sender, EventArgs e)
        {
        }
        public override void HttpApplicationBase_BeginRequest(Object sender, EventArgs e)
        {
            if (!IoC.IsInitialized)
                InitializeContainer(this);
            base.HttpApplicationBase_BeginRequest(sender, e);
        }

        public override void Init()
        {
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
        }

        public override void Application_Start(Object sender, EventArgs e)
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InitializeContainer(this);
            base.Application_Start(sender, e);
        }

        public override void Application_End(Object sender, EventArgs e)
        {
            IoC.Reset();

            base.Application_End(sender, e);
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
            IoC.Container = Bootstrapper.Initialize();
        }
    }
}