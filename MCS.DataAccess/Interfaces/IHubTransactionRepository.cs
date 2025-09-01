using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IHubTransactionRepository : IRepository<HubTransaction>
    {
        List<HubTransaction> GetOriginalHubTransactions(int TypeId);
        HubTransaction GetByTransactionNumber(string transactionNumber, int orgUnitId, OutboundClassification outboundClassification);
        void Confirm(int hubTransactionId, long? NewTransactionId, DateTime? NewTransactionTimeStamp);
        void Reject(int hubTransactionId);
        HubTransaction GetHubTransactionById(int TransactionId);
        bool MarkCopyAsSeen(int transactionId);
    }
}
