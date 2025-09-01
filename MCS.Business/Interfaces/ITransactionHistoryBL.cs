using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ITransactionHistoryBL
    {
        int AddTransactionHistory(Transaction transaction);
        TransactionHistory GetTransactionHistoryById(int transactionHistoryId);
        IList<TransactionHistory> GetTransactionHistory(int transactionId, string cultureName);
        TransactionHistory GetLastTransactionHistory(int transactionId);
    }
}
