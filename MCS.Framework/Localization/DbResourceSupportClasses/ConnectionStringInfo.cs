using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Localization
{
    /// <summary>
    /// Used to parse a connection string or connection string name
    ///             into a the base connection  string and dbProvider.
    /// 
    ///             If a connection string is passed that's just used.
    ///             If a ConnectionString entry name is passed the connection
    ///             string is extracted and the provider parsed.
    /// 
    /// </summary>
    public class ConnectionStringInfo
    {
        /// <summary>
        /// The default connection string provider
        /// 
        /// </summary>
        public static string DefaultProviderName = "System.Data.SqlClient";

        /// <summary>
        /// The connection string parsed
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The DbProviderFactory parsed from the connection string
        ///             or default provider
        /// 
        /// </summary>
        public DbProviderFactory Provider { get; set; }

        /// <summary>
        /// Figures out the Provider and ConnectionString from either a connection string
        ///             name in a config file or full  ConnectionString and provider.
        /// 
        /// </summary>
        /// <param name="connectionString">Config file connection name or full connection string</param><param name="providerName">optional provider name. If not passed with a connection string is considered Sql Server</param>
        public static ConnectionStringInfo GetConnectionStringInfo(string connectionString, string providerName = null)
        {
            ConnectionStringInfo connectionStringInfo = new ConnectionStringInfo();

            if (!connectionString.Contains("="))
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionString];

                connectionStringInfo.Provider = string.IsNullOrEmpty(connectionStringSettings.ProviderName) ? DbProviderFactories.GetFactory(ConnectionStringInfo.DefaultProviderName) : DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
                connectionString = connectionStringSettings.ConnectionString;
            }
            else
            {
                if (providerName == null)
                    providerName = ConnectionStringInfo.DefaultProviderName;
                connectionStringInfo.Provider = DbProviderFactories.GetFactory(providerName);
            }

            connectionStringInfo.ConnectionString = connectionString;
            return connectionStringInfo;
        }
    }
}
