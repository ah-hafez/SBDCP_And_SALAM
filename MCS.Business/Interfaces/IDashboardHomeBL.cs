using System;
using System.Collections.Generic;
using MCS.Domain;
using MCS.Domain.Search.SearchResult;

namespace MCS.Business
{
    public interface IDashboardHomeBL
    {
        DashboardHome GetDashboardHome(DateTime fromDate, DateTime toDate, int entityId, int userId, int level);
        GetDashboardReportResult GetDashboardReport(DateTime? fromDate, DateTime? toDate, int entityId, int? userId);
        List<GetDashboardReportBottomResult> GetDashboardReportBottom(DateTime? fromDate, DateTime? toDate, int entityId, int? userId);
        List<DashboardTransactionDetails> GetDashboardDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
        List<DashboardTransactionDetails> LateTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
        List<DashboardTransactionDetails> InProgressTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
    }
}
