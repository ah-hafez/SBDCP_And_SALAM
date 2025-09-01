using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DataAccess.Tenants.Migrations
{
    internal sealed class TenantsConfiguration : DbMigrationsConfiguration<MasterDbContext>
    {
        public TenantsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Tenants\Migrations";
        }

        protected override void Seed(MasterDbContext context)
        {
            //This method will be called after migrating to the latest version.
            //If you need to create a new database with initial data execute the following command:
            //Update-Database -ConfigurationTypeName MCS.DataAccess.Tenants.Migrations.TenantsConfiguration

            /*
             Oracle Multitenancy database:
             - Set service as startup project.
             - Alter the connection string.
                * Database name: ALL CAPS.
             - Set <add key="MultiTenantSchemaName"> value to be the same as the database name in connection string.
                * ALL CAPS
             - <add key="MultiTenantEnabled" value="false"/>
             - Add-Migration -ConfigurationTypeName MCS.DataAccess.Tenants.Migrations.TenantsConfiguration
             - Update-Database -ConfigurationTypeName MCS.DataAccess.Tenants.Migrations.TenantsConfiguration
             - <add key="IsOracleMigrationEnabled" value="true"/>
             - <add key="MultiTenantEnabled" value="true"/>
             - <add key="MultiTenantCreateDatabaseEnabled" value="false"/>
             */

            //var dbMigrationBase = new DbMigrationBase(context);
            //dbMigrationBase.ExecuteSqlFilesWithinSeedFolder(@"Tenants/Migrations");
        }
    }
}
