using MCS.Common;
using MCS.Domain; 
using MCS.Framework.Persistence;
using System.Collections.Generic;
namespace MCS.Business
{
    public interface IFollowUpProccessBL
    {
        int AddFollowUpProccess(FollowUpProccess followUpLook);
        void UpdateFollowUpProccess(FollowUpProccess followUpLook);
        void DeleteFollowUpProccess(IList<int> ids, out IList<int> followUpTypesCannotBeDeleted);
        FollowUpProccess GetFollowUpProccessId(int FollowUpId);
        IList<FollowUpProccess> GetFollowUpProccess(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpProccess> GetFollowUpProccess(TransactionCategories transactionCategories, string cultureName);
    }
}
