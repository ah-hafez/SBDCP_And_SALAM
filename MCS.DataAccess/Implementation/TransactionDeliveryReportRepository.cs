using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Data.Entity;

namespace MCS.DataAccess
{
    public class TransactionDeliveryReportRepository : BaseRepository<TransactionDeliveryReport>, ITransactionDeliveryReportRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionDeliveryReportRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Method

        public int AddTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport)
        {
            try
            {
                var transaction = _oMCSDbContext.Transactions.FirstOrDefault(t => t.Id == transactionDeliveryReport.TransactionId);
                _oMCSDbContext.TransactionDeliveryReports.Add(transactionDeliveryReport);

                _oMCSDbContext.SaveChanges();

                return transactionDeliveryReport.Id;
                //if (transaction.ExternalCopies != null && transaction.ExternalCopies.Count > 0)
                //{
                //    foreach (var item in transaction.ExternalCopies)
                //    {
                //        TransactionDeliveryReport DeliveryReport = new TransactionDeliveryReport
                //        {
                //            Date = transactionDeliveryReport.Date,
                //            DateH = transactionDeliveryReport.DateH,
                //            UserId = transactionDeliveryReport.UserId,
                //            TransactionId = transaction.Id,
                //            TransactionExternalCopyId = item.Id,
                //            OrgunitId = transactionDeliveryReport.OrgunitId
                //        };
                //        _oMCSDbContext.TransactionDeliveryReports.Add(DeliveryReport);
                //    }
                //}
                //if (transaction.Copies != null && transaction.Copies.Count > 0)
                //{
                //    foreach (var item in transaction.Copies)
                //    {
                //        TransactionDeliveryReport DeliveryReport = new TransactionDeliveryReport
                //        {
                //            Date = transactionDeliveryReport.Date,
                //            DateH = transactionDeliveryReport.DateH,
                //            UserId = transactionDeliveryReport.UserId,
                //            TransactionId = transaction.Id,
                //            TransactionCopyId = item.Id,
                //            OrgunitId = transactionDeliveryReport.OrgunitId
                //        };
                //        _oMCSDbContext.TransactionDeliveryReports.Add(DeliveryReport);
                //    }
                //}


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionDeliveryReportCopies(int transactionId, int? reporterId)
        {
            try
            {
                var transaction = _oMCSDbContext.Transactions.FirstOrDefault(t => t.Id == transactionId);

                if (transaction.ExternalCopies.Count > 0)
                {
                    foreach (var item in transaction.ExternalCopies)
                    {
                        if (!_oMCSDbContext.TransactionDeliveryReports.Any(a => a.TransactionId == transaction.Id && a.TransactionExternalCopyId == item.Id))
                        {
                            TransactionDeliveryReport DeliveryReport = new TransactionDeliveryReport
                            {
                                Date = DateTime.Now,
                                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                UserId = Framework.Web.UserContext.LoggedInUser.Id,
                                TransactionId = transaction.Id,
                                TransactionExternalCopyId = item.Id,
                                ReporterId = reporterId,
                                OrgunitId = item.FromEntityId.HasValue ? item.FromEntityId.Value : transaction.OrgUnitId
                            };
                            _oMCSDbContext.TransactionDeliveryReports.Add(DeliveryReport);
                        }
                    }
                }
                if (transaction.Copies.Count > 0)
                {
                    foreach (var item in transaction.Copies)
                    {
                        if (!_oMCSDbContext.TransactionDeliveryReports.Any(a => a.TransactionId == transaction.Id && a.TransactionCopyId == item.Id))
                        {
                            TransactionDeliveryReport DeliveryReport = new TransactionDeliveryReport
                            {
                                Date = DateTime.Now,
                                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                UserId = Framework.Web.UserContext.LoggedInUser.Id,
                                TransactionId = transaction.Id,
                                TransactionCopyId = item.Id,
                                ReporterId = reporterId,
                                OrgunitId = item.FromEntityId.HasValue ? item.FromEntityId.Value : transaction.OrgUnitId
                            };
                            _oMCSDbContext.TransactionDeliveryReports.Add(DeliveryReport);
                        }
                    }
                }


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport)
        {
            try
            {
                _oMCSDbContext.Entry(transactionDeliveryReport).State = System.Data.Entity.EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int UpdateDeliveryReportsDocumentByNumber(DocumentInfo document, string Number)
        {
            try
            {
                int? documentId = 0;
                bool Saved = false;
                IList<TransactionDeliveryReport> transactionDeliveryReports = GetTransactionDeliveryReport(n => n.Number == Number);

                foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
                {
                    if (!Saved)
                    {
                        transactionDeliveryReport.Document = new DocumentInfo()
                        {
                            MimeType = document.MimeType,
                            Size = document.Size,
                            CreatedBy = document.CreatedBy,
                            Document = new Document()
                            {
                                CreatedBy = document.CreatedBy,
                            }
                        };
                        TransactionDeliveryReport OldDeliveryReport = _oMCSDbContext.TransactionDeliveryReports.Where(n => n.Id == transactionDeliveryReport.Id).FirstOrDefault();

                        OldDeliveryReport.Document = transactionDeliveryReport.Document;
                        _oMCSDbContext.SaveChanges();
                        documentId = OldDeliveryReport.DocumentId;
                        Saved = true;

                    }
                    else
                    {
                        TransactionDeliveryReport OldDeliveryReport = _oMCSDbContext.TransactionDeliveryReports.Where(n => n.Id == transactionDeliveryReport.Id).FirstOrDefault();
                        OldDeliveryReport.Document = null;
                        OldDeliveryReport.DocumentId = documentId;
                        _oMCSDbContext.SaveChanges();
                    }
                }
                return documentId.Value;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, string cultureName)
        {
            try
            {
                DateTime? Objdate = new DateTime();
                Objdate = date.Value.Date;
                List<TransactionDeliveryReport> transactionDeliveryReports = _oMCSDbContext.TransactionDeliveryReports.Where(d => DbFunctions.TruncateTime(d.Date) == Objdate & (d.Number != null)).ToList();
                var result = transactionDeliveryReports.Select(t => new TransactionDeliveryReport
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    Document = t.Document,
                    DocumentId = t.DocumentId,
                    TransactionHistory = new TransactionHistory()
                    {
                        Confidentiality = new Permission() { LocalName = (t.TransactionHistory.Confidentiality != null) ? t.TransactionHistory.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        Priority = new Priority() { Text = (t.TransactionHistory.Priority != null) ? t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        TransactionType = new TransactionType() { Text = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        User = new UserProfile() { LocalName = (t.TransactionHistory.User != null) ? t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        DeliveryMethod = new Lookup() { Text = (t.TransactionHistory.DeliveryMethod != null) ? t.TransactionHistory.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        Subject = t.TransactionHistory.Subject,
                        ExternalParty = new ExternalParty() { LocalName = (t.TransactionHistory.ExternalParty != null) ? t.TransactionHistory.ExternalParty.LocalName : string.Empty },
                        Transaction = new Transaction()
                        {
                            TransactionCategory = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionCategory : null,
                            TransactionCategoryId = t.TransactionHistory.Transaction.TransactionCategoryId,
                            IsForIndividual = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.IsForIndividual : false,
                            ExternalParty = (t.TransactionHistory.Transaction.ExternalParty != null) ? new ExternalParty
                            {
                                Id = t.TransactionHistory.Transaction.ExternalParty.Id,
                                Number = t.TransactionHistory.Transaction.ExternalParty.Number,
                                LocalName = t.TransactionHistory.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            } : null,
                        },
                        TransactionCategory = new Lookup() { Text = (t.TransactionHistory.TransactionCategory != null) ? t.TransactionHistory.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty }
                    },
                    TransactionAssignmentHistory = new TransactionAssignmentHistory()
                    {
                        ToEntity = new OrgUnit() { LocalName = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToEntity != null ? (t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()) : string.Empty : string.Empty },
                        ToUser = new UserProfile() { LocalName = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToUser != null ? (t.TransactionAssignmentHistory.ToUser.LocalName) : string.Empty : string.Empty },
                    },
                    TransactionId = t.TransactionId,
                    Transaction = new Transaction()
                    {
                        TransactionCategory = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionCategory : null,
                        TransactionCategoryId = t.TransactionHistory.Transaction.TransactionCategoryId,
                        IsForIndividual = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.IsForIndividual : false,
                        ExternalParty = (t.TransactionHistory.Transaction.ExternalParty != null) ? new ExternalParty
                        {
                            Id = t.TransactionHistory.Transaction.ExternalParty.Id,
                            Number = t.TransactionHistory.Transaction.ExternalParty.Number,
                            LocalName = t.TransactionHistory.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        } : null,
                    },
                    Number = t.Number != null ? t.Number.ToString() : null
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, int? transactionNumber, string number, string cultureName)
        {
            try
            {
                DateTime? Objdate = null;
                //if (date.HasValue)
                //{
                //    Objdate = date.Value.Date;
                //}
                var transactionDeliveryReportsQuery = _oMCSDbContext.TransactionDeliveryReports.Where(d => (Objdate == null ? true : DbFunctions.TruncateTime(d.Date) == Objdate) && (number == null ? true : d.Number == number) && (transactionNumber == null ? true : d.Transaction.Number == transactionNumber));
                List<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportsQuery.ToList();

                var result = transactionDeliveryReports.Select(t => new TransactionDeliveryReport
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    Document = t.Document,
                    DocumentId = t.DocumentId,
                    TransactionHistory = t.TransactionHistory == null ? new TransactionHistory() : new TransactionHistory()
                    {
                        Confidentiality = new Permission() { LocalName = (t.TransactionHistory.Confidentiality != null) ? t.TransactionHistory.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        Priority = new Priority() { Text = (t.TransactionHistory.Priority != null) ? t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        TransactionType = new TransactionType() { Text = (t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        User = new UserProfile() { LocalName = (t.TransactionHistory.User != null) ? t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        DeliveryMethod = new Lookup() { Text = (t.TransactionHistory.DeliveryMethod != null) ? t.TransactionHistory.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty },
                        Subject = t.TransactionHistory.Subject,
                        ExternalParty = new ExternalParty() { LocalName = (t.TransactionHistory.ExternalParty != null) ? t.TransactionHistory.ExternalParty.LocalName : string.Empty },
                        Transaction = new Transaction()
                        {
                            TransactionCategory = (t.TransactionHistory != null && t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionCategory : null,
                            TransactionCategoryId = t.TransactionHistory != null ? t.TransactionHistory.Transaction.TransactionCategoryId : 0,
                            IsForIndividual = (t.TransactionHistory != null && t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.IsForIndividual : false,
                            ExternalParty = (t.TransactionHistory != null && t.TransactionHistory.Transaction.ExternalParty != null) ? new ExternalParty
                            {
                                Id = t.TransactionHistory.Transaction.ExternalParty.Id,
                                Number = t.TransactionHistory.Transaction.ExternalParty.Number,
                                LocalName = t.TransactionHistory.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            } : null,
                        },
                        TransactionCategory = new Lookup() { Text = (t.TransactionHistory != null && t.TransactionHistory.TransactionCategory != null) ? t.TransactionHistory.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty }
                    },
                    TransactionAssignmentHistory = new TransactionAssignmentHistory()
                    {
                        ToEntity = new OrgUnit() { LocalName = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToEntity != null ? (t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()) : string.Empty : string.Empty },
                        ToUser = new UserProfile() { LocalName = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToUser != null ? (t.TransactionAssignmentHistory.ToUser.LocalName) : string.Empty : string.Empty },
                    },
                    TransactionId = t.TransactionId,
                    Transaction = new Transaction()
                    {
                        TransactionCategory = (t.TransactionHistory != null && t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.TransactionCategory : null,
                        TransactionCategoryId = t.TransactionHistory != null ? t.TransactionHistory.Transaction.TransactionCategoryId : 0,
                        IsForIndividual = (t.TransactionHistory != null && t.TransactionHistory.Transaction != null) ? t.TransactionHistory.Transaction.IsForIndividual : false,
                        ExternalParty = (t.TransactionHistory != null && t.TransactionHistory.Transaction.ExternalParty != null) ? new ExternalParty
                        {
                            Id = t.TransactionHistory.Transaction.ExternalParty.Id,
                            Number = t.TransactionHistory.Transaction.ExternalParty.Number,
                            LocalName = t.TransactionHistory.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        } : null,
                    },
                    Number = t.Number != null ? t.Number.ToString() : null
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public TransactionDeliveryReport GetTransactionDeliveryReportById(int transactionDeliveryReportId)
        {
            try
            {
                return FindBy(p => p.Id == transactionDeliveryReportId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByIds(List<int> transactionDeliveryReportIds)
        {
            try
            {
                IList<TransactionDeliveryReport> transactionDeliveryReports = new List<TransactionDeliveryReport>();

                foreach (int transactionDeliveryReportId in transactionDeliveryReportIds)
                {
                    Expression<Func<TransactionDeliveryReport, bool>> Where = null;

                    Where = (dr => dr.Id > 0);

                    Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Id == transactionDeliveryReportId);

                    transactionDeliveryReports.Add(_oMCSDbContext.TransactionDeliveryReports.Where(Where).FirstOrDefault());
                }

                return transactionDeliveryReports;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionDeliveryReport> GetTransactionDeliveryReport(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                searchCriteria.PageSize = 50;
                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                System.Linq.Expressions.Expression<Func<TransactionDeliveryReport, bool>> where =
                    SearchDeliveryReport(searchCriteria);

                IQueryable<TransactionDeliveryReport> transactionDeliveryReports =
                    _oMCSDbContext.TransactionDeliveryReports.Where(where).AsQueryable();

                if (searchCriteria.FromDateTime.HasValue)
                {
                    transactionDeliveryReports = transactionDeliveryReports.Where(dr =>
                        dr.Date >= searchCriteria.FromDateTime.Value);
                }

                if (searchCriteria.ToDateTime.HasValue)
                {
                    transactionDeliveryReports = transactionDeliveryReports.Where(dr =>
                      dr.Date <= searchCriteria.ToDateTime.Value);
                }
                foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                {
                    switch (searchColunm.ColunmName)
                    {
                        case "ToEntity":
                            {
                                int toEntity = int.Parse(searchColunm.ColunmValue);

                                transactionDeliveryReports = transactionDeliveryReports.Where(dr =>
                                          dr.Transaction.ExternalPartyId == toEntity);
                                break;
                            }
                    }
                }

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Confidentiality")
                        {
                            transactionDeliveryReports = SortTextByToConfidentiality(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Priority")
                        {
                            transactionDeliveryReports = SortTextByToPriority(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "SourceType")
                        {
                            transactionDeliveryReports = SortTextByToTransactionType(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "User")
                        {
                            transactionDeliveryReports = SortTextByToUser(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "DeliveryMethod")
                        {
                            transactionDeliveryReports = SortTextByToDeliveryMethod(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(int).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "ToEntity")
                        {
                            transactionDeliveryReports = SortTextByToEntity(transactionDeliveryReports, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactionDeliveryReports.Count();

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values    
                switch (searchCriteria.OrderBy)
                {
                    case "Confidentiality":
                        transactionDeliveryReports = OrderByConfidentiality(transactionDeliveryReports, searchCriteria);
                        break;
                    case "Priority":
                        transactionDeliveryReports = OrderByPriority(transactionDeliveryReports, searchCriteria);
                        break;
                    case "SourceType":
                        transactionDeliveryReports = OrderByTransactionType(transactionDeliveryReports, searchCriteria);
                        break;
                    case "User":
                        transactionDeliveryReports = OrderByUser(transactionDeliveryReports, searchCriteria);
                        break;
                    case "DeliveryMethod":
                        transactionDeliveryReports = OrderByDeliveryMethod(transactionDeliveryReports, searchCriteria);
                        break;
                    case "Id":
                        transactionDeliveryReports = OrderById(transactionDeliveryReports, searchCriteria);
                        break;
                    case "Number":
                        transactionDeliveryReports = OrderByNumber(transactionDeliveryReports, searchCriteria);
                        break;
                    case "Date":
                        transactionDeliveryReports = OrderByDate(transactionDeliveryReports, searchCriteria);
                        break;
                }


                transactionDeliveryReports = transactionDeliveryReports.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);
                var result = transactionDeliveryReports.ToList().Select(t => new TransactionDeliveryReport
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    TransactionHistory = new TransactionHistory()
                    {
                        Confidentiality = new Permission() { LocalName = (t.Transaction.Confidentiality != null) ? t.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty },
                        Priority = new Priority() { Text = (t.Transaction.Priority != null) ? t.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty },
                        TransactionType = new TransactionType() { Text = (t.Transaction != null) ? t.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty },
                        User = new UserProfile() { LocalName = (t.Transaction.User != null) ? t.Transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty },
                        DeliveryMethod = new Lookup() { Text = (t.Transaction.DeliveryMethod != null) ? t.Transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty },
                        Subject = t.Transaction.Subject,
                        ExternalParty = new ExternalParty() { LocalName = (t.Transaction.ExternalParty != null) ? t.Transaction.ExternalParty.LocalName : string.Empty },
                        Transaction = new Transaction()
                        {
                            TransactionCategory = (t.Transaction != null) ? t.Transaction.TransactionCategory : null,
                            TransactionCategoryId = t.Transaction.TransactionCategoryId,
                            IsForIndividual = (t.Transaction != null) ? t.Transaction.IsForIndividual : false,
                            ExternalParty = (t.Transaction.ExternalParty != null) ? new ExternalParty
                            {
                                Id = t.Transaction.ExternalParty.Id,
                                Number = t.Transaction.ExternalParty.Number,
                                LocalName = t.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                            } : null,
                        },
                        TransactionCategory = new Lookup() { Text = (t.Transaction.TransactionCategory != null) ? t.Transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : string.Empty }
                    },
                    TransactionAssignmentHistory = new TransactionAssignmentHistory()
                    {
                        //محاله للمستخدم
                        //ToUser = new UserProfile() { LocalName = t.TransactionAssignmentHistory.ToUser != null ? (t.TransactionAssignmentHistory.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()) : string.Empty },
                        ToEntity = new OrgUnit() { LocalName = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToEntity != null ? (t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()) : string.Empty : string.Empty },
                        ToEntityId = t.TransactionAssignmentHistory != null ? t.TransactionAssignmentHistory.ToEntityId : 0
                    },
                    TransactionId = t.TransactionId,
                    TransactionExternalCopyId = t.TransactionExternalCopyId,
                    TransactionExternalCopy = t.TransactionExternalCopy,
                    TransactionCopyId = t.TransactionCopyId,
                    TransactionCopy = t.TransactionCopy,
                    Transaction = new Transaction()
                    {
                        Number = t.Transaction.Number,
                        TransactionCategory = (t.Transaction != null) ? t.Transaction.TransactionCategory : null,
                        TransactionCategoryId = t.Transaction.TransactionCategoryId,
                        IsForIndividual = (t.Transaction != null) ? t.Transaction.IsForIndividual : false,
                        ExternalParty = (t.Transaction.ExternalParty != null) ? new ExternalParty
                        {
                            Id = t.Transaction.ExternalParty.Id,
                            Number = t.Transaction.ExternalParty.Number,
                            LocalName = t.Transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        } : null,
                    },
                    Number = t.Number != null ? t.Number.ToString() : null// _oMCSDbContext.Transactions.ToList().Where(trans => trans.Id == t.TransactionId).FirstOrDefault().Number.ToString(),
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionDeliveryReport> GetTransactionDeliveryReport(Expression<Func<TransactionDeliveryReport, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionDeliveryReports.Where(where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private Expression<Func<TransactionDeliveryReport, bool>> SearchDeliveryReport(SearchCriteria searchCriteria)
        {
            //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values

            Expression<Func<TransactionDeliveryReport, bool>> Where = null;

            Where = (d => d.Id > 0);

            foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
            {
                switch (searchColunm.ColunmName)
                {
                    case "Priority":
                        {
                            int priorityId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionHistory.Priority.Id == priorityId);
                            break;
                        }
                    case "Confidentiality":
                        {
                            int confidentialityId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionHistory.ConfidentialityId == confidentialityId);
                            break;
                        }
                    case "LetterTypeId":
                        {
                            int letterTypeId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionHistory.LetterTypeId == letterTypeId);
                            break;
                        }
                    case "SourceId":
                        {
                            int sourceId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionHistory.TransactionTypeId == sourceId);
                            break;
                        }
                    //case "ToEntity":
                    //    {
                    //        int toEntity = int.Parse(searchColunm.ColunmValue);
                    //        Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionAssignmentHistory.Transaction.ExternalPartyId == toEntity);
                    //        break;
                    //    }
                    case "ToOrgUnit":
                        {
                            int toOrgUnit = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionAssignmentHistory.ToEntityId == toOrgUnit);
                            break;
                        }
                    case "ToUser":
                        {
                            int toUser = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionAssignmentHistory.ToUserId == toUser);
                            break;
                        }
                    case "FromOrgUnit":
                        {
                            int fromOrgUnit = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionAssignmentHistory.FromEntityId == fromOrgUnit);
                            break;
                        }
                    case "FromUser":
                        {
                            int fromUser = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionAssignmentHistory.FromUserId == fromUser);
                            break;
                        }
                    case "UserId":
                        {
                            int userId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.UserId == userId);
                            break;
                        }
                    case "OrgunitId":
                        {
                            int orgunitId = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.OrgunitId == orgunitId);
                            break;
                        }
                    case "IsPrinted":
                        {
                            bool isPrinted = bool.Parse(searchColunm.ColunmValue);

                            if (isPrinted)
                            {
                                Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Number != null);
                                break;
                            }

                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Number == null);
                            break;
                        }
                    case "TransactionCategory":
                        {
                            int transactionCategory = int.Parse(searchColunm.ColunmValue);
                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Transaction.TransactionCategoryId == transactionCategory);
                            break;
                        }
                    case "FromTransactionNumber":
                        {
                            int fromTransactionNumber = int.Parse(searchColunm.ColunmValue);

                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Transaction.Number >= fromTransactionNumber);
                            break;
                        }
                    case "ToTransactionNumber":
                        {
                            int toTransactionNumber = int.Parse(searchColunm.ColunmValue);

                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Transaction.Number <= toTransactionNumber);
                            break;
                        }
                    case "TransactionTypeId":
                        {
                            int transactionTypeId = int.Parse(searchColunm.ColunmValue);

                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.TransactionHistory.Transaction.TransactionTypeId == transactionTypeId);
                            break;
                        }
                    case "DeliveryReportNumber":
                        {
                            string DeliveryReportNumber = int.Parse(searchColunm.ColunmValue).ToString();

                            Where = ExpressionUtility.AndAlso<TransactionDeliveryReport>(Where, dr => dr.Number == DeliveryReportNumber);
                            break;
                        }
                }
            }

            return Where;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToConfidentiality(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionHistory.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionHistory.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionHistory.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionHistory.Confidentiality.Name.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToTransactionType(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToPriority(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToUser(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToDeliveryMethod(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionHistory.DeliveryMethod.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionHistory.DeliveryMethod.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionHistory.DeliveryMethod.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionHistory.DeliveryMethod.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> SortTextByToEntity(IQueryable<TransactionDeliveryReport> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByConfidentiality(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionHistory.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionHistory.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByPriority(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionHistory.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByTransactionType(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionHistory.TransactionType.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByUser(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionHistory.User.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByDeliveryMethod(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TransactionHistory.DeliveryMethod.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.TransactionHistory.DeliveryMethod.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderById(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(dr => dr.Id);
            }
            else
            {
                source = source.OrderByDescending(dr => dr.Id);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByNumber(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(dr => dr.TransactionHistory.Transaction.Number);
            }
            else
            {
                source = source.OrderByDescending(dr => dr.TransactionHistory.Transaction.Number);
            }

            return source;
        }

        private IQueryable<TransactionDeliveryReport> OrderByDate(IQueryable<TransactionDeliveryReport> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(dr => dr.TransactionHistory.Transaction.DateH);
            }
            else
            {
                source = source.OrderByDescending(dr => dr.TransactionHistory.Transaction.DateH);
            }

            return source;
        }

        #endregion Method
    }
}
