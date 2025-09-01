using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MCS.Framework
{
    public static class IoC
    {
        private static IUnityContainer container;

        public static IUnityContainer Container
        {
            get { return container; }
            set { container = value; }
        }
       
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return container.Resolve<T>(name);
        }

        public static object Resolve(Type service)
        {
            return container.Resolve(service);
        }

        public static bool IsInitialized
        {
            get { return container != null; }
        }

        public static void Reset()
        {
            if (container == null)
                return;

            container = null;
        }
    }
}
