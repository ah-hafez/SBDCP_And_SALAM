using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface INotificationRepository : IRepository<Notification>
    {
        int AddNotification(Notification notification);
        // NotificationTemplate GetNotificationTemplate(NotificationTemplateType notificationTemplateType);
        IList<Notification> GetNotifications(Expression<Func<Notification, bool>> where, SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        void DeleteNotification(int id,int userId);
        void MarkAsReadNotification(int id);
        List<NotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType);
        void UpdateNotifactionDetails(IList<NotificationDetail> notificationDetail);
    }
}
