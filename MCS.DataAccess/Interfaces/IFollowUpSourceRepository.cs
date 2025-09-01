using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IFollowUpSourceRepository : IRepository<FollowUpSource>
    {
        int AddFollowUpSource(FollowUpSource followUpSource);
        void UpdateFollowUpSource(FollowUpSource followUpSource);
        void DeleteFollowUpSource(int id);
        FollowUpSource GetFollowUpSourceById(int followUpSourceId);
        IList<FollowUpSource> GetFollowUpSources(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpSource> GetFollowUpSources(TransactionCategories transactionCategories, string cultureName);
        bool CheckIfFollowUpSourceUsed(int followUpSourceId);
        void LockUnlockLookup(int followUpSourceId, int UserId);
        void ActiveDeactiveLookup(int followUpSourceId);
    }
}
