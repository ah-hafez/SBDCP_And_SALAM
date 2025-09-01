using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchResult;

namespace MCS.DataAccess
{
    public class DashboardHomeWrapper : BaseWrappers, IDashboardHomeWrapper
    {
        public DashboardHomeWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }

        public List<DashboardTransactionDetails> GetDashboardDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            try
            {

                OracleParameter orcOutParam = new OracleParameter(":CUR", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleParameter orcTotalOutParam = new OracleParameter(":P_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);

                List<DashboardTransactionDetails> dashboardHomeDetails = new List<DashboardTransactionDetails>();
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("BEGIN DASHBOARDDETAILSGET(:P_FROMDATE,:P_TODATE,:P_ENTITID,:P_USERID,:P_LEVEL,:P_COUNTRID,:P_CULTURENAME,:P_PAGEINDEX,:P_PAGESIZE, :P_DRAFTOUTBOUND, :P_INTERNALOUTBOUND, :P_INBOUND, :P_EXTERNALOUTBOUND, :P_TOTALCOUNT,:CUR); END;",
                            new OracleParameter(":P_FROMDATE", OracleDbType.Date, fromDate, ParameterDirection.Input),
                            new OracleParameter(":P_TODATE", OracleDbType.Date, toDate, ParameterDirection.Input),
                            new OracleParameter(":P_ENTITID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                            new OracleParameter(":P_USERID", OracleDbType.Int32, userId, ParameterDirection.Input),
                            new OracleParameter(":P_LEVEL", OracleDbType.Int32, level, ParameterDirection.Input),
                            new OracleParameter(":P_COUNTRID", OracleDbType.Int32, itemId, ParameterDirection.Input),
                            new OracleParameter(":P_CULTURENAME", OracleDbType.NVarchar2, cultureId, ParameterDirection.Input),
                            new OracleParameter(":P_PAGEINDEX", OracleDbType.Int32, pageIndex, ParameterDirection.Input),
                            new OracleParameter(":P_PAGESIZE", OracleDbType.Int32, pageSize, ParameterDirection.Input),
                            new OracleParameter(":P_DRAFTOUTBOUND", OracleDbType.Int32, TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_INTERNALOUTBOUND", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_INBOUND", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_EXTERNALOUTBOUND", OracleDbType.Int32, TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("DashboardDetailsGet @FromDate, @ToDate, @EntitID, @UserID, @level, @CountrID, @CultureName, @PageIndex, @PageSize ,@DraftOutbound,@InternalOutbound ,@Inbound,@ExternalOutbound, @TotalCount out",
                            new SqlParameter("FromDate", fromDate),
                            new SqlParameter("ToDate", toDate),
                            new SqlParameter("EntitID", entityId),
                            new SqlParameter("UserID", userId),
                            new SqlParameter("level", level),
                            new SqlParameter("CountrID", itemId),
                            new SqlParameter("CultureName", cultureId),
                            new SqlParameter("pageIndex", pageIndex),
                            new SqlParameter("pageSize", pageSize),
                            new SqlParameter("DraftOutbound", DraftOutbound),
                            new SqlParameter("InternalOutbound", InternalOutbound),
                            new SqlParameter("Inbound", Inbound),
                            new SqlParameter("ExternalOutbound", ExternalOutbound),
                            sqlPTotalCount
                            ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                return dashboardHomeDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DashboardHome GetDashboardHomeData(DateTime fromDate, DateTime toDate, int entityId, int userId, int level)
        {
            try
            {
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                OracleParameter orcOutParam = null;
                DashboardHome dashboardHomeResult = null;
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    dashboardHomeResult = _oMCSDbContext.Database.SqlQuery<DashboardHome>("BEGIN DashboardHeaderGet(:P_FROMDATE,:P_TODATE,:p_ENTITID,:p_USERID,:p_LEVEL, :p_DRAFTOUTBOUND ,:p_INTERNALOUTBOUND ,:p_INBOUND ,:p_EXTERNALOUTBOUND ,:CV_1); END;",
                            new OracleParameter(":P_FROMDATE", OracleDbType.Date, fromDate, ParameterDirection.Input),
                            new OracleParameter(":P_TODATE", OracleDbType.Date, toDate, ParameterDirection.Input),
                            new OracleParameter(":p_ENTITID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                            new OracleParameter(":p_USERID", OracleDbType.Int32, userId, ParameterDirection.Input),
                            new OracleParameter(":p_LEVEL", OracleDbType.Int32, level, ParameterDirection.Input),
                            new OracleParameter(":p_DRAFTOUTBOUND", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":p_INTERNALOUTBOUND", OracleDbType.Int32, TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":p_INBOUND", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":p_EXTERNALOUTBOUND", OracleDbType.Int32, TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),

                            new OracleParameter(":cur", OracleDbType.RefCursor, orcOutParam, ParameterDirection.Output)).FirstOrDefault();
                }
                else
                {
                    dashboardHomeResult = _oMCSDbContext.Database.SqlQuery<DashboardHome>("DashboardHeaderGet @FromDate, @ToDate, @EntitID, @UserID, @level ,@DraftOutbound,@InternalOutbound ,@Inbound,@ExternalOutbound",
                            new SqlParameter("FromDate", fromDate),
                            new SqlParameter("ToDate", toDate),
                            new SqlParameter("EntitID", entityId),
                            new SqlParameter("UserID", userId),
                            new SqlParameter("level", level),
                            new SqlParameter("DraftOutbound", DraftOutbound),
                            new SqlParameter("InternalOutbound", InternalOutbound),
                            new SqlParameter("Inbound", Inbound),
                            new SqlParameter("ExternalOutbound", ExternalOutbound)
                            ).FirstOrDefault();
                }
                return dashboardHomeResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<DashboardTransactionDetails> LateTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            TotalCount = 0;

            try
            {
                OracleParameter orcOutParam = new OracleParameter(":p_Cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);

                List<DashboardTransactionDetails> dashboardHomeDetails = new List<DashboardTransactionDetails>();
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("BEGIN DASHBOARD_DETAILS_GET(:P_FROM_DATE,:P_TO_DATE,:P_ENTITY_ID,:P_USER_ID,:P_LEVEL,:P_CountrID,:P_CultureId,:P_PageIndex,:P_PageSize, :P_Inbound, :P_Outbound, :P_Draft, :P_Internal, :P_TotalCount,:p_cur); END;",
                            new OracleParameter(":P_FROM_DATE", OracleDbType.Date, fromDate, ParameterDirection.Input),
                            new OracleParameter(":P_TO_DATE", OracleDbType.Date, toDate, ParameterDirection.Input),
                            new OracleParameter(":P_ENTITY_ID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                            new OracleParameter(":P_USER_ID", OracleDbType.Int32, userId, ParameterDirection.Input),
                            new OracleParameter(":P_LEVEL", OracleDbType.Int32, level, ParameterDirection.Input),
                            new OracleParameter(":P_CountrID", OracleDbType.Int32, itemId, ParameterDirection.Input),
                            new OracleParameter(":P_CultureName", OracleDbType.NVarchar2, cultureId, ParameterDirection.Input),
                            new OracleParameter(":P_PageIndex", OracleDbType.Int32, pageIndex, ParameterDirection.Input),
                            new OracleParameter(":P_PageSize", OracleDbType.Int32, pageSize, ParameterDirection.Input),
                            new OracleParameter(":P_Inbound", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Outbound", OracleDbType.Int32, TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Draft", OracleDbType.Int32, TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Internal", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("ReportStatisticaldLateDetail @FromDate ,@ToDate,  @EntitID, @PageIndex, @PageSize, @TotalCount out",
                            new SqlParameter("FromDate", fromDate),
                            new SqlParameter("ToDate", toDate),
                            new SqlParameter("EntitID", entityId),
                            //new SqlParameter("UserID", userId),
                            //new SqlParameter("level", level),
                            //new SqlParameter("CountrID", itemId),
                            //new SqlParameter("CultureName", cultureId),
                            new SqlParameter("pageIndex", pageIndex),
                            new SqlParameter("pageSize", pageSize),
                            //new SqlParameter("DraftOutbound", DraftOutbound),
                            //new SqlParameter("InternalOutbound", InternalOutbound),
                            //new SqlParameter("Inbound", Inbound),
                            //new SqlParameter("ExternalOutbound", ExternalOutbound),
                            sqlPTotalCount
                            ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                return dashboardHomeDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<DashboardTransactionDetails> InProgressTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount)
        {
            TotalCount = 0;

            try
            {
                OracleParameter orcOutParam = new OracleParameter(":p_Cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);

                List<DashboardTransactionDetails> dashboardHomeDetails = new List<DashboardTransactionDetails>();
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("BEGIN DASHBOARD_DETAILS_GET(:P_FROM_DATE,:P_TO_DATE,:P_ENTITY_ID,:P_USER_ID,:P_LEVEL,:P_CountrID,:P_CultureId,:P_PageIndex,:P_PageSize, :P_Inbound, :P_Outbound, :P_Draft, :P_Internal, :P_TotalCount,:p_cur); END;",
                            new OracleParameter(":P_FROM_DATE", OracleDbType.Date, fromDate, ParameterDirection.Input),
                            new OracleParameter(":P_TO_DATE", OracleDbType.Date, toDate, ParameterDirection.Input),
                            new OracleParameter(":P_ENTITY_ID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                            new OracleParameter(":P_USER_ID", OracleDbType.Int32, userId, ParameterDirection.Input),
                            new OracleParameter(":P_LEVEL", OracleDbType.Int32, level, ParameterDirection.Input),
                            new OracleParameter(":P_CountrID", OracleDbType.Int32, itemId, ParameterDirection.Input),
                            new OracleParameter(":P_CultureName", OracleDbType.NVarchar2, cultureId, ParameterDirection.Input),
                            new OracleParameter(":P_PageIndex", OracleDbType.Int32, pageIndex, ParameterDirection.Input),
                            new OracleParameter(":P_PageSize", OracleDbType.Int32, pageSize, ParameterDirection.Input),
                            new OracleParameter(":P_Inbound", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Outbound", OracleDbType.Int32, TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Draft", OracleDbType.Int32, TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            new OracleParameter(":P_Internal", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    dashboardHomeDetails = _oMCSDbContext.Database.SqlQuery<DashboardTransactionDetails>("ReportStatisticaldInProgressDetail @FromDate ,@ToDate,  @EntitID, @PageIndex, @PageSize, @TotalCount out",
                            new SqlParameter("FromDate", fromDate),
                            new SqlParameter("ToDate", toDate),
                            new SqlParameter("EntitID", entityId),
                            //new SqlParameter("UserID", userId),
                            //new SqlParameter("level", level),
                            //new SqlParameter("CountrID", itemId),
                            //new SqlParameter("CultureName", cultureId),
                            new SqlParameter("pageIndex", pageIndex),
                            new SqlParameter("pageSize", pageSize),
                            //new SqlParameter("DraftOutbound", DraftOutbound),
                            //new SqlParameter("InternalOutbound", InternalOutbound),
                            //new SqlParameter("Inbound", Inbound),
                            //new SqlParameter("ExternalOutbound", ExternalOutbound),
                            sqlPTotalCount
                            ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                return dashboardHomeDetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public GetDashboardReportResult GetDashboardReport(DateTime? fromDate, DateTime? toDate, int entityId, int? userId)
        {
            try
            {
                if (userId.HasValue && userId.Value <= 0)
                {
                    userId = null;
                }

                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                var inprocessId = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                var result = _oMCSDbContext.Database.SqlQuery<GetDashboardReportResult>
                    ("GetDashboardReport @FromDate, @ToDate, @UserID, @EntitID, @DraftOutbound,@InternalOutbound ,@Inbound,@TransactionStatusInProcessID,@ExternalOutbound",
                     new SqlParameter("FromDate", fromDate.HasValue ? (object)fromDate.Value : DBNull.Value),
                     new SqlParameter("ToDate", toDate.HasValue ? (object)toDate.Value : DBNull.Value),
                     new SqlParameter("UserID", userId.HasValue ? (object)userId.Value : DBNull.Value),
                     new SqlParameter("EntitID", entityId),
                     new SqlParameter("DraftOutbound", DraftOutbound),
                     new SqlParameter("InternalOutbound", InternalOutbound),
                     new SqlParameter("Inbound", Inbound),
                     new SqlParameter("TransactionStatusInProcessID", inprocessId),
                     new SqlParameter("ExternalOutbound", ExternalOutbound)
                    ).FirstOrDefault();



                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<GetDashboardReportBottomResult> GetDashboardReportBottom(DateTime? fromDate, DateTime? toDate, int entityId, int? userId)
        {
            try
            {

                if (userId.HasValue && userId.Value <= 0)
                {
                    userId = null;
                }

                var result = _oMCSDbContext.Database.SqlQuery<GetDashboardReportBottomResult>
                    ("GetDashboardReportBottom @FromDate, @ToDate, @UserID, @EntitID",
                     new SqlParameter("FromDate", fromDate.HasValue ? (object)fromDate.Value : DBNull.Value),
                     new SqlParameter("ToDate", toDate.HasValue ? (object)toDate.Value : DBNull.Value),
                     new SqlParameter("UserID", userId.HasValue ? (object)userId.Value : DBNull.Value),
                     new SqlParameter("EntitID", entityId)

                    ).ToList();

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //
    }

    public interface IDashboardHomeWrapper
    {
        DashboardHome GetDashboardHomeData(DateTime fromDate, DateTime toDate, int entityId, int userId, int level);
        List<DashboardTransactionDetails> GetDashboardDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
        List<DashboardTransactionDetails> LateTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
        List<DashboardTransactionDetails> InProgressTransactionsDetails(DateTime fromDate, DateTime toDate, int entityId, int userId, int level, int itemId, string cultureId, int pageIndex, int pageSize, out int TotalCount);
        GetDashboardReportResult GetDashboardReport(DateTime? fromDate, DateTime? toDate, int entityId, int? userId);
        List<GetDashboardReportBottomResult> GetDashboardReportBottom(DateTime? fromDate, DateTime? toDate, int entityId, int? userId);
    }
}
