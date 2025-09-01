using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Common
{
    public interface ILookupHelper
    {
        int GetLookupInternalID(int lookupID, LookupCategory lookupCategory, string cultureName);
        int GetLookupIdentity(int lookupInternalID, LookupCategory lookupCategory, string cultureName);
    }
}
