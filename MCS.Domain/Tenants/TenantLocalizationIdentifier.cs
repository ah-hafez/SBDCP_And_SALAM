using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public  class TenantLocalizationIdentifier :  EntityBase
    {
      public virtual IList<TenantLocalization> Localizations { get; set; }
    }
}
