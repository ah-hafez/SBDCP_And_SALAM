using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Domain
{
    public class LookupLocalization : EntityBase,IText
    {
        public virtual Lookup Lookup { get; set; }
        public virtual Culture Culture { get; set; }
        public string Text { get; set; } 
    }
}
