using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;
using System.Configuration;
using System.Web.Mvc;
using MCS.Framework.Cache;
using MCS.Framework.Localization;
using MCS.Common;

namespace MCS.UI.TenantsAdmin
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = BuildUnityContainer();
            container.RegisterType<ICacheProvider, MultiTenantCacheWithSqlDependency>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor());

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