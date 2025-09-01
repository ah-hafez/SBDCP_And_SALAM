using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ConfidentialityAcknowledgmentRepository : BaseLookupRepository<ConfidentialityAcknowledgment>, IConfidentialityAcknowledgmentRepository
    {
        #region Constructors

        public ConfidentialityAcknowledgmentRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }

        #endregion Constructors

        #region Methods

        public void UpdateConfidentialityAcknowledgment(ConfidentialityAcknowledgment ConfidentialityAcknowledgment)
        {
            try
            {
                ConfidentialityAcknowledgment ConfidentialityAcknowledgmentOld = this.FindBy(a => a.Id == ConfidentialityAcknowledgment.Id);
                ConfidentialityAcknowledgment.IsActive = ConfidentialityAcknowledgmentOld.IsActive;
                _oMCSDbContext.Entry(ConfidentialityAcknowledgmentOld).CurrentValues.SetValues(ConfidentialityAcknowledgment);

                foreach (Localization localization in ConfidentialityAcknowledgment.LocalizationIdentifier.Localizations)
                {
                    Localization currentlocalization = ConfidentialityAcknowledgmentOld.LocalizationIdentifier.Localizations
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


        public IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<ConfidentialityAcknowledgment> ConfidentialityAcknowledgments = (from ConfidentialityAcknowledgment in _oMCSDbContext.ConfidentialityAcknowledgments
                                                                                            select ConfidentialityAcknowledgment);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(ConfidentialityAcknowledgment).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            ConfidentialityAcknowledgments = SortByText(ConfidentialityAcknowledgments, filter.Value, filter.Type);
                        }

                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            ConfidentialityAcknowledgments = SortByTransactionCategory(ConfidentialityAcknowledgments, filter.Value);
                        }
                        else
                        {
                            ConfidentialityAcknowledgments = WhereQuery(ConfidentialityAcknowledgments, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = ConfidentialityAcknowledgments.Count();

                if (searchCriteria.Ascending)
                {
                    ConfidentialityAcknowledgments = ConfidentialityAcknowledgments.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);

                }
                else
                {
                    ConfidentialityAcknowledgments = ConfidentialityAcknowledgments.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return ConfidentialityAcknowledgments.ToList().Select(a => new ConfidentialityAcknowledgment
                {
                    Id = a.Id,
                    IsMandatary = a.IsMandatary,
                    TransactionCategories = a.TransactionCategories,
                    Text = a.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = a.LocalizationIdentifier,
                    IsActive = a.IsActive,
                    IsLocked = a.IsLocked,
                    LockedBy = a.LockedBy
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(string cultureName)
        {
            try
            {
                IList<ConfidentialityAcknowledgment> ConfidentialityAcknowledgments = (from ConfidentialityAcknowledgment in _oMCSDbContext.ConfidentialityAcknowledgments
                                                                                       where (ConfidentialityAcknowledgment.IsActive == true)
                                                                                       select new
                                                                                       {
                                                                                           ConfidentialityAcknowledgment.Id,
                                                                                           ConfidentialityAcknowledgment.IsMandatary,
                                                                                           ConfidentialityAcknowledgment.TransactionCategories,
                                                                                           ConfidentialityAcknowledgment.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                                                       }).ToList().Select(l => new ConfidentialityAcknowledgment
                                                                                       {
                                                                                           Id = l.Id,
                                                                                           TransactionCategories = l.TransactionCategories,
                                                                                           Text = l.Text,
                                                                                           IsMandatary = l.IsMandatary
                                                                                       }).ToList();
                return ConfidentialityAcknowledgments;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }





        public bool CheckIfConfidentialityAcknowledgmentUsed(int attachmnetTypeId)
        {
            try
            {
                return (_oMCSDbContext.Attachments.FirstOrDefault(a => a.Type.Id == attachmnetTypeId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int ConfidentialityAcknowledgmentId, int userId)
        {
            try
            {
                var ConfidentialityAcknowledgmentToUpdate = _oMCSDbContext.ConfidentialityAcknowledgments.FirstOrDefault(f => f.Id == ConfidentialityAcknowledgmentId);
                if (ConfidentialityAcknowledgmentToUpdate != null)
                {
                    ConfidentialityAcknowledgmentToUpdate.IsLocked = !ConfidentialityAcknowledgmentToUpdate.IsLocked;

                    if (ConfidentialityAcknowledgmentToUpdate.IsLocked)
                    {
                        ConfidentialityAcknowledgmentToUpdate.LockedBy = userId;
                    }
                    else
                    {
                        ConfidentialityAcknowledgmentToUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int ConfidentialityAcknowledgmentId)
        {
            try
            {
                var ConfidentialityAcknowledgment = _oMCSDbContext.ConfidentialityAcknowledgments.FirstOrDefault(f => f.Id == ConfidentialityAcknowledgmentId);
                if (ConfidentialityAcknowledgment != null)
                {
                    ConfidentialityAcknowledgment.IsActive = !ConfidentialityAcknowledgment.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<ConfidentialityAcknowledgment> SortByText(IQueryable<ConfidentialityAcknowledgment> source, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from ConfidentialityAcknowledgment in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select ConfidentialityAcknowledgment);
                case FilterType.EndsWidth:
                    return (from ConfidentialityAcknowledgment in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select ConfidentialityAcknowledgment);
                case FilterType.StartsWith:
                    return (from ConfidentialityAcknowledgment in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select ConfidentialityAcknowledgment);
                case FilterType.Equals:
                    return (from ConfidentialityAcknowledgment in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select ConfidentialityAcknowledgment);
            }

            return source;
        }

        private IQueryable<ConfidentialityAcknowledgment> SortByTransactionCategory(IQueryable<ConfidentialityAcknowledgment> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from ConfidentialityAcknowledgment in source
                    where ((int)ConfidentialityAcknowledgment.TransactionCategories == value)
                    select ConfidentialityAcknowledgment);
        }

        #endregion Methods
    }
}
