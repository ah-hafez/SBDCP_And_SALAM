using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;
using System;

namespace MCS.Business
{
    public interface ITransactionDeliveryReportBL
    {
        int AddTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport);
        void UpdateTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport);
        int UpdateDeliveryReportDocumentByNumber(DocumentInfo document, string Number);

        int UpdateDeliveryReportsDocumentByDate(DocumentInfo document, string date , string DeliveryReportNumber);
        void UpdateTransactionDeliveryReportCopies(int transactionId, int? reporterId);
        IList<TransactionDeliveryReport> GetLastDeliveryReport(int transcationId, int userId);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(string number);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, string cultureName);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, int? transactionId, string number, string cultureName);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionId(int transcationId, bool? isCopy = false, bool? all = false);
        IList<TransactionDeliveryReport> GetDeliveryReport(List<int> deliveryReportIds);
        TransactionDeliveryReport GetTransactionDeliveryReportByHistoryId(int historyId);
        TransactionDeliveryReport GetTransactionDeliveryReportByAssignmentHistoryId(int assignmentHistoryId);
        IList<TransactionDeliveryReport> GetDeliveryReport(SearchCriteria searchCriteria, out int rowsCount);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionIds(List<int> transcationIds);
        int UpdateDeliveryReportsDocumentByDeliveryReportId(DocumentInfo document, string DateH, int Id);
    }
}
