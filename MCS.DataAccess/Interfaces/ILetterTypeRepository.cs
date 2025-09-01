using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ILetterTypeRepository : IRepository<LetterType>
    {

        int AddLetterType(LetterType letterType);
        void UpdateLetterType(LetterType letterType);
        void DeleteLetterType(int id);
        LetterType GetLetterTypeById(int letterType);
        IList<LetterType> GetLetterTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<LetterType> GetLetterTypes(string cultureName);
        IList<LetterType> GetLetterTypes(TransactionCategories transactionCategories, string cultureName);
    }
}
