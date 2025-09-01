using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ExplanationRepository : BaseRepository<Explanation>, IExplanationRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public ExplanationRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddExplanation(Transaction transaction, Explanation explanation)
        {
            try
            {
                transaction.Explanations.Add(explanation);

                _oMCSDbContext.SaveChanges();

                return explanation.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateExplanation(Explanation explanation)
        {
            try
            {
                Explanation explanationOld = _oMCSDbContext.Explanations.Where(e => e.Id == explanation.Id).FirstOrDefault();

                if (explanationOld != null)
                {
                    explanationOld.PermissionId = explanation.PermissionId;


                    if (explanation.Document != null)
                    {
                        explanation.Document.Id = explanationOld.Document.Id;
                        if (explanationOld.Document.Document != null)
                        {
                            explanation.Document.Document.Id = explanationOld.Document.Document.Id;
                        }
                        explanationOld.Document = explanation.Document;
                    }

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteExplanation(int explanationId)
        {
            try
            {
                Explanation explanation = _oMCSDbContext.Explanations.Where(e => e.Id == explanationId).FirstOrDefault();

                if (explanation != null)
                {
                    _oMCSDbContext.Explanations.Remove(explanation);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Explanation> GetExplanationsByTransactionId(int transactionId, int userId, string cultureName)
        {
            try
            {
                int row = 0;
                IList<Explanation> explanations = (from explanation in _oMCSDbContext.Explanations
                                                   where
                                                   explanation.TransactionId == transactionId
                                                   select new
                                                   {
                                                       explanation.Id,
                                                       explanation.ExplanationEditorType,
                                                       explanation.Date,
                                                       explanation.DateH,

                                                       FromUser = new
                                                       {
                                                           explanation.FromUser.Id,
                                                           explanation.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                       },
                                                       Permission = new
                                                       {
                                                           explanation.Permission.Id,
                                                           explanation.Permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                       },
                                                       Transaction = new
                                                       {
                                                           explanation.Transaction.Number
                                                       },
                                                       DocumentInfo = explanation.Document,

                                                   }).ToList().Select(e => new Explanation
                                                   {
                                                       Id = e.Id,
                                                       ExplanationEditorType = e.ExplanationEditorType,
                                                       Date = e.Date,
                                                       DateH = e.DateH,
                                                       FromUser = new UserProfile
                                                       {
                                                           Id = e.FromUser.Id,
                                                           LocalName = e.FromUser.Text
                                                       },
                                                       Permission = new Permission
                                                       {
                                                           Id = e.Permission.Id,
                                                           LocalName = e.Permission.Text
                                                       },
                                                       Transaction = new Transaction
                                                       {
                                                           Number = e.Transaction.Number
                                                       },
                                                       Document = e.DocumentInfo,
                                                       RowNumber = ++row
                                                   }).ToList();



                UserProfile userProfile = _oMCSDbContext.UserProfiles.Where(b => b.Id == userId).FirstOrDefault();
                IList<Permission> allPermissions = new List<Permission>();

                foreach (var group in userProfile.UserGroups)
                {
                    allPermissions = allPermissions.Concat(group.Group.Permissions.ToList()).ToList();

                }


                foreach (var explanation in explanations)
                {
                    explanations = explanations.Where(e => allPermissions.Any(p => p.Id == e.Permission.Id)).ToList();


                }

                return explanations;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Explanation> GetExplanationsByTransactionIdWithoutContent(int transactionId, int userId, string cultureName)
        {
            try
            {
                int row = 0;
                IList<Explanation> explanations = (from explanation in _oMCSDbContext.Explanations
                                                   where
                                                   explanation.TransactionId == transactionId
                                                   select new
                                                   {
                                                       explanation.Id,
                                                       explanation.ExplanationEditorType,
                                                       explanation.Date,
                                                       explanation.DateH,

                                                       FromUser = new
                                                       {
                                                           explanation.FromUser.Id,
                                                           explanation.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                       },
                                                       Permission = new
                                                       {
                                                           explanation.Permission.Id,
                                                           explanation.Permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                       },
                                                       Transaction = new
                                                       {
                                                           explanation.Transaction.Number
                                                       },
                                                       explanation.Document,

                                                   }).ToList().Select(e => new Explanation
                                                   {
                                                       Id = e.Id,
                                                       ExplanationEditorType = e.ExplanationEditorType,
                                                       Date = e.Date,
                                                       DateH = e.DateH,
                                                       FromUser = new UserProfile
                                                       {
                                                           Id = e.FromUser.Id,
                                                           LocalName = e.FromUser.Text
                                                       },
                                                       Permission = new Permission
                                                       {
                                                           Id = e.Permission.Id,
                                                           LocalName = e.Permission.Text
                                                       },
                                                       Transaction = new Transaction
                                                       {
                                                           Number = e.Transaction.Number
                                                       },
                                                       Document = e.Document != null ? new DocumentInfo
                                                       {
                                                           Id = e.Document.Id,
                                                           DocumentType = e.Document.DocumentType,
                                                           FromEntityId = e.Document.FromEntityId,
                                                           MimeType = e.Document.MimeType,
                                                           TransactionId = e.Document.TransactionId,
                                                           Document = e.Document.Document != null ? new Document
                                                           {
                                                               Id = e.Document.Document.Id,

                                                           } : null,
                                                           FromUserId = e.Document.FromUserId

                                                       } : null,
                                                       RowNumber = ++row
                                                   }).ToList();

                UserProfile userProfile = _oMCSDbContext.UserProfiles.Where(b => b.Id == userId).FirstOrDefault();
                IList<Permission> allPermissions = new List<Permission>();

                foreach (var group in userProfile.UserGroups)
                {
                    allPermissions = allPermissions.Concat(group.Group.Permissions.ToList()).ToList();

                }


                foreach (var explanation in explanations)
                {
                    explanations = explanations.Where(e => allPermissions.Any(p => p.Id == e.Permission.Id)).ToList();


                }

                return explanations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public Explanation GetExplanationById(int explanationId, string cultureName)
        {
            try
            {
                Explanation explanation = (from ex in _oMCSDbContext.Explanations
                                           where (ex.Id == explanationId)
                                           select new
                                           {
                                               ex.Id,
                                               ex.ExplanationEditorType,
                                               ex.Document,
                                               ex.Date,
                                               FromUser = new
                                               {
                                                   ex.FromUser.Id,
                                                   ex.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               Permission = new
                                               {
                                                   ex.Permission.Id,
                                                   ex.Permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               }
                                           }).ToList().Select(e => new Explanation
                                           {
                                               Id = e.Id,
                                               ExplanationEditorType = e.ExplanationEditorType,
                                               Date = e.Date,
                                               Document = e.Document,
                                               FromUser = new UserProfile
                                               {
                                                   Id = e.FromUser.Id,
                                                   LocalName = e.FromUser.Text
                                               },
                                               Permission = new Permission
                                               {
                                                   Id = e.Permission.Id,
                                                   LocalName = e.Permission.Text
                                               }
                                           }).FirstOrDefault();

                if (explanation == null)
                {
                    return null;
                }

                return explanation;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Explanation> GetExplanations(Func<Explanation, bool> where)
        {
            try
            {
                return _oMCSDbContext.Explanations.Where(where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Explanation GetExplanationByDocumentId(int DocumentId, string cultureName)
        {
            try
            {
                Explanation explanation = (from ex in _oMCSDbContext.Explanations
                                           where (ex.Document.Id == DocumentId)
                                           select new
                                           {
                                               ex.Id,
                                               ex.ExplanationEditorType,
                                               ex.Document,
                                               ex.Date,
                                               ex.DateH,
                                               FromUser = new
                                               {
                                                   ex.FromUser.Id,
                                                   ex.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               },
                                               Permission = new
                                               {
                                                   ex.Permission.Id,
                                                   ex.Permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                               }
                                           }).ToList().Select(e => new Explanation
                                           {
                                               Id = e.Id,
                                               ExplanationEditorType = e.ExplanationEditorType,
                                               Date = e.Date,
                                               DateH = e.DateH,
                                               Document = e.Document,
                                               FromUser = new UserProfile
                                               {
                                                   Id = e.FromUser.Id,
                                                   LocalName = e.FromUser.Text
                                               },
                                               Permission = new Permission
                                               {
                                                   Id = e.Permission.Id,
                                                   LocalName = e.Permission.Text
                                               }
                                           }).FirstOrDefault();

                if (explanation == null)
                {
                    return null;
                }

                return explanation;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion
    }
}
