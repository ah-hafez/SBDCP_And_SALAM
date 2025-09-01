using System.Collections.Generic;
using MCS.Framework.Persistence;

namespace MCS.UI
{
    public interface ISearcher
    {
        IList<ISearchResult> Search(SearchCriteria searchCriteria, out int rowsCount);
    }
}
