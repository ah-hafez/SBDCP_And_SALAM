using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserCategoryRepository : BaseRepository<UserCategory>, IUserCategoryRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public UserCategoryRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddUserCategory(UserCategory userCategory)
        {
            try
            {
                _oMCSDbContext.UserCategories.Add(userCategory);

                _oMCSDbContext.SaveChanges();

                return userCategory.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserCategory(UserCategory userCategory)
        {
            try
            {
                UserCategory userCategoryOld = GetUserCategoryById(userCategory.Id);
                if (userCategoryOld != null)
                {
                    userCategoryOld.CategoryName = userCategory.CategoryName;
                    userCategoryOld.Permission = userCategory.Permission;

                    _oMCSDbContext.Entry(userCategoryOld).CurrentValues.SetValues(userCategory);

                    foreach (Localization localization in userCategory.CategoryName.Localizations)
                    {
                        Localization currentlocalization = userCategoryOld.CategoryName.Localizations
                         .Where(l => l.Id == localization.Id)
                         .FirstOrDefault();

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

        public UserCategory GetUserCategoryById(int userCategoryId)
        {
            try
            {
                return this.FindBy(p => p.Id == userCategoryId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteUserCategory(int id)
        {
            try
            {
                UserCategory userCategory = _oMCSDbContext.UserCategories.FirstOrDefault(p => p.Id == id);
                if (userCategory != null)
                {
                    _oMCSDbContext.UserCategories.Remove(userCategory);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<UserCategory> GetUserCategories(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserCategory> userCategories = (from userCategory in _oMCSDbContext.UserCategories
                                                           select userCategory);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserCategory).GetProperty(filter.ColumnName).PropertyType))
                        {
                            userCategories = SortByText(userCategories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (typeof(UserCategory).GetProperty(filter.ColumnName).PropertyType == typeof(Permission))
                        {
                            userCategories = SortPermissionByText(userCategories, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else
                        {
                            userCategories = WhereQuery(userCategories, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = userCategories.Count();

                if (searchCriteria.OrderBy != null)
                {

                    if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserCategory).GetProperty(searchCriteria.OrderBy).PropertyType))
                    {
                        userCategories = this.OrderByText(userCategories, searchCriteria.CultureName, searchCriteria.Ascending);
                    }
                    else if (typeof(UserCategory).GetProperty(searchCriteria.OrderBy).PropertyType == typeof(Permission))
                    {
                        userCategories = this.OrderByPermissionText(userCategories, searchCriteria.CultureName, searchCriteria.Ascending);
                    }
                    else
                    {
                        userCategories = this.OrderQuery(userCategories, searchCriteria.OrderBy, searchCriteria.Ascending);
                    }
                }

                userCategories = userCategories.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                              .Take(searchCriteria.PageSize);

                return userCategories.ToList().Select(u => new UserCategory
                {
                    Id = u.Id,
                    Permission = u.Permission,
                    LocalName = u.CategoryName.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    CategoryTrays = u.CategoryTrays
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tray> GetUserCategoryTrays(int userCategoryId)
        {
            try
            {
                IList<Tray> trays = (from tray in _oMCSDbContext.Trays
                                          from userTrays in _oMCSDbContext.UserCategoryTrays
                                          where tray.Id == userTrays.Tary.Id && userTrays.UserCategory.Id == userCategoryId
                                          orderby tray.Sort ascending
                                          select tray).ToList();
                return trays;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUsersCategoriesTrays(IList<UserCategoryTray> userCategoryTrays)
        {
            try
            {
                IList<UserCategoryTray> existingUserCategoryTrays = _oMCSDbContext.UserCategoryTrays.ToList();

                foreach (UserCategoryTray userCategoryTray in existingUserCategoryTrays)
                {
                    _oMCSDbContext.Entry(userCategoryTray).State = EntityState.Deleted;
                }

                foreach (UserCategoryTray userCategoryTray in userCategoryTrays)
                {
                    _oMCSDbContext.Entry(userCategoryTray).State = EntityState.Added;
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tray> GetUserCategoryTrays(int userCategoryId, string cultureName)
        {
            try
            {
                IList<Tray> trays = (from tray in _oMCSDbContext.Trays
                                          //from userTrays in _oMCSDbContext.UserCategoryTrays
                                          //where tray.Id == userTrays.Tary.Id && userTrays.UserCategory.Id == userCategoryId
                                          orderby tray.Sort ascending
                                          select new
                                          {
                                             tray.Id,
                                              tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                          }).ToList().Select(t => new Tray
                                          {
                                              Id = t.Id,
                                              LocalName = t.Text
                                          }).ToList();
                return trays;
            }


            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<UserCategory> GetAllUsersCategoriesTrays(string cultureName)
        {
            try
            {
                IList<UserCategory> userCategories = (from userCategory in _oMCSDbContext.UserCategories
                                                           select new
                                                           {
                                                                userCategory.Id,
                                                                userCategory.Permission,
                                                                userCategory.CategoryName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                               userCategory.CategoryTrays
                                                           }).ToList().Select(u => new UserCategory
                                                           {
                                                               Id = u.Id,
                                                               Permission = u.Permission,
                                                               LocalName = u.Text,
                                                               CategoryTrays = u.CategoryTrays
                                                           }).ToList();

                return userCategories;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<UserCategory> SortByText(IQueryable<UserCategory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select userCategory);
                case FilterType.EndsWidth:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select userCategory);
                case FilterType.StartsWith:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select userCategory);
                case FilterType.Equals:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select userCategory);
            }

            return source;
        }

        private IQueryable<UserCategory> SortPermissionByText(IQueryable<UserCategory> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.Permission.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select userCategory);
                case FilterType.EndsWidth:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.Permission.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select userCategory);
                case FilterType.StartsWith:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.Permission.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select userCategory);
                case FilterType.Equals:
                    return (from userCategory in _oMCSDbContext.UserCategories.Where(p => p.Permission.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select userCategory);
            }

            return source;
        }

        private IQueryable<UserCategory> OrderByText(IQueryable<UserCategory> source, string culureName, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(userCategory => userCategory.CategoryName.Localizations
                             .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
            }

            return source.OrderByDescending(userCategory => userCategory.CategoryName.Localizations
                         .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
        }

        private IQueryable<UserCategory> OrderByPermissionText(IQueryable<UserCategory> source, string culureName, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(userCategory => userCategory.Permission.Name.Localizations
                             .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
            }

            return source.OrderByDescending(userCategory => userCategory.Permission.Name.Localizations
                         .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
        }

        #endregion Methods
    }
}
