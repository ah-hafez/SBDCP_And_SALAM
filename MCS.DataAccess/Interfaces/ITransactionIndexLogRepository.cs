using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionIndexLogRepository : IRepository<TransactionIndexLog>
    {
        int AddIndex(TransactionIndexLog transactionIndex);
        void UpdateIndex(TransactionIndexLog transactionIndex);
        TransactionIndexLog GetIndexByTransactionId(int transactionId);
        IList<TransactionIndexLog> GetIndexedTransactions(Expression<Func<TransactionIndexLog, bool>> @where);

    }
}
