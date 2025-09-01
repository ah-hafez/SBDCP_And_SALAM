using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public  class TenantLookup:  EntityBase
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
        public int? EnumReference { get; set; }
        public virtual IList<TenantLookupLocalization> Localizations { get; set; }
        public string Text { get; set; }
    }
}
