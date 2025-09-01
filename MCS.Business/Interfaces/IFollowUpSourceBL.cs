using MCS.Common;
using MCS.Domain; 
using MCS.Framework.Persistence;
using System.Collections.Generic;
namespace MCS.Business
{
    public interface IFollowUpSourceBL
    {
        int AddFollowUpSource(FollowUpSource followUpLook);
        void UpdateFollowUpSource(FollowUpSource followUpLook);
        void DeleteFollowUpSource(IList<int> ids, out IList<int> followUpTypesCannotBeDeleted);
        FollowUpSource GetFollowUpSourceId(int FollowUpId);
        IList<FollowUpSource> GetFollowUpSources(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpSource> GetFollowUpSources(TransactionCategories transactionCategories, string cultureName);
    }
}
