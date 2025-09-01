using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using Action = MCS.Domain.Action;

namespace MCS.DataAccess
{
    public class ActionRepository : BaseRepository<Domain.Action>, IActionRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public ActionRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddAction(Domain.Action process)
        {
            try
            {
                _oMCSDbContext.Actions.Add(process);

                _oMCSDbContext.SaveChanges();

                return process.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateAction(Action process)
        {
            try
            {
                Action actionOld = GetActionById(process.Id);

                _oMCSDbContext.Entry(actionOld).CurrentValues.SetValues(process);

                actionOld.Type = process.Type;

                foreach (Localization localization in process.LocalizationIdentifier.Localizations)
                {
                    Localization currentlocalization = actionOld.LocalizationIdentifier.Localizations
                                                                .Where(l => l.Id == localization.Id)
                                                                .FirstOrDefault();

                    if (currentlocalization != null)
                    {
                        _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                    }
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteAction(int id)
        {
            try
            {
                Action action = _oMCSDbContext.Actions.FirstOrDefault(p => p.Id == id);
                if (action != null)
                {
                    _oMCSDbContext.Actions.Remove(action);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfActionUsed(int actionId)
        {
            try
            {
                return (_oMCSDbContext.TransactionAssignmentHistories.FirstOrDefault(a => a.Action.Id == actionId) != null &&
                    _oMCSDbContext.AssignmentPaperActions.FirstOrDefault(a => a.Action.Id == actionId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Action> GetAllActions(string cultureName)
        {
            try
            {
                IList<Action> actions = (from action in _oMCSDbContext.Actions
                                         where (action.IsActive == true)
                                         select new
                                         {
                                             action.Id,
                                             action.IsAsCopy,
                                             action.IsActive,
                                             TransactionType = action.Type,
                                             action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                             action.SortNo
                                         }).ToList().Select(a => new Action
                                         {
                                             Id = a.Id,
                                             IsAsCopy = a.IsAsCopy,
                                             IsActive = a.IsActive,
                                             Type = a.TransactionType,
                                             LocalName = a.Text,
                                             SortNo = a.SortNo
                                         }).ToList();
                return actions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Action> GetActions(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IQueryable<Action> actions = (from action in _oMCSDbContext.Actions
                                              select action);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Domain.Action).GetProperty(filter.ColumnName).PropertyType))
                        {
                            actions = SortByText(actions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else
                        {
                            actions = WhereQuery(actions, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = actions.Count();

                if (searchCriteria.OrderBy != null)
                {

                    if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Domain.Action).GetProperty(searchCriteria.OrderBy).PropertyType))
                    {
                        actions = OrderByText(actions, searchCriteria.CultureName, searchCriteria.Ascending);
                    }
                    else
                    {
                        actions = OrderQuery(actions, searchCriteria.OrderBy, searchCriteria.Ascending);
                    }
                }

                actions = actions.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);

                return actions.ToList().Select(a => new Action
                {
                    Id = a.Id,
                    LocalName = a.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                    Type = a.Type,
                    LocalizationIdentifier = a.LocalizationIdentifier,
                    IsActive = a.IsActive,
                    IsLocked = a.IsLocked,
                    LockedBy = a.LockedBy,
                    IsAsCopy = a.IsAsCopy
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int ActionId, int userId)
        {
            try
            {
                var actionToUpdate = _oMCSDbContext.Actions.FirstOrDefault(f => f.Id == ActionId);
                if (actionToUpdate != null)
                {
                    actionToUpdate.IsLocked = !actionToUpdate.IsLocked;

                    if (actionToUpdate.IsLocked)
                    {
                        actionToUpdate.LockedBy = userId;
                    }
                    else
                    {
                        actionToUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int ActionId)
        {
            try
            {
                var action = _oMCSDbContext.Actions.FirstOrDefault(f => f.Id == ActionId);
                if (action != null)
                {
                    action.IsActive = !action.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Action GetActionById(int actionId)
        {
            try
            {
                return FindBy(a => a.Id == actionId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void ChangeEntitiesNameBeforeMove(ChangeEntityName changeEntityName)
        {
            try
            {
                var EntityFrom = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == changeEntityName.EntityFromId);
                var EntityTo = _oMCSDbContext.OrgUnits.FirstOrDefault(e => e.Id == changeEntityName.EntityToId);

                if (changeEntityName.EntityFromLocalizations != null && changeEntityName.EntityToLocalizations != null && EntityFrom != null && EntityTo != null)
                {
                    foreach (Localization localization in changeEntityName.EntityFromLocalizations)
                    {
                        Localization currentlocalization = EntityFrom.LocalizationIdentifier.Localizations
                         .Where(l => l.CultureId == localization.CultureId)
                         .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            localization.Id = currentlocalization.Id;
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    foreach (Localization localization in changeEntityName.EntityToLocalizations)
                    {
                        Localization currentlocalization = EntityTo.LocalizationIdentifier.Localizations
                         .Where(l => l.CultureId == localization.CultureId)
                         .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            localization.Id = currentlocalization.Id;
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<UsersClearance> CheckUserClearance(List<int> usersIds, string cultureName)
        {
            try
            {
                List<UsersClearance> checkUsersClearances = new List<UsersClearance>();
                foreach (var item in usersIds)
                {
                    UsersClearance checkUsersClearance = new UsersClearance();
                    checkUsersClearance.UserId = item;
                    checkUsersClearance.UserName = _oMCSDbContext.UserProfiles.FirstOrDefault(u => u.Id == item).LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;

                    int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                    List<TransactionAssignment> InboundTransactions = _oMCSDbContext.TransactionAssignments.Where(t => t.ToUserId == item && t.Transaction.StatusId != TempSave && (t.Transaction.TransactionCategoryId == Inbound || t.Transaction.TransactionCategoryId == InternalOutbound || t.Transaction.TransactionCategoryId == DraftOutbound)).ToList();
                    if (InboundTransactions.Count > 0)
                    {
                        checkUsersClearance.InboundTransactionsCount = InboundTransactions.Count();
                    }
                    List<Transaction> OutboundTransactions = _oMCSDbContext.Transactions.Where(t => t.UserId == item && t.TransactionCategoryId == ExternalOutbound && t.StatusId != Sent).ToList();
                    if (OutboundTransactions.Count > 0)
                    {
                        checkUsersClearance.OutboundTransactionsCount = OutboundTransactions.Count();
                    }
                    List<TransactionAssignment> SavedTransactions = _oMCSDbContext.TransactionAssignments.Where(t => t.ToUserId == item && t.Transaction.StatusId == TempSave).ToList();
                    if (SavedTransactions.Count > 0)
                    {
                        checkUsersClearance.SavedTransactionsCount = SavedTransactions.Count();
                    }
                    checkUsersClearances.Add(checkUsersClearance);
                }
                return checkUsersClearances;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Action> SortByText(IQueryable<Action> source, string textValue, FilterType filterType, string cultureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from action in _oMCSDbContext.Actions.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(textValue))
                            select action);
                case FilterType.EndsWidth:
                    return (from action in _oMCSDbContext.Actions.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.EndsWith(textValue))
                            select action);
                case FilterType.StartsWith:
                    return (from action in _oMCSDbContext.Actions.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.StartsWith(textValue))
                            select action);
                case FilterType.Equals:
                    return (from action in _oMCSDbContext.Actions.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Equals(textValue))
                            select action);
            }

            return source;
        }

        private IQueryable<Action> OrderByText(IQueryable<Action> source, string culureName, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(action => action.LocalizationIdentifier.Localizations
                             .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
            }

            return source.OrderByDescending(action => action.LocalizationIdentifier.Localizations
                         .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
        }

        #endregion
    }
}
