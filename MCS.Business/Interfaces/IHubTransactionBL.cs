using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IHubTransactionBL
    {
        int Add(HubTransaction hubTransaction);
        HubTransaction GetByTransactionNumber(string transactionNumber, int orgUnitId, OutboundClassification outboundClassification);
        void Delete(HubTransaction hubTransaction);
        void Confirm(HubTransaction hubTransaction, long? NewTransactionId, DateTime? NewTransactionTimeStamp);
        void Reject(HubTransaction hubTransaction);
        List<HubTransaction> GetOriginalHubTransactions(int TypeId);
        HubTransaction GetHubTransactionById(int TransactionId);
        bool MarkHubCopyAsSeen(int transactionId);
    }
}
