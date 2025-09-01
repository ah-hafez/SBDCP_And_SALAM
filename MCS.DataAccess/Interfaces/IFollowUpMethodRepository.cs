using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IFollowUpMethodRepository : IRepository<FollowUpMethod>
    {
        int AddFollowUpMethod(FollowUpMethod followUpMethod);
        void UpdateFollowUpMethod(FollowUpMethod followUpMethod);
        void DeleteFollowUpMethod(int id);
        FollowUpMethod GetFollowUpMethodById(int followUpMethodId);
        IList<FollowUpMethod> GetFollowUpMethods(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpMethod> GetFollowUpMethods(TransactionCategories transactionCategories, string cultureName);
        bool CheckIfFollowUpMethodUsed(int followUpMethodId);
        void LockUnlockLookup(int followUpMethodId, int UserId);
        void ActiveDeactiveLookup(int followUpMethodId);
    }
}
