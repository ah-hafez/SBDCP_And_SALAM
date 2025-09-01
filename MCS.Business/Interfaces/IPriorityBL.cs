using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IPriorityBL
    {
        int AddPriority(Priority priority);
        void UpdatePriority(Priority priority);
        void DeletePriorities(IList<int> ids, out IList<int> prioritiesCannotBeDeleted);
        Priority GetPriorityById(int priorityId);
        Priority GetPriorityById(SearchCriteria searchCriteria, int priorityId, out int PriorityExceptionsRowsCount);
        IList<Priority> GetPriorities(SearchCriteria searchCriteria, out int rowsCount);
        IList<Priority> GetPriorities(TransactionCategories transactionCategories, string cultureName, int OrgUnitId = 0, int UserId = 0);
    }
}
