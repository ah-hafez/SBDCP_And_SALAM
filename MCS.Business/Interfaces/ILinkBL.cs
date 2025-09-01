using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface ILinkBL
    {
        int AddLink(Link link);
        void UpdateLink(Link link);
        void DeleteLinks(IList<int> ids, out IList<int> linkTypesCannotBeDeleted);
        Link GetLinkById(int linkId);
        IList<Link> GetLinks(SearchCriteria searchCriteria, out int rowsCount);
        IList<Link> GetLinks(TransactionCategories transactionCategories, string cultureName);
    }
}
