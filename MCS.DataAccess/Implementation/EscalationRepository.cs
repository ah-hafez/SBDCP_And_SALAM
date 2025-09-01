using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class EscalationRepository : BaseRepository<Escalation>, IEscalationRepository
    {
        #region Attributes
        #endregion Attributes



        #region Constructors
        public EscalationRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors
        #region Methods
        public int AddEscalation(Escalation escalation)
        {
            try
            {
                _oMCSDbContext.Escalations.Add(escalation);

                _oMCSDbContext.SaveChanges();

                return escalation.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Escalation GetEscalationById(int escalationyId)
        {
            try
            {
                return this.FindBy(p => p.Id == escalationyId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetEscalationPriorityId(int escalationyId)
        {
            try
            {
                Escalation escalation = GetEscalationById(escalationyId);

               return escalation.PriorityId ;
                
                
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetEscalationCategoryId(int escalationyId)
        {
            try
            {
                Escalation escalation = GetEscalationById(escalationyId);

               return escalation.TransactionCategoryId ;
                
                
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateEscalation(Escalation escalation)
        {
            try
            {
                Escalation escalationOld = GetEscalationById(escalation.Id);

                if (escalationOld != null)
                {
                    escalationOld.EscalationActionId = escalation.EscalationActionId;
                    escalationOld.EscalationAction = escalation.EscalationAction;
                    escalationOld.EscalationAfterDays = escalation.EscalationAfterDays;
                    escalationOld.EscalationToId = escalation.EscalationToId;
                    escalationOld.EscalationTo = escalation.EscalationTo;
                    escalationOld.TransactionCategory = escalation.TransactionCategory;
                    escalationOld.TransactionCategoryId = escalation.TransactionCategoryId;
                    escalationOld.Priority = escalation.Priority;
                    escalationOld.PriorityId = escalation.PriorityId;
                    _oMCSDbContext.Entry(escalationOld).CurrentValues.SetValues(escalation);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void DeleteEscalation(int id)
        {
            try
            {
                Escalation escalation = _oMCSDbContext.Escalations.Where(p => p.Id == id).FirstOrDefault();

                if (escalation != null)
                {
                    _oMCSDbContext.Escalations.Remove(escalation);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Escalation> GetEscalations(int TransactionCategoryId, string cultureName)
        {
            try
            {
                IList<Escalation> Escalations = (from a in _oMCSDbContext.Escalations
                                                 where a.TransactionCategoryId == TransactionCategoryId
                                                 select new
                                                 {
                                                     a.Id,
                                                     a.EscalationAfterDays,
                                                     Priority = (a.Priority != null) ? new
                                                     {
                                                         PriorityId = a.Priority.Id,
                                                         PriorityText = a.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,

                                                     TransactionCategory = (a.TransactionCategory != null) ? new
                                                     {
                                                         TransactionCategoryId= a.TransactionCategory.Id,
                                                         Text = a.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,
                                                     EscalationAction = (a.EscalationAction != null) ? new
                                                     {
                                                         EscalationActionId= a.EscalationAction.Id,
                                                         EscalationActionText = a.EscalationAction.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,
                                                     EscalationTo = (a.EscalationTo != null) ? new
                                                     {
                                                         EscalationToId= a.EscalationTo.Id,
                                                         EscalationToText = a.EscalationTo.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,

                                                 }).ToList().Select(p => new Escalation()
                                                 {
                                                     Id = p.Id,
                                                     EscalationAfterDays = p.EscalationAfterDays,
                                                     Priority = new Priority
                                                     {
                                                         Id = p.Priority.PriorityId,
                                                         Text = p.Priority.PriorityText
                                                     },
                                                     TransactionCategory = new Lookup
                                                     {
                                                         Id = p.TransactionCategory.TransactionCategoryId,
                                                         Text = p.TransactionCategory.Text
                                                     },
                                                     EscalationAction = new Lookup
                                                     {
                                                         Id = p.EscalationAction.EscalationActionId,
                                                         Text = p.EscalationAction.EscalationActionText
                                                     },
                                                     EscalationTo = new Lookup
                                                     {
                                                         Id = p.EscalationTo.EscalationToId,
                                                         Text = p.EscalationTo.EscalationToText
                                                     },
                                                 }).ToList();

                return Escalations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Escalation> GetEscalationByPriority(int TransactionCategoryId, int PriorityId, string cultureName)
        {
            try
            {
                IList<Escalation> Escalations = (from a in _oMCSDbContext.Escalations
                                                 where (a.TransactionCategoryId == TransactionCategoryId & a.PriorityId == PriorityId)
                                                 select new
                                                 {
                                                     a.Id,
                                                     a.EscalationAfterDays,
                                                     Priority = (a.Priority != null) ? new
                                                     {
                                                         PriorityId = a.Priority.Id,
                                                         PriorityText = a.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,

                                                     TransactionCategory = (a.TransactionCategory != null) ? new
                                                     {
                                                         TransactionCategoryId = a.TransactionCategory.Id,
                                                         Text = a.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,
                                                     EscalationAction = (a.EscalationAction != null) ? new
                                                     {
                                                         EscalationActionId = a.EscalationAction.Id,
                                                         EscalationActionText = a.EscalationAction.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,
                                                     EscalationTo = (a.EscalationTo != null) ? new
                                                     {
                                                         EscalationToId = a.EscalationTo.Id,
                                                         EscalationToText = a.EscalationTo.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     } : null,

                                                 }).ToList().Select(p => new Escalation
                                                 {
                                                     Id = p.Id,
                                                     EscalationAfterDays = p.EscalationAfterDays,
                                                     Priority = new Priority
                                                     {
                                                         Id = p.Priority.PriorityId,
                                                         Text = p.Priority.PriorityText
                                                     },
                                                     TransactionCategory = new Lookup
                                                     {
                                                         Id = p.TransactionCategory.TransactionCategoryId,
                                                         Text = p.TransactionCategory.Text
                                                     },
                                                     EscalationAction = new Lookup
                                                     {
                                                         Id = p.EscalationAction.EscalationActionId,
                                                         Text = p.EscalationAction.EscalationActionText
                                                     },
                                                     EscalationTo = new Lookup
                                                     {
                                                         Id = p.EscalationTo.EscalationToId,
                                                         Text = p.EscalationTo.EscalationToText
                                                     },
                                                 }).ToList();

                return Escalations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        #endregion Methods
    }
}





