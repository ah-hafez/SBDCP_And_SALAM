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
    public class ReleaseNotesRepository : BaseRepository<Domain.ReleaseNote>, IReleaseNotesRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public ReleaseNotesRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int ReleaseNotesAdd(Domain.ReleaseNote release)
        {
            try
            {
                if (string.IsNullOrEmpty(release.ReleaseNumber))
                    release.ReleaseNumber = string.Empty;

                if (string.IsNullOrEmpty(release.Description))
                {
                    release.IsActive = false;
                    release.Description = string.Empty;
                }

                _oMCSDbContext.ReleaseNotes.Add(release);

                _oMCSDbContext.SaveChanges();

                return release.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void ReleaseNotesUpdate(Domain.ReleaseNote release)
        {
            try
            {
                Domain.ReleaseNote releaseOld = ReleaseNotesSelectById(release.Id);

                if (string.IsNullOrEmpty(release.ReleaseNumber))
                    release.ReleaseNumber = string.Empty;

                if (string.IsNullOrEmpty(release.Description))
                {
                    release.IsActive = false;
                    release.Description = string.Empty;
                }

                _oMCSDbContext.Entry(releaseOld).CurrentValues.SetValues(release);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void ReleaseNotesDelete(int id)
        {
            try
            {
                var release = _oMCSDbContext.ReleaseNotes.Where(p => p.Id == id);

                foreach (var item in release)
                {
                    _oMCSDbContext.ReleaseNotes.Remove(item);
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.ReleaseNote> ReleaseNotesSelect()
        {
            try
            {
                IList<ReleaseNote> actions = _oMCSDbContext.ReleaseNotes.ToList();
                return actions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Domain.ReleaseNote> ReleaseNotesSelect(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IQueryable<Domain.ReleaseNote> releaseList = (from release in _oMCSDbContext.ReleaseNotes
                                                              select release);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Domain.ReleaseNote).GetProperty(filter.ColumnName).PropertyType))
                        {
                            releaseList = SortByText(releaseList, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else
                        {
                            releaseList = WhereQuery(releaseList, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = releaseList.Count();

                if (searchCriteria.OrderBy != null)
                {

                    releaseList = this.OrderQuery(releaseList, searchCriteria.OrderBy, searchCriteria.Ascending);
                }

                releaseList = releaseList.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);

                return releaseList.ToList().Select(a => new ReleaseNote
                {
                    Id = a.Id,
                    DateHj = a.DateHj,
                    ReleaseNumber = a.ReleaseNumber,
                    IsActive = a.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool ReleaseNotesCheckIfUsed(int releaseId)
        {
            try
            {
                if (_oMCSDbContext.ReleaseNotesUsers.FirstOrDefault(a => a.ReleaseNoteId == releaseId) == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public ReleaseNote ReleaseNotesSelectById(int releaseId)
        {
            try
            {
                return this.FindBy(a => a.Id == releaseId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Domain.ReleaseNote> SortByText(IQueryable<Domain.ReleaseNote> source, string textValue, FilterType filterType, string cultureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from release in _oMCSDbContext.ReleaseNotes.Where(p => p.Description.Contains(textValue)) select release);
                case FilterType.EndsWidth:
                    return (from release in _oMCSDbContext.ReleaseNotes.Where(p => p.Description.EndsWith(textValue)) select release);
                case FilterType.StartsWith:
                    return (from release in _oMCSDbContext.ReleaseNotes.Where(p => p.Description.StartsWith(textValue)) select release);
                case FilterType.Equals:
                    return (from release in _oMCSDbContext.ReleaseNotes.Where(p => p.Description.Equals(textValue)) select release);
            }

            return source;
        }

        #endregion
    }
}
