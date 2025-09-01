using System;
using System.Configuration;
using MCS.Framework.Cache;
using MCS.Framework.MultiTenants;
using MCS.Framework.Web;

namespace MCS.Common
{
    public static class CacheHelper
    {
        static ICacheProvider _cacheProvider = null;

        static CacheHelper()
        {
            _cacheProvider = new CacheProvider();
        }

        public static object Get(string key, string culture)
        {
            if (SystemConfigurations.MultiTenantEnabled && !string.IsNullOrWhiteSpace(TenantHelper.GetTenantDatabaseNameFromHeader()))
            {
                return _cacheProvider.Get(string.Format("{0}_{1}", key, TenantHelper.GetTenantDatabaseNameFromHeader()), culture);
            }

            return _cacheProvider.Get(key, culture);
        }

        public static object Remove(string key, string culture)
        {
            if (SystemConfigurations.MultiTenantEnabled && !string.IsNullOrWhiteSpace(TenantHelper.GetTenantDatabaseNameFromHeader()))
            {
                return _cacheProvider.Remove(string.Format("{0}_{1}", key, TenantHelper.GetTenantDatabaseNameFromHeader()), culture);
            }

            return _cacheProvider.Remove(key, culture);
        }

        public static void RemoveBasedOnPrefix(string prefix)
        {
            if (SystemConfigurations.MultiTenantEnabled && !string.IsNullOrWhiteSpace(TenantHelper.GetTenantDatabaseNameFromHeader()))
            {
                _cacheProvider.RemoveBasedOnPrefix(string.Format("{0}_{1}", prefix, TenantHelper.GetTenantDatabaseNameFromHeader()));
            }

            _cacheProvider.RemoveBasedOnPrefix(prefix);
        }

        public static void Insert(string key, object kValue, string culture)
        {
            if (!SystemConfigurations.IsCachingEnabled)
            {
                return;
            }

            if (SystemConfigurations.MultiTenantEnabled && !string.IsNullOrWhiteSpace(TenantHelper.GetTenantDatabaseNameFromHeader()))
            {
                key = string.Format("{0}_{1}", key, TenantHelper.GetTenantDatabaseNameFromHeader());
            }

            int maxCacheExpirationPeriodInMinutes = 0;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxCacheExpirationPeriodInMinutes"]))
            {
                maxCacheExpirationPeriodInMinutes =
                    Convert.ToInt32(ConfigurationManager.AppSettings["MaxCacheExpirationPeriodInMinutes"]);
            }

            _cacheProvider.Insert(key, kValue, culture, maxCacheExpirationPeriodInMinutes);
        }
    }
}
