using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionTypeRepository : IRepository<Domain.TransactionType>
    {
        int AddTransactionType(Domain.TransactionType transactionType);
        void UpdateTransactionType(Domain.TransactionType transactionType);
        void DeleteTransactionType(int id);
        TransactionType GetTransactionTypeById(int transactionTypeId);
        IList<Domain.TransactionType> GetTransactionTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<Domain.TransactionType> GetTransactionTypes(string cultureName);
        IList<Domain.TransactionType> GetTransactionTypesByUserId(int userId, TransactionCategories transactionCategories, string cultureName);
        IList<Domain.TransactionType> GetTransactionTypes(TransactionCategories transactionCategories, string cultureName);
        IList<Domain.TransactionType> UserMobileGetTransactionTypes(TransactionCategories transactionCategories, string cultureName);
    }
}
