using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionDeliveryReportRepository : IRepository<TransactionDeliveryReport>
    {
        int AddTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport);
        void UpdateTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport);
        int UpdateDeliveryReportsDocumentByNumber(DocumentInfo document, string Number);

        //int UpdateDeliveryReportsDocumentByDate(DocumentInfo document, DateTime Date);

        void UpdateTransactionDeliveryReportCopies(int transactionId, int? reporterId);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByIds(List<int> transactionDeliveryReportIds);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReport(SearchCriteria searchCriteria, out int rowsCount);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReport(Expression<Func<TransactionDeliveryReport, bool>> @where);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, string cultureName);
        IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, int? transactionId, string number,string cultureName);
    }
}
