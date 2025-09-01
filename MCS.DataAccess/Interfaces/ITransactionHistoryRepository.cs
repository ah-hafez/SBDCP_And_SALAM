using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionHistoryRepository : IRepository<TransactionHistory>
    {
        int AddTransactionHistory(TransactionHistory transactionHistory);
        TransactionHistory GetLastTransactionHistory(int transactionId);
        TransactionHistory GetTransactionHistoryById(int transactionHistoryId);
        IList<TransactionHistory> GetTransactionHistory(int transactionId, string cultureName);

    }
}
