using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IReleaseNotesRepository : IRepository<ReleaseNote>
    {
        int ReleaseNotesAdd(Domain.ReleaseNote release);
        void ReleaseNotesDelete(int id);
        void ReleaseNotesUpdate(Domain.ReleaseNote release);
        IList<ReleaseNote> ReleaseNotesSelect(SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        IList<ReleaseNote> ReleaseNotesSelect();
        bool ReleaseNotesCheckIfUsed(int id);
    }
}
