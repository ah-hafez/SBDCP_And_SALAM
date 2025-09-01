using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface ITransactionLoggingRepository : IRepository<TransactionLog>
    {
        int Log(TransactionLog transactionLog);
        IList<TransactionLogInfo> GetTransactionLogInfo(int transactionId, string cultureName);
        IList<TransactionLogDetailInfo> GetTransactionLogDetailsInfo(int transactionId, string cultureName, bool IsForPrint, SearchCriteriaCustom searchCriteria, out int itemsCount);
        IList<TransactionLogDetailInfo> GetTransactionLogDetailsInfo(int transactionId, int userId, string cultureName);
        TransactionLog GetFirstView(int transactionId, AuditingActionCode auditingActionCode, int? userId, DateTime sendDate, string cultureName);
    }
}
