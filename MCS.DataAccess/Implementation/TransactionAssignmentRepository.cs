using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Common.Utility;
using MCS.Domain;
using MCS.Domain.MobileSearchCriteria;
using MCS.Domain.Search.SearchCriteria;
using Action = MCS.Domain.Action;

namespace MCS.DataAccess
{
    public class TransactionAssignmentRepository : BaseRepository<TransactionAssignment>, ITransactionAssignmentRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionAssignmentRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public void SetTransactionAssignmentToViewed(TransactionAssignment transactionAssignment)
        {
            try
            {
                transactionAssignment.Viewed = true;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void SetTransactionAssignmentToViewed(int transactionAssignmentId)
        {
            try
            {
                TransactionAssignment transactionAssignment = FindBy(p => p.Id == transactionAssignmentId);
                if (transactionAssignment != null)
                {
                    transactionAssignment.Viewed = true;
                    _oMCSDbContext.Entry(transactionAssignment).State = EntityState.Modified;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int AddTransactionAssignment(TransactionAssignment transactionAssignment)
        {
            try
            {
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.TransactionAssignmentProcessPeriod = DateTime.Now;
                _oMCSDbContext.TransactionAssignments.Add(transactionAssignment);

                _oMCSDbContext.SaveChanges();

                return transactionAssignment.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionAssignment(TransactionAssignment transactionAssignment)
        {
            try
            {
                _oMCSDbContext.Entry(transactionAssignment).State = EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteTransactionAssignment(int id)
        {
            try
            {
                TransactionAssignment transactionAssignment =
                    _oMCSDbContext.TransactionAssignments.Where(p => p.Id == id).FirstOrDefault();

                if (transactionAssignment != null)
                {
                    _oMCSDbContext.TransactionAssignments.Remove(transactionAssignment);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionAssignment GetTransactionAssignmentById(int transactionAssignmentId)
        {
            try
            {
                return FindBy(p => p.Id == transactionAssignmentId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionAssignment GetTransactionAssignment(Expression<Func<TransactionAssignment, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignments.Where(@where).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignments.Where(@where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int GetTransactionAssignmentCount(Expression<Func<TransactionAssignment, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignments.Where(@where).Count();//.Where(t => !t.Transaction.IsDeleted).Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public TransactionAssignment GetTransactionAssignmentLight(Expression<Func<TransactionAssignment, bool>> where)
        {
            try
            {
                var result = _oMCSDbContext.TransactionAssignments.Where(@where).Where(t => !t.Transaction.IsDeleted).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public TransactionAssignment GetTransactionAssignment(UserProfile toUser, Transaction transaction)
        {
            try
            {

                return _oMCSDbContext.TransactionAssignments.Where(ts => ts.ToUser.Id == toUser.Id & ts.Transaction.Id == transaction.Id).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<TransactionAssignment> GetAssignments(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria, out int rowsCount, int? UserWeight, int currentUserId)
        {
            try
            {
                IQueryable<TransactionAssignment> transactionAssignments = _oMCSDbContext.TransactionAssignments.Where(where).AsQueryable();


                if (searchCriteria.SearchColunms != null && searchCriteria.SearchColunms.Count > 0)
                {
                    foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                    {
                        if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(searchColunm.ColunmName).PropertyType) & searchColunm.ColunmName == "Number")
                        {
                            transactionAssignments = transactionAssignments.Where(p => p.Transaction.Number.ToString().Equals(searchColunm.ColunmValue));
                        }
                    }
                }

                if (searchCriteria.FromDateTime.HasValue)
                {
                    transactionAssignments = transactionAssignments.Where(
                        p => p.Date.Year >= searchCriteria.FromDateTime.Value.Year &
                            p.Date.Month >= searchCriteria.FromDateTime.Value.Month &
                            p.Date.Day >= searchCriteria.FromDateTime.Value.Day
                        );
                }

                if (searchCriteria.ToDateTime.HasValue)
                {
                    transactionAssignments = transactionAssignments.Where(
                        p => p.Date.Year <= searchCriteria.ToDateTime.Value.Year &
                            p.Date.Month <= searchCriteria.ToDateTime.Value.Month &
                            p.Date.Day <= searchCriteria.ToDateTime.Value.Day
                        );
                }

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {


                        if (filter.ColumnName == "Confidentiality")
                        {
                            transactionAssignments = SortTextByConfidentialityLevel(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Priority")
                        {
                            transactionAssignments = SortTextByPriorityLevel(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "AssignDate")
                        {
                            transactionAssignments = SortTextByAssignDate(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Number")
                        {
                            transactionAssignments = SortTextByNumber(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Subject")
                        {
                            transactionAssignments = SortTextBySubject(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactionAssignments.Count();

                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "ToEntity")
                            transactionAssignments = OrderByToEntity(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ToUser")
                            transactionAssignments = OrderByToUser(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Status")
                            transactionAssignments = OrderByStatus(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Confidentiality")
                            transactionAssignments = OrderByConfidentialityLevel(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Priority")
                            transactionAssignments = OrderByPriorityLevel(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Number")
                            transactionAssignments = OrderByNumber(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Id")
                            transactionAssignments = OrderById(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Date")
                            transactionAssignments = OrderByDate(transactionAssignments, searchCriteria, orderBy.IsAscending);
                    }
                }
                else
                {
                    transactionAssignments = OrderByNumber(transactionAssignments, searchCriteria, false);
                }


                transactionAssignments = transactionAssignments.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);

                return transactionAssignments.ToList().Select(ts => new TransactionAssignment()
                {
                    Id = ts.Id,
                    DateH = ts.DateH,
                    Date = ts.Date,
                    Viewed = ts.Viewed,
                    ToUser = (ts.ToUser != null) ? new UserProfile
                    {
                        Id = ts.ToUser.Id,
                        LocalName = ts.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    FromUser = new UserProfile
                    {
                        Id = ts.FromUser.Id,
                        LocalName = ts.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    ToEntity = (ts.ToEntity != null) ? new OrgUnit
                    {
                        Id = ts.ToEntity.Id,
                        LocalName = ts.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    FromEntity = (ts.FromEntity != null) ? new OrgUnit
                    {
                        Id = ts.FromEntity.Id,
                        LocalName = ts.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Transaction = new Transaction
                    {
                        Id = ts.Transaction.Id,
                        Date = ts.Transaction.Date,
                        DateH = ts.Transaction.DateH,
                        Priority = new Priority
                        {
                            Id = ts.Transaction.Priority.Id,
                            Text = ts.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        Confidentiality = new Permission
                        {
                            Id = ts.Transaction.Confidentiality.Id,
                            LocalName = ts.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        },
                        Status = new Lookup
                        {
                            Id = ts.Transaction.Status.Id,
                            Text = ts.Transaction.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        TransactionCategory = new Lookup
                        {
                            Id = ts.Transaction.TransactionCategory.Id,
                            Text = ts.Transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        TransactionType = (ts.Transaction.TransactionType != null) ? new TransactionType
                        {
                            Id = ts.Transaction.TransactionType.Id,
                            Text = ts.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                            Color = (ts.Transaction.TransactionType.Color != null) ? new Lookup
                            {
                                Id = ts.Transaction.TransactionType.Color.Id,
                            } : null
                        } : null,
                        Subject = ts.Transaction.Subject,
                        DocumentNumber = ts.Transaction.DocumentNumber,
                        Number = ts.Transaction.Number,
                        RemindDate = ts.Transaction.RemindDate,
                        RemindDateH = ts.Transaction.RemindDateH,
                        HasPermission = ts.Transaction.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : ts.Transaction.Confidentiality.Weight <= UserWeight ? true : false,
                        DeliveryMethodId = ts.Transaction.DeliveryMethodId,
                        HasLinks = ts.Transaction.Links.Any(),
                        ExternalParty = ts.Transaction.ExternalParty,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignment> GetTransactionAssignments(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionAssignment> transactionAssignments =
                    _oMCSDbContext.TransactionAssignments.Where(a => a.TransactionId == transactionId)
                      .Select(a => new
                      {

                          a.Id,
                          a.Date,
                          a.DateH,

                          ToUser = a.ToUser ?? null,
                          TouserId = a.ToUser == null ? -1 : a.ToUser.Id,
                          TouserName = a.ToUser == null ? string.Empty : a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                          FromUser = a.FromUser ?? null,
                          FromuserId = a.FromUser.Id,
                          FromuserName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                          Action = (a.Action ?? null),
                          ActionId = a.Action == null ? -1 : a.Action.Id,
                          ActionName = a.Action == null ? string.Empty : a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                          ActionType = a.Action == null ? null : a.Action.Type,

                          ToEntity = a.ToEntity ?? null,
                          ToEntityId = a.ToEntity.Id,
                          ToEntityName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                          FromEntity = a.FromEntity ?? null,
                          FromEntityId = a.FromEntity.Id,
                          FromEntityName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                          a.TransactionId
                      }
                      ).ToList().Select(a => new TransactionAssignment
                      {
                          Id = a.Id,
                          Date = a.Date,
                          DateH = a.DateH,

                          Action = (a.Action != null) ? new Action
                          {
                              Id = a.ActionId,
                              LocalName = a.ActionName,
                              Type = a.ActionType
                          } : null,

                          ToUser = (a.ToUser != null) ? new UserProfile
                          {
                              Id = a.TouserId,
                              LocalName = a.TouserName
                          } : null,

                          FromUser = (a.FromUser != null) ? new UserProfile
                          {
                              Id = a.FromuserId,
                              LocalName = a.FromuserName
                          } : null,

                          ToEntity = (a.ToEntity != null) ? new OrgUnit
                          {
                              Id = a.ToEntity.Id,
                              LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                          } : null,

                          FromEntity = (a.ToEntity != null) ? new OrgUnit
                          {
                              Id = a.FromEntity.Id,
                              LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                          } : null,

                          TransactionId = a.TransactionId
                      }).ToList();

                return transactionAssignments;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionAssignment GetLastTransactionAssignments(int transactionId, string cultureName)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignments
                                    .OrderByDescending(ta => ta.Id)
                                    .Where(a => a.TransactionId == transactionId)
                                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where, string cultureName)
        {
            try
            {
                IList<TransactionAssignment> transactionAssignments =
                    _oMCSDbContext.TransactionAssignments.Where(@where)
                    .Select(a => new

                    {
                        a.Id,
                        a.Date,
                        a.DateH,
                        a.PhysicalDate,
                        a.PhysicalDateH,
                        a.TransactionId,
                        ToUser = a.ToUser ?? null,
                        TouserId = a.ToUser == null ? -1 : a.ToUser.Id,
                        TouserName = a.ToUser == null ? string.Empty : a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        FromUser = a.FromUser ?? null,
                        FromuserId = a.FromUser.Id,
                        FromuserName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        physicalEntity = a.PhysicalEntity ?? null,
                        physicalEntityId = a.PhysicalEntityId,
                        physicalEntityName = a.PhysicalEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        PhysicalUser = a.PhysicalUser ?? null,
                        a.PhysicalUserId,
                        PhysicalUserName = a.PhysicalUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        Action = (a.Action ?? null),
                        ActionId = a.Action == null ? -1 : a.Action.Id,
                        ActionName = a.Action == null ? string.Empty : a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        ActionType = a.Action == null ? null : a.Action.Type,

                        ToEntity = a.ToEntity ?? null,
                        ToEntityId = a.ToEntity.Id,
                        ToEntityName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        a.Description,


                    }).ToList().Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Date = a.Date,
                        DateH = a.DateH,
                        Description = a.Description,
                        PhysicalDate = a.PhysicalDate,
                        PhysicalDateH = a.PhysicalDateH,
                        PhysicalEntity = (a.physicalEntity != null) ? new OrgUnit
                        {
                            Id = a.physicalEntityId,
                            LocalName = a.physicalEntityName

                        } : null,
                        PhysicalUser = (a.PhysicalUser != null) ? new UserProfile
                        {
                            Id = a.PhysicalUserId,
                            LocalName = a.PhysicalUserName

                        } : null,
                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.ActionId,
                            LocalName = a.ActionName,
                            Type = a.ActionType
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            InternalNumber = a.ToUser.InternalNumber,
                            Id = a.TouserId,
                            LocalName = a.TouserName
                        } : null,

                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            InternalNumber = a.FromUser.InternalNumber,
                            Id = a.FromuserId,
                            LocalName = a.FromuserName
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntityId,
                            LocalName = a.ToEntityName
                        } : null
                    }).ToList();

                return transactionAssignments;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetUserTransactionsByTray(Expression<Func<TransactionAssignment, bool>> where, string cultureName, int? transactionCountToRetrieve = null)
        {
            try
            {
                IQueryable<Transaction> transactions =
                    _oMCSDbContext.TransactionAssignments.Where(where).OrderBy(ts => ts.Date).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);

                transactions = transactions.Take(transactionCountToRetrieve.Value);

                return transactions.ToList().Select(t => new Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    Priority = new Priority
                    {
                        Id = t.Priority.Id,
                        Text = t.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    Confidentiality = new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                    },
                    Status = new Lookup
                    {
                        Id = t.Status.Id,
                        Text = t.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    Subject = t.Subject,
                    DocumentNumber = t.DocumentNumber,
                    Number = t.Number,
                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignment> GetTransactionAssignments(SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<TransactionAssignment> transactionAssignments = _oMCSDbContext.TransactionAssignments.AsQueryable();

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {

                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "TransactionAssignment.ToEntity")
                        {
                            transactionAssignments = SortTextByToEntityAssignment(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "TransactionAssignment.ToUser")
                        {
                            transactionAssignments = SortTextByUserAssignment(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Status")
                        {
                            transactionAssignments = SortTextByStatus(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(Permission).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Confidentiality")
                        {
                            transactionAssignments = SortTextByConfidentialityLevel(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Priority")
                        {
                            transactionAssignments = SortTextByPriorityLevel(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Number")
                        {
                            transactionAssignments = SortTextByNumber(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "DocumentNumber")
                        {
                            transactionAssignments = SortTextByDocumentNumber(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Subject")
                        {
                            transactionAssignments = SortTextBySubject(transactionAssignments, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                    }
                }

                rowsCount = transactionAssignments.Count();
                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "ToEntity")
                            transactionAssignments = OrderByToEntity(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ToUser")
                            transactionAssignments = OrderByToUser(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Status")
                            transactionAssignments = OrderByStatus(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Confidentiality")
                            transactionAssignments = OrderByConfidentialityLevel(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Priority")
                            transactionAssignments = OrderByPriorityLevel(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Number")
                            transactionAssignments = OrderByNumber(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Id")
                            transactionAssignments = OrderById(transactionAssignments, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Date")
                            transactionAssignments = OrderByDate(transactionAssignments, searchCriteria, orderBy.IsAscending);
                    }
                }
                else
                {
                    transactionAssignments = OrderByNumber(transactionAssignments, searchCriteria, false);
                }


                transactionAssignments = transactionAssignments.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);

                return transactionAssignments.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetTransactionsByIds(List<int> TransactionsIds, string CultureName, int? UserWeight, int currentUserId)
        {
            try
            {
                IQueryable<Transaction> transactions = _oMCSDbContext.TransactionAssignments.Where(ta => TransactionsIds.Contains(ta.Transaction.Id)).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);

                IList<Transaction> transactionslist = transactions.ToList().Select(t => new Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    TransactionCategoryId = t.TransactionCategoryId,
                    DeliveryMethodId = t.Assignments[0].DeliveryMethodId,
                    Attachments = t.Attachments,
                    SavedReason = t.SavedReason,
                    ExternalParty = (t.ExternalParty) != null ? new ExternalParty
                    {
                        Id = t.ExternalParty.Id,
                        LocalName = t.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Priority = (t.Priority != null) ? new Priority
                    {
                        Id = t.Priority.Id,
                        Text = t.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Confidentiality = (t.Confidentiality != null) ? new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Status = (t.Status != null) ? new Lookup
                    {
                        Id = t.Status.Id,
                        Text = t.Status.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Subject = t.Subject,
                    DocumentNumber = t.DocumentNumber,
                    TransactionCategory = (t.TransactionCategory != null) ? new Lookup
                    {
                        Id = t.TransactionCategory.Id,
                        Text = t.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    LetterType = (t.LetterType != null) ? new LetterType
                    {
                        Id = t.LetterType.Id,
                        Text = t.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    TransactionType = (t.TransactionType != null) ? new TransactionType
                    {
                        Id = t.TransactionType.Id,
                        Text = t.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText(),
                        Color = (t.TransactionType.Color != null) ? new Lookup
                        {
                            Id = t.TransactionType.Color.Id,
                        } : null
                    } : null,
                    ToUser = (t.ToUser != null) ? new UserProfile
                    {
                        Id = t.ToUser.Id,
                        LocalName = t.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    User = (t.User != null) ? new UserProfile
                    {
                        Id = t.User.Id,
                        LocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Number = t.Number,
                    OrgUnit = (t.OrgUnit != null) ? new OrgUnit
                    {
                        Id = t.OrgUnit.Id,
                        LocalName = t.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText()
                    } : null,
                    Assignments = t.Assignments.Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Action = new Domain.Action
                        {
                            LocalName = (a.Action != null) ? a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText() : null
                        },
                        ActionId = (a.ActionId != null) ? a.ActionId : null,
                        Date = a.Date,
                        DateH = a.DateH,
                        FromEntity = new OrgUnit
                        {
                            LocalName = (a.FromEntity != null) ? a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText() : null
                        },
                        FromEntityId = a.FromEntityId,
                        FromUser = new UserProfile
                        {
                            LocalName = (a.FromUser != null) ? a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText() : null
                        },
                        FromUserId = a.FromUserId,
                        ToEntity = new OrgUnit
                        {
                            LocalName = (a.ToEntity != null) ? a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText() : null
                        },
                        ToEntityId = (a.ToEntityId),
                        ToUser = new UserProfile
                        {
                            LocalName = (a.ToUser != null) ? a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText() : null
                        },
                        ToUserId = (a.ToUser != null) ? a.ToUser.Id : -1,
                        TransactionId = a.TransactionId
                    }).ToList(),
                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH,
                    StatusId = t.StatusId,
                    RejectionReason = t.RejectionReason,
                    HasPermission = t.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : t.Confidentiality.Weight <= UserWeight ? true : false,


                }).ToList();

                return transactionslist;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetUserTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, int? UserWeight, SearchCriteriaCustom searchCriteria, int currentUserId, out int rowsCount)
        {
            try
            {
                IQueryable<Transaction> transactions = _oMCSDbContext.TransactionAssignments.Include(x => x.Transaction.FollowUp).Where(where).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);
                //if (searchCriteria.IsDeleted)
                //{
                //    transactions = _oMCSDbContext.TransactionAssignments.Where(where).Select(t => t.Transaction).Where(tr => tr.IsDeleted);
                //}

                if (searchCriteria.SearchColunms != null && searchCriteria.SearchColunms.Count > 0)
                {
                    foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                    {
                        if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(searchColunm.ColunmName).PropertyType) & searchColunm.ColunmName == "Number")
                        {
                            transactions = transactions.Where(p => p.Number.ToString().Equals(searchColunm.ColunmValue));
                        }
                    }
                }

                if (searchCriteria.FromDateTime.HasValue)
                {
                    transactions = transactions.Where(p => p.Date >= searchCriteria.FromDateTime.Value);
                }

                if (searchCriteria.ToDateTime.HasValue)
                {
                    transactions = transactions.Where(p => p.Date <= searchCriteria.ToDateTime.Value);
                }

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (filter.Value == "-1")
                        {
                            continue;
                        }
                        if (filter.ColumnName == "ToEntity" || filter.ColumnName == "FromEntity")
                        {
                            if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "ToEntity")
                            {
                                transactions = SortTextByToEntity(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                            }

                            else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "FromEntity")
                            {
                                transactions = SortTextByFromEntity(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                        }
                        else if (filter.ColumnName == "FollowUpCreatedOn")
                        {
                            transactions = SortTextByFollowUpCreatedOn(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "UserId")
                        {
                            transactions = SortTextByUserId(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "ToUser")
                        {
                            transactions = SortTextByToUser(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "SourceType")
                        {
                            transactions = SortTextByTransactionType(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Status" && typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByStatus(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Confidentiality" && typeof(Permission).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByConfidentialityLevel(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Priority" && typeof(Priority).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByPriorityLevel(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Number" && typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByNumber(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "DocumentNumber" && typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByDocumentNumber(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "ReminderDate" && typeof(DateTime).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByReminderDate(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Subject" && typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextBySubject(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "ExternalParty" && typeof(ExternalParty).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByExternalParty(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "LetterType" && typeof(LetterType).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByLetterType(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Date" && typeof(DateTime).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByDate(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Id" && typeof(int).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextById(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "TransactionType")
                        {
                            transactions = SortTextByTransactionCategory(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        if (filter.ColumnName == "FromDateTime")
                        {

                            transactions = SortTextByFromDate(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        if (filter.ColumnName == "ToDateTime")
                        {
                            transactions = SortTextByToDateTime(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "PrivecyId" && typeof(int).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByPrivecy(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "ActionId" && typeof(int?).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByActionId(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "FromEntityId" && typeof(int?).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType))
                        {
                            transactions = SortTextByFromOrgUnitId(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactions.Count();


                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "ToEntity")
                            transactions = OrderByToEntity(transactions, searchCriteria, orderBy.IsAscending);
                        else if (orderBy.ColumnName == "FromEntity")
                            transactions = OrderByFromEntity(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ToUser")
                            transactions = OrderByToUser(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Status")
                            transactions = OrderByStatus(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Confidentiality")
                            transactions = OrderByConfidentialityLevel(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Priority")
                            transactions = OrderByPriorityLevel(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Number")
                            transactions = OrderByNumber(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Id")
                            transactions = OrderById(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Date")
                            transactions = OrderByDate(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ExternalParty")
                            transactions = OrderByExternalParty(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "DocumentType")
                            transactions = OrderByDocumentType(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "SourceType")
                            transactions = OrderBySourceType(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "FollowUpCreatedOn")
                            transactions = OrderByFollowUpCreatedOn(transactions, searchCriteria, orderBy.IsAscending);
                        else if (orderBy.ColumnName == "AssignDate")
                            transactions = OrderByAssignDate(transactions, searchCriteria, orderBy.IsAscending);
                    }
                }
                else
                {
                    transactions = OrderByDate(transactions, searchCriteria, false);
                }


                transactions = transactions.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);

                DateTime before = DateTime.Now;
                int taskStutsID = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, searchCriteria.CultureName);
                IList<Transaction> transactionslist = transactions.Include(x => x.FollowUp).ToList().Select(t => new Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    TransactionCategoryId = t.TransactionCategoryId,
                    DeliveryMethodId = t.DeliveryMethodId,
                    IsDeleted = t.IsDeleted,
                    Attachments = t.Attachments,
                    SavedReason = t.SavedReason,
                    IsForIndividual = t.IsForIndividual,
                    IsPresentationDraft = t.IsPresentationDraft,
                    IsElcOutBound = t.IsElcOutBound,
                    NeedAcknowled = t.NeedAcknowled,
                    Encrypted = t.Encrypted,
                    ProcessPeriodTransaction = t.ProcessPeriodTransaction.ToString() == null ? 0 : t.ProcessPeriodTransaction,
                    ExternalParty = (t.ExternalParty) != null ? new ExternalParty
                    {
                        Id = t.ExternalParty.Id,
                        LocalName = t.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        YasserRegistered = t.ExternalParty.YasserRegistered
                    } : null,
                    Priority = (t.Priority != null) ? new Priority
                    {
                        Id = t.Priority.Id,
                        Text = t.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Confidentiality = (t.Confidentiality != null) ? new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Status = (t.Status != null) ? new Lookup
                    {
                        Id = t.Status.Id,
                        Text = t.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Subject = t.Subject,
                    DocumentNumber = t.DocumentNumber,
                    TransactionCategory = (t.TransactionCategory != null) ? new Lookup
                    {
                        Id = t.TransactionCategory.Id,
                        Text = t.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    LetterType = (t.LetterType != null) ? new LetterType
                    {
                        Id = t.LetterType.Id,
                        Text = t.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    TransactionType = (t.TransactionType != null) ? new TransactionType
                    {
                        Id = t.TransactionType.Id,
                        Text = t.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        Color = (t.TransactionType.Color != null) ? new Lookup
                        {
                            Id = t.TransactionType.Color.Id,
                        } : null
                    } : null,
                    ToUser = (t.ToUser != null) ? new UserProfile
                    {
                        Id = t.ToUser.Id,
                        LocalName = t.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    User = (t.User != null) ? new UserProfile
                    {
                        Id = t.User.Id,
                        LocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Number = t.Number,
                    OrgUnit = (t.OrgUnit != null) ? new OrgUnit
                    {
                        Id = t.OrgUnit.Id,
                        LocalName = t.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    FollowUp = t.FollowUp.Select(f => new TransactionFollowUp
                    {
                        Id = f.Id,
                        DateTo = f.DateTo,
                        DateToH = f.DateToH,
                        FollowUpUserId = f.FollowUpUserId
                    }).OrderByDescending(f => f.Id).ToList(),
                    //Copies = t.Copies.Select(f => new TransactionCopy
                    //{ 
                    //    IsOpr = f.IsOpr,
                    //    OprEntityId = f.OprEntityId,
                    //    OprEntity = new OrgUnit
                    //    {
                    //        LocalName = (f.OprEntity != null) ? f.OprEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                    //    }
                    //}).OrderByDescending(f => f.Id).ToList(),
                    Assignments = t.Assignments.Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Action = new Domain.Action
                        {
                            LocalName = (a.Action != null) ? a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ActionId = (a.ActionId != null) ? a.ActionId : null,
                        Date = a.Date,
                        DateH = a.DateH,
                        FromEntity = new OrgUnit
                        {
                            LocalName = (a.FromEntity != null) ? a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        FromEntityId = a.FromEntityId,
                        FromUser = new UserProfile
                        {
                            LocalName = (a.FromUser != null) ? a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        FromUserId = a.FromUserId,
                        ToEntity = new OrgUnit
                        {
                            LocalName = (a.ToEntity != null) ? a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ToEntityId = (a.ToEntityId),
                        ToUser = new UserProfile
                        {
                            LocalName = (a.ToUser != null) ? a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ToUserId = (a.ToUser != null) ? a.ToUser.Id : -1,
                        TransactionId = a.TransactionId,
                        Viewed = a.Viewed,
                        Description = a.Description,
                        TransactionPathId = a.TransactionPathId.HasValue ? a.CurrentPathStep.HasValue ? a.CurrentPathStep == GetTransactionPathCount(a.TransactionPathId.Value) ? null : a.TransactionPathId : a.TransactionPathId : a.TransactionPathId,
                        DeliveryMethodId = a.DeliveryMethodId,
                        DeliveryMethod = (a.DeliveryMethod != null) ? new Lookup
                        {
                            Id = a.DeliveryMethod.Id,
                            Text = a.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                    }).ToList(),

                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH,
                    StatusId = t.StatusId,
                    RejectionReason = t.RejectionReason,
                    HasPermission = t.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : t.Confidentiality.Weight <= UserWeight ? true : false,
                    HasLinks = t.Links.Any(),
                    Privecy = (t.Privecy != null) ? new SpecificLevel
                    {
                        Id = t.Privecy.Id,
                        Text = t.Privecy.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    IsImportant = t.FollowUp != null && t.FollowUp.Any(x => x.IsImportant)

                }).ToList();
                DateTime after = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine("Duration: " + (after - before).TotalMilliseconds);
                return transactionslist;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Transaction> GetTransactionByUsername(Expression<Func<TransactionAssignment, bool>> where, BaseSearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Transaction> transactions = _oMCSDbContext.TransactionAssignments.Include(x => x.Transaction.FollowUp).Where(where).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                rowsCount = transactions.Count();
                transactions = transactions.OrderByDescending(x => x.CreatedOn).Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                              .Take(searchCriteria.PageSize); DateTime before = DateTime.Now;
                int taskStutsID = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, searchCriteria.CultureName);
                IList<Transaction> transactionslist = transactions.Include(x => x.FollowUp).ToList().Select(t => new Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    TransactionCategoryId = t.TransactionCategoryId,
                    DeliveryMethodId = t.DeliveryMethodId,
                    IsDeleted = t.IsDeleted,
                    Attachments = t.Attachments,
                    SavedReason = t.SavedReason,
                    IsForIndividual = t.IsForIndividual,
                    IsPresentationDraft = t.IsPresentationDraft,
                    IsElcOutBound = t.IsElcOutBound,
                    NeedAcknowled = t.NeedAcknowled,
                    ProcessPeriodTransaction = t.ProcessPeriodTransaction.ToString() == null ? 0 : t.ProcessPeriodTransaction,

                    Confidentiality = (t.Confidentiality != null) ? new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,

                    Subject = t.Subject,
                    DocumentNumber = t.DocumentNumber,
                    TransactionCategory = (t.TransactionCategory != null) ? new Lookup
                    {
                        Id = t.TransactionCategory.Id,
                        Text = t.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,

                    TransactionType = (t.TransactionType != null) ? new TransactionType
                    {
                        Id = t.TransactionType.Id,
                        Text = t.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        Color = (t.TransactionType.Color != null) ? new Lookup
                        {
                            Id = t.TransactionType.Color.Id,
                        } : null
                    } : null,

                    User = (t.User != null) ? new UserProfile
                    {
                        Id = t.User.Id,
                        LocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Number = t.Number,
                    OrgUnit = (t.OrgUnit != null) ? new OrgUnit
                    {
                        Id = t.OrgUnit.Id,
                        LocalName = t.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,

                    Assignments = t.Assignments.Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Action = new Domain.Action
                        {
                            LocalName = (a.Action != null) ? a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ActionId = (a.ActionId != null) ? a.ActionId : null,
                        Date = a.Date,
                        DateH = a.DateH,
                        FromEntity = new OrgUnit
                        {
                            LocalName = (a.FromEntity != null) ? a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        FromEntityId = a.FromEntityId,
                        FromUser = new UserProfile
                        {
                            LocalName = (a.FromUser != null) ? a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        FromUserId = a.FromUserId,
                        ToEntity = new OrgUnit
                        {
                            LocalName = (a.ToEntity != null) ? a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ToEntityId = (a.ToEntityId),
                        ToUser = new UserProfile
                        {
                            LocalName = (a.ToUser != null) ? a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                        },
                        ToUserId = (a.ToUser != null) ? a.ToUser.Id : -1,
                        TransactionId = a.TransactionId,
                        Viewed = a.Viewed,
                        Description = a.Description,
                        TransactionPathId = a.TransactionPathId.HasValue ? a.CurrentPathStep.HasValue ? a.CurrentPathStep == GetTransactionPathCount(a.TransactionPathId.Value) ? null : a.TransactionPathId : a.TransactionPathId : a.TransactionPathId,
                        DeliveryMethodId = a.DeliveryMethodId,
                        DeliveryMethod = (a.DeliveryMethod != null) ? new Lookup
                        {
                            Id = a.DeliveryMethod.Id,
                            Text = a.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                    }).ToList(),

                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH,
                    StatusId = t.StatusId,
                    RejectionReason = t.RejectionReason,

                    Privecy = (t.Privecy != null) ? new SpecificLevel
                    {
                        Id = t.Privecy.Id,
                        Text = t.Privecy.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,

                }).ToList();
                DateTime after = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine("Duration: " + (after - before).TotalMilliseconds);
                return transactionslist;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetNextTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                DateTime before = DateTime.Now;
                IQueryable<Transaction> transactions = _oMCSDbContext.TransactionAssignments.Where(where).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);
                //if (searchCriteria.IsDeleted)
                //{
                //    transactions = _oMCSDbContext.TransactionAssignments.Where(where).Select(t => t.Transaction).Where(tr => tr.IsDeleted);
                //}

                if (searchCriteria.SearchColunms != null && searchCriteria.SearchColunms.Count > 0)
                {
                    foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                    {
                        if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(searchColunm.ColunmName).PropertyType) & searchColunm.ColunmName == "Number")
                        {
                            transactions = transactions.Where(p => p.Number.ToString().Equals(searchColunm.ColunmValue));
                        }
                    }
                }
                transactions = OrderByDate(transactions, searchCriteria, false);
                var nextTransaction = transactions.FirstOrDefault();

                DateTime after = DateTime.Now;

                //System.Diagnostics.Debug.WriteLine("Duration: " + (after - before).TotalMilliseconds);
                return nextTransaction;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        private IQueryable<TransactionAssignment> SortTextBySubject(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }


        private IQueryable<TransactionAssignment> SortTextByUserAssignment(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByToEntityAssignment(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByDocumentNumber(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByNumber(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                long Number = Convert.ToInt64(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.Equals(Number));
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.Transaction.Number.ToString().Contains(textValue));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.Transaction.Number.ToString().EndsWith(textValue));
                    case FilterType.StartsWith:
                        return source.Where(p => p.Transaction.Number.ToString().StartsWith(textValue));
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.ToString().Equals(textValue));
                }
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByPriorityLevel(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(t => t.Transaction.Priority.Id == id);
            }

            return source;
        }
        private IQueryable<TransactionAssignment> SortTextByAssignDate(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                var dateTime = DateTime.ParseExact(textValue, "d/M/yyyy", null);

                return source.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(dateTime));
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByConfidentialityLevel(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.Confidentiality.Id == id);
            }

            return source;
        }

        private IQueryable<TransactionAssignment> SortTextByStatus(IQueryable<TransactionAssignment> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByToEntity(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            try
            {
                if (textValue != "" && textValue != null)
                {
                    var id = Convert.ToInt32(textValue);
                    //where = ExpressionUtility.AndAlso(where, dr => dr.ToEntity.Id == id);
                    return source.Where(dr => dr.Assignments.All(a => a.ToEntityId == id));
                }
                return source;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private IQueryable<Transaction> SortTextByUserId(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            try
            {
                if (textValue != "" && textValue != null)
                {
                    var id = Convert.ToInt32(textValue);
                    //where = ExpressionUtility.AndAlso(where, dr => dr.ToUser.Id == id);
                    return source.Where(dr => dr.Assignments.All(a => a.ToUserId == id));
                }
                return source;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private IQueryable<Transaction> SortTextByFromEntity(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            try
            {
                if (textValue != "" && textValue != null)
                {
                    var id = Convert.ToInt32(textValue);
                    //where = ExpressionUtility.AndAlso(where, dr => dr.FromEntity.Id == id);
                    return source.Where(dr => dr.Assignments.All(a => a.FromEntityId == id));
                }
                return source;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private IQueryable<Transaction> SortTextByToUser(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByStatus(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByTransactionCategory(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.TransactionCategoryId == id);
            }

            return source;
        }
        private IQueryable<Transaction> SortTextByTransactionType(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != "" && textValue != null)
            {
                var id = Convert.ToInt32(textValue);
                //where = ExpressionUtility.AndAlso(where, dr => dr.Transaction.TransactionTypeId == id);
                return source.Where(dr => dr.TransactionTypeId == id);
            }
            return source;
        }
        private IQueryable<Transaction> SortTextByConfidentialityLevel(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != "" && textValue != null)
            {
                var id = Convert.ToInt32(textValue);
                // where = ExpressionUtility.AndAlso(where, dr => dr.Transaction.ConfidentialityId == id);
                return source.Where(t => t.ConfidentialityId == id);
            }
            return source;
        }

        private IQueryable<Transaction> SortTextByPriorityLevel(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != "" && textValue != null)
            {
                var id = Convert.ToInt32(textValue);
                //where = ExpressionUtility.AndAlso(where, dr => dr.Transaction.PriorityId == id);
                return source.Where(t => t.PriorityId == id);

            }
            return source;
        }

        private IQueryable<Transaction> SortTextByNumber(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            try
            {
                if (textValue != "" && textValue != null)
                {
                    long Number = Convert.ToInt64(textValue);
                    //where = ExpressionUtility.AndAlso(where, p => p.Transaction.Number.Equals(Number)); 
                    return source.Where(t => t.Number == Number);
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private IQueryable<Transaction> SortTextByExternalParty(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                int externalPartyNumber = Convert.ToInt32(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.ExternalPartyId == externalPartyNumber);
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.ExternalPartyId.ToString().ToLower().Contains(textValue.ToLower()));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.ExternalPartyId.ToString().ToLower().EndsWith(textValue.ToLower()));
                    case FilterType.StartsWith:
                        return source.Where(p => p.ExternalPartyId.ToString().ToLower().StartsWith(textValue.ToLower()));
                    case FilterType.Equals:
                        return source.Where(p => p.ExternalPartyId.ToString().ToLower().Equals(textValue.ToLower()));
                }
            }

            return source;
        }
        private IQueryable<Transaction> SortTextByLetterType(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {

            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                int LetterTypeNumber = Convert.ToInt32(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.LetterTypeId.Value.Equals(LetterTypeNumber));
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.LetterTypeId.ToString().ToLower().Contains(textValue.ToLower()));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.LetterTypeId.ToString().ToLower().EndsWith(textValue.ToLower()));
                    case FilterType.StartsWith:
                        return source.Where(p => p.LetterTypeId.ToString().ToLower().StartsWith(textValue.ToLower()));
                    case FilterType.Equals:
                        return source.Where(p => p.LetterTypeId.ToString().ToLower().Equals(textValue.ToLower()));
                }
            }

            return source;
        }
        private IQueryable<Transaction> SortTextByDate(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                if (textValue != "" && textValue != null)
                {
                    var dateTime = DateTime.ParseExact(textValue, "d/M/yyyy", null);
                    //where = ExpressionUtility.AndAlso(where, p => DbFunctions.TruncateTime(p.Transaction.Date) == DbFunctions.TruncateTime(dateTime));
                    return source.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(dateTime));
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IQueryable<Transaction> SortTextByToDateTime(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var list = textValue.Split('/').ToList().Select(f => int.Parse(f)).ToList();
                DateTime dt = new DateTime(list[2], list[1], list[0]);
                return source.Where(p => p.Date <= dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IQueryable<Transaction> SortTextByFromDate(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var list = textValue.Split('/').ToList().Select(f => int.Parse(f)).ToList();
                DateTime dt = new DateTime(list[2], list[1], list[0]);
                return source.Where(p => p.Date >= dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IQueryable<Transaction> SortTextById(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            var id = Convert.ToInt32(textValue);
            if (filterType == FilterType.Equals)
            {
                return source.Where(p => p.Id == id);
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByFollowUpCreatedOn(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            var dateTime = DateTime.ParseExact(textValue, "d/M/yyyy", null);
            if (filterType == FilterType.Equals)
            {
                return source.Where(p => p.FollowUp.Any(f => DbFunctions.TruncateTime(f.CreatedOn) == DbFunctions.TruncateTime(dateTime)));
            }

            return source;
        }
        private IQueryable<Transaction> SortTextByDocumentNumber(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                int DocumentNumber = Convert.ToInt32(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.DocumentNumber.Equals(DocumentNumber));
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.DocumentNumber.ToString().ToLower().Contains(textValue.ToLower()));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.DocumentNumber.ToString().ToLower().EndsWith(textValue.ToLower()));
                    case FilterType.StartsWith:
                        return source.Where(p => p.DocumentNumber.ToString().ToLower().StartsWith(textValue.ToLower()));
                    case FilterType.Equals:
                        return source.Where(p => p.DocumentNumber.ToString().ToLower().Equals(textValue.ToLower()));
                }
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByReminderDate(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.RemindDate.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.RemindDate.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.RemindDate.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.RemindDate.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }
        private IQueryable<TransactionAssignment> OrderByStatus(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> SortTextBySubject(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Subject.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Subject.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Subject.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Subject.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignment> OrderByToEntity(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ToEntity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ToEntity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByToEntity(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Entity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Entity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByFromEntity(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (searchCriteria.SearchData != 0)
            {
                return source.SmartOrderBy(p => p.OrgUnitId == searchCriteria.SearchData);
            }
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.OrgUnitId);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.OrgUnitId);
            }

            return source;
        }
        private IQueryable<TransactionAssignment> OrderByToUser(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByToUser(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }


        private IQueryable<Transaction> OrderByStatus(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByConfidentialityLevel(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }
            if (searchCriteria.SearchData != 0)
            {
                source = source.SmartOrderByDescending(p => p.ConfidentialityId == searchCriteria.SearchData);
            }

            return source;
        }

        private IQueryable<TransactionAssignment> OrderByConfidentialityLevel(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByPriorityLevel(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }

            if (searchCriteria.SearchData != 0)
            {
                source = source.SmartOrderByDescending(p => p.Priority.Id == searchCriteria.SearchData);

            }
            return source;
        }

        private IQueryable<TransactionAssignment> OrderByPriorityLevel(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }
        private IQueryable<Transaction> OrderByNumber(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Number);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Number);
            }

            return source;
        }

        private IQueryable<TransactionAssignment> OrderByNumber(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Number);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Number);
            }

            return source;
        }

        private IQueryable<TransactionAssignment> OrderById(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Id);
            }

            return source;
        }

        private IQueryable<Transaction> OrderByDate(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Date);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Date);
            }

            return source;
        }
        private IQueryable<Transaction> OrderByExternalParty(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            if (searchCriteria.SearchData != 0)
            {
                source = source.SmartOrderByDescending(p => p.ExternalPartyId == searchCriteria.SearchData);
            }
            return source;
        }
        private IQueryable<Transaction> OrderByDocumentType(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.TransactionCategoryId);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.TransactionCategoryId);
            }
            if (searchCriteria.SearchData != 0)
            {
                source = source.SmartOrderByDescending(p => p.TransactionCategoryId == searchCriteria.SearchData);
            }
            return source;
        }
        private IQueryable<Transaction> OrderBySourceType(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            if (searchCriteria.SearchData != 0)
            {
                source = source.SmartOrderByDescending(p => p.TransactionTypeId == searchCriteria.SearchData);
            }
            return source;
        }
        private IQueryable<Transaction> OrderByFollowUpCreatedOn(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(s => s.FollowUp.FirstOrDefault(f => f.FollowUpUserId == searchCriteria.UserId).CreatedOn);
            }
            else
            {
                source = source.SmartOrderByDescending(s => s.FollowUp.FirstOrDefault(f => f.FollowUpUserId == searchCriteria.UserId).CreatedOn);
            }
            return source;
        }

        private IQueryable<TransactionAssignment> OrderByDate(IQueryable<TransactionAssignment> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Date);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Date);
            }

            return source;
        }

        private IQueryable<Transaction> OrderById(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Id);
            }

            return source;
        }

        public void SetTransactionAssignmentToViewedByTransactionId(int transactionId)
        {
            try
            {
                var transactionAssignment = _oMCSDbContext.TransactionAssignments.FirstOrDefault(a => a.TransactionId == transactionId);
                transactionAssignment.Viewed = true;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionPathDetails GetTransactionPathNextStep(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionPathDetails> transactionPath =
                                                        (from transAssign in _oMCSDbContext.TransactionAssignments
                                                         join transPath in _oMCSDbContext.TransactionPaths on transAssign.TransactionPathId equals transPath.Id
                                                         join pathDetails in _oMCSDbContext.TransactionPathDetails on transPath.Id equals pathDetails.TransactionPathId
                                                         where (transAssign.TransactionId == transactionId
                                                                && ((transAssign.CurrentPathStep != null && (transAssign.CurrentPathStep + 1) <= transPath.TransactionPathDetails.Count
                                                                    && (transAssign.CurrentPathStep + 1) == pathDetails.Sort) || (transAssign.CurrentPathStep == null && pathDetails.Sort == 1)))
                                                         select new
                                                         {
                                                             pathDetails.Id,
                                                             pathDetails.ActionId,
                                                             pathDetails.OrgUnitId,
                                                             pathDetails.TransactionPathId,
                                                             pathDetails.UserId,
                                                             pathDetails.Sort,
                                                             User = pathDetails.UserId.HasValue ? new
                                                             {
                                                                 pathDetails.User.Id,
                                                                 LocalName = pathDetails.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                             } : null,
                                                             OrgUnit = new
                                                             {
                                                                 pathDetails.OrgUnit.Id,
                                                                 LocalName = pathDetails.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                             },
                                                             Action = new
                                                             {
                                                                 pathDetails.Action.Id,
                                                                 LocalName = pathDetails.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                             }
                                                         }).ToList().Select(d => new TransactionPathDetails
                                                         {
                                                             Id = d.Id,
                                                             ActionId = d.ActionId,
                                                             OrgUnitId = d.OrgUnitId,
                                                             TransactionPathId = d.TransactionPathId,
                                                             UserId = d.UserId,
                                                             Sort = d.Sort,
                                                             User = d.UserId.HasValue ? new UserProfile
                                                             {
                                                                 Id = d.User.Id,
                                                                 LocalName = d.User.LocalName
                                                             } : null,
                                                             OrgUnit = new OrgUnit
                                                             {
                                                                 Id = d.OrgUnit.Id,
                                                                 LocalName = d.OrgUnit.LocalName
                                                             },
                                                             Action = new Action
                                                             {
                                                                 Id = d.Action.Id,
                                                                 LocalName = d.Action.LocalName
                                                             }
                                                         }).ToList();
                return transactionPath.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int GetTransactionPathCount(int pathId, bool excludeEntity = false)
        {
            try
            {
                if (excludeEntity)
                {
                    return _oMCSDbContext.TransactionPathDetails
                                        .Where(p => (p.TransactionPathId == pathId)
                                                    && (p.UserId.HasValue))
                                        .Select(r => new { r.Id }).Count();
                }
                return _oMCSDbContext.TransactionPathDetails
                                    .Where(p => (p.TransactionPathId == pathId))
                                    .Select(r => new { r.Id }).Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        private IQueryable<Transaction> SortTextByPrivecy(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                if (textValue != "" && textValue != null)
                {
                    int privecyId = Convert.ToInt32(textValue);
                    return source.Where(t => t.PrivecyId == privecyId);
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<Transaction> SortTextByActionId(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                if (textValue != "" && textValue != null)
                {
                    int actionId = Convert.ToInt32(textValue);
                    if (actionId != 0)
                    {
                        return source.Where(dr => dr.Assignments.All(p => p.ActionId == actionId));
                    }
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IQueryable<Transaction> OrderByAssignDate(IQueryable<Transaction> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(s => s.Assignments.FirstOrDefault(f => f.ToUser.Id == searchCriteria.UserId).Date);
            }
            else
            {
                source = source.SmartOrderByDescending(s => s.Assignments.FirstOrDefault(f => f.ToUser.Id == searchCriteria.UserId).Date);
            }

            return source;
        }

        private IQueryable<Transaction> SortTextByFromOrgUnitId(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                if (textValue != "" && textValue != null)
                {
                    int Id = Convert.ToInt32(textValue);
                    return source.Where(dr => dr.Assignments.All(p => p.FromEntityId == Id));
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria, out int rowsCount, int? UserWeight, int currentUserId)
        {
            try
            {
                IQueryable<TransactionAssignment> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignments.Where(where).GroupBy(t => t.TransactionId).Select(t => t.OrderByDescending(d => d.Id).FirstOrDefault());


                if (searchCriteria.SearchColunms != null && searchCriteria.SearchColunms.Count > 0)
                {
                    foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                    {
                        if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(searchColunm.ColunmName).PropertyType) & searchColunm.ColunmName == "Number")
                        {
                            transactionAssignmentHistories = transactionAssignmentHistories.Where(p => p.Transaction.Number.ToString().Equals(searchColunm.ColunmValue));
                        }
                    }
                }

                if (searchCriteria.FromDateTime.HasValue)
                {
                    transactionAssignmentHistories = transactionAssignmentHistories.Where(
                        p => p.Date >= searchCriteria.FromDateTime.Value
                        );
                }

                if (searchCriteria.ToDateTime.HasValue)
                {
                    transactionAssignmentHistories = transactionAssignmentHistories.Where(
                        p => p.Date.Year <= searchCriteria.ToDateTime.Value.Year &
                            p.Date.Month <= searchCriteria.ToDateTime.Value.Month &
                            p.Date.Day <= searchCriteria.ToDateTime.Value.Day
                        );
                }

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {


                        if (filter.ColumnName == "Confidentiality")
                        {
                            transactionAssignmentHistories = SortTextByConfidentialityLevel(transactionAssignmentHistories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Priority")
                        {
                            transactionAssignmentHistories = SortTextByPriorityLevel(transactionAssignmentHistories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "AssignDate")
                        {
                            transactionAssignmentHistories = SortTextByAssignDate(transactionAssignmentHistories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Number")
                        {
                            transactionAssignmentHistories = SortTextByNumber(transactionAssignmentHistories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "Subject")
                        {
                            transactionAssignmentHistories = SortTextBySubject(transactionAssignmentHistories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactionAssignmentHistories.Count();

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values    
                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "ToEntity")
                            transactionAssignmentHistories = OrderByToEntity(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ToUser")
                            transactionAssignmentHistories = OrderByToUser(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Status")
                            transactionAssignmentHistories = OrderByStatus(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Confidentiality")
                            transactionAssignmentHistories = OrderByConfidentialityLevel(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Priority")
                            transactionAssignmentHistories = OrderByPriorityLevel(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Number")
                            transactionAssignmentHistories = OrderByNumber(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Id")
                            transactionAssignmentHistories = OrderById(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Date")
                            transactionAssignmentHistories = OrderByDate(transactionAssignmentHistories, searchCriteria, orderBy.IsAscending);
                    }
                }
                else
                {
                    transactionAssignmentHistories = OrderByNumber(transactionAssignmentHistories, searchCriteria, false);
                }

                transactionAssignmentHistories = transactionAssignmentHistories.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);

                return transactionAssignmentHistories.ToList().Select(ts => new TransactionAssignment()
                {
                    Id = ts.Id,
                    DateH = ts.DateH,
                    Date = ts.Date,
                    Viewed = ts.Viewed,
                    ToUser = (ts.ToUser != null) ? new UserProfile
                    {
                        Id = ts.ToUser.Id,
                        LocalName = ts.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    FromUser = new UserProfile
                    {
                        Id = ts.FromUser.Id,
                        LocalName = ts.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    ToEntity = (ts.ToEntity != null) ? new OrgUnit
                    {
                        Id = ts.ToEntity.Id,
                        LocalName = ts.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    FromEntity = (ts.FromEntity != null) ? new OrgUnit
                    {
                        Id = ts.FromEntity.Id,
                        LocalName = ts.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    } : null,
                    Transaction = new Transaction
                    {
                        Id = ts.Transaction.Id,
                        Date = ts.Transaction.Date,
                        DateH = ts.Transaction.DateH,
                        Priority = new Priority
                        {
                            Id = ts.Transaction.Priority.Id,
                            Text = ts.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        Confidentiality = new Permission
                        {
                            Id = ts.Transaction.Confidentiality.Id,
                            LocalName = ts.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        },
                        Status = new Lookup
                        {
                            Id = ts.Transaction.Status.Id,
                            Text = ts.Transaction.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        TransactionCategory = new Lookup
                        {
                            Id = ts.Transaction.TransactionCategory.Id,
                            Text = ts.Transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        },
                        TransactionType = (ts.Transaction.TransactionType != null) ? new TransactionType
                        {
                            Id = ts.Transaction.TransactionType.Id,
                            Text = ts.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                            Color = (ts.Transaction.TransactionType.Color != null) ? new Lookup
                            {
                                Id = ts.Transaction.TransactionType.Color.Id,
                            } : null
                        } : null,
                        Subject = ts.Transaction.Subject,
                        DocumentNumber = ts.Transaction.DocumentNumber,
                        Number = ts.Transaction.Number,
                        RemindDate = ts.Transaction.RemindDate,
                        RemindDateH = ts.Transaction.RemindDateH,
                        HasPermission = ts.Transaction.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : ts.Transaction.Confidentiality.Weight <= UserWeight ? true : false,
                        DeliveryMethodId = ts.Transaction.DeliveryMethodId,
                        HasLinks = ts.Transaction.Links.Any(),
                        ExternalParty = ts.Transaction.ExternalParty,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<TransactionAssignmentHistory> SortTextBySubject(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }


        private IQueryable<TransactionAssignmentHistory> SortTextByUserAssignment(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByToEntityAssignment(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.ToEntity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByDocumentNumber(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByNumber(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                long Number = Convert.ToInt64(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.Equals(Number));
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.Transaction.Number.ToString().Contains(textValue));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.Transaction.Number.ToString().EndsWith(textValue));
                    case FilterType.StartsWith:
                        return source.Where(p => p.Transaction.Number.ToString().StartsWith(textValue));
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.ToString().Equals(textValue));
                }
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByPriorityLevel(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(t => t.Transaction.Priority.Id == id);
            }

            return source;
        }
        private IQueryable<TransactionAssignmentHistory> SortTextByAssignDate(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                var dateTime = DateTime.ParseExact(textValue, "d/M/yyyy", null);

                return source.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(dateTime));
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByConfidentialityLevel(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.Confidentiality.Id == id);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> SortTextByStatus(IQueryable<TransactionAssignmentHistory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderByToEntity(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ToEntity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ToEntity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }
        private IQueryable<TransactionAssignmentHistory> OrderByToUser(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderByStatus(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderByConfidentialityLevel(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }


        private IQueryable<TransactionAssignmentHistory> OrderByPriorityLevel(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderByNumber(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Number);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Number);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderById(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Id);
            }

            return source;
        }

        private IQueryable<TransactionAssignmentHistory> OrderByDate(IQueryable<TransactionAssignmentHistory> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Date);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Date);
            }

            return source;
        }

        public int GetTransactionAssignmentHistoryCount(Expression<Func<TransactionAssignment, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignments.Where(@where).Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #region MobileApi

        public List<Transaction> UserMobileGetUserTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, int? UserWeight, FilterCriteria filterCriteria, string cultureName, int currentUserId, bool isAscending = false)
        {
            try
            {
                IQueryable<Transaction> transactions = _oMCSDbContext.TransactionAssignments.Where(where).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);

                if (filterCriteria != null)
                {
                    if (filterCriteria.TransNo != 0)
                    {
                        transactions = transactions.Where(n => n.Number == filterCriteria.TransNo);
                    }

                    if (filterCriteria.Subject != null)
                    {
                        transactions = transactions.Where(s => s.Subject.Contains(filterCriteria.Subject));
                    }

                    if (filterCriteria.TransSource != 0)
                    {
                        transactions = transactions.Where(t => t.TransactionTypeId == filterCriteria.TransSource);
                    }
                }

                List<Transaction> transactionslist = transactions.ToList().Select(t => new Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    DateH = t.DateH,
                    TransactionCategoryId = t.TransactionCategoryId,
                    DeliveryMethodId = t.DeliveryMethodId,
                    IsDeleted = t.IsDeleted,
                    Attachments = t.Attachments,
                    SavedReason = t.SavedReason,
                    ExternalPartyId = t.ExternalPartyId,
                    EntityId = t.EntityId,
                    Entity = (t.Entity != null) ? new OrgUnit
                    {
                        Id = t.Entity.Id,
                        LocalName = t.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ExternalParty = (t.ExternalParty) != null ? new ExternalParty
                    {
                        Id = t.ExternalParty.Id,
                        LocalName = t.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Priority = (t.Priority != null) ? new Priority
                    {
                        Id = t.Priority.Id,
                        Text = t.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Confidentiality = (t.Confidentiality != null) ? new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = t.Confidentiality.Code
                    } : null,
                    Status = (t.Status != null) ? new Lookup
                    {
                        Id = t.Status.Id,
                        Text = t.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Subject = t.Subject,
                    DocumentNumber = t.DocumentNumber,
                    TransactionCategory = (t.TransactionCategory != null) ? new Lookup
                    {
                        Id = t.TransactionCategory.Id,
                        Text = t.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    LetterType = (t.LetterType != null) ? new LetterType
                    {
                        Id = t.LetterType.Id,
                        Text = t.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    TransactionType = (t.TransactionType != null) ? new TransactionType
                    {
                        Id = t.TransactionType.Id,
                        Text = t.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Color = (t.TransactionType.Color != null) ? new Lookup
                        {
                            Id = t.TransactionType.Color.Id,
                            Text = t.TransactionType.Color.Localizations.Where(l => l.Culture.ShortName == "en").LocalText()
                        } : null
                    } : null,
                    ToUser = (t.ToUser != null) ? new UserProfile
                    {
                        Id = t.ToUser.Id,
                        LocalName = t.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    User = (t.User != null) ? new UserProfile
                    {
                        Id = t.User.Id,
                        LocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Number = t.Number,
                    OrgUnit = (t.OrgUnit != null) ? new OrgUnit
                    {
                        Id = t.OrgUnit.Id,
                        LocalName = t.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Assignments = t.Assignments.Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Action = new Domain.Action
                        {
                            LocalName = (a.Action != null) ? a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null
                        },
                        ActionId = (a.ActionId != null) ? a.ActionId : null,
                        Date = a.Date,
                        DateH = a.DateH,
                        FromEntity = new OrgUnit
                        {
                            LocalName = (a.FromEntity != null) ? a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null
                        },
                        FromEntityId = a.FromEntityId,
                        FromUser = new UserProfile
                        {
                            LocalName = (a.FromUser != null) ? a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null
                        },
                        FromUserId = a.FromUserId,
                        ToEntity = new OrgUnit
                        {
                            LocalName = (a.ToEntity != null) ? a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null
                        },
                        ToEntityId = (a.ToEntityId),
                        ToUser = new UserProfile
                        {
                            LocalName = (a.ToUser != null) ? a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null
                        },
                        ToUserId = (a.ToUser != null) ? a.ToUser.Id : -1,
                        TransactionId = a.TransactionId,
                        Viewed = a.Viewed,
                        TransactionPathId = a.TransactionPathId.HasValue ? a.CurrentPathStep.HasValue ? a.CurrentPathStep == GetTransactionPathCount(a.TransactionPathId.Value) ? null : a.TransactionPathId : a.TransactionPathId : a.TransactionPathId
                    }).ToList(),
                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH,
                    StatusId = t.StatusId,
                    RejectionReason = t.RejectionReason,
                    HasPermission = t.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : t.Confidentiality.Weight <= UserWeight ? true : false,
                }).ToList();

                if (isAscending)
                {
                    transactionslist = transactionslist.OrderBy(a => a.Assignments.FirstOrDefault().Date).ToList();
                }
                else
                {
                    transactionslist = transactionslist.OrderByDescending(a => a.Assignments.FirstOrDefault().Date).ToList();
                }

                return transactionslist;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionAssignment GetTransactionAssignment(int transactionId, string cultureName, int? UserWeight)
        {
            try
            {
                TransactionAssignment transactionAssignment = _oMCSDbContext.TransactionAssignments
                                    .OrderByDescending(ta => ta.Id)
                                    .Where(a => a.TransactionId == transactionId && a.Transaction.Confidentiality.Weight <= UserWeight)
                                    .ToList()
                                    .Select(a => new TransactionAssignment
                                    {
                                        DateH = a.DateH,
                                        ToEntity = new OrgUnit
                                        {
                                            LocalName = a.ToEntity?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                        },
                                        ToEntityId = a.ToEntityId,
                                        ToUser = new UserProfile
                                        {
                                            LocalName = a.ToUser?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                        },
                                        ToUserId = a.ToUserId,
                                        PhysicalEntity = new OrgUnit
                                        {
                                            LocalName = a.PhysicalEntity?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                        },
                                        PhysicalUser = new UserProfile
                                        {
                                            LocalName = a.PhysicalUser?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                        }
                                    }).FirstOrDefault();

                return transactionAssignment;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void MoveAllUserTransactions(int UserId)
        {

            List<TransactionAssignment> transactionAssignments = _oMCSDbContext.TransactionAssignments.Where(t => t.ToUserId == UserId).ToList();
            foreach (TransactionAssignment item in transactionAssignments)
            {
                item.TrayId = (int)TrayType.OrgUnit;
                item.ToUserId = null;
            }
            _oMCSDbContext.SaveChanges();

        }

        public void SetCopyAsViewed(int transId, int? toUserId, int toOrgUnit, string ViewdOnDateH)
        {
            try
            {
                Expression<Func<TransactionCopy, bool>> where = null;
                where = (a => a.TransactionId == transId && (toUserId.HasValue ? a.UserId == toUserId && a.EntityId == toOrgUnit : a.EntityId == toOrgUnit));
                TransactionCopy transactionCopy = _oMCSDbContext.TransactionCopies.FirstOrDefault(where);

                transactionCopy.Viewed = true;
                transactionCopy.ViewedById = toUserId.Value;
                transactionCopy.ViewedOnDate = DateTime.Now;
                transactionCopy.ViewedOnDateH = ViewdOnDateH;
                transactionCopy.Status = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion

        #endregion Methods
    }
}

