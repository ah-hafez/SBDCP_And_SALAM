using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CollaborationRepository : BaseRepository<Collaboration>, ICollaborationRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public CollaborationRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public void AddCollaboration(Collaboration conversation)
        {
            try
            {
                _oMCSDbContext.Collaborations.Add(conversation);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateCollaboration(Collaboration conversation)
        {
            try
            {
                _oMCSDbContext.Entry(conversation).State = System.Data.Entity.EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int GetCollaborationCount(System.Linq.Expressions.Expression<Func<Collaboration, bool>> where)
        {
            try
            {
                return _oMCSDbContext.Collaborations.Where(where).Count();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaborations(System.Linq.Expressions.Expression<Func<Collaboration, bool>> where, int pageSize, string cultureName)
        {
            try
            {
                IList<Collaboration> conversations = _oMCSDbContext.Collaborations
                    .Where(where)

                    .OrderByDescending(c => c.Id)

                    .Take(pageSize)
                    .Select(conversation => new
                    {
                        conversation.Id,
                        conversation.Text,
                        conversation.Date,
                        conversation.DateH,
                        Sender = conversation.Sender ?? null,
                        SenderId = conversation.Sender.Id,
                        SenderName = conversation.Sender.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        Receiver = conversation.Receiver ?? null,
                        ReceiverId = conversation.Receiver.Id,
                        ReceiverName = conversation.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                    }).ToList().Select(c => new Collaboration
                    {
                        Id = c.Id,
                        Text = c.Text,
                        DateH = c.DateH,
                        Date = c.Date,
                        Sender = (c.Sender != null) ? new UserProfile
                        {
                            Id = c.SenderId,
                            LocalName = c.SenderName
                        } : null,

                        Receiver = (c.Receiver != null) ? new UserProfile
                        {
                            Id = c.ReceiverId,
                            LocalName = c.ReceiverName
                        } : null,
                    }).OrderBy(c => c.Id).ToList();
                return conversations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaborationEhab(System.Linq.Expressions.Expression<Func<Collaboration, bool>> where)
        {
            try
            {
                IList<Collaboration> collaborations = new List<Collaboration>();

                var conversations =
                    _oMCSDbContext.Collaborations.Where(where).ToList().Select(c => new { c.Sender }).Distinct();

                foreach (var conversation in conversations)
                {
                    Collaboration collaboration =
                        new Collaboration { Sender = conversation.Sender };

                    collaborations.Add(collaboration);
                }

                return collaborations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaboration(System.Linq.Expressions.Expression<Func<Collaboration, bool>> where, string cultureName)
        {
            try
            {
                IList<Collaboration> collaborations = new List<Collaboration>();

                var conversations =
                    _oMCSDbContext.Collaborations.Where(where).ToList().Select(c => new { c.Receiver }).Distinct();

                foreach (var conversation in conversations)
                {
                    conversation.Receiver.LocalName =
                        conversation.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();

                    Collaboration collaboration =
                        new Collaboration { Receiver = conversation.Receiver };

                    collaborations.Add(collaboration);
                }

                return collaborations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaborations(System.Linq.Expressions.Expression<Func<Collaboration, bool>> where)
        {
            try
            {
                return _oMCSDbContext.Collaborations.Where(where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaborations(Expression<Func<Collaboration, bool>> where, SearchCriteria searchCriteria, string cultureName)
        {
            try
            {
                IList<Collaboration> collaborations = new List<Collaboration>();

                IQueryable<Collaboration> existCollaboration = _oMCSDbContext.Collaborations.Where(where);

                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (Filter filter in searchCriteria.Filters)
                        {
                            if (filter.ColumnName == "Receiver")
                            {
                                existCollaboration = SortByUser(existCollaboration, filter.Value, filter.Type, searchCriteria.CultureName);
                            }

                            if (filter.ColumnName == "Transaction")
                            {
                                existCollaboration = SortTransactionByNumber(existCollaboration, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                        }
                    }
                }

                var conversations = existCollaboration.ToList().Select(c => new { c.Receiver }).Distinct();

                foreach (var conversation in conversations)
                {
                    conversation.Receiver.LocalName =
                        conversation.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();

                    Collaboration collaboration =
                        new Collaboration { Receiver = conversation.Receiver };

                    collaborations.Add(collaboration);
                }

                return collaborations;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<CollaborationUserInfo> GetAllCollaborationUsers(int userId, string cultureName)
        {
            try
            {
                IList<CollaborationUserInfo> collaborationUserInfos = _oMCSDbContext.UserProfiles
                                                                            .Where(u => u.IsActive == true && u.IsDeleted == false)
                                                                            .Select(userProfile => new
                                                                            {
                                                                                userProfile.Id,
                                                                                userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                                NotificationCount = (from collaboration in _oMCSDbContext.Collaborations where collaboration.ReceiverId == userId && collaboration.SenderId == userProfile.Id && collaboration.Status == Common.CollaborationMessageStatus.Unread select collaboration).Count()
                                                                            }).ToList().Select(c => new CollaborationUserInfo()
                                                                            {
                                                                                UserId = c.Id,
                                                                                UserName = c.Text != null ? c.Text : string.Empty,
                                                                                NotificationCount = c.NotificationCount
                                                                            }).ToList();

                return collaborationUserInfos;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Collaboration> SortByUser(IQueryable<Collaboration> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from collaboration in source.Where(c => c.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select collaboration);
                case FilterType.EndsWidth:
                    return (from collaboration in source.Where(c => c.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select collaboration);
                case FilterType.StartsWith:
                    return (from collaboration in source.Where(c => c.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select collaboration);
                case FilterType.Equals:
                    return (from collaboration in source.Where(c => c.Receiver.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select collaboration);
            }

            return source;
        }

        private IQueryable<Collaboration> SortTransactionByNumber(IQueryable<Collaboration> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Number.ToString().Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Number.ToString().EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Number.ToString().StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Number.ToString().Equals(textValue));
            }

            return source;
        }

        #endregion
    }
}


