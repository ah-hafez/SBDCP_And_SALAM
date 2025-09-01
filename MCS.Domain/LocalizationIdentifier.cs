using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class LocalizationIdentifier : EntityBase, ILocalizeEntity
    {       
        public virtual IList<Localization> Localizations { get; set; }
    }
}
