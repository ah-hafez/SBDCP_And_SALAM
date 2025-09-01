using System;
using System.Web.UI.Design;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;

namespace MCS.Framework.Localization
{
    /// <summary>
    /// The configuration class that is used to configure the Resource Provider.
    /// This class contains various configuration settings that the provider requires
    /// to operate both at design time and runtime.
    /// 
    /// The application uses the static Current property to access the actual
    /// configuration settings object. By default it reads the configuration settings
    /// from web.config (at runtime). You can override this behavior by creating your
    /// own configuration object and assigning it to the DbResourceConfiguration.Current property.
    /// </summary>
    public class DbResourceConfiguration
    {
        /// <summary>
        /// A global instance of the current configuration. By default this instance reads its
        /// configuration values from web.config at runtime, but it can be overridden to
        /// assign specific values or completely replace this object. 
        /// 
        /// NOTE: Any assignment made to this property should be made at Application_Start
        /// or other 'application initialization' event so that these settings are applied
        /// BEFORE the resource provider is used for the first time.
        /// </summary>
        public static DbResourceConfiguration Current = null;

        /// <summary>
        /// Static constructor for the Current property - guarantees this
        /// code fires exactly once giving us a singleton instance
        /// of the configuration object.
        /// </summary>
        static DbResourceConfiguration()
        {
            Current = new DbResourceConfiguration(true);
        }

        /// <summary>
        /// Database connection string to the resource data.
        /// 
        /// The string can either be a full connection string or an entry in the 
        /// ConnectionStrings section of web.config.
        /// <seealso>Class DbResource
        /// Compiling Your Applications with the Provider</seealso>
        /// </summary>
        public string ConnectionString
        {
            get
            {
                // If no = in the string assume it's a ConnectionStrings entry instead
                if (!_ConnectionString.Contains("="))
                {
                    try
                    {
                        return ConfigurationManager.ConnectionStrings[_ConnectionString].ConnectionString;
                    }
                    catch { }
                }
                return _ConnectionString;
            }
            set { _ConnectionString = value; }
        }
        private string _ConnectionString = "";

        /// <summary>
        /// Database table name used in the database
        /// </summary>
        public string ResourceTableName
        {
            get { return _ResourceTableName; }
            set { _ResourceTableName = value; }
        }
        private string _ResourceTableName = "Resources";



        /// <summary>
        /// Type that is instantiated to handle Database access
        /// </summary>
        public string DbResourceManagerType
        {
            get { return _DbResourceManagerType; }
            set { _DbResourceManagerType = value; }
        }
        private string _DbResourceManagerType = "MCS.Framework.Localization.DbResourceDataManager";

        /// <summary>
        /// Determines whether any resources that are not found are automatically
        /// added to the resource file.
        /// 
        /// Note only applies to the Invariant culture.
        /// </summary>
        public bool AddMissingResources
        {
            get { return _AddMissingResources; }
            set { _AddMissingResources = value; }
        }
        private bool _AddMissingResources = true;

        /// <summary>
        /// Base constructor that doesn't do anything to the default values.
        /// </summary>
        public DbResourceConfiguration()
        {
        }

        /// <summary>
        /// Default constructor used to read the configuration section to retrieve its values
        /// on startup.
        /// </summary>
        /// <param name="readConfigurationSection"></param>
        public DbResourceConfiguration(bool readConfigurationSection)
        {
            if (readConfigurationSection)
                ReadConfigurationSection();
        }


        /// <summary>
        /// Reads the DbResourceProvider Configuration Section and assigns the values 
        /// to the properties of this class
        /// </summary>
        /// <returns></returns>
        public bool ReadConfigurationSection()
        {
            object TSection = null;
            TSection = WebConfigurationManager.GetWebApplicationSection("DbResourceProvider");
            if (TSection == null)
                return false;

            var section = TSection as DbResourceProviderSection;
            ReadSectionValues(section);

            return true;
        }


        /// <summary>
        /// Reads the actual section values
        /// </summary>
        /// <param name="section"></param>
        private void ReadSectionValues(DbResourceProviderSection section)
        {
            ConnectionString = section.ConnectionString;
            AddMissingResources = section.AddMissingResources;
        }

        #region Allow for provider unloading

        /// <summary>
        /// Keep track of loaded providers so we can unload them
        /// </summary>
        internal static List<IeMorasalatResourceProvider> LoadedProviders = new List<IeMorasalatResourceProvider>();

        /// <summary>
        /// This static method clears all resources from the loaded Resource Providers 
        /// and forces them to be reloaded the next time they are requested.
        /// 
        /// Use this method after you've edited resources in the database and you want 
        /// to refresh the UI to show the newly changed values.
        /// 
        /// This method works by internally tracking all the loaded ResourceProvider 
        /// instances and calling the IwwResourceProvider.ClearResourceCache() method 
        /// on each of the provider instances. This method is called by the Resource 
        /// Administration form when you explicitly click the Reload Resources button.
        /// <seealso>Class DbResourceConfiguration</seealso>
        /// </summary>
        public static void ClearResourceCache()
        {
            foreach (IeMorasalatResourceProvider provider in LoadedProviders)
            {
                provider.ClearResourceCache();
            }

            // clear any resource managers
            DbRes.ClearResources();
        }
        #endregion
    }

    public enum CodeGenerationLanguage
    {
        CSharp,
        Vb
    }
}
