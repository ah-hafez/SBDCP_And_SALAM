using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LookupRepository : BaseRepository<Lookup>, ILookupRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public LookupRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public Lookup GetLookupItem(int lookupId)
        {
            try
            {
                return this.FindBy(l => l.Id == lookupId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Lookup GetLookupItem(int lookupId, string cultureName)
        {
            try
            {
                Lookup lookups = _oMCSDbContext.Lookups
                                                       .Where(l => l.Id == lookupId)
                                                       .Select(lookup => new
                                                       {
                                                           lookup.Id,
                                                           lookup.CategoryId,
                                                           lookup.IsActive,
                                                           lookup.Sort,
                                                           lookup.EnumReference,
                                                           lookup.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                       }).ToList().Select(l => new Lookup
                                                       {
                                                           Id = l.Id,
                                                           CategoryId = l.CategoryId,
                                                           IsActive = l.IsActive,
                                                           Sort = l.Sort,
                                                           EnumReference = l.EnumReference,
                                                           Text = l.Text
                                                       }).FirstOrDefault();

                return lookups;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Lookup> GetLookupItems(int lookupCategoryID, string cultureName)
        {
            try
            {
                IList<Lookup> lookups = _oMCSDbContext.Lookups
                                                           .Where(l => l.CategoryId == lookupCategoryID)
                                                           .Select(lookup => new
                                                           {
                                                               lookup.Id,
                                                               lookup.CategoryId,
                                                               lookup.IsActive,
                                                               lookup.Sort,
                                                               lookup.EnumReference,
                                                               lookup.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                           }).ToList().Select(l => new Lookup
                                                           {
                                                               Id = l.Id,
                                                               CategoryId = l.CategoryId,
                                                               IsActive = l.IsActive,
                                                               Sort = l.Sort,
                                                               EnumReference = l.EnumReference,
                                                               Text = l.Text
                                                           }).ToList();
                return lookups;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Lookup> GetActiveLookupItems(int lookupCategoryID, string cultureName)
        {
            try
            {
                IList<Lookup> lookups = _oMCSDbContext.Lookups
                                                           .Where(l => l.CategoryId == lookupCategoryID && l.IsActive == true)
                                                           .Select(lookup => new
                                                           {
                                                               lookup.Id,
                                                               lookup.CategoryId,
                                                               lookup.IsActive,
                                                               lookup.Sort,
                                                               lookup.EnumReference,
                                                               lookup.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                           }).ToList().Select(l => new Lookup
                                                           {
                                                               Id = l.Id,
                                                               CategoryId = l.CategoryId,
                                                               IsActive = l.IsActive,
                                                               Sort = l.Sort,
                                                               EnumReference = l.EnumReference,
                                                               Text = l.Text
                                                           }).ToList();
                return lookups;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int AddLookupItem(Lookup lookup)
        {
            try
            {
                _oMCSDbContext.Lookups.Add(lookup);
                _oMCSDbContext.SaveChanges();
                return lookup.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType)
        {
            try
            {
                LetterType letterType = _oMCSDbContext.LetterTypes.FirstOrDefault(l => l.Id == letterTypeId);
                letterType.Notify = operationType;

                _oMCSDbContext.Entry(letterType).CurrentValues.SetValues(letterType);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType)
        {
            try
            {
                LetterType letterType = _oMCSDbContext.LetterTypes.FirstOrDefault(l => l.Id == letterTypeId);
                letterType.WithExtraField = operationType;

                _oMCSDbContext.Entry(letterType).CurrentValues.SetValues(letterType);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void ActiveDeactiveLookup(int lookupId)
        {
            try
            {
                var lookp = _oMCSDbContext.Lookups.FirstOrDefault(l => l.Id == lookupId);
                if (lookp != null)
                {
                    lookp.IsActive = !lookp.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Methods
    }
}
