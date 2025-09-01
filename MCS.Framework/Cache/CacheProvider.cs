using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace MCS.Framework.Cache
{
    public class CacheProvider : ICacheProvider
    {
        #region Attributes

        private static System.Web.Caching.Cache cache = null;

        #endregion Attributes

        #region Constructors

        static CacheProvider()
        {
            if (HttpContext.Current != null)
            {
                cache = HttpContext.Current.Cache;
            }
            else // This is in case this DLL has been used from Non Web application.
            {
                cache = HttpRuntime.Cache;
            }
        }

        #endregion Constructors

        #region Methods

        public void Insert(string key, object kValue, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback,
            string culture)
        {
            string cultureKey = GetCultureKey(key, culture);

            CacheDependency cacheDependency = dependencies;

            lock (cache)
            {
                if (cache[cultureKey] == null)
                {
                    cache.Insert(cultureKey, kValue, cacheDependency, absoluteExpiration, slidingExpiration,
                        priority, onRemoveCallback);
                }
            }
        }

        public void Insert(string key, object kValue, string culture, int maxCacheExpirationPeriodInMinutes)
        {
            DateTime absoluteExpiration = DateTime.UtcNow.AddMinutes(maxCacheExpirationPeriodInMinutes);

            Insert(key, kValue, null, absoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null, culture);
        }

        public object Get(string key, string culture)
        {
            return cache[GetCultureKey(key, culture)];
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public object Remove(string key, string culture)
        {
            return cache.Remove(GetCultureKey(key, culture));
        }

        public void RemoveBasedOnPrefix(string prefix)
        {
            List<string> itemsToRemove = new List<string>();

            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower().StartsWith(prefix.ToLower()))
                {
                    itemsToRemove.Add(enumerator.Key.ToString());
                }
            }

            foreach (string itemToRemove in itemsToRemove)
            {
                cache.Remove(itemToRemove);
            }
        }
        private string GetCultureKey(string key, string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return key;
            }

            return key + "_" + culture;
        }

        #endregion Methods
    }
}
