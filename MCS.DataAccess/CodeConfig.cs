using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using MCS.Common;

namespace MCS.DataAccess
{
    public class CodeConfig : DbConfiguration
    {
        private const string EFOracleProviderInvariantName = "Oracle.ManagedDataAccess.Client";
        public CodeConfig()
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                SetDefaultConnectionFactory(new OracleConnectionFactory());
                SetProviderServices(EFOracleProviderInvariantName, EFOracleProviderServices.Instance);
                SetProviderFactory(EFOracleProviderInvariantName, new OracleClientFactory());
            }
            else
            {
                SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
                var localDb = new LocalDbConnectionFactory("v11.0");
                SetDefaultConnectionFactory(localDb);
            }
            AddInterceptor(new DbCommandInterceptor());
        }
    }
}
