using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using MCS.Common.TransactionContext;
using MCS.Domain;
using static MCS.Common.UserClaims;
using Action = MCS.Domain.Action;

namespace MCS.DataAccess
{
    public class TransactionAssignmentHistoryRepository : BaseRepository<TransactionAssignmentHistory>, ITransactionAssignmentHistoryRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionAssignmentHistoryRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddTransactionAssignmentHistory(TransactionAssignmentHistory transactionAssignmentHistory)
        {
            try
            {
                _oMCSDbContext.TransactionAssignmentHistories.Add(transactionAssignmentHistory);

                _oMCSDbContext.SaveChanges();

                return transactionAssignmentHistory.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistory(int transactionId, string cultureName, int? userWeight)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignmentHistories
                    .Where(a => a.TransactionId == transactionId && a.Transaction.Confidentiality.Weight <= userWeight)
                    .Select(a => new
                    {
                        a.Description,
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

                        Explanation = (a.Explanation ?? null),
                        ExplanationId = a.Explanation == null ? -1 : a.Explanation.Id,



                        ToEntity = a.ToEntity ?? null,
                        ToEntityId = a.ToEntity.Id,
                        ToEntityName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        FromEntity = a.FromEntity ?? null,
                        FromEntityId = a.FromEntity.Id,
                        FromEntityName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        CreatedOn = a.CreatedOn,
                        Tray = (a.Tray ?? null),
                        TrayId = a.Tray.Id,
                        TrayName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        a.SpecialExplanation,
                        a.GeneralExplanation

                    }).ToList().Select(a => new TransactionAssignmentHistory
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        ExplanationId = a.ExplanationId,
                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            InternalNumber = a.FromUser.InternalNumber,
                            Id = a.TouserId,
                            LocalName = a.TouserName
                        } : null,

                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            InternalNumber = a.FromUser.InternalNumber,
                            Id = a.FromuserId,
                            LocalName = a.FromuserName
                        } : null,

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.ActionId,
                            LocalName = a.ActionName,
                            Type = a.Action.Type
                        } : null,

