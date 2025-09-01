using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.DataAccess
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public NotificationRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddNotification(Notification notification)
        {
            try
            {
                _oMCSDbContext.Notifications.Add(notification);
                _oMCSDbContext.SaveChanges();

                return notification.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        //   public NotificationTemplate GetNotificationTemplate(NotificationTemplateType notificationTemplateType)
        //{
        //    try
        //    {
        //        int notificationTemplateTypeId = notificationTemplateType.LookupIdentity(LookupCategory.NotificationTemplateType, string.Empty);
        //        return this.FindBy(a => a.TypeId == notificationTemplateTypeId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw DataAccessException.Translate(ex);
        //    }
        //}

        public IList<Notification> GetNotifications(Expression<Func<Notification, bool>> @where, SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IQueryable<Notification> notifications = (from Notification in _oMCSDbContext.Notifications.Where(@where)

                                                          select Notification);

                if (searchCriteria != null)
                {
                    if (searchCriteria.FromDateTime.HasValue)
                    {
                        notifications = notifications.Where(n => n.Date >= searchCriteria.FromDateTime.Value);
                    }

                    if (searchCriteria.ToDateTime.HasValue)
                    {
                        notifications = notifications.Where(n => n.Date <= searchCriteria.ToDateTime.Value);
                    }

                    rowsCount = notifications.Where(@where).Count();

                    if (searchCriteria.Ascending)
                    {
                        notifications = notifications.OrderBy(n => n.Date)
                            .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                            .Take(searchCriteria.PageSize);
                    }
                    else
                    {
                        notifications = notifications.OrderByDescending(n => n.Date)
                            .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                            .Take(searchCriteria.PageSize);
                    }

                    return notifications.ToList().Select(n => new Notification()
                    {
                        SourceId = n.SourceId,
                        CreatedBy = n.CreatedBy,
                        Date = n.Date,
                        DateH = n.DateH,
                        CreatedOn = n.CreatedOn,
                        Id = n.Id,
                        IsRead = n.IsRead,
                        ModefiedBy = n.ModefiedBy,
                        ModefiedOn = n.ModefiedOn,
                        Users = (n.Users != null) ? n.Users.Select(a => new NotificationUser
                        {
                            UserId = a.UserId,
                            User = (a.User != null) ? new UserProfile
                            {
                                Id = a.User.Id,
                                LocalName = a.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,

                        }).ToList() : null,
                        Source = (n.Source != null) ? new Lookup
                        {
                            Id = n.Source.Id,
                            Text = n.Source.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        Details = (n.Details != null) ? n.Details.Select(d => new NotificationDetail
                        {
                            Id = d.Id,
                            Body = d.Body,
                            CreatedBy = d.CreatedBy,
                            CreatedOn = d.CreatedOn,
                            Email = d.Email,
                            IsSent = d.IsSent,
                            Link = d.Link,
                            FailureCount = d.FailureCount,
                            ModefiedBy = d.ModefiedBy,
                            ModefiedOn = d.ModefiedOn,
                            Subject = d.Subject,
                            NotificationTemplateType = (d.NotificationTemplateType != null) ? new Lookup
                            {
                                Id = d.NotificationTemplateType.Id,
                                Text = d.NotificationTemplateType.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                            } : null,
                            NotificationType = (d.NotificationType != null) ? new Lookup
                            {
                                Id = d.NotificationType.Id,
                                Text = d.NotificationType.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                            } : null,
                            Attachments = (d.Attachments != null) ? d.Attachments.Select(a => new NotificationAttachment
                            {
                                Id = a.Id,
                                Binary = a.Binary,
                                ContentLength = a.ContentLength,
                                ContentType = a.ContentType,
                                CreatedBy = a.CreatedBy,
                                CreatedOn = a.CreatedOn,
                                FileName = a.FileName,
                                ModefiedBy = a.ModefiedBy,
                                ModefiedOn = a.ModefiedOn
                            }).ToList() : null

                        }).ToList() : null


                    }).ToList();
                }
                rowsCount = 0;
                return null;




            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteNotification(int id, int userId)
        {
            try
            {
                Notification notification = _oMCSDbContext.Notifications.Where(n => n.Id == id).FirstOrDefault();
                if (notification != null)
                {
                    NotificationUser notificationUser = notification.Users.Where(u => u.UserId == userId).FirstOrDefault();
                    if (notificationUser != null)
                    {
                        _oMCSDbContext.NotificationUsers.Remove(notificationUser);
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void MarkAsReadNotification(int id)
        {
            try
            {
                Notification notification = _oMCSDbContext.Notifications.Where(n => n.Id == id).FirstOrDefault();
                if (notification != null)
                {
                    notification.IsRead = true;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<NotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType)
        {
            int notificationTypeId = notificationType.LookupIdentity(LookupCategory.NotificationType, string.Empty);
            return _oMCSDbContext.NotificationDetails.Where(x => x.IsSent == false &&
                        x.FailureCount <= failureCount && x.NotificationType.Id == notificationTypeId).Include(a => a.Attachments).ToList();
        }

        public void UpdateNotifactionDetails(IList<NotificationDetail> notificationDetail)
        {
            try
            {
                foreach (var item in notificationDetail)
                {
                    var row = _oMCSDbContext.NotificationDetails.Where(l => l.Id == item.Id).FirstOrDefault();
                    if (row != null)
                    {
                        row.IsSent = item.IsSent;
                        row.FailureCount = item.FailureCount;
                        _oMCSDbContext.Entry(row).State = EntityState.Modified;
                        _oMCSDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion
    }
}
