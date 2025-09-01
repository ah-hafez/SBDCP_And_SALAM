using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using MCS.Framework.Persistence;
using MCS.Common;
using YesserSqlMigrations = MCS.DataAccess.Migrations;
using YesserOracleMigrations = MCS.DataAccess.OracleMigrations;
using System;

namespace MCS.Business
{
    public class MigrationManager
    {
        private const string CONNECTIONSTRING_NAME = "eMorasalatTenants";
        private const string SQL_PROVIDER = "System.Data.SqlClient";
        private const string ORACLE_PROVIDER = "Oracle.ManagedDataAccess.Client";

        public static bool GenerateConnectionString(string dbName)
        {
            bool isSuccess = true;
            try
            {
                string connectionStringWithNewDatabase = string.Empty;
                string databaseProvider = string.Empty;
                string connectionString = ConfigurationManager.ConnectionStrings[CONNECTIONSTRING_NAME].ConnectionString;
                DbMigrator dbMigrator = null;
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    var tenantsOracleConfiguration = new YesserOracleMigrations.Configuration
                    {
                        TargetDatabase = new DbConnectionInfo(connectionStringWithNewDatabase, ORACLE_PROVIDER)
                    };
                    dbMigrator = new DbMigrator(tenantsOracleConfiguration);
                }
                else
                {
                    var tenantsSQLConfiguration = new YesserSqlMigrations.Configuration();
                    var sqlConnectionBuilder = new SqlConnectionBuilder();
                    connectionStringWithNewDatabase = sqlConnectionBuilder.UpdateDatabaseName(connectionString, dbName);
                    tenantsSQLConfiguration.TargetDatabase = new DbConnectionInfo(connectionStringWithNewDatabase, SQL_PROVIDER);
                    tenantsSQLConfiguration.AutomaticMigrationsEnabled = true;
                    dbMigrator = new DbMigrator(tenantsSQLConfiguration);
                }

                //if (dbMigrator.GetPendingMigrations().Any())
                dbMigrator.Update();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw;
            }
            return isSuccess;
        }
    }
}
