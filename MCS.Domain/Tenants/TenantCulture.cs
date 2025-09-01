using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TenantCulture : EntityBase
    {
        public string ShortName { get; set; }
        public int? NameId { get; set; }
        public virtual TenantLookup Name { get; set; }
    }
}
