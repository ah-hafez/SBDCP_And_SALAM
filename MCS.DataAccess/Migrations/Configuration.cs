namespace MCS.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<MCSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MCSDbContext context)
        {
            //This method will be called after migrating to the latest version.
            //If you need to create a new database with initial data execute the following command:
            ////Update-Database -ConfigurationTypeName MCS.DataAccess.Migrations.Configuration -Verbose
            //var dbMigrationBase = new DbMigrationBase(context);
            //dbMigrationBase.ExecuteSqlFilesWithinSeedFolder(@"Migrations");
        }
    }
}
