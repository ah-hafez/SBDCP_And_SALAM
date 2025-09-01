using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Data.Entity;

namespace MCS.DataAccess
{
    public class ReportWrapper : BaseWrappers, IReportWrapper
    {
        public ReportWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator) : base(ambienTTransactionContextLocator) { }
        public List<TransactionReportResult> TransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {

                List<TransactionReportResult> baseSearchResults = null;
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionReportResult>(@"ReportSearch
                                        @DateFrom,@DateTo,@TransactionTypeId,@TransactionNumber,@TransactioDescription,
                                        @IsAppointment,@AppointmentDate,@ConfidentialityId,@PriorityId,@LetterTypeId,@Remarks,@DeliveryMethodId,
                                        @FullName,@CivilID,@MobileNumber,
                                        @IsForIndividual,@InboundDateH,@ExternalPartyId,@DocumentNumber,@OutBoundDate,
                                        @FromOrgUnitId,@FromUserId,@ToOrgUnitId,@ToUserId,
                                        @CultureName,@PageIndex,@PageSize,@EntitID, @TotalCount out",
                    new SqlParameter("DateFrom", searchCriteriaTransactionReport.From),
                    new SqlParameter("DateTo", searchCriteriaTransactionReport.To),
                    new SqlParameter("TransactionTypeId", searchCriteriaTransactionReport.TransactionTypeId),
                    new SqlParameter("TransactionNumber", searchCriteriaTransactionReport.Number > 0 ? searchCriteriaTransactionReport.Number : (object)DBNull.Value),
                    new SqlParameter("TransactioDescription", searchCriteriaTransactionReport.Subject ?? (object)DBNull.Value),
                    new SqlParameter("IsAppointment", searchCriteriaTransactionReport.IsAppointment == false ? (object)DBNull.Value : searchCriteriaTransactionReport.IsAppointment),
                    new SqlParameter("AppointmentDate", searchCriteriaTransactionReport.AppointmentDate ?? (object)DBNull.Value),
                    new SqlParameter("ConfidentialityId", searchCriteriaTransactionReport.ConfidentialityLevelId > 0 ? searchCriteriaTransactionReport.ConfidentialityLevelId : (object)DBNull.Value),
                    new SqlParameter("PriorityId", searchCriteriaTransactionReport.PriorityLevelId > 0 ? searchCriteriaTransactionReport.PriorityLevelId : (object)DBNull.Value),
                    new SqlParameter("LetterTypeId", searchCriteriaTransactionReport.LetterTypeId > 0 ? searchCriteriaTransactionReport.LetterTypeId : (object)DBNull.Value),
                    new SqlParameter("Remarks", searchCriteriaTransactionReport.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("DeliveryMethodId", searchCriteriaTransactionReport.DeliveryMethodId > 0 ? searchCriteriaTransactionReport.DeliveryMethodId : (object)DBNull.Value),
                    // new SqlParameter("TransactionStatusId", searchCriteriaTransactionReport.TransactionStatusId > 0 ? searchCriteriaTransactionReport.TransactionStatusId : (object)DBNull.Value),
                    new SqlParameter("FullName", searchCriteriaTransactionReport.FullName ?? (object)DBNull.Value),
                    new SqlParameter("CivilID", searchCriteriaTransactionReport.CivilID ?? (object)DBNull.Value),
                    new SqlParameter("MobileNumber", searchCriteriaTransactionReport.MobileNumber ?? (object)DBNull.Value),

                    new SqlParameter("IsForIndividual", searchCriteriaTransactionReport.IsForIndividual),
                    new SqlParameter("InboundDateH", searchCriteriaTransactionReport.InboundDateH ?? (object)DBNull.Value),
                    new SqlParameter("ExternalPartyId", searchCriteriaTransactionReport.DestinationId > 0 ? searchCriteriaTransactionReport.DestinationId : (object)DBNull.Value),
                    new SqlParameter("DocumentNumber", searchCriteriaTransactionReport.InboundDocumentNumber ?? (object)DBNull.Value),
                    new SqlParameter("OutBoundDate", searchCriteriaTransactionReport.OutboundDateH ?? (object)DBNull.Value),

                    new SqlParameter("FromOrgUnitId", searchCriteriaTransactionReport.FromOrgUnitId > 0 ? searchCriteriaTransactionReport.FromOrgUnitId : (object)DBNull.Value),
                    new SqlParameter("FromUserId", searchCriteriaTransactionReport.FromEmployeeId > 0 ? searchCriteriaTransactionReport.FromEmployeeId : (object)DBNull.Value),
                    new SqlParameter("ToOrgUnitId", searchCriteriaTransactionReport.ToOrgUnitId > 0 ? searchCriteriaTransactionReport.ToOrgUnitId : (object)DBNull.Value),
                    new SqlParameter("ToUserId", searchCriteriaTransactionReport.ToEmployeeId > 0 ? searchCriteriaTransactionReport.ToEmployeeId : (object)DBNull.Value),

                    new SqlParameter("CultureName", searchCriteriaTransactionReport.CultureName),
                    new SqlParameter("PageIndex", searchCriteriaTransactionReport.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaTransactionReport.PageSize),

                    new SqlParameter("EntitID", searchCriteriaTransactionReport.EntityId),
                    //new SqlParameter("UserID", searchCriteriaTransactionReport.UserId),
                    //new SqlParameter("Level", searchCriteriaTransactionReport.Level),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                else
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_Cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    //p_TransactionStatusId,
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionReportResult>(
                         @"BEGIN REPORT_TRANSACTIONSSEARCH 
                    (:p_DateFrom, :p_DateTo, :p_TransactionCategoryId, :p_TransactionNumber, :p_TransactioDescription,
                    :p_TransactionTypeId, :p_IsAppointment, :p_AppointmentDate, :p_ConfidentialityId, :p_PriorityId,:p_LetterTypeId,:p_Remarks,:p_DeliveryMethodId,:p_TransactionStatusId,
                    :p_FullName,:p_CivilID,:p_MobileNumber,:p_IsForIndividual,:p_InboundDateH,:p_ExternalPartyId,:p_DocumentNumber,:p_OutBoundDate,:p_FromOrgUnitId,
                    :p_FromUserId,:p_ToOrgUnitId,:p_ToUserId,:p_CultureName,
                    :p_PageIndex,:p_PageSize,:P_ENTITY_ID,:P_USER_ID,:p_Level,:p_TotalCount,:p_Cur); END;",
                     new OracleParameter(":p_DateFrom", OracleDbType.Date, searchCriteriaTransactionReport.From, ParameterDirection.Input),
                     new OracleParameter(":p_DateTo", OracleDbType.Date, searchCriteriaTransactionReport.To, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionCategoryId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionCategoryId, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionNumber", OracleDbType.Int32, searchCriteriaTransactionReport.Number > 0 ? searchCriteriaTransactionReport.Number : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactioDescription", OracleDbType.NVarchar2, searchCriteriaTransactionReport.Subject ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionTypeId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionTypeId > 0 ? searchCriteriaTransactionReport.TransactionTypeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_IsAppointment", OracleDbType.Int32, searchCriteriaTransactionReport.IsAppointment == false ? (object)DBNull.Value : 1, ParameterDirection.Input),
                     new OracleParameter(":p_AppointmentDate", OracleDbType.Date, searchCriteriaTransactionReport.AppointmentDate ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ConfidentialityId", OracleDbType.Int32, searchCriteriaTransactionReport.ConfidentialityLevelId > 0 ? searchCriteriaTransactionReport.ConfidentialityLevelId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_PriorityId", OracleDbType.Int32, searchCriteriaTransactionReport.PriorityLevelId > 0 ? searchCriteriaTransactionReport.PriorityLevelId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_LetterTypeId", OracleDbType.Int32, searchCriteriaTransactionReport.LetterTypeId > 0 ? searchCriteriaTransactionReport.LetterTypeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_Remarks", OracleDbType.NVarchar2, searchCriteriaTransactionReport.Remarks ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_DeliveryMethodId", OracleDbType.Int32, searchCriteriaTransactionReport.DeliveryMethodId > 0 ? searchCriteriaTransactionReport.DeliveryMethodId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionStatusId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionStatusId > 0 ? searchCriteriaTransactionReport.TransactionStatusId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FullName", OracleDbType.NVarchar2, searchCriteriaTransactionReport.FullName ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_CivilID", OracleDbType.NVarchar2, searchCriteriaTransactionReport.CivilID ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_MobileNumber", OracleDbType.NVarchar2, searchCriteriaTransactionReport.MobileNumber ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_IsForIndividual", OracleDbType.Int32, searchCriteriaTransactionReport.IsForIndividual == false ? 0 : 1, ParameterDirection.Input),
                     new OracleParameter(":p_InboundDateH", OracleDbType.NVarchar2, searchCriteriaTransactionReport.InboundDateH ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ExternalPartyId", OracleDbType.Int32, searchCriteriaTransactionReport.DestinationId > 0 ? searchCriteriaTransactionReport.DestinationId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_DocumentNumber", OracleDbType.NVarchar2, searchCriteriaTransactionReport.InboundDocumentNumber ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_OutBoundDate", OracleDbType.NVarchar2, searchCriteriaTransactionReport.OutboundDateH ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FromOrgUnitId", OracleDbType.Int32, searchCriteriaTransactionReport.FromOrgUnitId > 0 ? searchCriteriaTransactionReport.FromOrgUnitId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FromUserId", OracleDbType.Int32, searchCriteriaTransactionReport.FromEmployeeId > 0 ? searchCriteriaTransactionReport.FromEmployeeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ToOrgUnitId", OracleDbType.Int32, searchCriteriaTransactionReport.ToOrgUnitId > 0 ? searchCriteriaTransactionReport.ToOrgUnitId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ToUserId", OracleDbType.Int32, searchCriteriaTransactionReport.ToEmployeeId > 0 ? searchCriteriaTransactionReport.ToEmployeeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaTransactionReport.CultureName, ParameterDirection.Input),
                     new OracleParameter(":P_ENTITY_ID", OracleDbType.Int32, searchCriteriaTransactionReport.EntityId.HasValue ? searchCriteriaTransactionReport.EntityId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":P_USER_ID", OracleDbType.Int32, searchCriteriaTransactionReport.UserId.HasValue ? searchCriteriaTransactionReport.UserId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_Level", OracleDbType.Int32, searchCriteriaTransactionReport.Level, ParameterDirection.Input),
                     new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaTransactionReport.PageIndex, ParameterDirection.Input),
                     new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaTransactionReport.PageSize, ParameterDirection.Input),
                         orcTotalOutParam,
                         orcOutParam
                     ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TransactionReportResult> SecretaryTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {

                List<TransactionReportResult> baseSearchResults = null;
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionReportResult>(@"SecretaryReportSearch
                                        @DateFrom,@DateTo,@TransactionTypeId,@TransactionNumber,@TransactioDescription,
                                        @IsAppointment,@AppointmentDate,@ConfidentialityId,@PriorityId,@LetterTypeId,@Remarks,@DeliveryMethodId,
                                        @FullName,@CivilID,@MobileNumber,
                                        @IsForIndividual,@InboundDateH,@ExternalPartyId,@DocumentNumber,@OutBoundDate,
                                        @FromOrgUnitId,@FromUserId,@ToOrgUnitId,@ToUserId,
                                        @CultureName,@PageIndex,@PageSize,@EntitID, @TotalCount out",
                    new SqlParameter("DateFrom", searchCriteriaTransactionReport.From),
                    new SqlParameter("DateTo", searchCriteriaTransactionReport.To),
                    new SqlParameter("TransactionTypeId", searchCriteriaTransactionReport.TransactionTypeId),
                    new SqlParameter("TransactionNumber", searchCriteriaTransactionReport.Number > 0 ? searchCriteriaTransactionReport.Number : (object)DBNull.Value),
                    new SqlParameter("TransactioDescription", searchCriteriaTransactionReport.Subject ?? (object)DBNull.Value),
                    new SqlParameter("IsAppointment", searchCriteriaTransactionReport.IsAppointment == false ? (object)DBNull.Value : searchCriteriaTransactionReport.IsAppointment),
                    new SqlParameter("AppointmentDate", searchCriteriaTransactionReport.AppointmentDate ?? (object)DBNull.Value),
                    new SqlParameter("ConfidentialityId", searchCriteriaTransactionReport.ConfidentialityLevelId > 0 ? searchCriteriaTransactionReport.ConfidentialityLevelId : (object)DBNull.Value),
                    new SqlParameter("PriorityId", searchCriteriaTransactionReport.PriorityLevelId > 0 ? searchCriteriaTransactionReport.PriorityLevelId : (object)DBNull.Value),
                    new SqlParameter("LetterTypeId", searchCriteriaTransactionReport.LetterTypeId > 0 ? searchCriteriaTransactionReport.LetterTypeId : (object)DBNull.Value),
                    new SqlParameter("Remarks", searchCriteriaTransactionReport.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("DeliveryMethodId", searchCriteriaTransactionReport.DeliveryMethodId > 0 ? searchCriteriaTransactionReport.DeliveryMethodId : (object)DBNull.Value),
                    // new SqlParameter("TransactionStatusId", searchCriteriaTransactionReport.TransactionStatusId > 0 ? searchCriteriaTransactionReport.TransactionStatusId : (object)DBNull.Value),
                    new SqlParameter("FullName", searchCriteriaTransactionReport.FullName ?? (object)DBNull.Value),
                    new SqlParameter("CivilID", searchCriteriaTransactionReport.CivilID ?? (object)DBNull.Value),
                    new SqlParameter("MobileNumber", searchCriteriaTransactionReport.MobileNumber ?? (object)DBNull.Value),

                    new SqlParameter("IsForIndividual", searchCriteriaTransactionReport.IsForIndividual),
                    new SqlParameter("InboundDateH", searchCriteriaTransactionReport.InboundDateH ?? (object)DBNull.Value),
                    new SqlParameter("ExternalPartyId", searchCriteriaTransactionReport.DestinationId > 0 ? searchCriteriaTransactionReport.DestinationId : (object)DBNull.Value),
                    new SqlParameter("DocumentNumber", searchCriteriaTransactionReport.InboundDocumentNumber ?? (object)DBNull.Value),
                    new SqlParameter("OutBoundDate", searchCriteriaTransactionReport.OutboundDateH ?? (object)DBNull.Value),

                    new SqlParameter("FromOrgUnitId", searchCriteriaTransactionReport.FromOrgUnitId > 0 ? searchCriteriaTransactionReport.FromOrgUnitId : (object)DBNull.Value),
                    new SqlParameter("FromUserId", searchCriteriaTransactionReport.FromEmployeeId > 0 ? searchCriteriaTransactionReport.FromEmployeeId : (object)DBNull.Value),
                    new SqlParameter("ToOrgUnitId", searchCriteriaTransactionReport.ToOrgUnitId > 0 ? searchCriteriaTransactionReport.ToOrgUnitId : (object)DBNull.Value),
                    new SqlParameter("ToUserId", searchCriteriaTransactionReport.ToEmployeeId > 0 ? searchCriteriaTransactionReport.ToEmployeeId : (object)DBNull.Value),

                    new SqlParameter("CultureName", searchCriteriaTransactionReport.CultureName),
                    new SqlParameter("PageIndex", searchCriteriaTransactionReport.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaTransactionReport.PageSize),

                    new SqlParameter("EntitID", searchCriteriaTransactionReport.EntityId),
                    //new SqlParameter("UserID", searchCriteriaTransactionReport.UserId),
                    //new SqlParameter("Level", searchCriteriaTransactionReport.Level),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                else
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_Cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    //p_TransactionStatusId,
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionReportResult>(
                         @"BEGIN REPORT_TRANSACTIONSSEARCH 
                    (:p_DateFrom, :p_DateTo, :p_TransactionCategoryId, :p_TransactionNumber, :p_TransactioDescription,
                    :p_TransactionTypeId, :p_IsAppointment, :p_AppointmentDate, :p_ConfidentialityId, :p_PriorityId,:p_LetterTypeId,:p_Remarks,:p_DeliveryMethodId,:p_TransactionStatusId,
                    :p_FullName,:p_CivilID,:p_MobileNumber,:p_IsForIndividual,:p_InboundDateH,:p_ExternalPartyId,:p_DocumentNumber,:p_OutBoundDate,:p_FromOrgUnitId,
                    :p_FromUserId,:p_ToOrgUnitId,:p_ToUserId,:p_CultureName,
                    :p_PageIndex,:p_PageSize,:P_ENTITY_ID,:P_USER_ID,:p_Level,:p_TotalCount,:p_Cur); END;",
                     new OracleParameter(":p_DateFrom", OracleDbType.Date, searchCriteriaTransactionReport.From, ParameterDirection.Input),
                     new OracleParameter(":p_DateTo", OracleDbType.Date, searchCriteriaTransactionReport.To, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionCategoryId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionCategoryId, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionNumber", OracleDbType.Int32, searchCriteriaTransactionReport.Number > 0 ? searchCriteriaTransactionReport.Number : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactioDescription", OracleDbType.NVarchar2, searchCriteriaTransactionReport.Subject ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionTypeId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionTypeId > 0 ? searchCriteriaTransactionReport.TransactionTypeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_IsAppointment", OracleDbType.Int32, searchCriteriaTransactionReport.IsAppointment == false ? (object)DBNull.Value : 1, ParameterDirection.Input),
                     new OracleParameter(":p_AppointmentDate", OracleDbType.Date, searchCriteriaTransactionReport.AppointmentDate ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ConfidentialityId", OracleDbType.Int32, searchCriteriaTransactionReport.ConfidentialityLevelId > 0 ? searchCriteriaTransactionReport.ConfidentialityLevelId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_PriorityId", OracleDbType.Int32, searchCriteriaTransactionReport.PriorityLevelId > 0 ? searchCriteriaTransactionReport.PriorityLevelId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_LetterTypeId", OracleDbType.Int32, searchCriteriaTransactionReport.LetterTypeId > 0 ? searchCriteriaTransactionReport.LetterTypeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_Remarks", OracleDbType.NVarchar2, searchCriteriaTransactionReport.Remarks ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_DeliveryMethodId", OracleDbType.Int32, searchCriteriaTransactionReport.DeliveryMethodId > 0 ? searchCriteriaTransactionReport.DeliveryMethodId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_TransactionStatusId", OracleDbType.Int32, searchCriteriaTransactionReport.TransactionStatusId > 0 ? searchCriteriaTransactionReport.TransactionStatusId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FullName", OracleDbType.NVarchar2, searchCriteriaTransactionReport.FullName ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_CivilID", OracleDbType.NVarchar2, searchCriteriaTransactionReport.CivilID ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_MobileNumber", OracleDbType.NVarchar2, searchCriteriaTransactionReport.MobileNumber ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_IsForIndividual", OracleDbType.Int32, searchCriteriaTransactionReport.IsForIndividual == false ? 0 : 1, ParameterDirection.Input),
                     new OracleParameter(":p_InboundDateH", OracleDbType.NVarchar2, searchCriteriaTransactionReport.InboundDateH ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ExternalPartyId", OracleDbType.Int32, searchCriteriaTransactionReport.DestinationId > 0 ? searchCriteriaTransactionReport.DestinationId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_DocumentNumber", OracleDbType.NVarchar2, searchCriteriaTransactionReport.InboundDocumentNumber ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_OutBoundDate", OracleDbType.NVarchar2, searchCriteriaTransactionReport.OutboundDateH ?? (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FromOrgUnitId", OracleDbType.Int32, searchCriteriaTransactionReport.FromOrgUnitId > 0 ? searchCriteriaTransactionReport.FromOrgUnitId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_FromUserId", OracleDbType.Int32, searchCriteriaTransactionReport.FromEmployeeId > 0 ? searchCriteriaTransactionReport.FromEmployeeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ToOrgUnitId", OracleDbType.Int32, searchCriteriaTransactionReport.ToOrgUnitId > 0 ? searchCriteriaTransactionReport.ToOrgUnitId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_ToUserId", OracleDbType.Int32, searchCriteriaTransactionReport.ToEmployeeId > 0 ? searchCriteriaTransactionReport.ToEmployeeId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaTransactionReport.CultureName, ParameterDirection.Input),
                     new OracleParameter(":P_ENTITY_ID", OracleDbType.Int32, searchCriteriaTransactionReport.EntityId.HasValue ? searchCriteriaTransactionReport.EntityId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":P_USER_ID", OracleDbType.Int32, searchCriteriaTransactionReport.UserId.HasValue ? searchCriteriaTransactionReport.UserId : (object)DBNull.Value, ParameterDirection.Input),
                     new OracleParameter(":p_Level", OracleDbType.Int32, searchCriteriaTransactionReport.Level, ParameterDirection.Input),
                     new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaTransactionReport.PageIndex, ParameterDirection.Input),
                     new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaTransactionReport.PageSize, ParameterDirection.Input),
                         orcTotalOutParam,
                         orcOutParam
                     ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SentTransactionReportResult> SentTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                List<SentTransactionReportResult> baseSearchResults = null;

                var assignmentTrxQuery = _oMCSDbContext.TransactionAssignments.Where(a => ((a.FromUserId != a.ToUserId) || (a.FromUserId == a.ToUserId && a.FromEntityId != a.ToEntityId))
                  && (a.TrayId == (int)TrayType.MyTransactions | a.TrayId == (int)TrayType.OrgUnit | a.TrayId == (int)TrayType.DraftOutbound)
                    && a.Transaction.StatusId == InProcess
                    && a.Transaction.TransactionCategoryId != ExternalOutbound && a.Transaction.CreatedOn >= searchCriteriaTransactionReport.From && a.Transaction.CreatedOn <= searchCriteriaTransactionReport.To);


                if (searchCriteriaTransactionReport.FromOrgUnitId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.FromEntityId == searchCriteriaTransactionReport.FromOrgUnitId);
                }

                if (searchCriteriaTransactionReport.ToOrgUnitId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.ToEntityId == searchCriteriaTransactionReport.ToOrgUnitId);
                }
                if (searchCriteriaTransactionReport.TransactionCategoryId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.Transaction.TransactionCategoryId == searchCriteriaTransactionReport.TransactionCategoryId);
                }
                TotalCount = assignmentTrxQuery.Count();
                int skip = searchCriteriaTransactionReport.PageIndex > 0 ? searchCriteriaTransactionReport.PageIndex * searchCriteriaTransactionReport.PageSize : 0;

                var model = (from assignmentTrx in assignmentTrxQuery
                             select new
                             {
                                 assignmentTrx.Transaction.Number,
                                 assignmentTrx.Id,
                                 LetterTypeText = assignmentTrx.Transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 FromEntityText = assignmentTrx.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ToEntityText = assignmentTrx.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 AssignedDate = assignmentTrx.CreatedOn,
                                 TransactionStatus = assignmentTrx.Transaction.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionStatusId = assignmentTrx.Transaction.StatusId,
                                 FromEntityId = assignmentTrx.FromEntityId,
                                 ToEntityId = assignmentTrx.ToEntityId,
                                 ConfidentialityText = assignmentTrx.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ConfidentialityId = assignmentTrx.Transaction.ConfidentialityId,
                                 PriorityLevel = assignmentTrx.Transaction.Confidentiality.Weight,
                                 TransactionTypeText = assignmentTrx.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionTypeId = assignmentTrx.Transaction.TransactionTypeId,
                                 PriorityText = assignmentTrx.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryText = assignmentTrx.Transaction.TransactionCategory.Localizations.Where(x => x.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryId = assignmentTrx.Transaction.TransactionCategoryId,
                                 TransactionDate = assignmentTrx.Transaction.CreatedOn,
                                 Subject = assignmentTrx.Transaction.Subject,
                                 TransactionElcOwner =assignmentTrx.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionPhysicalOwner = assignmentTrx.PhysicalEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,


                             }).ToList().Select(t => new SentTransactionReportResult
                             {
                                 PriorityId = t.PriorityLevel,
                                 TransactionId = t.Id,
                                 TransactionStatusText = t.TransactionStatus,
                                 ConfidentialityText = t.ConfidentialityText,
                                 ConfidentialityId = t.ConfidentialityId,
                                 TransactionStatusId = t.TransactionStatusId,
                                 LetterTypeText = t.LetterTypeText,
                                 AssignedDate = t.AssignedDate,
                                 ToEntityText = t.ToEntityText,
                                 ToEntityId = t.ToEntityId,
                                 FromEntityId = t.FromEntityId,
                                 FromEntityText = t.FromEntityText,
                                 TransactionTypeText = t.TransactionTypeText,
                                 TransactionTypeId = t.TransactionTypeId.HasValue ? t.TransactionTypeId.Value : 0,
                                 Number = t.Number,
                                 PriorityText = t.PriorityText,
                                 TransactionCategoryText = t.TransactionCategoryText,
                                 TransactionCategoryId = t.TransactionCategoryId,
                                 TransactionDate = t.TransactionDate,
                                 Subject = t.Subject,
                                 TransactionElcOwner= t.TransactionElcOwner,
                                 TransactionPhysicalOwner = t.TransactionPhysicalOwner

                             });


                baseSearchResults = searchCriteriaTransactionReport.PageIndex >= 0 ? baseSearchResults = model.Skip(skip).Take(searchCriteriaTransactionReport.PageSize).ToList() : baseSearchResults = model.ToList();
                return baseSearchResults;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<SentTransactionReportResult> SentTransactionReportStatusSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                List<SentTransactionReportResult> baseSearchResults = null;

                var assignmentTrxQuery = _oMCSDbContext.TransactionAssignments.Where(a => ((a.FromUserId != a.ToUserId) || (a.FromUserId == a.ToUserId && a.FromEntityId != a.ToEntityId))
                  && (a.TrayId == (int)TrayType.MyTransactions | a.TrayId == (int)TrayType.OrgUnit | a.TrayId == (int)TrayType.DraftOutbound)
                    && a.Transaction.StatusId == InProcess
                    && a.Transaction.TransactionCategoryId != ExternalOutbound && a.Transaction.CreatedOn >= searchCriteriaTransactionReport.From && a.Transaction.CreatedOn <= searchCriteriaTransactionReport.To);


                if (searchCriteriaTransactionReport.FromOrgUnitId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.FromEntityId == searchCriteriaTransactionReport.FromOrgUnitId);
                }

                if (searchCriteriaTransactionReport.ToOrgUnitId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.ToEntityId == searchCriteriaTransactionReport.ToOrgUnitId);
                }
                if (searchCriteriaTransactionReport.TransactionCategoryId > 0)
                {
                    assignmentTrxQuery = assignmentTrxQuery.Where(x => x.Transaction.TransactionCategoryId == searchCriteriaTransactionReport.TransactionCategoryId);
                }
                TotalCount = assignmentTrxQuery.Count();
                int skip = searchCriteriaTransactionReport.PageIndex > 0 ? searchCriteriaTransactionReport.PageIndex * searchCriteriaTransactionReport.PageSize : 0;

                var model = (from assignmentTrx in assignmentTrxQuery
                             select new
                             {
                                 assignmentTrx.Transaction.Number,
                                 assignmentTrx.Id,
                                 LetterTypeText = assignmentTrx.Transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 FromEntityText = assignmentTrx.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ToEntityText = assignmentTrx.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 AssignedDate = assignmentTrx.CreatedOn,
                                 TransactionStatus = assignmentTrx.Transaction.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionStatusId = assignmentTrx.Transaction.StatusId,
                                 FromEntityId = assignmentTrx.FromEntityId,
                                 ToEntityId = assignmentTrx.ToEntityId,
                                 ConfidentialityText = assignmentTrx.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ConfidentialityId = assignmentTrx.Transaction.ConfidentialityId,
                                 PriorityLevel = assignmentTrx.Transaction.Confidentiality.Weight,
                                 TransactionTypeText = assignmentTrx.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionTypeId = assignmentTrx.Transaction.TransactionTypeId,
                                 PriorityText = assignmentTrx.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryText = assignmentTrx.Transaction.TransactionCategory.Localizations.Where(x => x.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryId = assignmentTrx.Transaction.TransactionCategoryId,
                                 TransactionDate = assignmentTrx.Transaction.CreatedOn,
                                 Subject = assignmentTrx.Transaction.Subject,
                                 TransactionElcOwner = assignmentTrx.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionPhysicalOwner = assignmentTrx.PhysicalEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 Viewed = assignmentTrx.Viewed,

                             }).ToList().Select(t => new SentTransactionReportResult
                             {
                                 PriorityId = t.PriorityLevel,
                                 TransactionId = t.Id,
                                 TransactionStatusText = t.TransactionStatus,
                                 ConfidentialityText = t.ConfidentialityText,
                                 ConfidentialityId = t.ConfidentialityId,
                                 TransactionStatusId = t.TransactionStatusId,
                                 LetterTypeText = t.LetterTypeText,
                                 AssignedDate = t.AssignedDate,
                                 ToEntityText = t.ToEntityText,
                                 ToEntityId = t.ToEntityId,
                                 FromEntityId = t.FromEntityId,
                                 FromEntityText = t.FromEntityText,
                                 TransactionTypeText = t.TransactionTypeText,
                                 TransactionTypeId = t.TransactionTypeId.HasValue ? t.TransactionTypeId.Value : 0,
                                 Number = t.Number,
                                 PriorityText = t.PriorityText,
                                 TransactionCategoryText = t.TransactionCategoryText,
                                 TransactionCategoryId = t.TransactionCategoryId,
                                 TransactionDate = t.TransactionDate,
                                 Subject = t.Subject,
                                 TransactionElcOwner = t.TransactionElcOwner,
                                 TransactionPhysicalOwner = t.TransactionPhysicalOwner,
                                 Viewed = t.Viewed,

                             });


                baseSearchResults = searchCriteriaTransactionReport.PageIndex >= 0 ? baseSearchResults = model.Skip(skip).Take(searchCriteriaTransactionReport.PageSize).ToList() : baseSearchResults = model.ToList();
                return baseSearchResults;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TaskReportResult> TasksReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {

                List<TaskReportResult> baseSearchResults = null;



                var trxQuery = _oMCSDbContext.Tasks.Where(x => x.CreatedOn >= searchCriteriaTransactionReport.From && x.CreatedOn <= searchCriteriaTransactionReport.To).Include(x => x.Transaction);
                if (searchCriteriaTransactionReport.TransactionCategoryId > 0)
                {
                    trxQuery = trxQuery.Where(x => x.Transaction.TransactionCategoryId == searchCriteriaTransactionReport.TransactionCategoryId);
                }

                if (searchCriteriaTransactionReport.TransactionStatusId > 0)
                {
                    trxQuery = trxQuery.Where(x => x.StatusId == searchCriteriaTransactionReport.TransactionStatusId);
                }

                TotalCount = trxQuery.Count();
                int skip = searchCriteriaTransactionReport.PageIndex > 0 ? searchCriteriaTransactionReport.PageIndex * searchCriteriaTransactionReport.PageSize : 0;

                var model = (from trx in trxQuery
                             select new
                             {
                                 trx.Transaction.Number,
                                 trx.Id,
                                 LetterTypeText = trx.Transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 FromEntityText = trx.Transaction.Assignments.FirstOrDefault().FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ToEntityText = trx.Transaction.Assignments.FirstOrDefault().ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 Date = trx.CreatedOn,
                                 TransactionStatus = trx.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionStatusId = trx.StatusId,
                                 FromEntityId = trx.Transaction.Assignments.FirstOrDefault().FromEntityId,
                                 ToEntityId = trx.Transaction.Assignments.FirstOrDefault().ToEntityId,
                                 ConfidentialityText = trx.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ConfidentialityId = trx.Transaction.ConfidentialityId,
                                 PriorityLevel = trx.Transaction.Confidentiality.Weight,
                                 TransactionTypeText = trx.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionTypeId = trx.Transaction.TransactionTypeId,
                                 PriorityText = trx.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryId = trx.Transaction.TransactionCategoryId,
                                 TransactionCategoryText = trx.Transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text

                             }).ToList().Select(t => new TaskReportResult
                             {
                                 PriorityId = t.PriorityLevel,
                                 TransactionId = t.Id,
                                 TransactionStatusText = t.TransactionStatus,
                                 ConfidentialityText = t.ConfidentialityText,
                                 ConfidentialityId = t.ConfidentialityId,
                                 TransactionStatusId = t.TransactionStatusId,
                                 LetterTypeText = t.LetterTypeText,
                                 Date = t.Date,
                                 ToEntityText = t.ToEntityText,
                                 ToEntityId = t.ToEntityId,
                                 FromEntityId = t.FromEntityId,
                                 FromEntityText = t.FromEntityText,
                                 TransactionTypeText = t.TransactionTypeText,
                                 TransactionTypeId = t.TransactionTypeId.HasValue ? t.TransactionTypeId.Value : 0,
                                 Number = t.Number,
                                 PriorityText = t.PriorityText,
                                 TransactionCategoryId = t.TransactionCategoryId,
                                 TransactionCategoryText = t.TransactionCategoryText,
                                 CreatedOn = t.Date

                             });


                baseSearchResults = searchCriteriaTransactionReport.PageIndex >= 0 ? baseSearchResults = model.Skip(skip).Take(searchCriteriaTransactionReport.PageSize).ToList() : baseSearchResults = model.ToList();
                return baseSearchResults;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<FollowupReportResult> FollowupReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {

                List<FollowupReportResult> baseSearchResults = null;

                var trxQuery = _oMCSDbContext.TransactionFollowUps.Where(x => x.CreationDate >= searchCriteriaTransactionReport.From && x.CreationDate <= searchCriteriaTransactionReport.To);


                if (searchCriteriaTransactionReport.TransactionStatusId > 0)
                {
                    switch (searchCriteriaTransactionReport.TransactionStatusId)
                    {
                        case (int)FollowupStatus.All:
                            trxQuery = trxQuery.Where(x=> 1==1);
                            break;
                        case (int)FollowupStatus.New:
                            trxQuery = trxQuery.Where(x => x.FollowUpStatusId == (int)FollowupStatus.New && x.FollowUpExpireDate > DateTime.Now);
                            break;
                        case (int)FollowupStatus.UnderFollowup:
                            trxQuery = trxQuery.Where(x => x.FollowUpStatusId == (int)FollowupStatus.UnderFollowup && x.FollowUpExpireDate > DateTime.Now);
                            break;
                        case (int)FollowupStatus.Completed:
                            trxQuery = trxQuery.Where(x => x.FollowUpStatusId == (int)FollowupStatus.Completed && x.FollowUpExpireDate > DateTime.Now);
                            break;
                        case (int)FollowupStatus.Delayed:
                            trxQuery = trxQuery.Where(x => x.FollowUpStatusId == (int)FollowupStatus.Delayed ||  x.FollowUpExpireDate <  DateTime.Now);
                            break;
                        case (int)FollowupStatus.Cancled:
                            trxQuery = trxQuery.Where(x => x.FollowUpStatusId == (int)FollowupStatus.Cancled);
                            break;
                    }

                }

                TotalCount = trxQuery.Count();
                int skip = searchCriteriaTransactionReport.PageIndex > 0 ? searchCriteriaTransactionReport.PageIndex * searchCriteriaTransactionReport.PageSize : 0;

                var model = (from TrxFollowup in trxQuery
                             join trx in _oMCSDbContext.Transactions on TrxFollowup.TransactionId equals trx.Id

                             select new
                             {
                                 trx.Number,
                                 trx.Id,
                                 ToUserText = trx.Assignments.FirstOrDefault().ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 //ToUserId = TrxFollowup.FollowUpUserId,
                                 FromUserText = trx.Assignments.FirstOrDefault().FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 //FromUserId = TrxFollowup.CreatingUserId,
                                 LetterTypeText = trx.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 FromEntityText = trx.Assignments.FirstOrDefault().FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ToEntityText = trx.Assignments.FirstOrDefault().ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ToDate = TrxFollowup.FollowUpExpireDate,
                                 TransactionStatus = trx.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionStatusId = trx.StatusId,
                                 ConfidentialityText = trx.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 ConfidentialityId = trx.ConfidentialityId,
                                 PriorityLevel = trx.Confidentiality.Weight,
                                 TransactionTypeText = trx.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionTypeId = trx.TransactionTypeId,
                                 PriorityText = trx.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 TransactionCategoryId = trx.TransactionCategoryId,
                                 TransactionCategoryText = trx.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteriaTransactionReport.CultureName).FirstOrDefault().Text,
                                 CreatedOn = TrxFollowup.CreationDate,
                                 Subject = trx.Subject,
                                 GeneralExplanation = trx.Assignments.FirstOrDefault().GeneralExplanation,
                                 TransactionAssignment = trx.Assignments,
                                 TrxFollowup.FollowUpStatusId

                             }).ToList().Select(t => new FollowupReportResult
                             {
                                 PriorityId = t.PriorityLevel,
                                 TransactionId = t.Id,
                                 TransactionStatusText = t.TransactionStatus,
                                 ConfidentialityText = t.ConfidentialityText,
                                 ConfidentialityId = t.ConfidentialityId,
                                 TransactionStatusId = t.TransactionStatusId,
                                 LetterTypeText = t.LetterTypeText,
                                 RemindDate = t.ToDate,
                                 ToEntityText = t.ToEntityText,
                                 FromEntityText = t.FromEntityText,
                                 TransactionTypeText = t.TransactionTypeText,
                                 TransactionTypeId = t.TransactionTypeId.HasValue ? t.TransactionTypeId.Value : 0,
                                 Number = t.Number,
                                 PriorityText = t.PriorityText,
                                 TransactionCategoryId = t.TransactionCategoryId,
                                 TransactionCategoryText = t.TransactionCategoryText,
                                 CreatedOn = t.CreatedOn,
                                 Date = t.CreatedOn,
                                 FromUserText = t.FromUserText,
                                 ToUserText = t.ToUserText,
                                 ToUserId = t.TransactionAssignment.FirstOrDefault()?.ToUserId,
                                 FromUserId = t.TransactionAssignment.FirstOrDefault()?.FromUserId,
                                 Subject = t.Subject,
                                 GeneralExplanation = t.GeneralExplanation,
                                 Assignments = t.TransactionAssignment,
                                 FromEntityId = t.TransactionAssignment.FirstOrDefault()?.FromEntityId,
                                 ToEntityId = t.TransactionAssignment.FirstOrDefault()?.ToEntityId,
                             });
                if (searchCriteriaTransactionReport.TransactionCategoryId > 0)
                {
                    model = model.Where(x => x.TransactionCategoryId == searchCriteriaTransactionReport.TransactionCategoryId);
                }
                baseSearchResults = searchCriteriaTransactionReport.PageIndex >= 0 ? baseSearchResults = model.Skip(skip).Take(searchCriteriaTransactionReport.PageSize).ToList() : baseSearchResults = model.ToList();
                return baseSearchResults;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<PerformanceMeasurementReportResult> PerformanceMeasurementReportSearch(SearchCriteriaPerformanceMeasurementReport searchCriteriaPerformanceMeasurementReport, out int TotalCount)
        {
            try
            {

                List<PerformanceMeasurementReportResult> baseSearchResults = null;
                TotalCount = 0;
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;

                    string storedProcedureName = searchCriteriaPerformanceMeasurementReport.ReportType == 1 ? "ReportStatisticalSecretary" : "ReportStatisticalEmployee";

                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<PerformanceMeasurementReportResult>(storedProcedureName + @" 
                                        @ReportType,@FromDate,@ToDate,@EntitID,@UserID,@level,
                                        @LetterTypeId,@AppointmentDate,@ConfidentialityId,@PriorityId,@TransactionTypeId,@Remarks,@DeliveryMethodId,
                                        @PageIndex,@PageSize ,@DraftOutbound,@InternalOutbound ,@Inbound,@ExternalOutbound,@TotalCount out",
                    new SqlParameter("ReportType", searchCriteriaPerformanceMeasurementReport.ReportType),
                    new SqlParameter("FromDate", searchCriteriaPerformanceMeasurementReport.From),
                    new SqlParameter("ToDate", searchCriteriaPerformanceMeasurementReport.To),
                    new SqlParameter("EntitID", searchCriteriaPerformanceMeasurementReport.OrgUnitId > 0 ? searchCriteriaPerformanceMeasurementReport.OrgUnitId : -1),
                    new SqlParameter("UserID", searchCriteriaPerformanceMeasurementReport.EmployeeId > 0 ? searchCriteriaPerformanceMeasurementReport.EmployeeId : -1),
                    new SqlParameter("level", searchCriteriaPerformanceMeasurementReport.Level > 0 ? searchCriteriaPerformanceMeasurementReport.Level : -1),
                    new SqlParameter("LetterTypeId", searchCriteriaPerformanceMeasurementReport.LetterTypeId > 0 ? searchCriteriaPerformanceMeasurementReport.LetterTypeId : -1),
                    new SqlParameter("AppointmentDate", searchCriteriaPerformanceMeasurementReport.AppointmentDate ?? (object)DBNull.Value),
                    new SqlParameter("ConfidentialityId", searchCriteriaPerformanceMeasurementReport.ConfidentialityLevelId > 0 ? searchCriteriaPerformanceMeasurementReport.ConfidentialityLevelId : -1),
                    new SqlParameter("PriorityId", searchCriteriaPerformanceMeasurementReport.PriorityLevelId > 0 ? searchCriteriaPerformanceMeasurementReport.PriorityLevelId : -1),
                    new SqlParameter("TransactionTypeId", searchCriteriaPerformanceMeasurementReport.TransactionTypeId > 0 ? searchCriteriaPerformanceMeasurementReport.TransactionTypeId : -1),
                    new SqlParameter("Remarks", searchCriteriaPerformanceMeasurementReport.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("DeliveryMethodId", searchCriteriaPerformanceMeasurementReport.DeliveryMethodId > 0 ? searchCriteriaPerformanceMeasurementReport.DeliveryMethodId : -1),
                    new SqlParameter("PageIndex", searchCriteriaPerformanceMeasurementReport.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaPerformanceMeasurementReport.PageSize),
                    new SqlParameter("DraftOutbound", DraftOutbound),
                    new SqlParameter("InternalOutbound", InternalOutbound),
                    new SqlParameter("Inbound", Inbound),
                    new SqlParameter("ExternalOutbound", ExternalOutbound),
                    sqlPTotalCount
                    ).ToList();
                    if (!string.IsNullOrEmpty(sqlPTotalCount.Value.ToString()))
                    {
                        TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                    }
                }
                else
                {
                    OracleParameter orcOutParam = new OracleParameter(":cv_1", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<PerformanceMeasurementReportResult>(
                         @"BEGIN REPORT_STATISTICALS 
                    (:P_ReportType,:P_FromDate,:P_ToDate,:P_EntitID,:P_UserID,:P_level,
                     :P_LetterTypeId,:P_AppointmentDate,:P_ConfidentialityId,:P_PriorityId,:P_TransactionTypeId,:P_DeliveryMethodId,:P_Remarks,
                     :p_PageIndex,:p_PageSize, :p_Status, :p_Inbound,:p_Outbound, :p_Draft, :p_Internal, :p_TotalCount,:cv_1); END;",
                    new OracleParameter(":P_ReportType", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.ReportType, ParameterDirection.Input),
                    new OracleParameter(":P_FromDate", OracleDbType.Date, searchCriteriaPerformanceMeasurementReport.From, ParameterDirection.Input),
                    new OracleParameter(":P_ToDate", OracleDbType.Date, searchCriteriaPerformanceMeasurementReport.To, ParameterDirection.Input),
                    new OracleParameter(":P_EntitID", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.OrgUnitId > 0 ? searchCriteriaPerformanceMeasurementReport.OrgUnitId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_UserID", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.EmployeeId > 0 ? searchCriteriaPerformanceMeasurementReport.EmployeeId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_level", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.Level > 0 ? searchCriteriaPerformanceMeasurementReport.Level : -1, ParameterDirection.Input),
                    new OracleParameter(":P_LetterTypeId", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.LetterTypeId > 0 ? searchCriteriaPerformanceMeasurementReport.LetterTypeId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_AppointmentDate", OracleDbType.Date, searchCriteriaPerformanceMeasurementReport.AppointmentDate ?? (object)DBNull.Value, ParameterDirection.Input),
                    new OracleParameter(":P_ConfidentialityId", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.ConfidentialityLevelId > 0 ? searchCriteriaPerformanceMeasurementReport.ConfidentialityLevelId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_PriorityId", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.PriorityLevelId > 0 ? searchCriteriaPerformanceMeasurementReport.PriorityLevelId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_TransactionTypeId", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.TransactionTypeId > 0 ? searchCriteriaPerformanceMeasurementReport.TransactionTypeId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_DeliveryMethodId", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.DeliveryMethodId > 0 ? searchCriteriaPerformanceMeasurementReport.DeliveryMethodId : -1, ParameterDirection.Input),
                    new OracleParameter(":P_Remarks", OracleDbType.NVarchar2, searchCriteriaPerformanceMeasurementReport.Remarks ?? (object)DBNull.Value, ParameterDirection.Input),
                    new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.PageIndex, ParameterDirection.Input),
                    new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaPerformanceMeasurementReport.PageSize, ParameterDirection.Input),
                    new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                    new OracleParameter(":p_Inbound", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                    new OracleParameter(":p_Outbound", OracleDbType.Int32, TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                    new OracleParameter(":p_Draft", OracleDbType.Int32, TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                    new OracleParameter(":p_Internal", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                         orcTotalOutParam,
                         orcOutParam
                     ).ToList();

                    if (int.TryParse(orcTotalOutParam.Value.ToString(), out TotalCount))
                    {
                        TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                    }
                }
                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
    public interface IReportWrapper
    {
        List<TransactionReportResult> TransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);
        List<TransactionReportResult> SecretaryTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);
        List<PerformanceMeasurementReportResult> PerformanceMeasurementReportSearch(SearchCriteriaPerformanceMeasurementReport searchCriteriaPerformanceMeasurementReport, out int TotalCount);
        List<SentTransactionReportResult> SentTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);
        List<TaskReportResult> TasksReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);
        List<FollowupReportResult> FollowupReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);
        List<SentTransactionReportResult> SentTransactionReportStatusSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);

    }
}
