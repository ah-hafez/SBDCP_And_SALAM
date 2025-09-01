using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IFollowUpProccessRepository : IRepository<FollowUpProccess>
    {
        int AddFollowUpProccess(FollowUpProccess followUpProccess);
        void UpdateFollowUpProccess(FollowUpProccess followUpProccess);
        void DeleteFollowUpProccess(int id);
        FollowUpProccess GetFollowUpProccessById(int followUpProccessId);
        IList<FollowUpProccess> GetFollowUpProccesss(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpProccess> GetFollowUpProccesss(TransactionCategories transactionCategories, string cultureName);
        bool CheckIfFollowUpProccessUsed(int followUpProccessId);
        void LockUnlockLookup(int followUpProccessId, int UserId);
        void ActiveDeactiveLookup(int followUpProccessId);
    }
}
