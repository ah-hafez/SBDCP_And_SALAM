using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface ILookupBL
    {
        Lookup GetLookupItem(int lookupId);
        Lookup GetLookupItem(int lookupId, string cultureName);
        IList<Lookup> GetLookupItems(LookupCategory lookupCategoryID, string cultureName);
        int AddLookupItem(Lookup lookup);
        void LockUnlockLookup(int lookupType, int lookUpId, int UserId);
        void ActiveDeactiveLookup(int lookupType, int lookUpId);
        void UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType);
        void UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType);
        IList<Lookup> GetLookupItemsWithoutCach(LookupCategory lookupCategory, string cultureName);
        IList<Lookup> GetActiveLookupItemsWithoutCach(LookupCategory lookupCategory, string cultureName);

    }
}
