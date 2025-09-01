using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Notifications;

namespace MCS.UI.Areas.User.Mappers.Notification
{
    public static class NotificationMapper
    {
        public static List<NotificationVM> Map(IList<NotificationDTO> notificationDTOs)
        {
            if (notificationDTOs == null || !notificationDTOs.Any())
            {
                return new List<NotificationVM>();
            }
            List<NotificationVM> notificationVMs = notificationDTOs
                .Select(b => new NotificationVM()
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Body = b.Body,
                    Link = b.Link,
                    Date = b.Date,
                    Sender = b.Sender,
                    IsRead = b.IsRead,
                    NotificationTemplateTypeId = b.NotificationTemplateTypeId
                }).ToList();

            return notificationVMs;
        }
        public static List<NotificationDTO> Map(IList<NotificationVM> notificationVMs)
        {
            if (notificationVMs == null || !notificationVMs.Any())
            {
                return new List<NotificationDTO>();
            }
            List<NotificationDTO> notificationDTOs = notificationVMs
                .Select(b => new NotificationDTO()
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Body = b.Body,
                    Link = b.Link,
                    Date = b.Date,
                    Sender = b.Sender,
                    IsRead = b.IsRead,
                    NotificationTemplateTypeId = b.NotificationTemplateTypeId
                }).ToList();

            return notificationDTOs;
        }
    }
}