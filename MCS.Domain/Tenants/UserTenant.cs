using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserTenant : EntityBase
    {
        public string UserName { get; set; }
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
