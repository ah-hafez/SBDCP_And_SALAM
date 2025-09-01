using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface ITransactionTypeBL
    {
        int AddTransactionSourceType(Domain.TransactionType transactionType);
        void UpdateTransactionSourceType(Domain.TransactionType transactionType);
        void DeleteTransactionSourceTypes(IList<int> ids, out IList<int> transactionTypesCannotBeDeleted);
        TransactionType GetTransactionSourceTypeById(int transactionTypeId);
        IList<Domain.TransactionType> GetTransactionSourceTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<Domain.TransactionType> GetTransactionTypesByUserId(TransactionCategories sourceTransactionType, string cultureName);
        IList<Domain.TransactionType> GetTransactionSourceTypes(TransactionCategories sourceTransactionType, string cultureName);
    }
}
