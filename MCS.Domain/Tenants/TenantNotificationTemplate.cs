using System;
using MCS.Framework.Entities;


namespace MCS.Domain
{
    public class TenantNotificationTemplate : EntityBase
    {
        public int TypeId { get; set; }
        public virtual TenantLookup Type { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
    }
}
