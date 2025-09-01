using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Web;
using MCS.Framework;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.Framework.Web;
using MCS.Common;
using MCS.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Core.Metadata.Edm;

namespace MCS.Business.ASPNETIdentity
{
    public class CustomIdentityDbContext : IdentityDbContext<ASPNetIdentityUser>, IDbContext, IDbModelCacheKeyProvider
    {
        public CustomIdentityDbContext()
            : base("eMorasalat")
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                //string headerTenantId = HttpContextHelper.GetHeaderValue(Constants.TenantId);
                //TenantInfo tenantInfo = null;
                //if (MultiTenantsContext.LoggedInTenant != null)
                //{
                //    tenantInfo = MultiTenantsContext.LoggedInTenant as TenantInfo;
                //}
                //if (int.TryParse(headerTenantId, out int tenantId))
                //{
                //    ITenantBL tenantBL = IoC.Resolve<ITenantBL>();
                //    Tenant tenant = tenantBL.GetTenantById(tenantId, true);
                //    tenantInfo = new TenantInfo
                //    {
                //        Id = tenant.Id,
                //        HostName = tenant.HostName,
                //        DatabaseName = tenant.DatabaseName,
                //        ECMProfileId = tenant.ECMProfileId,
                //        ECMCategoryId = tenant.ECMCategoryId
                //    };
                //}

                //string hostName = HttpContextHelper.GetHeaderValue(Constants.SubDomainName);
                //if (!string.IsNullOrEmpty(hostName) && tenantInfo == null)
                //{
                //    ITenantBL tenantBL = IoC.Resolve<ITenantBL>();
                //    Tenant tenant = tenantBL.GetTenantByHostName(hostName, false);
                //    tenantInfo = new TenantInfo
                //    {
                //        Id = tenant.Id,
                //        HostName = tenant.HostName,
                //        DatabaseName = tenant.DatabaseName
                //    };
                //}

                //if (tenantInfo != null)
                //{
                //MultiTenantsContext.SetLoggedInTenantInWebSession(tenantInfo);
                if (!string.IsNullOrWhiteSpace(TenantHelper.GetTenantDatabaseNameFromHeader()))
                {
                    if (!SystemConfigurations.IsOracleMigrationEnabled)
                    {
                        IConnectionStringBuilder connectionStringBuilder = IoC.Resolve<IConnectionStringBuilder>();
                        string updateDatabaseConnection = connectionStringBuilder.UpdateDatabaseName(Database.Connection.ConnectionString, TenantHelper.GetTenantDatabaseNameFromHeader());
                        Database.Connection.ConnectionString = updateDatabaseConnection;
                    }
                    else
                    {
                        IOracleConnectionStringBuilder oracleConnectionStringBuilder = IoC.Resolve<IOracleConnectionStringBuilder>();
                        string updateDatabaseConnection = oracleConnectionStringBuilder.UpdateDatabaseName(Database.Connection.ConnectionString, TenantHelper.GetTenantDatabaseNameFromHeader());
                        Database.Connection.ConnectionString = updateDatabaseConnection;
                    }
                }
                //}
            }
        }

        public CustomIdentityDbContext(string databaseName)
            : base(databaseName)
        {
        }

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

        public static CustomIdentityDbContext Create()
        {
            return new CustomIdentityDbContext();
        }

        public static CustomIdentityDbContext Create(string databaseName)
        {
            return new CustomIdentityDbContext(databaseName);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.Database.Connection.ConnectionString);
                    //modelBuilder.HasDefaultSchema(sqlConnectionStringBuilder.InitialCatalog);
                    modelBuilder.HasDefaultSchema(SystemConfigurations.SchemaNameDatabaseType);
                }
                else
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(this.Database.Connection.ConnectionString);
                    modelBuilder.HasDefaultSchema(sqlConnectionStringBuilder.UserID);
                }


            }
            else
            {
                modelBuilder.HasDefaultSchema(SystemConfigurations.SchemaNameDatabaseType);
            }

            modelBuilder.Conventions.Add<CapitalizeColumnName>();
            modelBuilder.Conventions.Add<CapitalizeTableName>();

            base.OnModelCreating(modelBuilder);
        }

        public class CapitalizeColumnName : IStoreModelConvention<EdmProperty>
        {
            public void Apply(EdmProperty item, DbModel model)
            {
                item.Name = item.Name.ToUpper();
            }
        }
        public class CapitalizeTableName : IStoreModelConvention<EntitySet>
        {
            public void Apply(EntitySet item, DbModel model)
            {
                item.Table = item.Table.ToUpper();
            }
        }
    }
}
