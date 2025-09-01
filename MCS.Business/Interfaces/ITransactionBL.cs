using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Transaction;

namespace MCS.Business
{
    public interface ITransactionBL
    {
        IList<ExternalPartyAttachment> GetExternalPartiesAttach(int transactionId, int externalPartyId, string cultureName);
        void Update(Transaction transaction);
        void UpdateCanceledOutBound(Transaction transaction);
        TransactionDetails Save(Transaction transaction, byte[] content = null);
        TransactionBarcodesInfo GetTransactionBarcodes(int transactionId, int OrgUnitId, string cultureName);
        TransactionTicket PrintTransactionTicket(Transaction transaction);
        Transaction GetPreviousTransaction(int OrgUnitId, string cultureName, bool IsForIndividual);
        Transaction GetPreviousTransactionByID(int transactionsId, int OrgUnitId, string cultureName, bool IsForIndividual);
        void AddTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks);
        IList<DeliveryReportInfoDTO> DeliveryReport(Transaction transaction, string cultureName, bool perTransaction = true);
        Transaction GetTransaction(int userId, int transactionNumber, TransactionCategory transactionCategory, int year, int sourceTypeId, int OrgUnitId, string cultureName);
        Transaction GetTransactionByNumberAndYear(int year, int transactionNumber);
        List<Transaction> GetTransactionsByNationalId(string nationalId);
        TransactionVisitTicketInfo GetVisitTicket(Transaction transaction, int OrgUnitId, string cultureName);
        TransactionCertificateInfo GetTransactionCertificate(int transactionId, string cultureName);
        Transaction GetTransaction(Expression<Func<Transaction, bool>> @where);
        void SetTransactionCopyToViewed(TransactionCopy transactionCopy);
        void SetTransactionCopyToDelete(TransactionCopy transactionCopy);
        void SetTransactionCopyToUndo(TransactionCopy transactionCopy);
        void SetTransactionExternalCopyToViewed(TransactionExternalCopy transactionExternalCopy);
        void SetTransactionCopiesSent(int transactionId);
        IList<DeliveryReportInfoDTO> DeliveryReport(Transaction transaction, string cultureName, List<int> reportIds, bool perTransaction = true, bool IsNew = false);
        void SaveTransactionDeliveryNumber(Transaction transaction);
        List<int> GetTransactionDeliveryReportByTransactionId(int transcationId);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionIds(List<int> transcationIds);
        void UpdateTransactionStatus(int transId, int statusId);
        void UpdateTransactionDelivary(int transId, int DelivaryId);
        bool IsMatchNumberOrBarcode(int transId, string number, string barcode, int UserId, int EntityId);
        void UpdateTransactionExternalCopyStatus(int transactionId, int value, int status);
        void UpdateTransactionExternalCopyStatusById(long transactionNumber, int transactionsCopyId, int unableToDeliver);
        void SaveTransactionReservation(TransactionReservation transactionReservation);
        void FollowUpUpdateIsDeleted(int Id, string cultureName);
        void FollowUpChangeStatus(int Id, int FollowupStatus, bool IsActive);
        void FollowUpUpdateReceive(int Id, int userid);
        void FollowUpUpdateReminderStatus(int Id, bool IsReminder);
        void FollowUpUpdateEscalatedStatus(int Id, bool IsEscalated);
        int TransactionFollowUpAdd(TransactionFollowUp oTransactionFollowUp, string cultureName);
        void TransactionFollowUpUpdate(TransactionFollowUp oTransactionFollowUp, string cultureName);
        void SendFollowUpReminder(int FollowUpId, int TransactionId, int FollowUpUserID, string cultureName);
        void EscalateFollowUp(int FollowUpId, int TransactionId, int FollowUpUserID, string cultureName);
        int? GetChildFollowUpUserId(int FollowUpId);
        bool CheckIfFollowUpAdd(int TransactionId);
        Transaction GetTransactionByIdForNotification(int transactionId);
        void FollowUpUpdateIsDeleted(int transactionId, int userId, string culture);
        void UpdateTransactionLinks(int transactionId, IList<TransactionLink> Links);
        void FollowUpAddTransactionLinks(int transactionId, IList<TransactionLink> Links);
        void UpdateTransactionDeleteByTransId(long transactionId, bool isDeleted);
        void UpdateTransactionSubject(EditSubjectTransactionDTO editSubjectTransactionDTO);
        void DeleteDraftTransaction(long transactionId, bool isDeleted);
        Transaction UpdateVipOutboundDraft(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, string mainDocumentContent, string pdfMainDocumentContent, bool isSigned);
        void SetViewedTransactionCopy(int transactionCopyId);
        void DeleteDocument(int documentId);
        Transaction UpdateVipInbound(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary);
        Transaction UpdateVipOutboundInternal(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary);
        void UpdateAssignmentSelectedoption(int transactionId, string assignmentList);
        List<int> GetTransactionDeliveryReportByTransactionId(int transcationId, bool? all = false);
    }
}
