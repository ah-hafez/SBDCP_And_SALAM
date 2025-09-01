using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface INotificationBL
    {
        void SendNotification(Notification notification, Dictionary<string, string> labels);
        //NotificationTemplate GetNotificationTemplate(NotificationTemplateType notificationTemplateType);
        IList<Notification> GetNotifications(SearchCriteria searchCriteria, bool isRead, string CultureName, out int rowsCount);
        void DeleteNotifications(IList<int> ids);
        void SendFollowUpNotification(Transaction transaction, NotificationTemplateType notificationTemplateType, NotificationWebSubject notificationWebSubject, NotificationUser notificationUser, string cultureName);
        void MarkAsReadNotification(IList<int> ids);
        List<NotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType);
        void UpdateNotifactionDetails(IList<NotificationDetail> tenantNotificationDetail);
        void SendAssignmentNotification(Transaction transaction, IList<TransactionAssignment> transactionAssignments, string cultureName = "ar");
        void SendTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
              NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
                          IList<NotificationUser> notificationUsers, string cultureName);
    }
}
