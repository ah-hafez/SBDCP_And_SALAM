using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.SqlClient;
using MCS.Framework;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.Framework.Web;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business.ASPNETIdentity
{
    public class MultiTenantCustomIdentityDbContext : IdentityDbContext<ASPNetIdentityUser>, IDbContext
    {
        public MultiTenantCustomIdentityDbContext()
            : base("eMorasalatTenants")
        {
        }

        public MultiTenantCustomIdentityDbContext(string databaseName)
            : base(databaseName)
        {
        }

        public static MultiTenantCustomIdentityDbContext Create()
        {
            return new MultiTenantCustomIdentityDbContext();
        }

        public static MultiTenantCustomIdentityDbContext Create(string databaseName)
        {
            return new MultiTenantCustomIdentityDbContext(databaseName);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SystemConfigurations.SchemaNameDatabaseType);

            base.OnModelCreating(modelBuilder);
        }
    }
}
