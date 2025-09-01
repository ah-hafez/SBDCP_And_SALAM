using MCS.Common;
using MCS.Domain; 
using MCS.Framework.Persistence;
using System.Collections.Generic;
namespace MCS.Business
{
    public interface IFollowUpPriorityTypeBL
    {
        int AddFollowUpPrioritytype(FollowUpPriorityType followUpLook);
        void UpdateFollowUpPrioritytype(FollowUpPriorityType followUpLook);
        void DeleteFollowUpPrioritytype(IList<int> ids, out IList<int> followUpTypesCannotBeDeleted);
        FollowUpPriorityType GetFollowUpPrioritytypeId(int FollowUpId);
        IList<FollowUpPriorityType> GetFollowUpPrioritytypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpPriorityType> GetFollowUpPrioritytypes(TransactionCategories transactionCategories, string cultureName);
    }
}
