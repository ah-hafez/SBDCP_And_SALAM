using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Domain
{
    public class TenantLocalization : EntityBase, IText
    {
        public int CultureId { get; set; }
        public int LocalizationIdentifierId { get; set; }
        public virtual TenantCulture Culture { get; set; }
        public virtual string Text { get; set; }
        public virtual TenantLocalizationIdentifier LocalizationIdentifier { get; set; }
    }
}
