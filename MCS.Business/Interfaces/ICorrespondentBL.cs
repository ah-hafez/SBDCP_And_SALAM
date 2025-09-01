using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.Business
{
    public interface ICorrespondentBL
    {
        int AddReporter(Reporter Reporter);
        void UpdateReporter(Reporter Reporter);
        void DeleteReporter(int id);
        Reporter GetReporterById(int ReporterId);
        IList<Reporter> GetReporters(SearchCriteria searchCriteria, out int rowsCount);
        void LockUnlockLookup(int ReporterId, int UserId);
        void ActiveDeactiveLookup(int ReporterId);
    }
}
