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
    public class TransactionTypeRepository : BaseLookupRepository<Domain.TransactionType>, ITransactionTypeRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionTypeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddTransactionType(Domain.TransactionType transactionType)
        {
            try
            {
                _oMCSDbContext.TransactionTypes.Add(transactionType);

                _oMCSDbContext.SaveChanges();

                return transactionType.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionType(Domain.TransactionType transactionType)
        {
            try
            {
                Domain.TransactionType transactionTypeOld = GetTransactionTypeById(transactionType.Id);

                if (transactionTypeOld != null)
                {
                    transactionTypeOld.Permission = transactionType.Permission;
                    transactionTypeOld.Color = transactionType.Color;

                    _oMCSDbContext.Entry(transactionTypeOld).CurrentValues.SetValues(transactionType);

                    foreach (Localization localization in transactionType.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = transactionTypeOld.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    foreach (Localization localization in transactionType.Abbreviation.Localizations)
                    {
                        Localization currentlocalization = transactionTypeOld.Abbreviation.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

                        if (currentlocalization != null)
                        {
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

        public TransactionType GetTransactionTypeById(int transactionTypeId)
        {
            try
            {
                return FindBy(t => t.Id == transactionTypeId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteTransactionType(int id)
        {
            try
            {
                TransactionType transactionType = _oMCSDbContext.TransactionTypes.Where(t => t.Id == id).FirstOrDefault();
                if (transactionType != null)
                {
                    int localizationCount = transactionType.LocalizationIdentifier.Localizations.Count;
                    for (int i = 0; i < localizationCount; i++)
                    {
                        _oMCSDbContext.Entry(transactionType.LocalizationIdentifier.Localizations[0]).State = EntityState.Deleted;
                    }
                    _oMCSDbContext.Entry(transactionType.LocalizationIdentifier).State = EntityState.Deleted;
                    localizationCount = transactionType.Abbreviation.Localizations.Count;
                    for (int i = 0; i < localizationCount; i++)
                    {
                        _oMCSDbContext.Entry(transactionType.Abbreviation.Localizations[0]).State = EntityState.Deleted;
                    }
                    _oMCSDbContext.Entry(transactionType.Abbreviation).State = EntityState.Deleted;
                    _oMCSDbContext.TransactionTypes.Remove(transactionType);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionType> GetTransactionTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Domain.TransactionType> transactionTypes = (from transactionType in _oMCSDbContext.TransactionTypes
                                                                       select transactionType);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(Domain.TransactionType).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            transactionTypes = SortByText(transactionTypes, filter.Value, filter.Type);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            transactionTypes = SortByTransactionCategory(transactionTypes, filter.Value);
                        }
                        else
                        {
                            transactionTypes = WhereQuery(transactionTypes, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = transactionTypes.Count();

                if (searchCriteria.Ascending)
                {
                    transactionTypes = transactionTypes.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    transactionTypes = transactionTypes.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return transactionTypes.ToList().Select(t => new TransactionType
                {
                    Id = t.Id,
                    TransactionCategories = t.TransactionCategories,
                    Text = t.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = t.LocalizationIdentifier
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.TransactionType> GetTransactionTypes(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<TransactionType> transactionTypes = (from transactionType in _oMCSDbContext.TransactionTypes
                                                           where transactionType.TransactionCategories.HasFlag(sourceTransactionType)
                                                           select new
                                                           {
                                                               transactionType.Id,
                                                               transactionType.TransactionCategories,
                                                               transactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                           }).ToList().Select(t => new TransactionType
                                                           {
                                                               Id = t.Id,
                                                               TransactionCategories = t.TransactionCategories,
                                                               Text = t.Text
                                                           }).ToList();

                return transactionTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.TransactionType> GetTransactionTypesByUserId(int userId, TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.Where(b => b.Id == userId).FirstOrDefault();

                IQueryable<TransactionType> transactionTypesList = (from transactionType in _oMCSDbContext.TransactionTypes.Include(a => a.Permission).Where(a => a.TransactionCategories.HasFlag(sourceTransactionType))
                                                                    select new
                                                                    {
                                                                        transactionType.Id,
                                                                        SourceTransactionType = transactionType.TransactionCategories,
                                                                        transactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                        transactionType.PermissionId,
                                                                        transactionType.Permission
                                                                    }).ToList().Select(t => new TransactionType
                                                                    {
                                                                        Id = t.Id,
                                                                        TransactionCategories = t.SourceTransactionType,
                                                                        Text = t.Text,
                                                                        PermissionId=t.PermissionId,
                                                                        Permission = t.Permission
                                                                    }).AsQueryable();

                //IList<TransactionType> transactionTypes = (from transactionType in transactionTypesList.ToList()
                //                                           join userPermission in userProfile.Permissions on
                //                                            transactionType.PermissionId equals userPermission.Group.Id
                //                                           select new
                //                                           {
                //                                               transactionType.Id,
                //                                               SourceTransactionType = transactionType.TransactionCategories,
                //                                               transactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                //                                           }).ToList().Select(t => new TransactionType
                //                                           {
                //                                               Id = t.Id,
                //                                               TransactionCategories = t.SourceTransactionType,
                //                                               Text = t.Text
                //                                           }).ToList();

                return transactionTypesList.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionType> GetTransactionTypes(string cultureName)
        {
            try
            {
                IList<TransactionType> transactionTypes = (from transactionType in _oMCSDbContext.TransactionTypes
                                                           select new
                                                           {
                                                               transactionType.Id,
                                                               SourceTransactionType = transactionType.TransactionCategories,
                                                               transactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                               transactionType.Permission

                                                           }).ToList().Select(t => new TransactionType
                                                           {
                                                               Id = t.Id,
                                                               TransactionCategories = t.SourceTransactionType,
                                                               Text = t.Text,
                                                               Permission = t.Permission
                                                           }).ToList();

                return transactionTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<TransactionType> SortByText(IQueryable<TransactionType> source, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from transactionType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select transactionType);
                case FilterType.EndsWidth:
                    return (from transactionType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select transactionType);
                case FilterType.StartsWith:
                    return (from transactionType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select transactionType);
                case FilterType.Equals:
                    return (from transactionType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select transactionType);
            }

            return source;
        }

        private IQueryable<TransactionType> SortByTransactionCategory(IQueryable<Domain.TransactionType> source, string textValue)
        {
            int value = Convert.ToInt32(textValue);

            return (from sourceType in source
                    where ((int)sourceType.TransactionCategories == value)
                    select sourceType);
        }

        public IList<Domain.TransactionType> UserMobileGetTransactionTypes(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<TransactionType> transactionTypes = (from transactionType in _oMCSDbContext.TransactionTypes
                                                           where transactionType.TransactionCategories.HasFlag(sourceTransactionType)
                                                           select new
                                                           {
                                                               transactionType.Id,
                                                               transactionType.TransactionCategories,
                                                               transactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                               transactionType.Permission
                                                           }).ToList().Select(t => new TransactionType
                                                           {
                                                               Id = t.Id,
                                                               TransactionCategories = t.TransactionCategories,
                                                               Text = t.Text,
                                                               Permission = t.Permission
                                                           }).ToList();

                return transactionTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
