using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface ILetterTypeBL
    {
        int AddLetterType(LetterType letterType);
        void UpdateLetterType(LetterType letterType);
        void DeleteLetterTypes(IList<int> ids, out IList<int> letterTypesCannotBeDeleted);
        LetterType GetLetterTypeById(int letterTypeId);
        IList<LetterType> GetLetterTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<LetterType> GetLetterTypes(TransactionCategories transactionCategories, string cultureName);
    }
}
