using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service
{
    //NotDone
    public class NotificationMapper
    {
        public static List<NotificationDTO> Map(IList<Notification> notifications, string cultureName)
        {
            if (notifications == null || !notifications.Any())
            {
                return new List<NotificationDTO>();
            }
            List<NotificationDTO> notificationsDTOs = new List<NotificationDTO>();

            foreach (var notification in notifications)
            {
                notificationsDTOs.Add(NotificationMapper.MapDetails(notification, cultureName));
            }

            return notificationsDTOs;
        }

        private static NotificationDTO MapDetails(Notification notification, string cultureName)
        {
            if (notification == null)
                return null;
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            NotificationDetail detail = notification.Details.Where(nd => nd.NotificationType.Id == NotificationType.Web.LookupIdentity(LookupCategory.NotificationType, cultureName)).FirstOrDefault();
            NotificationDTO notificationDTO = new NotificationDTO();

            notificationDTO.Id = notification.Id;

            if (detail != null)
            {
                if (detail.CreatedBy.HasValue && detail.CreatedBy.Value != -1)
                {
                    notificationDTO.Sender = userManagementBL.GetUserName(detail.CreatedBy.Value, cultureName);
                }
                notificationDTO.Date = DateTimeUtility.ConvertToUmAlQuraCalendar(detail.CreatedOn);
                notificationDTO.Subject = detail.Subject;
                notificationDTO.Body = detail.Body;
                notificationDTO.Link = detail.Link;
                notificationDTO.IsRead = notification.IsRead;
                if (detail.NotificationTemplateType != null)
                {
                    notificationDTO.NotificationTemplateTypeId = detail.NotificationTemplateType.Id;
                }
            }

            return notificationDTO;
        }
    }
}