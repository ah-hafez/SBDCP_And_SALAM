using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MCS.Framework;

namespace MCS.UI.TenantsAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += new EventHandler(HttpApplicationBase_BeginRequest);
        }
        protected void Application_Start()
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeContainer(this);
        }


        public void HttpApplicationBase_BeginRequest(Object sender, EventArgs e)
        {
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("ar-JO");

            //CultureInfo cultureInfo = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();

            //cultureInfo.DateTimeFormat.ShortDatePattern = UIHelper.SystemDateFormat;
            //cultureInfo.DateTimeFormat = culture.DateTimeFormat;

            //System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            if (!IoC.IsInitialized)
            {
                InitializeContainer(this);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void InitializeContainer(MvcApplication self)
        {
            if (IoC.IsInitialized)
            {
                return;
            }

            self.CreateContainer();
        }

        private void CreateContainer()
        {
            IoC.Container = Bootstrapper.Initialize();
        }
    }
}
