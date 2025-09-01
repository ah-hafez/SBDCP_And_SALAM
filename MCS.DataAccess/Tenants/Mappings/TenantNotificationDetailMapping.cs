using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Tenants.Mappings
{
   public class TenantNotificationDetailMapping : EntityTypeConfiguration<TenantNotificationDetail>
    {
        public TenantNotificationDetailMapping()
        {
         //  this.Property(x=>x.Attachments)
        }
    }
}
