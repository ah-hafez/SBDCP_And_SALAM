using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        int AddPriority(Priority priority);
        void UpdatePriority(Priority priority);
        void DeletePriority(int id);
        Priority GetPriorityById(int priorityId);
        Priority GetPriorityById(SearchCriteria searchCriteria, int priorityId, out int PriorityExceptionsRowsCount);
        IList<Priority> GetPriorities(SearchCriteria searchCriteria, out int rowsCount);
        IList<Priority> GetPriorities(string cultureName, int OrgUnitId = 0, int UserId = 0);

    }
}
