using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.Framework;

namespace MCS.DataAccess
{
    public class TransactionLoggingRepository : BaseRepository<TransactionLog>, ITransactionLoggingRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionLoggingRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
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
        public IList<TransactionLogDetailInfo> GetTransactionLogDetailsInfo(int transactionId, string cultureName, bool IsForPrint, SearchCriteriaCustom searchCriteria, out int itemsCount)
        {
            try
            {
                var page = 0;
                var pageSize = 0;
                IQueryable<TransactionLog> transactionLogs = _oMCSDbContext.TransactionLogs.Include(tl => tl.User).Include(tl => tl.AuditingActionCode).Where(tl => tl.TransactionId == transactionId);
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (var item in searchCriteria.Filters)
                        {
                            if (item.ColumnName == "LogedByUserName")
                            {
                                int UserId = Convert.ToInt32(item.Value);
                                transactionLogs = transactionLogs.Where(tl => tl.UserId == UserId);
                            }
                            else if (item.ColumnName == "LogDateFrom")
                            {
                                DateTime LogDateFrom = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                                transactionLogs = transactionLogs.Where(tl => DbFunctions.TruncateTime(tl.Date) >= LogDateFrom);
                            }
                            else if (item.ColumnName == "LogDateTo")
                            {
                                DateTime LogDateTo = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                                transactionLogs = transactionLogs.Where(tl => DbFunctions.TruncateTime(tl.Date) <= LogDateTo);
                            }
                            else if (item.ColumnName == "LogType")
                            {
                                int LogType = Convert.ToInt32(item.Value);
                                transactionLogs = transactionLogs.Where(tl => tl.AuditingActionCode.Id == LogType);
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
                                transactionLogs = OrderTransactionLogByCreateOn(transactionLogs, searchCriteria.Ascending);
                                break;
                            default:
                                transactionLogs = transactionLogs.OrderBy(tl => tl.Id);
                                break;
                        }
                    }
                }
                itemsCount = transactionLogs.Count();

                if (!IsForPrint)
                {
                    transactionLogs = transactionLogs.Skip((page) * pageSize)
                                              .Take(pageSize);
                }

                IList<TransactionLogDetailInfo> transactionLogDetailInfos = transactionLogs.ToList().Select(tl =>
                {
                    TransactionLogDetailInfo transactionLogDetailInfo = new TransactionLogDetailInfo
                    {
                        UserId = tl.UserId,
                        UserName = tl.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        Date = tl.Date,
                        Description = tl.AuditingActionCode != null ? tl.AuditingActionCode.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : ""
                    };
                    return transactionLogDetailInfo;
                }).ToList();

                return transactionLogDetailInfos;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<TransactionLog> OrderTransactionLogByCreateOn(IQueryable<TransactionLog> transactionLogs, bool asc)
        {
            if (asc)
            {
                return transactionLogs.OrderBy(tl => tl.CreatedOn);
            }
            else
            {
                return transactionLogs.OrderByDescending(tl => tl.CreatedOn);
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
