using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class HubTransactionRepository : BaseRepository<HubTransaction>, IHubTransactionRepository
    {
        public HubTransactionRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }
        public List<HubTransaction> GetOriginalHubTransactions(int TypeId)
        {
            try
            {
                var classification = (OutboundClassification)TypeId;
                List<HubTransaction> hubTransactionList = _oMCSDbContext.HubTransactions.Where(
                    t => t.Status == HubTransactionStatus.Pending &&
                    t.Classification == classification &&
                    !t.IsDeleted).OrderByDescending(t => t.CreatedOn).ToList();
                return hubTransactionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HubTransaction GetHubTransactionById(int TransactionId)
        {
            try
            {
                HubTransaction hubTransaction = _oMCSDbContext.HubTransactions.Where(
                    t => t.Status == HubTransactionStatus.Pending &&
                    t.Id == TransactionId &&
                    !t.IsDeleted).FirstOrDefault();
                return hubTransaction;
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
                HubTransaction hubTransaction = _oMCSDbContext.HubTransactions.Where(
                    ht => ht.TransactionNumber == transactionNumber.ToString() &&
                    ht.OrgUnitId == orgUnitId &&
                    ht.Classification == outboundClassification /*&&*/
                    //ht.IsDeleted //Because the accecpted transaction is logically deleted
                    ).OrderByDescending(ht => ht.Id).FirstOrDefault();
                return hubTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DocumentInfo MapWithContent(DocumentInfo documentInfo)
        {
            if (documentInfo != null)
            {
                DocumentInfo documentDTO = new DocumentInfo()
                {
                    Id = documentInfo.Id,
                    MimeType = documentInfo.MimeType,
                    Name = documentInfo.Name,
                    Size = documentInfo.Size,
                    Document = new Document
                    {
                        Content = documentInfo.Document?.Content
                    }
                };

                return documentDTO;
            }
            return null;
        }
        public void Confirm(int hubTransactionId, long? NewTransactionId, DateTime? NewTransactionTimeStamp)
        {
            try
            {
                HubTransaction hubTransaction = _oMCSDbContext.HubTransactions.First(t => t.Id == hubTransactionId);
                hubTransaction.Status = HubTransactionStatus.Confirmed;
                hubTransaction.NewTransactionId = NewTransactionId;
                hubTransaction.NewTransactionTimestamp = NewTransactionTimeStamp;
                hubTransaction.IsDeleted = true;
                _oMCSDbContext.Entry(hubTransaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void Reject(int hubTransactionId)
        {
            try
            {
                HubTransaction hubTransaction = _oMCSDbContext.HubTransactions.First(t => t.Id == hubTransactionId);
                hubTransaction.Status = HubTransactionStatus.Rejected;
                _oMCSDbContext.Entry(hubTransaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool MarkCopyAsSeen(int transactionId)
        {
            try
            {
                HubTransaction hubTransaction = _oMCSDbContext.HubTransactions.Where(
                    ht => ht.Id == transactionId &&
                    ht.Classification == OutboundClassification.Copy).FirstOrDefault();

                hubTransaction.IsDeleted = true;
                _oMCSDbContext.Entry(hubTransaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
                return hubTransaction.IsDeleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
