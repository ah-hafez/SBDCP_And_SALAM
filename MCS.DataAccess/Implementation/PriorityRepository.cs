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
    public class PriorityRepository : BaseLookupRepository<Priority>, IPriorityRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public PriorityRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddPriority(Priority priority)
        {
            try
            {
                _oMCSDbContext.Priorities.Add(priority);

                _oMCSDbContext.SaveChanges();

                return priority.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdatePriority(Priority priority)
        {
            try
            {
                Priority priorityOld = GetPriorityById(priority.Id);

                if (priorityOld != null)
                {
                    //priorityOld.LocalizationIdentifier = priority.LocalizationIdentifier;

                    //_oMCSDbContext.Entry(priorityOld).CurrentValues.SetValues(priority);

                    //foreach (Localization localization in priority.LocalizationIdentifier.Localizations)
                    //{
                    //    Localization currentlocalization = priorityOld.LocalizationIdentifier.Localizations
                    //     .Where(l => l.Id == localization.Id)
                    //     .FirstOrDefault();

                    //    if (currentlocalization != null)
                    //    {
                    //        _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                    //    }
                    //}
                    if (priorityOld.PriorityExceptions.Count > 0)
                    {
                        priorityOld.HasPriorityExceptions = true;
                    }
                    priorityOld.LateForEntity = priority.LateForEntity;
                    priorityOld.LateForUser = priority.LateForUser;
                    priorityOld.ProcessPeriod = priority.ProcessPeriod;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Priority GetPriorityById(int priorityId)
        {
            try
            {
                return this.FindBy(p => p.Id == priorityId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Priority GetPriorityById(SearchCriteria searchCriteria, int priorityId, out int PriorityExceptionsRowsCount)
        {
            try
            {
                IQueryable<Priority> priority = (from p in _oMCSDbContext.Priorities
                                                 where p.Id == priorityId
                                                 select p);

                PriorityExceptionsRowsCount = priority.FirstOrDefault().PriorityExceptions.Count();

                List<PriorityException> priorityExceptions;


                if (searchCriteria.Ascending)
                {
                    priorityExceptions = priority.SingleOrDefault().PriorityExceptions.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize).ToList();
                }
                else
                {
                    priorityExceptions = priority.SingleOrDefault().PriorityExceptions.OrderByDescending(p => p.Id)
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize).ToList();
                }

                Priority finalResult = priority.FirstOrDefault();
                finalResult.PriorityExceptions = priorityExceptions;

                return finalResult;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeletePriority(int id)
        {
            try
            {
                Priority priority = _oMCSDbContext.Priorities.Where(p => p.Id == id).FirstOrDefault();

                if (priority != null)
                {
                    _oMCSDbContext.Priorities.Remove(priority);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Priority> GetPriorities(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Priority> priorities = (from priority in _oMCSDbContext.Priorities
                                                   select priority);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(Priority).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            priorities = SortByText(priorities, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            priorities = this.SortByTransactionCategory(priorities, filter.Value);
                        }
                        else
                        {
                            priorities = WhereQuery(priorities, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = priorities.Count();

                if (searchCriteria.Ascending)
                {
                    priorities = priorities.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    priorities = priorities.OrderByDescending(p => p.Id)
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return priorities.ToList().Select(p => new Priority
                {
                    Id = p.Id,
                    TransactionCategories = p.TransactionCategories,
                    Text = p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = p.LocalizationIdentifier,
                    LateForEntity = p.LateForEntity,
                    LateForUser = p.LateForUser,
                    HasPriorityExceptions = p.PriorityExceptions.Count() > 0 ? true : false
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Priority> GetPriorities(string cultureName , int OrgUnitId, int UserId)
        {
            try
            {
                IList<Priority> prioritiesWithOutExeptons = new List<Priority>();
                IList<Priority> priorities = (from priority in _oMCSDbContext.Priorities
                                              select new
                                              {
                                                  priority.Id,
                                                  priority.TransactionCategories,
                                                  Name = priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                  priority.HasDate,
                                                  priority.HasPriorityExceptions,
                                                  priorityException = priority.PriorityExceptions.ToList().Select(d => new
                                                  {
                                                      d.PriorityId,
                                                      d.OrgUnitId,
                                                      d.UserProfileId

                                                  }).ToList(),
                                              }).ToList().Select(p => new Priority
                                              {
                                                  Id = p.Id,
                                                  TransactionCategories = p.TransactionCategories,
                                                  Text = p.Name,
                                                  HasDate = p.HasDate,
                                                  HasPriorityExceptions = p.HasPriorityExceptions,
                                                  PriorityExceptions = p.priorityException.ToList().Select(o => new PriorityException
                                                  {
                                                      OrgUnitId = o.OrgUnitId,
                                                      Id = o.PriorityId,
                                                      UserProfileId = o.UserProfileId

                                                  }).ToList(),
                                              }).ToList();
                foreach (Priority p in priorities)
                {
                    if (p.HasPriorityExceptions == true)
                    {
                       if(p.PriorityExceptions.Find(exe => exe.OrgUnitId == OrgUnitId & exe.UserProfileId == UserId) == null)
                        {
                            prioritiesWithOutExeptons.Add(p);

                        }

                    }
                    else
                    {
                        prioritiesWithOutExeptons.Add(p);
                    }
                }

                return prioritiesWithOutExeptons;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Priority> SortByText(IQueryable<Priority> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from priority in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select priority);
                case FilterType.EndsWidth:
                    return (from priority in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select priority);
                case FilterType.StartsWith:
                    return (from priority in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select priority);
                case FilterType.Equals:
                    return (from priority in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select priority);
            }

            return source;
        }

        private IQueryable<Priority> SortByTransactionCategory(IQueryable<Priority> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from priority in source
                    where ((int)priority.TransactionCategories == value)
                    select priority);
        }

        #endregion Methods
    }
}
