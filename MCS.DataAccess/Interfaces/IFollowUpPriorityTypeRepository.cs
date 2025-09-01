using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IFollowUpPriorityTypeRepository : IRepository<FollowUpPriorityType>
    {
        int AddFollowUpPriorityType(FollowUpPriorityType followUpPriorityType);
        void UpdateFollowUpPriorityType(FollowUpPriorityType followUpPriorityType);
        void DeleteFollowUpPriorityType(int id);
        FollowUpPriorityType GetFollowUpPriorityTypeById(int followUpPriorityTypeId);
        IList<FollowUpPriorityType> GetFollowUpPriorityTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpPriorityType> GetFollowUpPriorityTypes(TransactionCategories transactionCategories, string cultureName);
        bool CheckIfFollowUpPriorityTypeUsed(int followUpPriorityTypeId);
        void LockUnlockLookup(int followUpPriorityTypeId, int UserId);
        void ActiveDeactiveLookup(int followUpPriorityTypeId);
    }
}
