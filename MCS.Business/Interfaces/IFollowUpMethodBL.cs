using MCS.Common;
using MCS.Domain; 
using MCS.Framework.Persistence;
using System.Collections.Generic;
namespace MCS.Business
{
    public interface IFollowUpMethodBL
    {
        int AddFollowUpMethod(FollowUpMethod followUpLook);
        void UpdateFollowUpMethod(FollowUpMethod followUpLook);
        void DeleteFollowUpMethod(IList<int> ids, out IList<int> followUpTypesCannotBeDeleted);
        FollowUpMethod GetFollowUpMethodId(int FollowUpId);
        IList<FollowUpMethod> GetFollowUpMethods(SearchCriteria searchCriteria, out int rowsCount);
        IList<FollowUpMethod> GetFollowUpMethods(TransactionCategories transactionCategories, string cultureName);
    }
}
