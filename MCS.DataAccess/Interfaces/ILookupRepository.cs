using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ILookupRepository : IRepository<Lookup>
    {
        Lookup GetLookupItem(int lookupId);
        Lookup GetLookupItem(int lookupId, string cultureName);
        IList<Lookup> GetLookupItems(int lookupCategoryID, string cultureName);
        int AddLookupItem(Lookup lookup);
        void UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType);
        void UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType);
        void ActiveDeactiveLookup(int lookupId);
        IList<Lookup> GetActiveLookupItems(int lookupCategoryID, string cultureName);
    }
}
