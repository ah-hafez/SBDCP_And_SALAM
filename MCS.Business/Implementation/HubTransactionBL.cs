using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class HubTransactionBL : IHubTransactionBL
    {
        public List<HubTransaction> GetOriginalHubTransactions(int TypeId)
        {
            List<HubTransaction> hubTransactionList = new List<HubTransaction>();
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                hubTransactionList = hubTransactionRepository.GetOriginalHubTransactions(TypeId);
                return hubTransactionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HubTransaction GetHubTransactionById(int TransactionId)
        {
            HubTransaction hubTransaction = new HubTransaction();
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                hubTransaction = hubTransactionRepository.GetHubTransactionById(TransactionId);
                return hubTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Add(HubTransaction hubTransaction)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                var hubTransactionId = hubTransactionRepository.Add(hubTransaction);
                return hubTransactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HubTransaction GetByTransactionNumber(string transactionNumber, int orgUnitId, OutboundClassification outboundClassification)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                return hubTransactionRepository.GetByTransactionNumber(transactionNumber, orgUnitId, outboundClassification);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(HubTransaction hubTransaction)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                hubTransactionRepository.Delete(hubTransaction.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Confirm(HubTransaction hubTransaction, long? NewTransactionId, DateTime? NewTransactionTimeStamp)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                hubTransactionRepository.Confirm(hubTransaction.Id, NewTransactionId, NewTransactionTimeStamp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Reject(HubTransaction hubTransaction)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                hubTransactionRepository.Reject(hubTransaction.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool MarkHubCopyAsSeen(int transactionId)
        {
            try
            {
                IHubTransactionRepository hubTransactionRepository = IoC.Resolve<IHubTransactionRepository>();
                return hubTransactionRepository.MarkCopyAsSeen(transactionId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
