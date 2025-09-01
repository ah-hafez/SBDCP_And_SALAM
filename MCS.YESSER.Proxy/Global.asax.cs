using MCS.Framework;
using MCS.Framework.Localization;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace MCS.YESSER.Proxy
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
            InitializeContainer(this);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            IoC.Reset();
        }

        private static void InitializeContainer(Global self)
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

    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = BuildUnityContainer();

            container.RegisterType<IDataAccess, SqlDataAccess>(new TransientLifetimeManager(),
            new InjectionConstructor(DbResourceConfiguration.Current.ConnectionString));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            IUnityContainer container = new UnityContainer().LoadConfiguration();

            container.RegisterInstance<IUnityContainer>(container);

            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            section.Configure(container);

            return container;
        }
    }
}