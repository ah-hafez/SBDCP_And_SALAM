using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class SuggestedTopicRepository : BaseLookupRepository<SuggestedTopic>, ISuggestedTopicRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SuggestedTopicRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddSuggestedTopic(SuggestedTopic suggestedTopic)
        {
            try
            {
                _oMCSDbContext.SuggestedTopics.Add(suggestedTopic);

                _oMCSDbContext.SaveChanges();

                return suggestedTopic.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteSuggestedTopic(int id)
        {
            try
            {
                SuggestedTopic suggestedTopic = _oMCSDbContext.SuggestedTopics.Where(p => p.Id == id).FirstOrDefault();

                if (suggestedTopic != null)
                {
                    _oMCSDbContext.SuggestedTopics.Remove(suggestedTopic);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SuggestedTopic> GetAllSuggestedTopics()
        {
            try
            {
                IList<SuggestedTopic> suggestedTopics =
                    (from suggestedTopic in _oMCSDbContext.SuggestedTopics
                     select
                        new
                        {
                            suggestedTopic.Id,
                            suggestedTopic.IsGroup,
                            suggestedTopic.ParentId,
                            suggestedTopic.LocalizationIdentifier,
                            SubjectOrgUnits = suggestedTopic.SubjectOrgUnits.Select(o => new
                            {
                                o.OrgUnitId
                            })
                        }
                     ).ToList().Select(s => new SuggestedTopic
                     {
                         Id = s.Id,
                         IsGroup = s.IsGroup,
                         ParentId = s.ParentId,
                         LocalizationIdentifier = s.LocalizationIdentifier,
                         SubjectOrgUnits = s.SubjectOrgUnits.ToList().Select(o => new SubjectOrgUnit
                         {
                             OrgUnitId = o.OrgUnitId
                         }).ToList()
                     }).ToList();
                return suggestedTopics;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SuggestedTopic> GetSuggestedTopicsByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                IList<SuggestedTopic> suggestedTopics =
                     _oMCSDbContext.SuggestedTopics
                     .Where(s => s.SubjectOrgUnits.Any(o => o.OrgUnitId == orgUnitId) || s.SubjectOrgUnits.Count == 0)
                     .Select(suggestedTopic => new {
                         suggestedTopic.Id,
                         suggestedTopic.IsGroup,
                         suggestedTopic.ParentId,
                         suggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                     })
                     .ToList().Select(s => new SuggestedTopic
                {
                    Id = s.Id,
                    IsGroup = s.IsGroup,
                    ParentId = s.ParentId,
                    Text = s.Text
                }).ToList();
                return suggestedTopics;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSuggestedTopic(SuggestedTopic suggestedTopic)
        {
            try
            {

                SuggestedTopic updatedSuggestedTopicOld =
                    _oMCSDbContext.SuggestedTopics.Where(s => s.Id == suggestedTopic.Id).FirstOrDefault();

                if (updatedSuggestedTopicOld != null)
                {
                    _oMCSDbContext.SubjectOrgUnits.RemoveRange(updatedSuggestedTopicOld.SubjectOrgUnits);

                    updatedSuggestedTopicOld.SubjectOrgUnits = suggestedTopic.SubjectOrgUnits;
                    updatedSuggestedTopicOld.ParentId = suggestedTopic.ParentId;

                    foreach (Localization localization in updatedSuggestedTopicOld.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = suggestedTopic.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

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

        #endregion Methods
    }
}
