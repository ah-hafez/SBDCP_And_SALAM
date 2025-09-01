using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IPriorityExceptionRepository : IRepository<PriorityException>
    {
        int AddPriorityException(PriorityException priorityException);
        void UpdatePriorityException(PriorityException priorityException);
        void DeletePriorityException(int priorityExceptionId);
        PriorityException GetPriorityExceptionById(int priorityExceptionId);
        PriorityException GetPriorityExceptionByPriorityId(int priorityId);
        IList<PriorityException> GetPriorityExceptions(SearchCriteria searchCriteria, int priorityId, out int rowsCount);
    }
}
