namespace MCS.DataAccess.OracleMigrations
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<MCSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MCSDbContext context)
        {
            //This method will be called after migrating to the latest version.
            //If you need to create a new database with initial data execute the following command:
            //Add-Migration -ConfigurationTypeName MCS.DataAccess.OracleMigrations.Configuration
            //Update-Database -ConfigurationTypeName MCS.DataAccess.OracleMigrations.Configuration
            //var dbMigrationBase = new DbMigrationBase(context);
            //dbMigrationBase.ExecuteSqlFilesWithinSeedFolder(@"OracleMigrations");
        }
    }
}
