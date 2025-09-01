using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class SubjectClassificationRepository : BaseLookupRepository<SubjectClassification>, ISubjectClassificationRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SubjectClassificationRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddSubjectClassification(SubjectClassification subjectClassification)
        {
            try
            {
                _oMCSDbContext.SubjectClassifications.Add(subjectClassification);

                _oMCSDbContext.SaveChanges();

                return subjectClassification.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteSubjectClassification(int id)
        {
            try
            {
                SubjectClassification subjectClassification =
                    _oMCSDbContext.SubjectClassifications.Where(p => p.Id == id).FirstOrDefault();

                if (subjectClassification != null)
                {
                    _oMCSDbContext.SubjectClassifications.Remove(subjectClassification);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SubjectClassification> GetAllSubjectClassifications()
        {
            try
            {
                IList<SubjectClassification> subjectClassifications =
                    (from s in _oMCSDbContext.SubjectClassifications
                     select new
                     {
                         s.Id,
                         s.IsGroup,
                         s.ParentId,
                         s.LocalizationIdentifier,
                         SubjectOrgUnits = s.SubjectOrgUnits.ToList().Select(o => new
                         {
                             o.OrgUnitId
                         })

                     }).ToList().Select(s => new SubjectClassification
                     {
                         Id = s.Id,
                         IsGroup = s.IsGroup,
                         ParentId = s.ParentId,
                         LocalizationIdentifier = s.LocalizationIdentifier,
                         SubjectOrgUnits = s.SubjectOrgUnits.ToList().Select(o => new SubjectOrgUnit
                         {
                             OrgUnitId = o.OrgUnitId
                         }).ToList(),
                     }).ToList();
                return subjectClassifications;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SubjectClassification> GetSubjectClassificationByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                IList<SubjectClassification> subjectClassifications =
                   _oMCSDbContext.SubjectClassifications
                   .Where(s => s.SubjectOrgUnits.Any(o => o.OrgUnitId == orgUnitId) || s.SubjectOrgUnits.Count == 0)
                   .Select(s => new
                   {
                       s.Id,
                       s.IsGroup,
                       s.ParentId,
                       s.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text

                   }
                   ).ToList().Select(s => new SubjectClassification
                   {
                       Id = s.Id,
                       IsGroup = s.IsGroup,
                       ParentId = s.ParentId,
                       Text = s.Text
                   }).ToList();
                return subjectClassifications;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSubjectClassification(SubjectClassification subjectClassification)
        {
            try
            {
                SubjectClassification updatedsubjectClassificationOld =
                    _oMCSDbContext.SubjectClassifications.Where(s => s.Id == subjectClassification.Id).FirstOrDefault();

                if (updatedsubjectClassificationOld != null)
                {
                    _oMCSDbContext.SubjectOrgUnits.RemoveRange(updatedsubjectClassificationOld.SubjectOrgUnits);

                    updatedsubjectClassificationOld.SubjectOrgUnits = subjectClassification.SubjectOrgUnits;
                    updatedsubjectClassificationOld.ParentId = subjectClassification.ParentId;

                    foreach (Localization localization in updatedsubjectClassificationOld.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = subjectClassification.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id)
                         .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(localization).CurrentValues.SetValues(currentlocalization);
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
        public SubjectClassification GetSubjectClassificationById(int subjectClassificationId)
        {
            try
            {
                SubjectClassification subjectClassification = this.FindBy(t => t.Id == subjectClassificationId);
                return subjectClassification;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods
    }
}
