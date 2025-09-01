using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.Framework;

namespace MCS.DataAccess
{
    public class AuditLogRepository : AuditRepository<AuditLog>, IAuditLogRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public AuditLogRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int Log(TransactionLog transactionLog)
        {
            try
            {
                _oMCSDbContext.TransactionLogs.Add(transactionLog);

                _oMCSDbContext.SaveChanges();

                return transactionLog.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TransactionLogInfo> GetTransactionLogInfo(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionLogInfo> transactionLogs = (from transactionLogGroup in _oMCSDbContext.TransactionLogs
                                                             where transactionLogGroup.TransactionId == transactionId
                                                             select new
                                                             {
                                                                 transactionLogGroup.UserId,
                                                                 transactionLogGroup.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                 Description = transactionLogGroup.AuditingActionCode.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                 transactionLogGroup.Date
                                                             }).GroupBy(t => t.UserId).Select(tl => new TransactionLogInfo()
                                                             {
                                                                 UserId = tl.Key,
                                                                 UserName = (tl.FirstOrDefault() != null) ? (tl.FirstOrDefault().Text != null) ? tl.FirstOrDefault().Text : string.Empty : string.Empty,

                                                                 TransactionLogDetails = tl.ToList().Select(td => new TransactionLogDetailInfo()
                                                                 {
                                                                     Date = td.Date,
                                                                     Description = td.Description ?? string.Empty
                                                                 }).ToList()
                                                             }).ToList();

                return transactionLogs;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TransactionLogDetailInfo> GetTransactionLogDetailsInfo(int transactionId, int userId, string cultureName)
        {
            try
            {
                TransactionLogInfo transactionLogInfo = (from transactionLogGroup in _oMCSDbContext.TransactionLogs
                                                         where transactionLogGroup.TransactionId == transactionId && transactionLogGroup.UserId == userId
                                                         select new
                                                         {
                                                             transactionLogGroup.UserId,
                                                             transactionLogGroup.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                             Description = transactionLogGroup.AuditingActionCode.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                             transactionLogGroup.Date
                                                         }).GroupBy(t => t.UserId).Select(tl => new TransactionLogInfo()
                                                         {
                                                             UserId = tl.Key,
                                                             UserName = (tl.FirstOrDefault() != null) ? (tl.FirstOrDefault().Text != null) ? tl.FirstOrDefault().Text : string.Empty : string.Empty,

                                                             TransactionLogDetails = tl.ToList().Select(td => new TransactionLogDetailInfo()
                                                             {
                                                                 Date = td.Date,
                                                                 Description = td.Description ?? string.Empty
                                                             }).ToList()
                                                         }).FirstOrDefault();

                return transactionLogInfo.TransactionLogDetails.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<AuditLog> GetAuditLog(string cultureName, bool IsForPrint, SearchCriteriaCustom searchCriteria, out int itemsCount)
        {
            try
            {
                var page = 0;
                var pageSize = 0;
                IQueryable<AuditLog> AuditLogs = _oMCSDbContext.AuditLogs;//.Where(tl => tl.TransactionId == transactionId);
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (var item in searchCriteria.Filters)
                        {
                            if (item.ColumnName == "LogedByUserName")
                            {
                                int UserId = Convert.ToInt32(item.Value);
                                AuditLogs = AuditLogs.Where(tl => tl.AuditUser == UserId);
                            }
                            else if (item.ColumnName == "LogDateFrom")
                            {
                                DateTime LogDateFrom = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                                AuditLogs = AuditLogs.Where(tl => DbFunctions.TruncateTime(tl.AuditDate) >= LogDateFrom);
                            }
                            else if (item.ColumnName == "LogDateTo")
                            {
                                DateTime LogDateTo = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                                AuditLogs = AuditLogs.Where(tl => DbFunctions.TruncateTime(tl.AuditDate) <= LogDateTo);
                            }
                            else if (item.ColumnName == "LogType")
                            {
                                
                                AuditLogs = AuditLogs.Where(tl => tl.AuditAction == item.Value);
                            }
                        }
                    }
                    page = searchCriteria.PageIndex - 1;
                    pageSize = searchCriteria.PageSize;

                    if (searchCriteria.OrderBy != null)
                    {
                        switch (searchCriteria.OrderBy)
                        {
                            case "LogedOn":
                                AuditLogs = OrderTransactionLogByCreateOn(AuditLogs, searchCriteria.Ascending);
                                break;
                            default:
                                AuditLogs = AuditLogs.OrderBy(tl => tl.Id);
                                break;
                        }
                    }
                }
                itemsCount = AuditLogs.Count();

                if (!IsForPrint)
                {
                    AuditLogs = AuditLogs.Skip((page) * pageSize)
                                              .Take(pageSize);
                }

                IList<AuditLog> AuditLogDetailInfos = AuditLogs.ToList().Select(tl =>
                {
                    AuditLog auditLog = new AuditLog
                    {
                        AuditUser = tl.AuditUser,
                       
                        AuditDate = tl.AuditDate,
                        AuditAction = tl.AuditAction,
                        AuditData = tl.AuditData,
                        EntityType = tl.EntityType,
                        GuidId = tl.GuidId,

                    };
                    return auditLog;
                }).ToList();

                return AuditLogDetailInfos;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<AuditLog> OrderTransactionLogByCreateOn(IQueryable<AuditLog> AuditLogs, bool asc)
        {
            if (asc)
            {
                return AuditLogs.OrderBy(tl => tl.AuditData);
            }
            else
            {
                return AuditLogs.OrderByDescending(tl => tl.AuditData);
            }
        }

        public TransactionLog GetFirstView(int transactionId, AuditingActionCode auditingActionCode, int? userId, DateTime sendDate, string cultureName)
        {
            try
            {
                List<int> actionIds = new List<int>();
                List<int> auditCodes = new List<int>();
                auditCodes.Add((int)auditingActionCode);
                if (AuditingActionCode.ViewTransaction == auditingActionCode)
                {
                    auditCodes.Add((int)AuditingActionCode.ViewBasicInformation);
                    auditCodes.Add((int)AuditingActionCode.UpadteTransaction);
                    auditCodes.Add((int)AuditingActionCode.OpenEditor);
                    auditCodes.Add((int)AuditingActionCode.ViewCertificate);
                    auditCodes.Add((int)AuditingActionCode.ViewTransactionArchiving);
                    auditCodes.Add((int)AuditingActionCode.ViewTransactionAttachmentsArchiving);
                    auditCodes.Add((int)AuditingActionCode.ViewTransactionNames);
                    auditCodes.Add((int)AuditingActionCode.ViewTransactionLinks);
                }

                actionIds = _oMCSDbContext.Lookups.Where(l => l.CategoryId == (int)LookupCategory.AuditingActionCode && auditCodes.Any(enm => enm == l.EnumReference)).Select(x => x.Id).ToList();

                return _oMCSDbContext.TransactionLogs.Include(x => x.User.LocalizationIdentifier.Localizations).Where(tl => tl.TransactionId == transactionId &&
                tl.AuditingActionCode.CategoryId == (int)LookupCategory.AuditingActionCode && auditCodes.Any(enm => enm == tl.AuditingActionCode.EnumReference) &&
                (tl.UserId == userId || !userId.HasValue)
                && tl.CreatedOn > sendDate).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods
    }
}
