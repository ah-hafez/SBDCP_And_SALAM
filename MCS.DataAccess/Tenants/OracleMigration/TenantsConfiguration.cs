using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;


namespace YESSER.NCS.MCS.DataAccess.Tenants.OracleMigration
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
            //Update-Database -ConfigurationTypeName YESSER.NCS.MCS.DataAccess.Tenants.OracleMigration.TenantsConfiguration
            //var dbMigrationBase = new DbMigrationBase(context);
            //dbMigrationBase.ExecuteSqlFilesWithinSeedFolder(@"Tenants/Migrations");
        }
    }
}
