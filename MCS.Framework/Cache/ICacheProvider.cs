using System;

namespace MCS.Framework.Cache
{
    public interface ICacheProvider
    {
        object Get(string key, string culture);
        void Insert(string key, object kValue, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback, string culture);
        void Insert(string key, object kValue, string culture, int maxCacheExpirationPeriodInMinutes);
        object Remove(string key, string culture);
        void RemoveBasedOnPrefix(string prefix);
    }
}
