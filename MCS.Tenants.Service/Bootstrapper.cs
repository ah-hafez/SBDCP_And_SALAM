using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Tenants.Service
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = BuildUnityContainer();

            container.RegisterType<IDataAccess, SqlDataAccess>(new PerRequestLifetimeManager(),
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