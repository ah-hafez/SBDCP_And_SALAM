using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class SearchWrapper : BaseWrappers, ISearchWrapper
    {
        public SearchWrapper(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator) { }
        public IList<InboundSearchResult> InboundSearch(SearchCriteriaByInbound searchCriteriaByInbound, out int TotalCount)
        {
            try
            {

                if (searchCriteriaByInbound.Number == null)
                {
                    searchCriteriaByInbound.Number = -1;
                }

                searchCriteriaByInbound.OrderBy = "TransactionTypeId";

                IList<InboundSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "BEGIN SEARCHINBOUND (:p_NUMBER,:p_ORGUNITID, :p_TRANSACTIONTYPEID,:p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_YEAR,  :p_TOTALCOUNT , :cur); END;",
                        new OracleParameter(":p_NUMBER", OracleDbType.Int32, searchCriteriaByInbound.Number, ParameterDirection.Input),
                        new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByInbound.OrgUnitId.HasValue ? searchCriteriaByInbound.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_TRANSACTIONTYPEID", OracleDbType.Int32, searchCriteriaByInbound.TransactionTypeId, ParameterDirection.Input),
                        new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByInbound.FromDateTime.HasValue ? searchCriteriaByInbound.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                        new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByInbound.ToDateTime.HasValue ? searchCriteriaByInbound.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                        new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByInbound.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByInbound.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByInbound.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByInbound.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByInbound.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_YEAR", OracleDbType.Int32, searchCriteriaByInbound.Year, ParameterDirection.Input),

                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "SearchInbound @Number, @OrgUnitId, @TransactionTypeId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@SourceTypeId, @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @Year,@DeliveryMethodId, @TotalCount out",

                    new SqlParameter("Number", searchCriteriaByInbound.Number),
                    new SqlParameter("OrgUnitId", !searchCriteriaByInbound.Global ? searchCriteriaByInbound.OrgUnitId : -1),
                    new SqlParameter("TransactionTypeId", searchCriteriaByInbound.TransactionTypeId),
                     new SqlParameter("ConfidentialityId", searchCriteriaByInbound.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByInbound.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByInbound.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByInbound.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByInbound.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByInbound.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByInbound.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("SourceTypeId", -1),
                    new SqlParameter("DateFrom", searchCriteriaByInbound.FromDateTime.HasValue ? searchCriteriaByInbound.FromDateTime : (object)DBNull.Value),
                    new SqlParameter("DateTo", searchCriteriaByInbound.ToDateTime.HasValue ? searchCriteriaByInbound.ToDateTime : (object)DBNull.Value),
                    new SqlParameter("PageIndex", searchCriteriaByInbound.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByInbound.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByInbound.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByInbound.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByInbound.OrderBy),
                    new SqlParameter("Year", searchCriteriaByInbound.Year ?? -1),
                    new SqlParameter("DeliveryMethodId", searchCriteriaByInbound.DeliveryMethodId),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (InboundSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByInbound.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<InboundSearchResult> SearchDocumentNumber(SearchCriteriaByDocumentNumber searchCriteriaByDocumentNumber, out int TotalCount)
        {
            try
            {

                searchCriteriaByDocumentNumber.OrderBy = "TransactionTypeId";

                IList<InboundSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "BEGIN SEARCHDOCUMENTNUMBER (:p_DOCUMENTNUMBER, :p_ORGUNITID, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_YEAR, :p_TOTALCOUNT, :cur); END;",
                        new OracleParameter(":p_DOCUMENTNUMBER", OracleDbType.NVarchar2, searchCriteriaByDocumentNumber.DocumentNumber, ParameterDirection.Input),
                        new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByDocumentNumber.OrgUnitId.HasValue ? searchCriteriaByDocumentNumber.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByDocumentNumber.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByDocumentNumber.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByDocumentNumber.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByDocumentNumber.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByDocumentNumber.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_YEAR", OracleDbType.Int32, searchCriteriaByDocumentNumber.Year, ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "SearchDocumentNumber @DocumentNumber, @OrgUnitId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @Year, @TotalCount out",
                    new SqlParameter("DocumentNumber", searchCriteriaByDocumentNumber.DocumentNumber),
                    new SqlParameter("OrgUnitId", !searchCriteriaByDocumentNumber.Global ? searchCriteriaByDocumentNumber.OrgUnitId : -1),
                    new SqlParameter("ConfidentialityId", searchCriteriaByDocumentNumber.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByDocumentNumber.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByDocumentNumber.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByDocumentNumber.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByDocumentNumber.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByDocumentNumber.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByDocumentNumber.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("PageIndex", searchCriteriaByDocumentNumber.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByDocumentNumber.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByDocumentNumber.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByDocumentNumber.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByDocumentNumber.OrderBy),
                    new SqlParameter("Year", searchCriteriaByDocumentNumber.Year ?? -1),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (InboundSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByDocumentNumber.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<InboundSearchResult> SearchRecordNumber(SearchCriteriaByRecordNumber searchCriteriaByRecordNumber, out int TotalCount)
        {
            try
            {

                searchCriteriaByRecordNumber.OrderBy = "TransactionTypeId";

                IList<InboundSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "BEGIN SEARCH_DOCUMENT_NUMBER (:p_DocumentNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_RecordNumber", OracleDbType.Int32, searchCriteriaByRecordNumber.RecordNumber, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByRecordNumber.OrgUnitId.HasValue ? searchCriteriaByRecordNumber.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByRecordNumber.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByRecordNumber.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByRecordNumber.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByRecordNumber.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByRecordNumber.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<InboundSearchResult>(
                        "SearchRecordNumber @RecordNumber, @OrgUnitId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("RecordNumber", searchCriteriaByRecordNumber.RecordNumber ?? -1),
                    new SqlParameter("OrgUnitId", !searchCriteriaByRecordNumber.Global ? searchCriteriaByRecordNumber.OrgUnitId : -1),
                    new SqlParameter("ConfidentialityId", searchCriteriaByRecordNumber.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByRecordNumber.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByRecordNumber.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByRecordNumber.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByRecordNumber.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByRecordNumber.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByRecordNumber.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("PageIndex", searchCriteriaByRecordNumber.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByRecordNumber.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByRecordNumber.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByRecordNumber.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByRecordNumber.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (InboundSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByRecordNumber.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<OutboundSearchResult> OutboundSearch(SearchCriteriaByOutbound searchCriteriaByOutbound, out int TotalCount)
        {
            try
            {

                if (searchCriteriaByOutbound.Number == null)
                {
                    searchCriteriaByOutbound.Number = -1;
                }

                searchCriteriaByOutbound.OrderBy = "TransactionTypeId";

                IList<OutboundSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundSearchResult>(
                    "BEGIN SEARCHOUTBOUNDEXTERNAL (:p_NUMBER,:p_ORGUNITID, :p_TRANSACTIONTYPEID, :p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_YEAR, :p_TOTALCOUNT, :cur); END;",
                            new OracleParameter(":p_NUMBER", OracleDbType.Int32, searchCriteriaByOutbound.Number, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByOutbound.OrgUnitId.HasValue ? searchCriteriaByOutbound.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONTYPEID", OracleDbType.Int32, searchCriteriaByOutbound.TypeId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByOutbound.FromDateTime.HasValue ? searchCriteriaByOutbound.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByOutbound.ToDateTime.HasValue ? searchCriteriaByOutbound.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByOutbound.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByOutbound.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByOutbound.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByOutbound.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByOutbound.OrderBy, ParameterDirection.Input),
                            new OracleParameter(":p_YEAR", OracleDbType.Int32, searchCriteriaByOutbound.Year, ParameterDirection.Input),
                             orcTotalOutParam,
                             orcOutParam
                             ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {

                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;

                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundSearchResult>(
                         "SearchOutboundExternal @Number, @OrgUnitId, @TransactionTypeId,@SourceTypeId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId, @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @Year,@DeliveryMethodId, @TotalCount out",
                     new SqlParameter("Number", searchCriteriaByOutbound.Number),
                    // new SqlParameter("HasFullPrivilege", searchCriteriaByOutbound.HasFullPrivilege),
                     new SqlParameter("OrgUnitId", !searchCriteriaByOutbound.Global ? searchCriteriaByOutbound.OrgUnitId : -1),
                     new SqlParameter("TransactionTypeId", searchCriteriaByOutbound.TypeId),
                     new SqlParameter("SourceTypeId", -1),
                     new SqlParameter("ConfidentialityId", searchCriteriaByOutbound.AdvancedSearch.ConfidentialityId ?? -1),
                     new SqlParameter("LetterTypeId", searchCriteriaByOutbound.AdvancedSearch.LetterTypeId ?? -1),
                     new SqlParameter("StatusId", searchCriteriaByOutbound.AdvancedSearch.StatusId ?? -1),
                     new SqlParameter("PriorityId", searchCriteriaByOutbound.AdvancedSearch.PriorityId ?? -1),
                     new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByOutbound.AdvancedSearch.DirectedToUserId) ? searchCriteriaByOutbound.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                     new SqlParameter("DestinationPartyId", searchCriteriaByOutbound.AdvancedSearch.DestinationPartyId ?? -1),
                     new SqlParameter("CreatedDepartmentId", searchCriteriaByOutbound.AdvancedSearch.CreatedDepartmentId ?? -1),
                     new SqlParameter("DirectedToId", searchCriteriaByOutbound.AdvancedSearch.DirectedToId ?? -1),
                     new SqlParameter("DateFrom", searchCriteriaByOutbound.FromDateTime.HasValue ? searchCriteriaByOutbound.FromDateTime : (object)DBNull.Value),
                     new SqlParameter("DateTo", searchCriteriaByOutbound.ToDateTime.HasValue ? searchCriteriaByOutbound.ToDateTime : (object)DBNull.Value),
                     new SqlParameter("PageIndex", searchCriteriaByOutbound.PageIndex),
                     new SqlParameter("PageSize", searchCriteriaByOutbound.PageSize),
                     new SqlParameter("Ascending", searchCriteriaByOutbound.Ascending),
                     new SqlParameter("CultureName", searchCriteriaByOutbound.CultureName),
                     new SqlParameter("OrderBy", searchCriteriaByOutbound.OrderBy),
                     new SqlParameter("Year", searchCriteriaByOutbound.Year ?? -1),
                     new SqlParameter("DeliveryMethodId", searchCriteriaByOutbound.DeliveryMethodId),
                     sqlPTotalCount
                     ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (OutboundSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByOutbound.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return baseSearchResults;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<OutboundInternalSearchResult> OutboundInternalSearch(SearchCriteriaByOutboundInternal searchCriteriaByOutboundInternal, out int TotalCount)
        {
            try
            {

                if (searchCriteriaByOutboundInternal.Number == null)
                {
                    searchCriteriaByOutboundInternal.Number = -1;
                }

                searchCriteriaByOutboundInternal.OrderBy = "TransactionTypeId";

                IList<OutboundInternalSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundInternalSearchResult>(
                    "BEGIN SEARCHOUTBOUNDINTERNAL (:p_NUMBER, :p_ORGUNITID, :p_TRANSACTIONTYPEID, :p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_YEAR, :p_TOTALCOUNT, :cur); END;",
                            new OracleParameter(":p_NUMBER", OracleDbType.Int32, searchCriteriaByOutboundInternal.Number, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByOutboundInternal.OrgUnitId.HasValue ? searchCriteriaByOutboundInternal.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONTYPEID", OracleDbType.Int32, searchCriteriaByOutboundInternal.TypeId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByOutboundInternal.FromDateTime.HasValue ? searchCriteriaByOutboundInternal.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByOutboundInternal.ToDateTime.HasValue ? searchCriteriaByOutboundInternal.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByOutboundInternal.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByOutboundInternal.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByOutboundInternal.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByOutboundInternal.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByOutboundInternal.OrderBy, ParameterDirection.Input),
                            new OracleParameter(":p_YEAR", OracleDbType.Int32, searchCriteriaByOutboundInternal.Year, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;

                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundInternalSearchResult>(
                       "SearchOutboundInternal @Number, @OrgUnitId, @TransactionTypeId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@SourceTypeId, @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @Year, @TotalCount out",
                   new SqlParameter("Number", searchCriteriaByOutboundInternal.Number),
                   //     new SqlParameter("HasFullPrivilege", searchCriteriaByOutboundInternal.HasFullPrivilege),
                   new SqlParameter("OrgUnitId", !searchCriteriaByOutboundInternal.Global ? searchCriteriaByOutboundInternal.OrgUnitId : -1),
                   //  new SqlParameter("UserId", searchCriteriaByOutboundInternal.UserId.HasValue ? searchCriteriaByOutboundInternal.UserId : -1),
                   new SqlParameter("TransactionCategoryId", searchCriteriaByOutboundInternal.TransactionTypeId),
                   new SqlParameter("TransactionTypeId", searchCriteriaByOutboundInternal.TypeId),
                     new SqlParameter("ConfidentialityId", searchCriteriaByOutboundInternal.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByOutboundInternal.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByOutboundInternal.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByOutboundInternal.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByOutboundInternal.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByOutboundInternal.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByOutboundInternal.AdvancedSearch.SignedById ?? -1),
                   new SqlParameter("SourceTypeId", -1),
                   new SqlParameter("DateFrom", searchCriteriaByOutboundInternal.FromDateTime.HasValue ? searchCriteriaByOutboundInternal.FromDateTime : (object)DBNull.Value),
                   new SqlParameter("DateTo", searchCriteriaByOutboundInternal.ToDateTime.HasValue ? searchCriteriaByOutboundInternal.ToDateTime : (object)DBNull.Value),
                   new SqlParameter("PageIndex", searchCriteriaByOutboundInternal.PageIndex),
                   new SqlParameter("PageSize", searchCriteriaByOutboundInternal.PageSize),
                   new SqlParameter("Ascending", searchCriteriaByOutboundInternal.Ascending),
                   new SqlParameter("CultureName", searchCriteriaByOutboundInternal.CultureName),
                   new SqlParameter("OrderBy", searchCriteriaByOutboundInternal.OrderBy),
                   new SqlParameter("Year", searchCriteriaByOutboundInternal.Year ?? -1),
                   sqlPTotalCount
                   ).ToList();

                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (OutboundInternalSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByOutboundInternal.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<OutboundDraftSearchResult> OutboundDraftSearch(SearchCriteriaByOutboundDraft searchCriteriaByOutboundDraft, out int TotalCount)
        {
            try
            {

                if (searchCriteriaByOutboundDraft.Number == null)
                {
                    searchCriteriaByOutboundDraft.Number = -1;
                }

                searchCriteriaByOutboundDraft.OrderBy = "TransactionTypeId";

                IList<OutboundDraftSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundDraftSearchResult>(
                    "BEGIN SEARCHOUTBOUNDDRAFT (:p_NUMBER,:p_HASFULLPRIVILEGE, :p_ORGUNITID,:p_USERID, :p_TRANSACTIONTYPEID, :p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_YEAR, :p_TOTALCOUNT , :cur); END;",
                            new OracleParameter(":p_NUMBER", OracleDbType.Int32, searchCriteriaByOutboundDraft.Number, ParameterDirection.Input),
                            new OracleParameter(":p_HASFULLPRIVILEGE", OracleDbType.Int32, searchCriteriaByOutboundDraft.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByOutboundDraft.OrgUnitId.HasValue ? searchCriteriaByOutboundDraft.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_USERID", OracleDbType.Int32, searchCriteriaByOutboundDraft.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONTYPEID", OracleDbType.Int32, searchCriteriaByOutboundDraft.TypeId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByOutboundDraft.FromDateTime.HasValue ? searchCriteriaByOutboundDraft.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByOutboundDraft.ToDateTime.HasValue ? searchCriteriaByOutboundDraft.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByOutboundDraft.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByOutboundDraft.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByOutboundDraft.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByOutboundDraft.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByOutboundDraft.OrderBy, ParameterDirection.Input),
                            new OracleParameter(":p_YEAR", OracleDbType.Int32, searchCriteriaByOutboundDraft.Year, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;

                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<OutboundDraftSearchResult>(
                         "SearchOutboundDraft @Number, @HasFullPrivilege, @OrgUnitId, @UserId, @TransactionTypeId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId, @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @Year, @TotalCount out",
                     new SqlParameter("Number", searchCriteriaByOutboundDraft.Number),
                     new SqlParameter("HasFullPrivilege", searchCriteriaByOutboundDraft.HasFullPrivilege),
                     new SqlParameter("OrgUnitId", !searchCriteriaByOutboundDraft.Global ? searchCriteriaByOutboundDraft.OrgUnitId : -1),
                     new SqlParameter("UserId", searchCriteriaByOutboundDraft.UserId.HasValue ? searchCriteriaByOutboundDraft.UserId : -1),
                     new SqlParameter("TransactionTypeId", searchCriteriaByOutboundDraft.TypeId),
                      new SqlParameter("ConfidentialityId", searchCriteriaByOutboundDraft.AdvancedSearch.ConfidentialityId ?? -1),
                     new SqlParameter("LetterTypeId", searchCriteriaByOutboundDraft.AdvancedSearch.LetterTypeId ?? -1),
                     new SqlParameter("StatusId", searchCriteriaByOutboundDraft.AdvancedSearch.StatusId ?? -1),
                     new SqlParameter("PriorityId", searchCriteriaByOutboundDraft.AdvancedSearch.PriorityId ?? -1),
                     new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByOutboundDraft.AdvancedSearch.DirectedToUserId) ? searchCriteriaByOutboundDraft.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                     new SqlParameter("DestinationPartyId", searchCriteriaByOutboundDraft.AdvancedSearch.DestinationPartyId ?? -1),
                     new SqlParameter("CreatedDepartmentId", searchCriteriaByOutboundDraft.AdvancedSearch.CreatedDepartmentId ?? -1),
                     new SqlParameter("DirectedToId", searchCriteriaByOutboundDraft.AdvancedSearch.DirectedToId ?? -1),
                     new SqlParameter("DateFrom", searchCriteriaByOutboundDraft.FromDateTime.HasValue ? searchCriteriaByOutboundDraft.FromDateTime : (object)DBNull.Value),
                     new SqlParameter("DateTo", searchCriteriaByOutboundDraft.ToDateTime.HasValue ? searchCriteriaByOutboundDraft.ToDateTime : (object)DBNull.Value),
                     new SqlParameter("PageIndex", searchCriteriaByOutboundDraft.PageIndex),
                     new SqlParameter("PageSize", searchCriteriaByOutboundDraft.PageSize),
                     new SqlParameter("Ascending", searchCriteriaByOutboundDraft.Ascending),
                     new SqlParameter("CultureName", searchCriteriaByOutboundDraft.CultureName),
                     new SqlParameter("OrderBy", searchCriteriaByOutboundDraft.OrderBy),
                     new SqlParameter("Year", searchCriteriaByOutboundDraft.Year ?? -1),
                     sqlPTotalCount
                     ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (OutboundDraftSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByOutboundDraft.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<SubjectSearchResult> SubjectSearch(SearchCriteriaBySubject searchCriteriaBySubject, out int TotalCount)
        {
            try
            {

                TotalCount = 0;
                IList<SubjectSearchResult> subjectSearchResults;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    searchCriteriaBySubject.OrderBy = "p_TransactionCategoryId";
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    subjectSearchResults = _oMCSDbContext.Database.SqlQuery<SubjectSearchResult>(@"BEGIN SEARCH_SUBJECT 
                (:p_Subject,:p_HasFullPrivilege, :p_OrgUnitId,:p_UserId, :p_TransactionCategoryId, :p_PageIndex, 
                 :p_PageSize, :p_Ascending, :p_CultureName, 
                 :p_OrderBy, :p_Year, :p_TotalCount, :p_cur); END;",
                            new OracleParameter(":p_Subject", OracleDbType.NVarchar2, searchCriteriaBySubject.Subject, ParameterDirection.Input),
                            new OracleParameter(":p_HasFullPrivilege", OracleDbType.Int32, searchCriteriaBySubject.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaBySubject.OrgUnitId.HasValue ? searchCriteriaBySubject.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_UserId", OracleDbType.Int32, searchCriteriaBySubject.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TransactionCategoryId", OracleDbType.Int32, searchCriteriaBySubject.TransactionCategoryId, ParameterDirection.Input),
                            new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaBySubject.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaBySubject.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaBySubject.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaBySubject.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaBySubject.OrderBy, ParameterDirection.Input),
                            new OracleParameter(":p_Year", OracleDbType.Int32, searchCriteriaBySubject.Year, ParameterDirection.Input),
                            //new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {

                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    searchCriteriaBySubject.OrderBy = "TransactionTypeId";
                    var subjectSearchResultsQuery = _oMCSDbContext.Database.SqlQuery<SubjectSearchResult>(@"
                                Search_bySubject @Subject,@HasFullPrivilege,@OrgUnitId,@UserId,@TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId,@PageIndex,@PageSize,@Ascending,@CultureName,@OrderBy,@Year,@TotalCount out",
                               new SqlParameter("Subject", searchCriteriaBySubject.Subject),
                               new SqlParameter("HasFullPrivilege", searchCriteriaBySubject.HasFullPrivilege),
                               new SqlParameter("OrgUnitId", !searchCriteriaBySubject.Global ? searchCriteriaBySubject.OrgUnitId : -1),
                               new SqlParameter("UserId", searchCriteriaBySubject.UserId.HasValue ? searchCriteriaBySubject.UserId : -1),
                               new SqlParameter("TransactionCategoryId", searchCriteriaBySubject.TransactionCategoryId ?? (int)TransactionCategory.All),
                               new SqlParameter("ConfidentialityId", searchCriteriaBySubject.AdvancedSearch.ConfidentialityId ?? -1),
                               new SqlParameter("LetterTypeId", searchCriteriaBySubject.AdvancedSearch.LetterTypeId ?? -1),
                               new SqlParameter("StatusId", searchCriteriaBySubject.AdvancedSearch.StatusId ?? -1),
                               new SqlParameter("PriorityId", searchCriteriaBySubject.AdvancedSearch.PriorityId ?? -1),
                               new SqlParameter("FromPartyId", searchCriteriaBySubject.AdvancedSearch.FromPartyId ?? -1),
                               new SqlParameter("SignedByDepartmentId", searchCriteriaBySubject.AdvancedSearch.SignedByDepartmentId ?? -1),
                               new SqlParameter("SignedById", searchCriteriaBySubject.AdvancedSearch.SignedById ?? -1),
                               new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaBySubject.AdvancedSearch.DirectedToUserId) ? searchCriteriaBySubject.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                               new SqlParameter("DestinationPartyId", searchCriteriaBySubject.AdvancedSearch.DestinationPartyId ?? -1),
                               new SqlParameter("CreatedDepartmentId", searchCriteriaBySubject.AdvancedSearch.CreatedDepartmentId ?? -1),
                               new SqlParameter("DirectedToId", searchCriteriaBySubject.AdvancedSearch.DirectedToId ?? -1),
                               new SqlParameter("PageIndex", searchCriteriaBySubject.PageIndex),
                               new SqlParameter("PageSize", searchCriteriaBySubject.PageSize),
                               new SqlParameter("Ascending", searchCriteriaBySubject.Ascending),
                               new SqlParameter("CultureName", searchCriteriaBySubject.CultureName),
                               new SqlParameter("OrderBy", searchCriteriaBySubject.OrderBy),
                               new SqlParameter("Year", searchCriteriaBySubject.Year ?? -1),
                               sqlPTotalCount
                              );
                    subjectSearchResults = subjectSearchResultsQuery.ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (SubjectSearchResult inboundSearchResult in subjectSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaBySubject.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return subjectSearchResults;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<BaseSearchResult> BarcodeSearch(SearchCriteriaByBarcode searchCriteriaByBarcode, out int TotalCount)
        {
            try
            {
                IList<BaseSearchResult> BaseSearchResult;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    searchCriteriaByBarcode.OrderBy = "TransactionTypeId";
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    BaseSearchResult = _oMCSDbContext.Database.SqlQuery<BaseSearchResult>(@"BEGIN SEARCH_BARCODE 
                (:p_Barcode, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_TotalCount, :p_cur); END;",
                            new OracleParameter(":p_Barcode", OracleDbType.NVarchar2, searchCriteriaByBarcode.Barcode, ParameterDirection.Input),
                            new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByBarcode.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByBarcode.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByBarcode.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaByBarcode.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByBarcode.OrderBy, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    searchCriteriaByBarcode.OrderBy = "TransactionTypeId";
                    BaseSearchResult = _oMCSDbContext.Database.SqlQuery<BaseSearchResult>(@"Search_bySubject @Subject, @OrgUnitId, @TransactionTypeId, @PageIndex, @PageSize, 
                                @Ascending, @CultureName, @OrderBy, @Year, @TotalCount out",
                               new SqlParameter("p_Barcode", searchCriteriaByBarcode.Barcode),
                               new SqlParameter("PageIndex", searchCriteriaByBarcode.PageIndex),
                               new SqlParameter("PageSize", searchCriteriaByBarcode.PageSize),
                               new SqlParameter("Ascending", searchCriteriaByBarcode.Ascending),
                               new SqlParameter("CultureName", searchCriteriaByBarcode.CultureName),
                               new SqlParameter("OrderBy", searchCriteriaByBarcode.OrderBy),
                               sqlPTotalCount
                              ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                return BaseSearchResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<EntitySearchResult> EntitySearch(SearchCriteriaByEntityName searchCriteriaByEntityName, out int TotalCount)
        {
            try
            {

                searchCriteriaByEntityName.OrderBy = "TransactionTypeId";

                IList<EntitySearchResult> baseSearchResults = null;
                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<EntitySearchResult>(
                    "BEGIN SEARCHENTITY (:p_EXTERNALPARTY,:p_ORGUNITID, :p_TRANSACTIONTYPEID,:p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_TOTALCOUNT , :cur); END;",
                            new OracleParameter(":p_EXTERNALPARTY", OracleDbType.Int32, searchCriteriaByEntityName.ExternalPartyId, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByEntityName.OrgUnitId.HasValue ? searchCriteriaByEntityName.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONTYPEID", OracleDbType.Int32, searchCriteriaByEntityName.TransactionCategoryId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByEntityName.FromDateTime.HasValue ? searchCriteriaByEntityName.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByEntityName.ToDateTime.HasValue ? searchCriteriaByEntityName.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByEntityName.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByEntityName.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByEntityName.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByEntityName.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByEntityName.OrderBy, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                           ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    // @HasFullPrivilege, ,@UserId
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<EntitySearchResult>(
                        "SearchEntity @ExternalParty,@DocumentNumber,@Number, @OrgUnitId,@UserId,@TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId , @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @TotalCount out",
                    new SqlParameter("ExternalParty", searchCriteriaByEntityName.ExternalPartyId),
                    new SqlParameter("DocumentNumber", !string.IsNullOrWhiteSpace(searchCriteriaByEntityName.DocumentNumber) ? searchCriteriaByEntityName.DocumentNumber : "-1"),
                    new SqlParameter("Number", searchCriteriaByEntityName.Number ?? -1),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByEntityName.HasFullPrivilege),
                    new SqlParameter("OrgUnitId", !searchCriteriaByEntityName.Global ? searchCriteriaByEntityName.OrgUnitId : -1),
                    new SqlParameter("UserId", searchCriteriaByEntityName.UserId.HasValue ? searchCriteriaByEntityName.UserId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByEntityName.TransactionCategoryId ?? (int)TransactionCategory.All),
                    new SqlParameter("ConfidentialityId", searchCriteriaByEntityName.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByEntityName.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByEntityName.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByEntityName.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByEntityName.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByEntityName.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByEntityName.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByEntityName.AdvancedSearch.DirectedToUserId) ? searchCriteriaByEntityName.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaByEntityName.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaByEntityName.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaByEntityName.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByEntityName.FromDateTime.HasValue ? searchCriteriaByEntityName.FromDateTime : (object)DBNull.Value),
                    new SqlParameter("DateTo", searchCriteriaByEntityName.ToDateTime.HasValue ? searchCriteriaByEntityName.ToDateTime : (object)DBNull.Value),
                    new SqlParameter("PageIndex", searchCriteriaByEntityName.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByEntityName.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByEntityName.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByEntityName.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByEntityName.OrderBy),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (EntitySearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByEntityName.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<CreatorSearchResult> CreatorSearch(SearchCriteriaByCreator searchCriteriaByCreator, out int TotalCount)
        {
            try
            {

                searchCriteriaByCreator.OrderBy = "TransactionTypeId";

                IList<CreatorSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TOTALCOUNT", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<CreatorSearchResult>(
                    "BEGIN SEARCHCREATOR (:p_CREATOR,:p_HASFULLPRIVILEGE, :p_ORGUNITID,:p_USERID, :p_TRANSACTIONCATEGORYID, :p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_TOTALCOUNT , :cur); END;",
                            new OracleParameter(":p_CREATOR", OracleDbType.Int32, searchCriteriaByCreator.CreatorUserId, ParameterDirection.Input),
                            new OracleParameter(":p_HASFULLPRIVILEGE", OracleDbType.Int32, searchCriteriaByCreator.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByCreator.OrgUnitId.HasValue ? searchCriteriaByCreator.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_USERID", OracleDbType.Int32, searchCriteriaByCreator.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONCATEGORYID", OracleDbType.Int32, searchCriteriaByCreator.TransactionCategoryId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByCreator.FromDateTime.HasValue ? searchCriteriaByCreator.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByCreator.ToDateTime.HasValue ? searchCriteriaByCreator.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByCreator.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByCreator.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByCreator.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByCreator.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByCreator.OrderBy, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<CreatorSearchResult>(
                        "SearchCreator @Creator,@HasFullPrivilege, @OrgUnitId,@UserId, @TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId , @DateFrom, @DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy, @TotalCount out",
                    new SqlParameter("Creator", searchCriteriaByCreator.CreatorUserId),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByCreator.HasFullPrivilege),
                    new SqlParameter("OrgUnitId", !searchCriteriaByCreator.Global ? searchCriteriaByCreator.OrgUnitId : -1),
                    new SqlParameter("UserId", searchCriteriaByCreator.UserId.HasValue ? searchCriteriaByCreator.UserId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByCreator.TransactionCategoryId ?? (int)TransactionCategory.All),
                    new SqlParameter("ConfidentialityId", searchCriteriaByCreator.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByCreator.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByCreator.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByCreator.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByCreator.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByCreator.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByCreator.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByCreator.AdvancedSearch.DirectedToUserId) ? searchCriteriaByCreator.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaByCreator.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaByCreator.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaByCreator.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByCreator.FromDateTime.HasValue ? searchCriteriaByCreator.FromDateTime : (object)DBNull.Value),
                    new SqlParameter("DateTo", searchCriteriaByCreator.ToDateTime.HasValue ? searchCriteriaByCreator.ToDateTime : (object)DBNull.Value),
                    new SqlParameter("PageIndex", searchCriteriaByCreator.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByCreator.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByCreator.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByCreator.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByCreator.OrderBy),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (CreatorSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByCreator.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<AssignTransactionSearchResult> AssignTransactionSearch(SearchCriteriaByAssignTransaction searchCriteriaByAssignTransaction, out int TotalCount)
        {
            try
            {

                searchCriteriaByAssignTransaction.OrderBy = "TransactionTypeId";

                IList<AssignTransactionSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_ORDERBY", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<AssignTransactionSearchResult>(
                    "BEGIN SEARCHASSIGNTRANSACTION (:p_FROMENTITY,:p_HASFULLPRIVILEGE,:p_ENTITYID, :p_ORGUNITID,:p_USERID, :p_TRANSACTIONCATEGORYID, :p_DATEFROM, :p_DATETO, :p_PAGEINDEX, :p_PAGESIZE, :p_ASCENDING, :p_CULTURENAME, :p_ORDERBY, :p_TOTALCOUNT , :cur); END;",
                            new OracleParameter(":p_FROMENTITY", OracleDbType.Int32, searchCriteriaByAssignTransaction.FromEntity, ParameterDirection.Input),
                            new OracleParameter(":p_HASFULLPRIVILEGE", OracleDbType.Int32, searchCriteriaByAssignTransaction.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_ENTITYID", OracleDbType.Int32, searchCriteriaByAssignTransaction.EntityId, ParameterDirection.Input),
                            new OracleParameter(":p_ORGUNITID", OracleDbType.Int32, searchCriteriaByAssignTransaction.OrgUnitId.HasValue ? searchCriteriaByAssignTransaction.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_USERID", OracleDbType.Int32, searchCriteriaByAssignTransaction.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TRANSACTIONCATEGORYID", OracleDbType.Int32, searchCriteriaByAssignTransaction.TransactionTypeId, ParameterDirection.Input),
                            new OracleParameter(":p_DATEFROM", OracleDbType.Date, searchCriteriaByAssignTransaction.FromDateTime.HasValue ? searchCriteriaByAssignTransaction.FromDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_DATETO", OracleDbType.Date, searchCriteriaByAssignTransaction.ToDateTime.HasValue ? searchCriteriaByAssignTransaction.ToDateTime : (object)DBNull.Value, ParameterDirection.Input),
                            new OracleParameter(":p_PAGEINDEX", OracleDbType.Int32, searchCriteriaByAssignTransaction.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PAGESIZE", OracleDbType.Int32, searchCriteriaByAssignTransaction.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_ASCENDING", OracleDbType.Int32, searchCriteriaByAssignTransaction.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CULTURENAME", OracleDbType.Varchar2, searchCriteriaByAssignTransaction.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_ORDERBY", OracleDbType.NVarchar2, searchCriteriaByAssignTransaction.OrderBy, ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<AssignTransactionSearchResult>(
                        "SearchAssignTransaction @FromEntity,@HasFullPrivilege,@EntityId, @OrgUnitId,@UserId,@TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId,@DateFrom,@DateTo,@PageIndex,@PageSize,@Ascending,@CultureName,@OrderBy,@TotalCount out",
                    new SqlParameter("FromEntity", searchCriteriaByAssignTransaction.FromEntity),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByAssignTransaction.HasFullPrivilege),
                    new SqlParameter("EntityId", searchCriteriaByAssignTransaction.EntityId),
                    new SqlParameter("OrgUnitId", !searchCriteriaByAssignTransaction.Global ? searchCriteriaByAssignTransaction.OrgUnitId : -1),
                    new SqlParameter("UserId", searchCriteriaByAssignTransaction.UserId.HasValue ? searchCriteriaByAssignTransaction.UserId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByAssignTransaction.TransactionTypeId ?? (int)TransactionCategory.All),
                    new SqlParameter("ConfidentialityId", searchCriteriaByAssignTransaction.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByAssignTransaction.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByAssignTransaction.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByAssignTransaction.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByAssignTransaction.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByAssignTransaction.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByAssignTransaction.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByAssignTransaction.AdvancedSearch.DirectedToUserId) ? searchCriteriaByAssignTransaction.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaByAssignTransaction.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaByAssignTransaction.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaByAssignTransaction.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByAssignTransaction.FromDateTime.HasValue ? searchCriteriaByAssignTransaction.FromDateTime : (object)DBNull.Value),
                    new SqlParameter("DateTo", searchCriteriaByAssignTransaction.ToDateTime.HasValue ? searchCriteriaByAssignTransaction.ToDateTime : (object)DBNull.Value),
                    new SqlParameter("PageIndex", searchCriteriaByAssignTransaction.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByAssignTransaction.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByAssignTransaction.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByAssignTransaction.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByAssignTransaction.OrderBy),
                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (AssignTransactionSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
       _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByAssignTransaction.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IList<NamesSearchResult> SearchNames(SearchCriteriaByNames searchCriteriaByNames, out int TotalCount)
        {
            try
            {

                TotalCount = 0;
                IList<NamesSearchResult> subjectSearchResults;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    searchCriteriaByNames.OrderBy = "p_TransactionCategoryId";
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    subjectSearchResults = _oMCSDbContext.Database.SqlQuery<NamesSearchResult>(@"BEGIN SEARCH_NAMES 
                (:p_FirstName,:p_SecondName,:p_ThirdName,:p_FamilyName,:p_HasFullPrivilege, :p_OrgUnitId,:p_UserId, :p_TransactionCategoryId, :p_PageIndex, 
                 :p_PageSize, :p_Ascending, :p_CultureName, 
                 :p_OrderBy, :p_Year, :p_TotalCount, :p_cur); END;",
                            new OracleParameter(":p_FirstName", OracleDbType.NVarchar2, searchCriteriaByNames.FirstName, ParameterDirection.Input),
                            new OracleParameter(":p_SecondName", OracleDbType.NVarchar2, searchCriteriaByNames.SecondName, ParameterDirection.Input),
                            new OracleParameter(":p_ThirdName", OracleDbType.NVarchar2, searchCriteriaByNames.ThirdName, ParameterDirection.Input),
                            new OracleParameter(":p_FamilyName", OracleDbType.NVarchar2, searchCriteriaByNames.FamilyName, ParameterDirection.Input),
                            new OracleParameter(":p_HasFullPrivilege", OracleDbType.Int32, searchCriteriaByNames.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByNames.OrgUnitId.HasValue ? searchCriteriaByNames.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_UserId", OracleDbType.Int32, searchCriteriaByNames.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TransactionCategoryId", OracleDbType.Int32, searchCriteriaByNames.TransactionTypeId, ParameterDirection.Input),
                            new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByNames.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByNames.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByNames.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaByNames.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByNames.OrderBy, ParameterDirection.Input),
                            //new OracleParameter(":p_Year", OracleDbType.Int32, searchCriteriaByNames.Year, ParameterDirection.Input),
                            //new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    searchCriteriaByNames.OrderBy = "TransactionTypeId";
                    var namesSearchResultsQuery = _oMCSDbContext.Database.SqlQuery<NamesSearchResult>(
                               @"Search_byNames @FirstName,@SecondName,@ThirdName,@FamilyName, @DateFrom , @DateTo,
                               @HasFullPrivilege, @OrgUnitId, @UserId, @TransactionCategoryId,@ConfidentialityId,
                               @LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId,
                               @SearchNamesType, @PageIndex, @PageSize, 
                                @Ascending, @CultureName, @OrderBy, @TotalCount out",
                               new SqlParameter("FirstName", !string.IsNullOrEmpty(searchCriteriaByNames.FirstName) ? searchCriteriaByNames.FirstName : (object)DBNull.Value),
                               new SqlParameter("SecondName", !string.IsNullOrEmpty(searchCriteriaByNames.SecondName) ? searchCriteriaByNames.SecondName : (object)DBNull.Value),
                               new SqlParameter("ThirdName", !string.IsNullOrEmpty(searchCriteriaByNames.ThirdName) ? searchCriteriaByNames.ThirdName : (object)DBNull.Value),
                               new SqlParameter("FamilyName", !string.IsNullOrEmpty(searchCriteriaByNames.FamilyName) ? searchCriteriaByNames.FamilyName : (object)DBNull.Value),
                               new SqlParameter("DateFrom", searchCriteriaByNames.DateFrom),
                               new SqlParameter("DateTo", searchCriteriaByNames.DateTo),
                               new SqlParameter("HasFullPrivilege", searchCriteriaByNames.HasFullPrivilege),
                               new SqlParameter("OrgUnitId", !searchCriteriaByNames.Global ? searchCriteriaByNames.OrgUnitId : -1),
                               new SqlParameter("UserId", searchCriteriaByNames.UserId.HasValue ? searchCriteriaByNames.UserId : -1),
                               new SqlParameter("TransactionCategoryId", searchCriteriaByNames.TransactionTypeId ?? (int)TransactionCategory.All),
                               new SqlParameter("ConfidentialityId", searchCriteriaByNames.AdvancedSearch.ConfidentialityId ?? -1),
                               new SqlParameter("LetterTypeId", searchCriteriaByNames.AdvancedSearch.LetterTypeId ?? -1),
                               new SqlParameter("StatusId", searchCriteriaByNames.AdvancedSearch.StatusId ?? -1),
                               new SqlParameter("PriorityId", searchCriteriaByNames.AdvancedSearch.PriorityId ?? -1),
                               new SqlParameter("FromPartyId", searchCriteriaByNames.AdvancedSearch.FromPartyId ?? -1),
                               new SqlParameter("SignedByDepartmentId", searchCriteriaByNames.AdvancedSearch.SignedByDepartmentId ?? -1),
                               new SqlParameter("SignedById", searchCriteriaByNames.AdvancedSearch.SignedById ?? -1),
                               new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByNames.AdvancedSearch.DirectedToUserId) ? searchCriteriaByNames.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                               new SqlParameter("DestinationPartyId", searchCriteriaByNames.AdvancedSearch.DestinationPartyId ?? -1),
                               new SqlParameter("CreatedDepartmentId", searchCriteriaByNames.AdvancedSearch.CreatedDepartmentId ?? -1),
                               new SqlParameter("DirectedToId", searchCriteriaByNames.AdvancedSearch.DirectedToId ?? -1),
                               new SqlParameter("SearchNamesType", searchCriteriaByNames.SearchNamesType),
                               new SqlParameter("PageIndex", searchCriteriaByNames.PageIndex),
                               new SqlParameter("PageSize", searchCriteriaByNames.PageSize),
                               new SqlParameter("Ascending", searchCriteriaByNames.Ascending),
                               new SqlParameter("CultureName", searchCriteriaByNames.CultureName),
                               new SqlParameter("OrderBy", searchCriteriaByNames.OrderBy),
                                sqlPTotalCount
                              );
                    subjectSearchResults = namesSearchResultsQuery.ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (NamesSearchResult inboundSearchResult in subjectSearchResults)
                {
                    int count =
                        _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByNames.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return subjectSearchResults;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<DailySearchResult> SearchDaily(SearchCriteriaByDaily searchCriteriaByDaily, out int TotalCount)
        {
            try
            {

                searchCriteriaByDaily.OrderBy = "TransactionTypeId";

                IList<DailySearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<DailySearchResult>(
                        "BEGIN SEARCH_Daily (:p_TodayDate, :p_UserId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_TodayDate", OracleDbType.Int32, searchCriteriaByDaily.TodayDate, ParameterDirection.Input),
                        new OracleParameter(":p_UserId", OracleDbType.Int32, searchCriteriaByDaily.UserId.HasValue ? searchCriteriaByDaily.UserId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByDaily.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByDaily.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByDaily.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByDaily.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByDaily.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<DailySearchResult>(
                        "SearchDaily @TodayDate,@UserId,@OrgUnitId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("TodayDate", searchCriteriaByDaily.TodayDate),
                    new SqlParameter("UserId", searchCriteriaByDaily.UserId ?? -1),
                    new SqlParameter("OrgUnitId", searchCriteriaByDaily.UserId ?? -1),
                      new SqlParameter("PageIndex", searchCriteriaByDaily.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByDaily.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByDaily.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByDaily.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByDaily.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (DailySearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.CreatedBy == searchCriteriaByDaily.UserId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<AssignmentNoteSearchResult> SearchAssignmentNote(SearchCriteriaByAssignmentNote searchCriteriaByAssignmentNote, out int TotalCount)
        {
            try
            {

                TotalCount = 0;
                IList<AssignmentNoteSearchResult> subjectSearchResults;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    searchCriteriaByAssignmentNote.OrderBy = "p_TransactionCategoryId";
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    subjectSearchResults = _oMCSDbContext.Database.SqlQuery<AssignmentNoteSearchResult>(@"BEGIN SEARCH_NAMES 
                (:p_FirstName,:p_SecondName,:p_ThirdName,:p_FamilyName,:p_HasFullPrivilege, :p_OrgUnitId,:p_UserId, :p_TransactionCategoryId, :p_PageIndex, 
                 :p_PageSize, :p_Ascending, :p_CultureName, 
                 :p_OrderBy, :p_Year, :p_TotalCount, :p_cur); END;",
                                                    new OracleParameter(":p_AssignmentNote", OracleDbType.Int32, searchCriteriaByAssignmentNote.AssignmentNote, ParameterDirection.Input),
                          new OracleParameter(":p_HasFullPrivilege", OracleDbType.Int32, searchCriteriaByAssignmentNote.HasFullPrivilege, ParameterDirection.Input),
                            new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByAssignmentNote.OrgUnitId.HasValue ? searchCriteriaByAssignmentNote.OrgUnitId : -1, ParameterDirection.Input),
                            new OracleParameter(":p_UserId", OracleDbType.Int32, searchCriteriaByAssignmentNote.UserId, ParameterDirection.Input),
                            new OracleParameter(":p_TransactionCategoryId", OracleDbType.Int32, searchCriteriaByAssignmentNote.TransactionTypeId, ParameterDirection.Input),
                            new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByAssignmentNote.PageIndex, ParameterDirection.Input),
                            new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByAssignmentNote.PageSize, ParameterDirection.Input),
                            new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByAssignmentNote.Ascending, ParameterDirection.Input),
                            new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, searchCriteriaByAssignmentNote.CultureName, ParameterDirection.Input),
                            new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByAssignmentNote.OrderBy, ParameterDirection.Input),
                            //new OracleParameter(":p_Year", OracleDbType.Int32, searchCriteriaByNames.Year, ParameterDirection.Input),
                            //new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                            orcTotalOutParam,
                            orcOutParam
                            ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    searchCriteriaByAssignmentNote.OrderBy = "TransactionTypeId";
                    var namesSearchResultsQuery = _oMCSDbContext.Database.SqlQuery<AssignmentNoteSearchResult>(
                              @"SearchAssignmentNote @AssignmentNote,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DateFrom,@DateTo,@HasFullPrivilege, @OrgUnitId,@UserId,
                     @PageIndex,@TransactionCategoryId, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("AssignmentNote", !string.IsNullOrEmpty(searchCriteriaByAssignmentNote.AssignmentNote) ? searchCriteriaByAssignmentNote.AssignmentNote : (object)DBNull.Value),
                    new SqlParameter("ConfidentialityId", searchCriteriaByAssignmentNote.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByAssignmentNote.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByAssignmentNote.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByAssignmentNote.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByAssignmentNote.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByAssignmentNote.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByAssignmentNote.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByAssignmentNote.DateFrom),
                    new SqlParameter("DateTo", searchCriteriaByAssignmentNote.DateTo),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByAssignmentNote.HasFullPrivilege),
                    new SqlParameter("OrgUnitId", !searchCriteriaByAssignmentNote.Global ? searchCriteriaByAssignmentNote.OrgUnitId : -1),
                    new SqlParameter("UserId", searchCriteriaByAssignmentNote.UserId),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByAssignmentNote.TransactionTypeId ?? (int)TransactionCategory.All),
                    new SqlParameter("PageIndex", searchCriteriaByAssignmentNote.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByAssignmentNote.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByAssignmentNote.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByAssignmentNote.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByAssignmentNote.OrderBy),
                                sqlPTotalCount
                              );
                    subjectSearchResults = namesSearchResultsQuery.ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (AssignmentNoteSearchResult inboundSearchResult in subjectSearchResults)
                {
                    int count =
                        _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByAssignmentNote.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }
                return subjectSearchResults;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<ManifestNumberSearchResult> SearchManifestNumber(SearchCriteriaByManifestNumber searchCriteriaByManifestNumber, out int TotalCount)
        {
            try
            {

                searchCriteriaByManifestNumber.OrderBy = "TransactionTypeId";

                IList<ManifestNumberSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ManifestNumberSearchResult>(
                        "BEGIN SEARCH_MANIFEST_NUMBER (:p_ManifestNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_ManifestNumber", OracleDbType.Int32, searchCriteriaByManifestNumber.ManifestNumber, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByManifestNumber.OrgUnitId.HasValue ? searchCriteriaByManifestNumber.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByManifestNumber.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByManifestNumber.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByManifestNumber.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByManifestNumber.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByManifestNumber.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ManifestNumberSearchResult>(
                        "SearchManifestNumber @ManifestNumber, @OrgUnitId,@TransactionCategoryId,@DateFrom,@DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("ManifestNumber", searchCriteriaByManifestNumber.ManifestNumber),
                    new SqlParameter("OrgUnitId", !searchCriteriaByManifestNumber.Global ? searchCriteriaByManifestNumber.OrgUnitId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByManifestNumber.TransactionTypeId ?? (int)TransactionCategory.All),
                    new SqlParameter("DateFrom", searchCriteriaByManifestNumber.DateFrom),
                    new SqlParameter("DateTo", searchCriteriaByManifestNumber.DateTo),
                    new SqlParameter("PageIndex", searchCriteriaByManifestNumber.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByManifestNumber.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByManifestNumber.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByManifestNumber.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByManifestNumber.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (ManifestNumberSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByManifestNumber.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<MilitaryNumberOrIdentitySearchResult> SearchMilitaryNumberOrIdentity(SearchCriteriaByMilitaryNumberOrIdentity searchCriteriaByMilitaryNumberOrIdentity, out int TotalCount)
        {
            try
            {

                searchCriteriaByMilitaryNumberOrIdentity.OrderBy = "TransactionTypeId";

                IList<MilitaryNumberOrIdentitySearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<MilitaryNumberOrIdentitySearchResult>(
                        "BEGIN SEARCH_IDENTIFICATION_NUMBER (:p_Identification, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_Identificationr", OracleDbType.Varchar2, searchCriteriaByMilitaryNumberOrIdentity.IdentificationNumber, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByMilitaryNumberOrIdentity.OrgUnitId.HasValue ? searchCriteriaByMilitaryNumberOrIdentity.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByMilitaryNumberOrIdentity.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByMilitaryNumberOrIdentity.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByMilitaryNumberOrIdentity.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByMilitaryNumberOrIdentity.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByMilitaryNumberOrIdentity.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<MilitaryNumberOrIdentitySearchResult>(
                        "SearchIdentificationrNumber @IdentificationNumber, @OrgUnitId,@TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("IdentificationNumber", searchCriteriaByMilitaryNumberOrIdentity.IdentificationNumber),
                    new SqlParameter("OrgUnitId", !searchCriteriaByMilitaryNumberOrIdentity.Global ? searchCriteriaByMilitaryNumberOrIdentity.OrgUnitId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByMilitaryNumberOrIdentity.TransactionTypeId ?? (int)TransactionCategory.All),
                    new SqlParameter("ConfidentialityId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.DirectedToUserId) ? searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaByMilitaryNumberOrIdentity.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("PageIndex", searchCriteriaByMilitaryNumberOrIdentity.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByMilitaryNumberOrIdentity.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByMilitaryNumberOrIdentity.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByMilitaryNumberOrIdentity.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByMilitaryNumberOrIdentity.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (MilitaryNumberOrIdentitySearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByMilitaryNumberOrIdentity.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<TransactionNumberSearchResult> SearchTransactionNumber(SearchCriteriaByTransactionNumber searchCriteriaByTransactionNumber, out int TotalCount)
        {
            try
            {

                searchCriteriaByTransactionNumber.OrderBy = "TransactionTypeId";

                IList<TransactionNumberSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionNumberSearchResult>(
                        "BEGIN SEARCH_TRANSACTION_NUMBER (:p_TransactionNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_TransactionNumber", OracleDbType.Varchar2, searchCriteriaByTransactionNumber.TransactionNumber, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByTransactionNumber.OrgUnitId.HasValue ? searchCriteriaByTransactionNumber.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByTransactionNumber.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByTransactionNumber.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByTransactionNumber.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByTransactionNumber.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByTransactionNumber.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionNumberSearchResult>(
                        "SearchTransactionNumber @TransactionNumber, @OrgUnitId,@TransactionCategoryId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("TransactionNumber", searchCriteriaByTransactionNumber.TransactionNumber),
                    new SqlParameter("OrgUnitId", !searchCriteriaByTransactionNumber.Global ? searchCriteriaByTransactionNumber.OrgUnitId : -1),
                     new SqlParameter("TransactionCategoryId", searchCriteriaByTransactionNumber.TransactionTypeId ?? (int)TransactionCategory.All),
                     new SqlParameter("PageIndex", searchCriteriaByTransactionNumber.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByTransactionNumber.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByTransactionNumber.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByTransactionNumber.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByTransactionNumber.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (TransactionNumberSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByTransactionNumber.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<TransactionNotsSearchResult> SearchTransactionNots(SearchCriteriaByTransactionNots searchCriteriaTransactionNots, out int TotalCount)
        {
            try
            {

                searchCriteriaTransactionNots.OrderBy = "TransactionTypeId";

                IList<TransactionNotsSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionNotsSearchResult>(
                        "BEGIN SEARCH_DOCUMENT_NUMBER (:p_TransactionNots, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_TransactionNots", OracleDbType.NVarchar2, searchCriteriaTransactionNots.TransactionNots, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaTransactionNots.OrgUnitId.HasValue ? searchCriteriaTransactionNots.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaTransactionNots.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaTransactionNots.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaTransactionNots.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaTransactionNots.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaTransactionNots.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<TransactionNotsSearchResult>(
                        "SearchTransactionNot @TransactionNots, @OrgUnitId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("TransactionNots", searchCriteriaTransactionNots.TransactionNots),
                    new SqlParameter("OrgUnitId", !searchCriteriaTransactionNots.Global ? searchCriteriaTransactionNots.OrgUnitId : -1),
                     new SqlParameter("PageIndex", searchCriteriaTransactionNots.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaTransactionNots.PageSize),
                    new SqlParameter("Ascending", searchCriteriaTransactionNots.Ascending),
                    new SqlParameter("CultureName", searchCriteriaTransactionNots.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaTransactionNots.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (TransactionNotsSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaTransactionNots.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<ElcEmployeeSearchResult> SearchELcEmployee(SearchCriteriaByElcEmployee searchCriteriaByElcEmployee, out int TotalCount)
        {
            try
            {

                searchCriteriaByElcEmployee.OrderBy = "TransactionTypeId";

                IList<ElcEmployeeSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ElcEmployeeSearchResult>(
                        "BEGIN SEARCH_DOCUMENT_NUMBER (:p_DocumentNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_ElcEmployeeId", OracleDbType.Int32, searchCriteriaByElcEmployee.ElcEmployeeId, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByElcEmployee.OrgUnitId.HasValue ? searchCriteriaByElcEmployee.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByElcEmployee.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByElcEmployee.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByElcEmployee.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByElcEmployee.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByElcEmployee.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ElcEmployeeSearchResult>(
                        "SearchELcEmployee @ElcEmployeeId,@HasFullPrivilege,@OrgUnitId,@TransactionCategoryId,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId,@DateFrom,@DateTo, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("ElcEmployeeId", searchCriteriaByElcEmployee.ElcEmployeeId),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByElcEmployee.HasFullPrivilege),
                    new SqlParameter("OrgUnitId", !searchCriteriaByElcEmployee.Global ? searchCriteriaByElcEmployee.OrgUnitId : -1),
                    new SqlParameter("TransactionCategoryId", searchCriteriaByElcEmployee.TransactionCategoryId ?? (int)TransactionCategory.All),
                    new SqlParameter("ConfidentialityId", searchCriteriaByElcEmployee.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByElcEmployee.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByElcEmployee.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByElcEmployee.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByElcEmployee.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByElcEmployee.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaByElcEmployee.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByElcEmployee.AdvancedSearch.DirectedToUserId) ? searchCriteriaByElcEmployee.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaByElcEmployee.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaByElcEmployee.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaByElcEmployee.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByElcEmployee.DateFrom),
                    new SqlParameter("DateTo", searchCriteriaByElcEmployee.DateTo),
                     new SqlParameter("PageIndex", searchCriteriaByElcEmployee.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByElcEmployee.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByElcEmployee.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByElcEmployee.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByElcEmployee.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (ElcEmployeeSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByElcEmployee.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<ExternalOutBoundOrManifestNumberSearchResult> SearchExternalOutBoundOrManifestNumber(SearchCriteriaByExternalOutBoundOrManifestNumber searchCriteriaByExternalOutBoundOrManifestNumber, out int TotalCount)
        {
            try
            {

                searchCriteriaByExternalOutBoundOrManifestNumber.OrderBy = "TransactionTypeId";

                IList<ExternalOutBoundOrManifestNumberSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ExternalOutBoundOrManifestNumberSearchResult>(
                        "BEGIN SEARCH_DOCUMENT_NUMBER (:p_DocumentNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_RecordNumber", OracleDbType.Int32, searchCriteriaByExternalOutBoundOrManifestNumber.Number, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByExternalOutBoundOrManifestNumber.OrgUnitId.HasValue ? searchCriteriaByExternalOutBoundOrManifestNumber.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByExternalOutBoundOrManifestNumber.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByExternalOutBoundOrManifestNumber.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByExternalOutBoundOrManifestNumber.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByExternalOutBoundOrManifestNumber.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByExternalOutBoundOrManifestNumber.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<ExternalOutBoundOrManifestNumberSearchResult>(
                        "SearchExternalOrManifest @RecordNumber,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@DirectedToUserId,@DestinationPartyId,@CreatedDepartmentId,@DirectedToId,@Year, @OrgUnitId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("RecordNumber", searchCriteriaByExternalOutBoundOrManifestNumber.Number ?? -1),
                      new SqlParameter("ConfidentialityId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.ConfidentialityId ?? -1),
                     new SqlParameter("LetterTypeId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.LetterTypeId ?? -1),
                     new SqlParameter("StatusId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.StatusId ?? -1),
                     new SqlParameter("PriorityId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.PriorityId ?? -1),
                     new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.DirectedToUserId) ? searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                     new SqlParameter("DestinationPartyId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.DestinationPartyId ?? -1),
                     new SqlParameter("CreatedDepartmentId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.CreatedDepartmentId ?? -1),
                     new SqlParameter("DirectedToId", searchCriteriaByExternalOutBoundOrManifestNumber.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("Year", searchCriteriaByExternalOutBoundOrManifestNumber.Year ?? -1),
                    new SqlParameter("OrgUnitId", !searchCriteriaByExternalOutBoundOrManifestNumber.Global ? searchCriteriaByExternalOutBoundOrManifestNumber.OrgUnitId : -1),
                     new SqlParameter("PageIndex", searchCriteriaByExternalOutBoundOrManifestNumber.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByExternalOutBoundOrManifestNumber.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByExternalOutBoundOrManifestNumber.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByExternalOutBoundOrManifestNumber.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByExternalOutBoundOrManifestNumber.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (ExternalOutBoundOrManifestNumberSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByExternalOutBoundOrManifestNumber.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<CopyAssignemntSearchResult> SearchCopyAssignemnt(SearchCriteriaByCopyAssignemnt searchCriteriaByCopyAssignemnt, out int TotalCount)
        {
            try
            {

                searchCriteriaByCopyAssignemnt.OrderBy = "TransactionTypeId";

                IList<CopyAssignemntSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<CopyAssignemntSearchResult>(
                        "BEGIN SEARCH_DOCUMENT_NUMBER (:p_DocumentNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_FromEntityId", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.FromEntityId, ParameterDirection.Input),
                        new OracleParameter(":p_ToEntityId", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.ToEntityId, ParameterDirection.Input),
                        new OracleParameter(":p_DateFrom", OracleDbType.Date, searchCriteriaByCopyAssignemnt.DateFrom, ParameterDirection.Input),
                        new OracleParameter(":p_DateTo", OracleDbType.Date, searchCriteriaByCopyAssignemnt.DateTo, ParameterDirection.Input),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.OrgUnitId.HasValue ? searchCriteriaByCopyAssignemnt.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaByCopyAssignemnt.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaByCopyAssignemnt.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaByCopyAssignemnt.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<CopyAssignemntSearchResult>(
                        "SearchCopyAssignemnt @FromEntityId,@ToEntityId,@DateFrom,@ConfidentialityId,@LetterTypeId,@StatusId,@PriorityId,@FromPartyId,@SignedByDepartmentId,@SignedById,@DateTo, @OrgUnitId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("FromEntityId", searchCriteriaByCopyAssignemnt.FromEntityId),
                    new SqlParameter("ToEntityId", searchCriteriaByCopyAssignemnt.ToEntityId),
                      new SqlParameter("ConfidentialityId", searchCriteriaByCopyAssignemnt.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaByCopyAssignemnt.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaByCopyAssignemnt.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaByCopyAssignemnt.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaByCopyAssignemnt.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaByCopyAssignemnt.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("DateFrom", searchCriteriaByCopyAssignemnt.DateFrom ?? DateTime.Now),
                    new SqlParameter("DateTo", searchCriteriaByCopyAssignemnt.DateTo ?? DateTime.Now),
                    new SqlParameter("OrgUnitId", !searchCriteriaByCopyAssignemnt.Global ? searchCriteriaByCopyAssignemnt.OrgUnitId : -1),
                     new SqlParameter("PageIndex", searchCriteriaByCopyAssignemnt.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByCopyAssignemnt.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByCopyAssignemnt.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByCopyAssignemnt.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByCopyAssignemnt.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (CopyAssignemntSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaByCopyAssignemnt.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<SubjectLetterSearchResult> SearchSubjectLetter(SearchCriteriaBySubjectLetter searchCriteriaBySubjectLetter, out int TotalCount)
        {
            try
            {

                searchCriteriaBySubjectLetter.OrderBy = "TransactionTypeId";

                IList<SubjectLetterSearchResult> baseSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleParameter orcTotalOutParam = new OracleParameter(":p_TotalCount", OracleDbType.Int32, ParameterDirection.Output);
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<SubjectLetterSearchResult>(
                        "BEGIN SearchSubjectLetter (:p_DocumentNumber, :p_OrgUnitId, :p_PageIndex, :p_PageSize, :p_Ascending, :p_CultureName, :p_OrderBy, :p_Year, :p_Status, :p_TotalCount , :p_cur); END;",
                        new OracleParameter(":p_FirstLetter", OracleDbType.NVarchar2, searchCriteriaBySubjectLetter.FirstLetter, ParameterDirection.Input),
                        new OracleParameter(":p_SecondLetter", OracleDbType.NVarchar2, searchCriteriaBySubjectLetter.SecondLetter, ParameterDirection.Input),
                        new OracleParameter(":p_ThirdLetter", OracleDbType.NVarchar2, searchCriteriaBySubjectLetter.ThirdLetter, ParameterDirection.Input),
                        new OracleParameter(":p_FourthLetter", OracleDbType.NVarchar2, searchCriteriaBySubjectLetter.FourthLetter, ParameterDirection.Input),
                        new OracleParameter(":p_DateFrom", OracleDbType.Date, searchCriteriaBySubjectLetter.DateFrom, ParameterDirection.Input),
                        new OracleParameter(":p_DateTo", OracleDbType.Date, searchCriteriaBySubjectLetter.DateTo, ParameterDirection.Input),
                        new SqlParameter("TransactionCategoryId", searchCriteriaBySubjectLetter.TransactionTypeId ?? (int)TransactionCategory.All),
                        //new SqlParameter("TransactionTypeId", searchCriteriaBySubjectLetter.TypeId),
                        new OracleParameter(":p_OrgUnitId", OracleDbType.Int32, searchCriteriaBySubjectLetter.OrgUnitId.HasValue ? searchCriteriaBySubjectLetter.OrgUnitId : -1, ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, searchCriteriaBySubjectLetter.PageIndex, ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, searchCriteriaBySubjectLetter.PageSize, ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, searchCriteriaBySubjectLetter.Ascending, ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.Varchar2, searchCriteriaBySubjectLetter.CultureName, ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, searchCriteriaBySubjectLetter.OrderBy, ParameterDirection.Input),
                        new OracleParameter(":p_Status", OracleDbType.Int32, TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), ParameterDirection.Input),
                        orcTotalOutParam,
                        orcOutParam
                        ).ToList();
                    TotalCount = int.Parse(orcTotalOutParam.Value.ToString());
                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    baseSearchResults = _oMCSDbContext.Database.SqlQuery<SubjectLetterSearchResult>(
                        @"SearchSubjectLetter @FirstLetter,@SecondLetter,@ThirdLetter,@FourthLetter,@DateFrom,@DateTo,@TransactionCategoryId,@SearchTypeForFiltersId,@ConfidentialityId,@LetterTypeId, @StatusId, @PriorityId, @FromPartyId, @SignedByDepartmentId, @SignedById, @DirectedToUserId, @DestinationPartyId, @CreatedDepartmentId, @DirectedToId, @OrgUnitId, @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("FirstLetter", !string.IsNullOrEmpty(searchCriteriaBySubjectLetter.FirstLetter) ? searchCriteriaBySubjectLetter.FirstLetter : (object)DBNull.Value),
                    new SqlParameter("SecondLetter", !string.IsNullOrEmpty(searchCriteriaBySubjectLetter.SecondLetter) ? searchCriteriaBySubjectLetter.SecondLetter : (object)DBNull.Value),
                    new SqlParameter("ThirdLetter", !string.IsNullOrEmpty(searchCriteriaBySubjectLetter.ThirdLetter) ? searchCriteriaBySubjectLetter.ThirdLetter : (object)DBNull.Value),
                    new SqlParameter("FourthLetter", !string.IsNullOrEmpty(searchCriteriaBySubjectLetter.FourthLetter) ? searchCriteriaBySubjectLetter.FourthLetter : (object)DBNull.Value),
                    new SqlParameter("DateFrom", searchCriteriaBySubjectLetter.DateFrom),
                    new SqlParameter("DateTo", searchCriteriaBySubjectLetter.DateTo),
                    new SqlParameter("TransactionCategoryId", searchCriteriaBySubjectLetter.TransactionTypeId ?? (int)TransactionCategory.All),
                    new SqlParameter("SearchTypeForFiltersId", searchCriteriaBySubjectLetter.SearchTypeForFiltersId),
                    new SqlParameter("ConfidentialityId", searchCriteriaBySubjectLetter.AdvancedSearch.ConfidentialityId ?? -1),
                    new SqlParameter("LetterTypeId", searchCriteriaBySubjectLetter.AdvancedSearch.LetterTypeId ?? -1),
                    new SqlParameter("StatusId", searchCriteriaBySubjectLetter.AdvancedSearch.StatusId ?? -1),
                    new SqlParameter("PriorityId", searchCriteriaBySubjectLetter.AdvancedSearch.PriorityId ?? -1),
                    new SqlParameter("FromPartyId", searchCriteriaBySubjectLetter.AdvancedSearch.FromPartyId ?? -1),
                    new SqlParameter("SignedByDepartmentId", searchCriteriaBySubjectLetter.AdvancedSearch.SignedByDepartmentId ?? -1),
                    new SqlParameter("SignedById", searchCriteriaBySubjectLetter.AdvancedSearch.SignedById ?? -1),
                    new SqlParameter("DirectedToUserId", !string.IsNullOrEmpty(searchCriteriaBySubjectLetter.AdvancedSearch.DirectedToUserId) ? searchCriteriaBySubjectLetter.AdvancedSearch.DirectedToUserId : (object)DBNull.Value),
                    new SqlParameter("DestinationPartyId", searchCriteriaBySubjectLetter.AdvancedSearch.DestinationPartyId ?? -1),
                    new SqlParameter("CreatedDepartmentId", searchCriteriaBySubjectLetter.AdvancedSearch.CreatedDepartmentId ?? -1),
                    new SqlParameter("DirectedToId", searchCriteriaBySubjectLetter.AdvancedSearch.DirectedToId ?? -1),
                    new SqlParameter("OrgUnitId", !searchCriteriaBySubjectLetter.Global ? searchCriteriaBySubjectLetter.OrgUnitId : -1),
                    new SqlParameter("PageIndex", searchCriteriaBySubjectLetter.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaBySubjectLetter.PageSize),
                    new SqlParameter("Ascending", searchCriteriaBySubjectLetter.Ascending),
                    new SqlParameter("CultureName", searchCriteriaBySubjectLetter.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaBySubjectLetter.OrderBy),

                    sqlPTotalCount
                    ).ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }
                foreach (SubjectLetterSearchResult inboundSearchResult in baseSearchResults)
                {
                    int count =
                _oMCSDbContext.TransactionEntityDetails.Where(a => a.EntityId == searchCriteriaBySubjectLetter.OrgUnitId && inboundSearchResult.Id == a.TransactionId).ToList().Count();
                    if (count > 0)
                    {
                        inboundSearchResult.IsView = true;
                    }
                }

                return baseSearchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<ExternalPartyCopiesSearchResult> SearchExternalPartyCopies(SearchCriteriaByExternalPartyCopies searchCriteriaByExternalPartyCopies, out int TotalCount)
        {
            try
            {

                TotalCount = 0;
                IList<ExternalPartyCopiesSearchResult> externalCopiesSearchResults = null;

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {

                }
                else
                {
                    _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                    SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                    sqlPTotalCount.Direction = ParameterDirection.Output;
                    searchCriteriaByExternalPartyCopies.OrderBy = "TransactionTypeId";
                    var ExternalPartyCopiesSearchResultsQuery = _oMCSDbContext.Database.SqlQuery<ExternalPartyCopiesSearchResult>(
                              @"SearchExternalPartyCopies @ExternalPartyId,@DateFrom,@DateTo,@HasFullPrivilege, @UserId,
                     @PageIndex, @PageSize, @Ascending, @CultureName, @OrderBy,  @TotalCount out",
                    new SqlParameter("ExternalPartyId", searchCriteriaByExternalPartyCopies.ExternalPartyId ?? (object)DBNull.Value),
                    new SqlParameter("DateFrom", searchCriteriaByExternalPartyCopies.FromDateTime.HasValue ? searchCriteriaByExternalPartyCopies.FromDateTime : (object)DBNull.Value),
                    new SqlParameter("DateTo", searchCriteriaByExternalPartyCopies.ToDateTime.HasValue ? searchCriteriaByExternalPartyCopies.ToDateTime : (object)DBNull.Value),
                    new SqlParameter("HasFullPrivilege", searchCriteriaByExternalPartyCopies.HasFullPrivilege),
                    new SqlParameter("UserId", searchCriteriaByExternalPartyCopies.UserId),
                    new SqlParameter("PageIndex", searchCriteriaByExternalPartyCopies.PageIndex),
                    new SqlParameter("PageSize", searchCriteriaByExternalPartyCopies.PageSize),
                    new SqlParameter("Ascending", searchCriteriaByExternalPartyCopies.Ascending),
                    new SqlParameter("CultureName", searchCriteriaByExternalPartyCopies.CultureName),
                    new SqlParameter("OrderBy", searchCriteriaByExternalPartyCopies.OrderBy),
                                sqlPTotalCount
                              );
                    externalCopiesSearchResults = ExternalPartyCopiesSearchResultsQuery.ToList();
                    TotalCount = int.Parse(sqlPTotalCount.Value.ToString());
                }

                return externalCopiesSearchResults;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        IList<ICSearchResult> ISearchWrapper.ICSearch(int year, string transNumber, int orgId, int type, int userId, string culutre)
        {
            try
            {
                List<ICSearchResult> ICSearchResultList = null;

                ICSearchResultList =

           _oMCSDbContext.Database.SqlQuery<ICSearchResult>("SearchIC @TransNumber,@TransType,@Year , @OrgUnitId , @culutre , @userId",
                                    new SqlParameter("TransNumber", transNumber),
                                    new SqlParameter("TransType", type),
                                    new SqlParameter("Year", year),
                                    new SqlParameter("OrgUnitId", orgId),
                                    new SqlParameter("culutre", culutre),
                                    new SqlParameter("userId", userId)

                                    ).Select(X => new ICSearchResult()
                                    {
                                        Id = X.Id,
                                        //ColorCode = 0,
                                        ConfidentialityName = X.ConfidentialityName,
                                        ConfidentialityId = X.ConfidentialityId,
                                        Date = X.Date,
                                        DateH = X.DateH,
                                        HasLinks = 0,
                                        //HasPermission = X.HasPermission,
                                        IsDeleted = 0,
                                        Number = X.Number,
                                        OrgUnitName = X.OrgUnitName,
                                        PartyName = X.PartyName, // transaction.ExternalParty.LocalName,
                                        PriorityName = X.PriorityName,
                                        StatusId = X.StatusId,
                                        StatusName = X.StatusName,
                                        Subject = X.Subject,
                                        ToEntityId = X.ToEntityId,
                                        //TotalCount = 1,
                                        ToUserId = X.ToUserId,
                                        TransactionCategoryId = X.TransactionCategoryId,
                                        TransactionCategoryName = "معاملة داخلية",
                                        //TransactionType = "Ehab",
                                        Weight = X.Weight,
                                        WithArchiving = 0,
                                        RemindDate = X.RemindDate,
                                        RemindDateH = X.RemindDateH,
                                        MainDocId = X.MainDocId,
                                        IsInIc = X.IsInIc,
                                        IcName = X.IcName,
                                        OrderFileNumber = X.OrderFileNumber,
                                        Description = X.Description,

                                    }).ToList();



                return ICSearchResultList;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




    }

    public interface ISearchWrapper
    {
        IList<InboundSearchResult> InboundSearch(SearchCriteriaByInbound searchCriteriaByInbound, out int TotalCount);
        IList<OutboundInternalSearchResult> OutboundInternalSearch(SearchCriteriaByOutboundInternal searchCriteriaByOutboundInternal, out int TotalCount);
        IList<OutboundSearchResult> OutboundSearch(SearchCriteriaByOutbound searchCriteriaByOutbound, out int TotalCount);
        IList<OutboundDraftSearchResult> OutboundDraftSearch(SearchCriteriaByOutboundDraft searchCriteriaByOutboundDraft, out int TotalCount);
        IList<SubjectSearchResult> SubjectSearch(SearchCriteriaBySubject searchCriteriaBySubject, out int TotalCount);
        IList<BaseSearchResult> BarcodeSearch(SearchCriteriaByBarcode searchCriteriaByBarcode, out int TotalCount);
        IList<EntitySearchResult> EntitySearch(SearchCriteriaByEntityName searchCriteriaByInbound, out int TotalCount);
        IList<CreatorSearchResult> CreatorSearch(SearchCriteriaByCreator searchCriteriaByInbound, out int TotalCount);
        IList<AssignTransactionSearchResult> AssignTransactionSearch(SearchCriteriaByAssignTransaction searchCriteriaByInbound, out int TotalCount);
        IList<InboundSearchResult> SearchDocumentNumber(SearchCriteriaByDocumentNumber searchCriteriaByDocumentNumber, out int TotalCount);
        IList<InboundSearchResult> SearchRecordNumber(SearchCriteriaByRecordNumber searchCriteriaByRecordNumber, out int TotalCount);
        IList<NamesSearchResult> SearchNames(SearchCriteriaByNames searchCriteriaByNames, out int TotalCount);
        IList<DailySearchResult> SearchDaily(SearchCriteriaByDaily searchCriteriaByDaily, out int TotalCount);
        IList<AssignmentNoteSearchResult> SearchAssignmentNote(SearchCriteriaByAssignmentNote searchCriteriaByAssignmentNote, out int TotalCount);
        IList<ManifestNumberSearchResult> SearchManifestNumber(SearchCriteriaByManifestNumber searchCriteriaByManifestNumber, out int TotalCount);
        IList<ExternalPartyCopiesSearchResult> SearchExternalPartyCopies(SearchCriteriaByExternalPartyCopies searchCriteriaByExternalPartyCopies, out int TotalCount);

        IList<MilitaryNumberOrIdentitySearchResult> SearchMilitaryNumberOrIdentity(SearchCriteriaByMilitaryNumberOrIdentity searchCriteriaByMilitaryNumberOrIdentity, out int TotalCount);
        IList<TransactionNotsSearchResult> SearchTransactionNots(SearchCriteriaByTransactionNots searchCriteriaTransactionNots, out int TotalCount);
        IList<ElcEmployeeSearchResult> SearchELcEmployee(SearchCriteriaByElcEmployee searchCriteriaByElcEmployee, out int TotalCount);
        IList<ExternalOutBoundOrManifestNumberSearchResult> SearchExternalOutBoundOrManifestNumber(SearchCriteriaByExternalOutBoundOrManifestNumber searchCriteriaByExternalOutBoundOrManifestNumber, out int TotalCount);
        IList<CopyAssignemntSearchResult> SearchCopyAssignemnt(SearchCriteriaByCopyAssignemnt searchCriteriaByCopyAssignemnt, out int TotalCount);
        IList<SubjectLetterSearchResult> SearchSubjectLetter(SearchCriteriaBySubjectLetter searchCriteriaBySubjectLetter, out int TotalCount);
        IList<TransactionNumberSearchResult> SearchTransactionNumber(SearchCriteriaByTransactionNumber searchCriteriaByTransactionNumber, out int TotalCount);
        IList<ICSearchResult> ICSearch(int year, string transNumber, int orgId, int type, int userId, string culutre);

    }
}
