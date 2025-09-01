using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchResult;

namespace MCS.Business
{
    public class DashboardHomeBL : BaseBL, IDashboardHomeBL
    {
        public DashboardHome GetDashboardHome(DateTime fromDate, DateTime toDate, int entityId, int userId, int level)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();

            DashboardHome dashboardHomeResult = dashboardHomeWrapper.GetDashboardHomeData(fromDate, toDate, entityId, userId, level);


            return dashboardHomeResult;
        }
        public List<DashboardTransactionDetails> GetDashboardDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();
            List<DashboardTransactionDetails> dashboardDetails = dashboardHomeWrapper.GetDashboardDetails(fromDate, toDate, entityId, userId, level, itemId, cultureId, pageIndex, pageSize, out TotalCount);
            return dashboardDetails;
        }

        public List<DashboardTransactionDetails> LateTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();
            List<DashboardTransactionDetails> dashboardDetails = dashboardHomeWrapper.LateTransactionsDetails(fromDate, toDate, entityId, userId, level, itemId, cultureId, pageIndex, pageSize, out TotalCount);
            return dashboardDetails;
        }

        public List<DashboardTransactionDetails> InProgressTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();
            List<DashboardTransactionDetails> dashboardDetails = dashboardHomeWrapper.InProgressTransactionsDetails(fromDate, toDate, entityId, userId, level, itemId, cultureId, pageIndex, pageSize, out TotalCount);
            return dashboardDetails;
        }

        public List<GetDashboardReportBottomResult> GetDashboardReportBottom(DateTime? fromDate, DateTime? toDate, int entityId, int? userId)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();
            var dashboardHomeResult = dashboardHomeWrapper.GetDashboardReportBottom(fromDate, toDate, entityId, userId);

            return dashboardHomeResult;
        }

        public GetDashboardReportResult GetDashboardReport(DateTime? fromDate, DateTime? toDate, int entityId, int? userId)
        {
            IDashboardHomeWrapper dashboardHomeWrapper = IoC.Resolve<IDashboardHomeWrapper>();
            var dashboardHomeResult = dashboardHomeWrapper.GetDashboardReport(fromDate, toDate, entityId, userId);

            return dashboardHomeResult;
        }
    }
}
