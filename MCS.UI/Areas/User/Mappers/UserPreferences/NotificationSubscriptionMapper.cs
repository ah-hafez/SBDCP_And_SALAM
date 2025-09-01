using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers.UserPreferences
{
    public static class NotificationSubscriptionMapper
    {
        public static List<NotificationSubscriptionVM> Map(IList<NotificationSubscriptionDTO> notificationSubscriptionDTOs)
        {
            if (notificationSubscriptionDTOs == null || !notificationSubscriptionDTOs.Any())
            {
                return new List<NotificationSubscriptionVM>();
            }
            List<NotificationSubscriptionVM> notificationSubscriptionVMs = notificationSubscriptionDTOs
                .Select(notificationSubscriptionDTO => new NotificationSubscriptionVM()
                {
                    Id = notificationSubscriptionDTO.Id,
                    IsSelected = notificationSubscriptionDTO.IsSelected,
                    Name = notificationSubscriptionDTO.Name
                }).ToList();

            return notificationSubscriptionVMs;
        }
        public static List<NotificationSubscriptionDTO> Map(IList<NotificationSubscriptionVM> notificationSubscriptionVMs)
        {
            if (notificationSubscriptionVMs == null || !notificationSubscriptionVMs.Any())
            {
                return new List<NotificationSubscriptionDTO>();
            }
            List<NotificationSubscriptionDTO> notificationSubscriptionDTOs = notificationSubscriptionVMs
                .Select(notificationSubscriptionVM => new NotificationSubscriptionDTO()
                {
                    Id = notificationSubscriptionVM.Id,
                    IsSelected = notificationSubscriptionVM.IsSelected,
                    Name = notificationSubscriptionVM.Name
                }).ToList();

            return notificationSubscriptionDTOs;
        }
    }
}