using System.Collections.Generic;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Domain
{
    public class Lookup : EntityBase, ILocalizeEntity,IText
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
        public int? EnumReference { get; set; }
        public virtual IList<LookupLocalization> Localizations { get; set; }
        public string Text { get; set; }
    }
}
