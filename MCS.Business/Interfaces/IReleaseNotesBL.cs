using System.Collections.Generic;
using MCS.Domain;
using MCS.Framework.Persistence;

namespace MCS.Business
{
    public interface IReleaseNotesBL
    {
        int ReleaseNotesAdd(Domain.ReleaseNote release);
        void ReleaseNotesDelete(IList<int> ids, out IList<int> actionesCannotBeDeleted);
        void ReleaseNotesUpdate(Domain.ReleaseNote release);
        IList<ReleaseNote> ReleaseNotesSelect(SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        ReleaseNote ReleaseNotesSelectById(int noteId);
    }
}
