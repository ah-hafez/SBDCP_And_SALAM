using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class CorrespondentBL : BaseBL, ICorrespondentBL
    {
        public void ActiveDeactiveLookup(int ReporterId)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            correspondentRepository.ActiveDeactiveLookup(ReporterId);
        }

        public int AddReporter(Reporter Reporter)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            return correspondentRepository.AddReporter(Reporter);
        }

        public void DeleteReporter(int id)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            correspondentRepository.DeleteReporter(id);
        }

        public Reporter GetReporterById(int ReporterId)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            return correspondentRepository.GetReporterById(ReporterId);
        }

        public IList<Reporter> GetReporters(SearchCriteria searchCriteria, out int rowsCount)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            return correspondentRepository.GetReporters(searchCriteria, out rowsCount);
        }

        public void LockUnlockLookup(int ReporterId, int UserId)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            correspondentRepository.LockUnlockLookup(ReporterId, UserId);
        }

        public void UpdateReporter(Reporter reporter)
        {
            ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
            correspondentRepository.UpdateReporter(reporter);
        }
    }
}
