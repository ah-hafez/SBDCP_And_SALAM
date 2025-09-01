using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IList<ExternalPartyAttachment> GetExternalPartiesAttach(int transactionID, int externalPartyId, string cultureName);
        int AddTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction, bool updateDocument, bool isReserved = false);
        Transaction GetTransactionBasicInfo(int transactionId, string cultureName);
        void UpdateTransactionNames(int transactionId, IList<TransactionName> transactionNames);
        void UpdateTransactionCopies(int transactionId, IList<TransactionCopy> transactionCopies);
        void UpdateCopy(int CopyId);
        void UpdateAssignmentPaperCopies(int transactionId, IList<TransactionCopy> transactionCopies);
        void UpdateTransactionExternalCopies(int transactionId, IList<TransactionExternalCopy> transactionExternalCopies);
        int GetTransactionCopiesCount(Expression<Func<TransactionCopy, bool>> where);
        int GetTransactionExternalCopiesCount(Expression<Func<TransactionExternalCopy, bool>> where);
        IList<TransactionCopy> GetTransactionCopies(Expression<Func<TransactionCopy, bool>> where, TrayType trayType, SearchCriteriaCustom searchCriteria, out int rowsCount, int? UserWeight,int currentUserId);
        IList<TransactionExternalCopy> GetTransactionExternalCopiesByTransactionId(int transactionId, string cultureName);
        void UpdateTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks);
        void FollowUpAddTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks);
        void UpdateTransactionContactDate(int transactionId, string ContactDateH);
        void UpdateTransactionAttachments(int transactionId, IList<Attachment> attachments);
        void AddDeliveryReportToAttachment(Attachment attachments);

        Transaction GetPreviousTransaction(int userId, int orgUnitId, TransactionCategory transactionCategory, string cultureName, bool IsForIndividual);
        Transaction GetPreviousTransactionByID(int transactionsId, int orgUnitId, TransactionCategory transactionCategory, string cultureName, bool IsForIndividual);

        int? GetTransactionId(Expression<Func<Transaction, bool>> @where);
        Transaction GetTransactionById(int transactionId);
        int GetTransactionByIdAndOrgUnit(int transactionId, int OrgUnitId);
        Transaction GetTransactionByIdAsNotacking(int transactionId);
        Transaction LoadTransaction(int transactionId);
        TransactionCertificateInfo GetTransactionCertificate(int transactionId, string cultureName, int? userWeight);

        Transaction GetTransactionByNumberAndYear(Expression<Func<Transaction, bool>> @where, string cultureName);
        List<Transaction> GetTransactionsByNationalId(Expression<Func<Transaction, bool>> @where, string cultureName);
        Transaction GetTransactionLight(Expression<Func<Transaction, bool>> @where, string cultureName);
        Transaction GetTransaction(Expression<Func<Transaction, bool>> @where);
        IList<Transaction> GetTransactions(Expression<Func<Transaction, bool>> @where);
        IList<Transaction> GetTransactions(int orgUnitId, int year);
        IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName);
        IList<TransactionLink> GetTransactionLinksForCertificate(int transactionId, string cultureName);
        DocumentInfo GetMainDocumentByTransactionId(int transactionId);
        void TransactionElcOutBoundAdd(TransactionElcOutBound transactionElcOutBound);
        void AddConfidentialityAcknowledgment(int TransactionId, int UserId, int OrgUnitId, DateTime CreatedDate);
        void TransactionElcOutBoundUpdate(int userId, int orgUnitId, bool ishidden, int transactionId);
        void AcknowledgeElcOutBound(int userId, int orgUnitId, bool ishidden, int transactionId);
        void SetTransactionCopyToViewed(TransactionCopy transactionCopy);
        void SetTransactionCopyToDelete(TransactionCopy transactionCopy);
        void SetTransactionCopyToUndo(TransactionCopy transactionCopy);
        void SetTransactionExternalCopyToViewed(TransactionExternalCopy transactionExternalCopy);
        void UpdateTransactionSubjectClassifications(int transactionId, IList<TransactionSubjectClassification> transactionSubjectClassifications);
        bool CheckIfTransactionSigned(int transactionId);
        void UpdateTransactionStatusByTransNo(long transactionNumber, int status, string rejectionReason = null);
        void UpdateTransactionStatusAndEntityByTransId(long transactionId, int status,int Entityid , int? Userid, string rejectionReason = null);
        void UpdateTransactionDeleteByTransId(long transactionId, bool isDeleted);
        void UpdateTransactionStatus(int transId, int statusId);
        void UpdateTransactionDelivary(int transId, int DelivaryId);
        void UpdateTransactionSavedReason(int transId, string reason);
        void SetTransactionCopiesSent(int transactionId);
        void UpdateTransactionEntityAndToUser(int transactionId, int entityId, int? userId);
        List<MainAudit> GetAuditByEntityName(int userId, int orgUnitId, int transactionId, string EntityName, string culture, AuditFor auditFor, bool IsForPrint, out int itemsCount, SearchCriteriaCustom searchCriteria = null);
        List<AuditDetails> GetEntityAuditing(AuditFor auditFor, int auditId, string PropName, string culture);
        IList<Attachment> GetTransactionAttachments(int transactionId, string cultureName);
        //Attachment GetAttachmentById(int attachmentId, string cultureName);
        IList<TransactionName> GetTransactionNames(int transactionId, string cultureName);
        Transaction GetTransactionBasicInfo(int transactionId, int year, string cultureName);
        void SaveTransactionDeliveryNumber(Transaction transaction);
        bool IsMatchNumberOrBarcode(int transId, string number, string barcode);
        void UpdateMainDcument(DocumentInfo mainDocument, int transactionId);
        void UpdateTransactionExternalCopyStatus(int transactionId, int value, int status);
        void UpdateTransactionExternalCopyStatusById(long transactionNumber, int transactionsCopyId, int unableToDeliver);
        IList<int> GetUserTasksTransactionsIds(int userId, int OrgUnitId);
        IList<int> GetELcOutBoundIds(int userId, int OrgUnitId);
        IList<int> GetSentTransactionsIds(int userId, int OrgUnitId);
        IList<int> GetSavedCopiesIds(int userId, int OrgUnitId);
        IList<int> GetOrgUnitIds(int userId, int OrgUnitId);
        IList<int> GetOutboundExternalIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowUpTransactionsIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowProcessIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowCompleteIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowLateIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowDeleteIds(int userId, int OrgUnitId);

        IList<int> GetUserFollowReminderIds(int userId, int OrgUnitId);
        IList<int> GetUserFollowUpEscalationIds(int userId, int OrgUnitId);
        void UpdateTransactionFollowUps(int transactionId, IList<TransactionFollowUp> transactionfollowups);
        void FollowUpDetailsAdd(int transactionId, int orgUnitId, int userId, string note);
        void AddFollowupUditTrial(FollowUpAuditTrail followUpAuditTrail);
        void FollowUpUpdateIsDeleted(int transactionId, int userId);
        void FollowUpChangeStatus(int Id, int FollowupStatus, bool IsActive);
        void FollowUpUpdateReceive(int Id, int userid);
        void FollowUpUpdateReminderStatus(int Id, bool IsReminder);
        void FollowUpUpdateEscalatedStatus(int Id, bool IsEscalated);
        TransactionFollowUp GetFollowUpByTransactionIdAndUserId(int transactionId, int userId);
        int TransactionFollowUpAdd(TransactionFollowUp follow);
        void TransactionFollowUpUpdate(TransactionFollowUp follow);

        void ReminderTransactionFollowUp(int FollowUpId);
        void EscalateTransactionFollowUp(int FollowUpId);
        TransactionFollowUp FollowUpDetailsByTransId(int transId, int FollowUpStatusId, int UserId, int OrgUnitId, string cultureName);
        TransactionFollowUp FollowUpDetailsByFollowUpId(int FollowUpId, string cultureName);
        IList<FollowUpDetails> FollowUpDetailsById(int id, string cultureName);
        IList<FollowUpAuditTrail> GetListFollowupUditTrial(int id, string cultureName);
        IList<TransactionFollowUp> TransactionFollowUpSelectByTransId(int transId, string cultureName);
        IList<TransactionFollowUp> TransactionFollowUpSelectByFollowUpId(int transId, string cultureName);
        IList<FollowUpAuditTrail> GetFollowUpAuditTrail(int followUpId, string cultureName);
        bool CheckIfFollowUpAdd(int TransactionId);
        int? GetChildFollowUpUserId(int FollowUpId);

        void FollowUpUpdateIsDeleted(int Id);
        int AddTransactionReservation(TransactionReservation transactionReservation);
        List<TransactionReservation> GetTransactionReservations(int? orgUnitId, int? userId, SearchCriteria searchCriteria, out int rowsCount);
        List<Transaction> GetReservedTransaction(int reservationId);
        TransactionFollowUp GetFollowUpById(int id);
        Transaction GetTransactionByIdForNotification(int transactionId);
        void UpdatePhysicalTransactionAssignment(int TransactionId, int UserId, int EntityId);
        List<Transaction> GetLateTransactions();
        List<Transaction> GetTransactionsByExternalPartyId(int externalPartyId, int orgUnitId);
        void UpdateProcessPeriodTransaction(int trnsId, int? ProcessPeriod);
        #region MobileApi
        void UserMobileUpdateTransactionStatus(int transId, int statusId, string reason);
        Transaction GetUserMobileTransaction(int transId, string cultureName);
        #endregion
        List<Transaction> LateTransactionWithNotifyLetterTypes();
        List<Transaction> SendNearlyLateTransaction();
        IList<Transaction> GetTransactionsByNumber(string Number, int inquiryType, int yearH, int? DestinationId, string subject, int userId, int entityId);
        void UpdateTransactionSubject(int transactionId, string newSubject);
        List<ReleaseNote> ReleaaseNotesUsersSelect(int userId);
        void ReleaaseNotesUsersAdd(int userId);
        void DeleteDraftTransaction(long transactionId, bool isDeleted);
        Transaction GetTransaction_VIP(Expression<Func<Transaction, bool>> @where, string cultureName, bool isNotification = false);
        Transaction UpdateVipOutboundDraft(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, string mainDocumentContent, string pdfDocumentContent, bool isSigned);
        void CleanAttachment(int transactionId);
        Transaction GetTransaction(Expression<Func<Transaction, bool>> @where, int userid, string cultureName, bool isNotification = false);
        IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, int userId, string cultureName);
        void SetViewedTransactionCopy(int transactionCopyId, int userId);
        void DeletedTransaction(int transId);
        DocumentInfo GetOldMainDocumentByTransactionId(int transactionId);
        Transaction UpdateVipInbound(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary);
        Transaction UpdateVipOutboundInternal(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary);
        TransactionCopy GetTransactionCopyById(int id);
        bool CheckUserHasPermission(List<int> transactionId, int? userId);
        void AddTransactionSpecialAuthorize(int transactionId, int userId);
        void AddTransactionEncryptionCode(TransactionEncryptionCode transactionEncryptionCode);
        Transaction GetTransactionById(int TransactionId, string cultureName, bool isNotification = false);
        bool HasSpecialAuthorize(int transactionId, int userId);
        void UpdateAssignmentSelectedoption(int transactionId, string assignmentList);
        Transaction GetTransaction(Expression<Func<Transaction, bool>> @where, string cultureName);

    }
}

