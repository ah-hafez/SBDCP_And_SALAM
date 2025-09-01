using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using MCS.Framework.Cache;
using MCS.Framework.Web;

namespace MCS.Common
{
    public class MultiTenantCacheWithSqlDependency : ICacheProvider
    {
        #region Attributes

        private static System.Web.Caching.Cache cache = null;
        private static string _databaseName = string.Empty;
        private static string _connectionString = string.Empty;

        #endregion Attributes

        #region Constructors

        static MultiTenantCacheWithSqlDependency()
        {
            if (HttpContext.Current != null)
            {
                cache = HttpContext.Current.Cache;
            }
            else // This is in case this DLL has been used from Non Web application.
            {
                cache = HttpRuntime.Cache;
            }

            object configSection = ConfigurationManager.GetSection("system.web/caching/sqlCacheDependency");

            if (configSection == null)
            {
                throw new ConfigurationErrorsException("SqlCacheDependency not configured in the web.config file");
            }

            System.Web.Configuration.SqlCacheDependencySection sqlCacheDependencySection =
                configSection as SqlCacheDependencySection;

            if (sqlCacheDependencySection == null)
            {
                throw new ConfigurationErrorsException("SqlCacheDependency not configured in the web.config file");
            }

            if (sqlCacheDependencySection.Databases == null &&
                sqlCacheDependencySection.Databases.Count == 0 &&
                sqlCacheDependencySection.Databases[0].Name == string.Empty)
            {
                throw new ConfigurationErrorsException("SqlCacheDependency databases not configured in the web.config file");
            }

            _connectionString =
                ConfigurationManager.ConnectionStrings[sqlCacheDependencySection.Databases[0].ConnectionStringName].ConnectionString;

            _databaseName = sqlCacheDependencySection.Databases[0].Name;
        }

        #endregion Constructors

        public object Get(string key, string culture)
        {
            string hostName = HttpContextHelper.HostName;

            key = hostName + key;

            return cache[GetCultureKey(key, culture)];
        }

        public void Insert(string key, object kValue, System.Web.Caching.CacheDependency dependencies,
            DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority,
            System.Web.Caching.CacheItemRemovedCallback onRemoveCallback, string culture)
        {
            string hostName = HttpContextHelper.HostName;

            key = hostName + key;

            string cultureKey = GetCultureKey(key, culture);

            if (cache[key] != null)
            {
                throw new Exception(string.Format("This key ({0}) already inserted in the cache", key));
            }

            SqlCacheDependency sqlCacheDependency = null;
            AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();

            switch (key)
            {
                case CachedObjectsKey.AttachmentTypes:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AttachmentTypes");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.Cultures:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Cultures");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.ExternalParties:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "ExternalParties");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.LetterTypes:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "LetterTypes");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.LinkTypes:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Links");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.Lookups:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Lookups");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "LookupLocalizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.OrgUnits:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "OrgUnits");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "OrgUnitLinks");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Counters");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPapers");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPaperActions");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPaperBeneficiaries");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.OrgUnitStructure:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "OrgUnits");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "OrgUnitLinks");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Counters");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPapers");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPaperActions");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "AssignmentPaperBeneficiaries");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.Priorities:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Priorities");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                case CachedObjectsKey.TransactionTypes:
                    {
                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "SourceTypes");
                        aggregateCacheDependency.Add(sqlCacheDependency);

                        sqlCacheDependency = new SqlCacheDependency(_databaseName, "Localizations");
                        aggregateCacheDependency.Add(sqlCacheDependency);
                    }
                    break;
                default:
                    {
                        if (key.Contains(CachedObjectsKey.Lookups))
                        {
                            sqlCacheDependency = new SqlCacheDependency(_databaseName, "Lookups");
                            aggregateCacheDependency.Add(sqlCacheDependency);

                            sqlCacheDependency = new SqlCacheDependency(_databaseName, "LookupLocalizations");
                            aggregateCacheDependency.Add(sqlCacheDependency);
                        }
                    }
                    break;
            }

            lock (cache)
            {
                if (cache[cultureKey] == null)
                {
                    cache.Insert(cultureKey, kValue, aggregateCacheDependency, absoluteExpiration, slidingExpiration,
                        priority, onRemoveCallback);
                }
            }
        }

        public void Insert(string key, object kValue, string culture, int maxCacheExpirationPeriodInMinutes)
        {
            DateTime absoluteExpiration = DateTime.UtcNow.AddMinutes(maxCacheExpirationPeriodInMinutes);

            Insert(key, kValue, null, absoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null, culture);
        }

        public object Remove(string key, string culture)
        {
            string hostName = HttpContextHelper.HostName;

            key = hostName + key;

            return cache.Remove(GetCultureKey(key, culture));
        }

        public void RemoveBasedOnPrefix(string prefix)
        {
            string hostName = HttpContextHelper.HostName;

            prefix = hostName + prefix;

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
    }
}
