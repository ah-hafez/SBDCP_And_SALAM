using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.Business
{
    public interface ITransactionIndexLogBL
    {
        int AddIndex(TransactionIndexLog transactionIndex);
        void UpdateIndex(TransactionIndexLog transactionIndex);
        IList<TransactionIndexLog> GetIndexedTransactions(Expression<Func<TransactionIndexLog, bool>> @where);
    }
}
