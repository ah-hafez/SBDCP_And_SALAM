using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class NotificationSubscriptionMapper
    {
        public static NotificationSubscriptions Map(IList<NotificationSubscriptionDTO> notificationSubscriptionDTOs)
        {
            if (notificationSubscriptionDTOs == null || !notificationSubscriptionDTOs.Any())
            {
                return NotificationSubscriptions.None;
            }
            NotificationSubscriptions notificationSubscriptions = NotificationSubscriptions.None;

            foreach (NotificationSubscriptionDTO notificationSubscriptionDTO in notificationSubscriptionDTOs)
            {
                if (notificationSubscriptionDTO.IsSelected)
                {
                    notificationSubscriptions = notificationSubscriptions ^ (NotificationSubscriptions)notificationSubscriptionDTO.Id;
                }
            }

            return notificationSubscriptions;
        }

        public static List<NotificationSubscriptionDTO> Map(NotificationSubscriptions notificationSubscriptions, string cultureName)
        {
            if (notificationSubscriptions == NotificationSubscriptions.None)
            {
                return null;
            }
            List<NotificationSubscriptionDTO> notificationSubscriptionDTOs = new List<NotificationSubscriptionDTO>();

            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

            foreach (NotificationSubscriptions notificationSubscription in Enum.GetValues(typeof(NotificationSubscriptions)))
            {
                if (notificationSubscription == NotificationSubscriptions.None)
                    continue;

                NotificationSubscriptionDTO notificationSubscriptionDTO = new NotificationSubscriptionDTO();

                Domain.Lookup lkup = lookupBL.GetLookupItem((int)EnumMapper.GetNotificationSubscription(notificationSubscription), cultureName);

                if (lkup != null)
                {
                    notificationSubscriptionDTO.Name = lkup.Text;

                    notificationSubscriptionDTO.Id = (int)notificationSubscription;

                    if (Convert.ToBoolean((NotificationSubscriptions)notificationSubscriptions & notificationSubscription))
                    {
                        notificationSubscriptionDTO.IsSelected = true;
                    }

                    notificationSubscriptionDTOs.Add(notificationSubscriptionDTO);
                }
            }

            return notificationSubscriptionDTOs;
        }

    }
}