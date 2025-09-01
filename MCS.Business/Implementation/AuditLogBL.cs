using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Business
{
    public class AuditLogBL : BaseBL, IAuditLogBL
    {
        #region TransactionLog
        public int Log(TransactionLog transactionLog)
        {
            try
            {
                ITransactionLoggingRepository transactionLoggingRepository = IoC.Resolve<TransactionLoggingRepository>();

                int logId = transactionLoggingRepository.Log(transactionLog);

                return logId;
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
        public IList<TransactionLogInfo> GetTransactionLogInfo(int transactionId, string cultureName)
        {
            try
            {
                ITransactionLoggingRepository transactionLoggingRepository = IoC.Resolve<TransactionLoggingRepository>();

                IList<TransactionLogInfo> transactionLogInfos = transactionLoggingRepository.GetTransactionLogInfo(transactionId, cultureName);

                return transactionLogInfos.ToList();
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
        public IList<AuditLog> GetAuditLog( string cultureName, bool IsForPrint, SearchCriteriaCustom searchCriteria, out int itemsCount)
        {
            try
            {
                IAuditLogRepository transactionLoggingRepository = IoC.Resolve<AuditLogRepository>();

                IList<AuditLog> transactionLogInfos = transactionLoggingRepository.GetAuditLog(cultureName, IsForPrint, searchCriteria, out itemsCount);

                return transactionLogInfos.ToList();
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
        public IList<TransactionLogDetailInfo> GetAuditLog(int userId, string cultureName)
        {
            try
            {
                ITransactionLoggingRepository transactionLoggingRepository = IoC.Resolve<TransactionLoggingRepository>();

                IList<TransactionLogDetailInfo> transactionLogDetailInfos = transactionLoggingRepository.GetTransactionLogDetailsInfo(1, userId, cultureName);

                return transactionLogDetailInfos.ToList();
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
        public TransactionCertificateInfo GetTransactionBasicInfo(int transactionId, string cultureName)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionCertificate(transactionId, cultureName, userWeight);
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
        public IList<TransactionName> GetTransactionNames(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionNames(transactionId, cultureName);
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
        public IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionLinks(transactionId, cultureName);
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
        public IList<TransactionLink> GetTransactionLinksForCertificate(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionLinksForCertificate(transactionId, cultureName);
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
        public IList<Attachment> GetTransactionAttachments(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionAttachments(transactionId, cultureName);
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
        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(int transactionId, string cultureName)
        {
            try
            {

                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                var transactionAssignmentHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistories(transactionId, cultureName);

                if (transactionAssignmentHistories != null && transactionAssignmentHistories.Count > 0)
                {
                    foreach (var transactionAssignmentHistory in transactionAssignmentHistories)
                    {

                        var firstView = GetFirstView(transactionId, AuditingActionCode.ViewTransaction, transactionAssignmentHistory?.ToUser?.Id ?? 0, transactionAssignmentHistory.CreatedOn, cultureName);
                        if (firstView != null)
                        {
                            transactionAssignmentHistory.ReceivedDate = firstView.DateH + "-" + firstView.Date.ToShortTimeString();
                            if (firstView.UserId != transactionAssignmentHistory.ToUserId)
                            {

                                transactionAssignmentHistory.ToUser = new UserProfile
                                {

                                    Id = firstView.User.Id,
                                    LocalName = firstView.User.LocalizationIdentifier.Localizations.Where(x => x.Culture.ShortName == cultureName).LocalText()

                                };

                            }
                        }



                    }

                }
                return transactionAssignmentHistories;
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

        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName)
        {
            try
            {
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                return transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryWithContent(transactionId, cultureName);
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

        public IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                var copies = transactionRepository.GetTransactionCopiesByTransactionId(transactionId, User.Id, cultureName);
                return copies;

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
        public IList<TransactionExternalCopy> GetTransactionExternalCopiesByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionExternalCopiesByTransactionId(transactionId, cultureName);

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
        public IList<Explanation> GetExplanationsByTransactionId(int transactionId, string cultureName)
        {
            try
            {

                IEditorBL editorBL = new EditorBL();
                return editorBL.GetExplanationsCertifByTransactionId(transactionId, cultureName);

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
        public TransactionAssignment GetTransactionAssignment(int transactionId, string cultureName)
        {
            try
            {
                int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, cultureName);
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                return transactionAssignmentBL.GetTransactionAssignments(a => a.TransactionId == transactionId && a.Action.Type.Id != SendCopyToView, cultureName).FirstOrDefault();

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

        public TransactionLog GetFirstView(int transactionId, AuditingActionCode auditingActionCode, int? userId, DateTime sendDate, string cultureName)
        {
            try
            {

                ITransactionLoggingRepository transactionLoggingRepository = IoC.Resolve<TransactionLoggingRepository>();
                return transactionLoggingRepository.GetFirstView(transactionId, auditingActionCode, userId, sendDate, cultureName);

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
        #endregion
    }
}