                        Explanation = (a.Explanation != null) ? new Explanation
                        {
                            Id = a.ExplanationId,
                            Document = (a.Explanation.Document != null) ? new DocumentInfo
                            {
                                Id = a.Explanation.Document.Id,
                                Document = a.Explanation.Document.Document
                            } : null


                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntityId,
                            LocalName = a.FromEntityName
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntityId,
                            LocalName = a.ToEntityName
                        } : null,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.TrayId,
                            LocalName = a.TrayName
                        } : null,
                        GeneralExplanation = a.GeneralExplanation,
                        SpecialExplanation = a.SpecialExplanation,
                        CreatedOn = a.CreatedOn,

                    }).OrderBy(x => x.Id).ToList();
                return transactionAssignmentHistories;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryByTransactionId(int transactionId, int? userWeight)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignmentHistories.Where(a => a.Transaction.Id == transactionId
                && a.Transaction.Confidentiality.Weight <= userWeight).ToList();

                return transactionAssignmentHistories;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionAssignmentHistory GetTransactionAssignmentHistoryById(int assignmentHistoryId, int? userWeight)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignmentHistories.Where(a => a.Id == assignmentHistoryId && a.Transaction.Confidentiality.Weight <= userWeight).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where, int? userWeight)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = (from transactionAssignmentHistory in _oMCSDbContext.TransactionAssignmentHistories.Where(@where)
                                                                                      .Where(a => a.Transaction.Confidentiality.Weight <= userWeight)
                                                                                      select new
                                                                                      {
                                                                                          transactionAssignmentHistory.Id,
                                                                                      }).ToList().Select(t => new TransactionAssignmentHistory
                                                                                      {
                                                                                          Id = t.Id,
                                                                                      }).ToList();

                return transactionAssignmentHistories;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TransactionAssignmentHistory> GetUserMobileTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = (from transactionAssignmentHistory in _oMCSDbContext.TransactionAssignmentHistories.Where(@where)
                                                                                      select new
                                                                                      {
                                                                                          transactionAssignmentHistory.Id,
                                                                                          transactionAssignmentHistory.FromEntityId,
                                                                                          transactionAssignmentHistory.FromUserId
                                                                                      }).ToList().Select(t => new TransactionAssignmentHistory
                                                                                      {
                                                                                          Id = t.Id,
                                                                                          FromEntityId = t.FromEntityId,
                                                                                          FromUserId = t.FromUserId
                                                                                      }).ToList();

                return transactionAssignmentHistories;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionAssignmentHistoryExplanation(int Id, int ExplanationId, int? userWeight)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = GetTransactionAssignmentHistoryByTransactionId(Id, userWeight).OrderBy(a => a.DateH).ToList();
                //Update Last Assignment History Ordered Descending 
                TransactionAssignmentHistory transactionAssignmentHistory = GetTransactionAssignmentHistoryById(transactionAssignmentHistories[transactionAssignmentHistories.Count - 1].Id, userWeight);
                transactionAssignmentHistory.ExplanationId = ExplanationId > 0 ? ExplanationId : (int?)null;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TransactionAssignmentHistory GetLastTransactionAssignmentHistory(int transactionId, int? userWeight, int userId)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignmentHistories.Where(a => a.Transaction.Id == transactionId
                && (a.Transaction.Confidentiality.Weight <= userWeight || a.Transaction.SpecialAuthorizations.Any(t => t.UserProfileId == userId
                && (!t.ExpiredDate.HasValue || t.ExpiredDate > DateTime.Now)))).OrderByDescending(h => h.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName, int? userWeight)
        {
            try
            {
                IList<TransactionAssignmentHistory> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignmentHistories
                    .Where(a => a.Transaction.Id == transactionId && a.Transaction.Confidentiality.Weight <= userWeight)
                    .Select(a => new
                    {
                        a.Description,
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

                        Explanation = (a.Explanation ?? null),
                        ExplanationId = a.Explanation == null ? -1 : a.Explanation.Id,


                        ToEntity = a.ToEntity ?? null,
                        ToEntityId = a.ToEntity.Id,
                        ToEntityName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        FromEntity = a.FromEntity ?? null,
                        FromEntityId = a.FromEntity.Id,
                        FromEntityName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                        Tray = (a.Tray ?? null),
                        TrayId = a.Tray.Id,
                        TrayName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                    }).ToList().Select(a => new TransactionAssignmentHistory
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        ExplanationId = a.ExplanationId,
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

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.ActionId,
                            LocalName = a.ActionName,
                            Type = a.Action.Type
                        } : null,

                        Explanation = (a.Explanation != null) ? new Explanation
                        {
                            Id = a.ExplanationId,
                            Document = (a.Explanation.Document != null) ? new DocumentInfo
                            {
                                Id = a.Explanation.Document.Id,
                                Document = a.Explanation.Document.Document
                            } : null


                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntityId,
                            LocalName = a.FromEntityName
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntityId,
                            LocalName = a.ToEntityName
                        } : null,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.TrayId,
                            LocalName = a.TrayName
                        } : null

                    }).OrderBy(x => x.Date).ToList();
                return transactionAssignmentHistories;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void HideTransactionHistory(int assignmentId)
        {
            try
            {
                TransactionAssignment transactionAssignmentHistory = _oMCSDbContext.TransactionAssignments.FirstOrDefault(ah => ah.Id == assignmentId);

                List<TransactionAssignmentHistory> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignmentHistories.Where(ah => ah.TransactionId == transactionAssignmentHistory.TransactionId &&
                ah.FromEntityId == transactionAssignmentHistory.FromEntityId &&
                ah.FromUserId == transactionAssignmentHistory.FromUserId).ToList();

                foreach (TransactionAssignmentHistory assignmentHistory in transactionAssignmentHistories)
                {
                    assignmentHistory.Viewed = true;
                    assignmentHistory.IsHidden = true;
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void HideTransaction(int transactionId)
        {
            try
            {
                TransactionAssignment transactionAssignment = _oMCSDbContext.TransactionAssignments.FirstOrDefault(ah => ah.TransactionId == transactionId);


                transactionAssignment.Viewed = true;
                transactionAssignment.IsHidden = true;


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void HideTransactionHistories(string assignmentIds)
        {
            try
            {
                List<int> transactionAssignmentsList = assignmentIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => Int32.Parse(x))?.ToList();
                List<TransactionAssignment> transactionAssignments = _oMCSDbContext.TransactionAssignments.Where(ah => transactionAssignmentsList.Any(ta => ta == ah.Id)).ToList();
                foreach (var transactionAssignmentHistory in transactionAssignments)
                {
                    List<TransactionAssignmentHistory> transactionAssignmentHistories = _oMCSDbContext.TransactionAssignmentHistories.Where(ah => ah.TransactionId == transactionAssignmentHistory.TransactionId &&
              ah.FromEntityId == transactionAssignmentHistory.FromEntityId &&
              ah.FromUserId == transactionAssignmentHistory.FromUserId).ToList();

                    foreach (TransactionAssignmentHistory assignmentHistory in transactionAssignmentHistories)
                    {
                        assignmentHistory.Viewed = true;
                        assignmentHistory.IsHidden = true;
                    }
                }


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void HideTransactions(string transactionIds)
        {
            try
            {
                List<int> transactions = transactionIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => Int32.Parse(x))?.ToList();
                List<TransactionAssignment> transactionAssignments = _oMCSDbContext.TransactionAssignments.Where(ah => transactions.Any(t => t == ah.TransactionId)).ToList();
                transactionAssignments.ForEach(x => { x.Viewed = true; x.IsHidden = true; });
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion
    }
}
