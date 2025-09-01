using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ILinkRepository : IRepository<Link>
    {
        int AddLink(Link link);
        void UpdateLink(Link link);
        void DeleteLink(int id);
        Link GetLinkById(int linkId);
        IList<Link> GetLinks(SearchCriteria searchCriteria, out int rowsCount);
        IList<Link> GetLinks(TransactionCategories transactionCategories, string cultureName);
        bool CheckIfLinkTypeUsed(int linkTypeId);
        void LockUnlockLookup(int LinkId, int UserId);
        void ActiveDeactiveLookup(int LinkId);
    }
}
