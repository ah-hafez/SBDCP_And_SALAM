using System;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using YESSER.MCS.Tenants.Service;
using MCS.Framework;
using MCS.Framework.Web;
using MCS.Tenants.Service.Binders;

namespace MCS.Tenants.Service
{
    public class WebApiApplication : HttpApplicationBase
    {
        protected void Application_Start()
        {

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
        }
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
        }

        public override void Application_Start(object sender, EventArgs e)
        {
            //remove the MVC header
            MvcHandler.DisableMvcResponseHeader = true;

            ModelBinders.Binders.Add(typeof(string), new TrimModelBinder());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            if (!IoC.IsInitialized)
                InitializeContainer(this);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            base.Application_Start(sender, e);
        }

        public override void Application_End(Object sender, EventArgs e)
        {
            IoC.Reset();

            base.Application_End(sender, e);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void InitializeContainer(WebApiApplication self)
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
