using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Domain
{
    public  class TenantLookupLocalization : EntityBase,IText
    {
     public virtual TenantLookup Lookup { get; set; }
     public virtual TenantCulture Culture { get; set; }
     public string Text { get; set; } 
    }
}
