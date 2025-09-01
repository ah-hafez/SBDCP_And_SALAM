using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionHistoryBL : BaseBL, ITransactionHistoryBL
    {
        public IList<TransactionHistory> GetTransactionHistory(int transactionId, string cultureName)
        {
            try
            {
                ITransactionHistoryRepository transactionHistoryRepository = IoC.Resolve<TransactionHistoryRepository>();
                return transactionHistoryRepository.GetTransactionHistory(transactionId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int AddTransactionHistory(Transaction transaction)
        {
            try
            {
                TransactionHistory transactionHistory = new TransactionHistory()
                {
                    UserId = User.Id,
                    TransactionId = transaction.Id,
                    ConfidentialityId = transaction.ConfidentialityId,
                    DestinationId = transaction.EntityId != null && transaction.EntityId > 0 ? transaction.EntityId : (int?)null,
                    ExternalPartyId = transaction.ExternalPartyId > 0 ? transaction.ExternalPartyId : null,
                    LetterTypeId = transaction.LetterTypeId,
                    PriorityId = transaction.PriorityId,
                    Remarks = transaction.Remarks,
                    SignedByUserId = transaction.SignedByUserId,
                    StatusId = transaction.StatusId,
                    Subject = transaction.Subject,
                    PrintedDeliveryReport = transaction.PrintedDeliveryReport,
                    AttchmentCount = transaction.Attachments.Count,
                    ToEntityId = transaction.EntityId != null && transaction.EntityId > 0 ? transaction.EntityId : (int?)null,
                    ToUserId = transaction.ToUserId,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    DeliveryReportNumber = transaction.DeliveryReportNumber,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.DateH,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    TransactionTypeId = transaction.TransactionTypeId,
                    LetterNumber = transaction.LetterNumber
                };

                ITransactionHistoryRepository transactionHistoryRepository = IoC.Resolve<TransactionHistoryRepository>();
                return transactionHistoryRepository.AddTransactionHistory(transactionHistory);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public TransactionHistory GetLastTransactionHistory(int transactionId)
        {
            try
            {
                ITransactionHistoryRepository transactionHistoryRepository = IoC.Resolve<TransactionHistoryRepository>();
                return transactionHistoryRepository.GetLastTransactionHistory(transactionId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public TransactionHistory GetTransactionHistoryById(int transactionHistoryId)
        {
            try
            {
                ITransactionHistoryRepository transactionHistoryRepository = IoC.Resolve<TransactionHistoryRepository>();
                return transactionHistoryRepository.Get(transactionHistoryId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
