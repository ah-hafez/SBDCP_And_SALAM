using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess.Mappings;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class MasterDbContext : DbContextBase, IDbModelCacheKeyProvider
    {
        public static EntityState State;
        static MasterDbContext()
        {
            Database.SetInitializer<MasterDbContext>(new NullDatabaseInitializer<MasterDbContext>());
        }

        public MasterDbContext() : base("eMorasalatTenants")
        {
            Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<UserTenant> UserTenants { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantLookup> TenantLookups { get; set; }
        public DbSet<TenantCulture> Cultures { get; set; }
        public DbSet<TenantNotification> Notifications { get; set; }
        public DbSet<TenantNotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<TenantNotificationDetail> TenantNotificationDetails { get; set; }

        public string CacheKey
        {
            get
            {
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.Database.Connection.ConnectionString);
                    return sqlConnectionStringBuilder.InitialCatalog + "_" + sqlConnectionStringBuilder.DataSource;
                }
                else
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.Database.Connection.ConnectionString);
                    return sqlConnectionStringBuilder.UserID + "_" + sqlConnectionStringBuilder.DataSource;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SystemConfigurations.MultiTenantSchemaName);

            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(1000));
            }

            modelBuilder.Configurations.Add(new TenantMapping());
            modelBuilder.Configurations.Add(new TenantLookupMapping());
            modelBuilder.Configurations.Add(new TenantLocalizationMapping());
            modelBuilder.Configurations.Add(new TenantLocalizationIdentifierMapping());
            modelBuilder.Configurations.Add(new TenantCultureMapper());
            modelBuilder.Configurations.Add(new AspNetRoleMapping());
            modelBuilder.Configurations.Add(new AspNetUserLoginMapping());
            modelBuilder.Configurations.Add(new AspNetUserClaimMapping());
            modelBuilder.Configurations.Add(new AspNetUserMapping());
            modelBuilder.Configurations.Add(new ResourceMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
