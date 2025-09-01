using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.MobileSearchCriteria;

namespace MCS.DataAccess
{
    public interface IMobileWrapper
    {
        UserAccompleshmentsReportResult GetUserAccompleshmentsReport(int userId, int entityId);
        List<EntitiesAccompleshmentsReportResult> GetEntitiesAccompleshmentsReport(int entityId, int periodCount, int selectedPeriod);
        List<MobileSearchResult> MobileSearch(SearchCriteria searchCriteriaByCreator, string cultureName);
    }
    public class MobileWrapper : BaseWrappers, IMobileWrapper
    {
        public MobileWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
          : base(ambienTTransactionContextLocator) { }

        public UserAccompleshmentsReportResult GetUserAccompleshmentsReport(int userId, int entityId)
        {
            UserAccompleshmentsReportResult userAccompleshmentsReportResult = null;
            if (!SystemConfigurations.IsOracleMigrationEnabled)
            {
                _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                userAccompleshmentsReportResult = _oMCSDbContext.Database.SqlQuery<UserAccompleshmentsReportResult>(@"USER_MOBILE_USER_ACCOMPLESHMENTS 
                                        @UserId, @EntityId",
                new SqlParameter("UserId", userId),
                new SqlParameter("EntityId", entityId)).FirstOrDefault();
            }
            else
            {
                OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                userAccompleshmentsReportResult = _oMCSDbContext.Database.SqlQuery<UserAccompleshmentsReportResult>(
                     @"BEGIN USER_MOBILE_DASHBOARD_USER_ACCOMPLESHMENTS 
                    (:p_ENTITY_ID,:p_USER_ID, :p_Status, :p_Inbound, :p_Internal, :p_cur); END;",
                new OracleParameter(":p_ENTITY_ID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                new OracleParameter(":p_USER_ID", OracleDbType.Int32, userId, ParameterDirection.Input),
                new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                new OracleParameter(":p_Inbound", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                new OracleParameter(":p_Internal", OracleDbType.Int32,TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                    orcOutParam).FirstOrDefault();
            }
            return userAccompleshmentsReportResult;
        }
        public List<EntitiesAccompleshmentsReportResult> GetEntitiesAccompleshmentsReport(int entityId, int periodCount, int selectedPeriod)
        {
            List<EntitiesAccompleshmentsReportResult> entitiesAccompleshmentsReportResults = null;
            if (!SystemConfigurations.IsOracleMigrationEnabled)
            {
                _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                entitiesAccompleshmentsReportResults = _oMCSDbContext.Database.SqlQuery<EntitiesAccompleshmentsReportResult>(@"USER_MOBILE_ENTITIES_ACCOMPLESHMENTS 
                                        @UserId, @PeriodCount, @SelectedPeriod",
                new SqlParameter("UserId", entityId),
                new SqlParameter("PeriodCount", periodCount),
                new SqlParameter("SelectedPeriod", selectedPeriod)).ToList();
            }
            else
            {
                OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                entitiesAccompleshmentsReportResults = _oMCSDbContext.Database.SqlQuery<EntitiesAccompleshmentsReportResult>(
                     @"BEGIN USER_MOBILE_DASHBOARD_ENTITIES_ACCOMPLESHMENTS 
                    (:p_ENTITY_ID, :p_PERIOD_COUNT, :p_SELECTED_PERIOD, :p_Status, :p_Inbound,:p_Internal, :p_cur); END;",
                new OracleParameter(":p_ENTITY_ID", OracleDbType.Int32, entityId, ParameterDirection.Input),
                new OracleParameter(":p_PERIOD_COUNT", OracleDbType.Int32, periodCount, ParameterDirection.Input),
                new OracleParameter(":p_SELECTED_PERIOD", OracleDbType.Int32, selectedPeriod, ParameterDirection.Input),
                new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                new OracleParameter(":p_Inbound", OracleDbType.Int32, TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                new OracleParameter(":p_Internal", OracleDbType.Int32, TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), ParameterDirection.Input),
                orcOutParam).ToList();
            }
            return entitiesAccompleshmentsReportResults;
        }
        public List<MobileSearchResult> MobileSearch(SearchCriteria searchCriteriaByCreator, string cultureName)
        {
            List<MobileSearchResult> baseSearchResults = null;

            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                baseSearchResults = _oMCSDbContext.Database.SqlQuery<MobileSearchResult>(
                "BEGIN MOBILE_SEARCH (:p_Number, :p_OrgUnitId, :p_TransactionTypeId, :p_Subject, :p_TransCategory, :p_CultureName, :p_cur); END;",
                        new OracleParameter(":p_Number", OracleDbType.Int32, searchCriteriaByCreator.TransNo, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByCreator.EntityId, ParameterDirection.Input),
                        new OracleParameter(":p_TransactionTypeId", OracleDbType.Int32, searchCriteriaByCreator.TransSource, ParameterDirection.Input),
                        new OracleParameter(":p_Subject", OracleDbType.NVarchar2, searchCriteriaByCreator.Subject, ParameterDirection.Input),
                        new OracleParameter(":p_TransCategory", OracleDbType.Int32, searchCriteriaByCreator.TransCategory, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, cultureName, ParameterDirection.Input),
                        orcOutParam
                        ).ToList();
            }
            else
            {
                _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                baseSearchResults = _oMCSDbContext.Database.SqlQuery<MobileSearchResult>(
                    "MobileSearch @Number, @OrgUnitId, @TransactionTypeId , @Subject, @TransCategory, @CultureName",
                new SqlParameter("Number", searchCriteriaByCreator.TransNo),
                new SqlParameter("OrgUnitId", searchCriteriaByCreator.EntityId),
                new SqlParameter("TransactionTypeId", searchCriteriaByCreator.TransSource),
                new SqlParameter("Subject", searchCriteriaByCreator.Subject),
                new SqlParameter("TransCategory", searchCriteriaByCreator.TransCategory),
                new SqlParameter("CultureName", cultureName)
                ).ToList();
            }

            return baseSearchResults;
        }
    }
}
