using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Domain
{
    public class Localization : EntityBase,IText
    {
        public int CultureId { get; set; }
        public virtual Culture Culture { get; set; }
        public virtual string Text { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
    }
}
