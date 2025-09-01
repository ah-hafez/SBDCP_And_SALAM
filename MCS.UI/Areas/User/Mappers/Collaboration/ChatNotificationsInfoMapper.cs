using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Collaboration;

namespace MCS.UI.Areas.User.Mappers.Collaboration
{
    public static class ChatNotificationsInfoMapper
    {
        public static List<ChatNotificationsInfoVM> Map(IList<ChatNotificationsInfoDTO> chatNotificationsInfoDTOs)
        {
            if (chatNotificationsInfoDTOs == null || !chatNotificationsInfoDTOs.Any())
            {
                return new List<ChatNotificationsInfoVM>();
            }
            List<ChatNotificationsInfoVM> chatNotificationsInfoVMs = chatNotificationsInfoDTOs
                .Select(chatNotificationsInfoDTO => new ChatNotificationsInfoVM()
                { 
                    TotalChatNotifications = chatNotificationsInfoDTO.TotalChatNotifications
                }).ToList();

            return chatNotificationsInfoVMs;
        }

        public static List<ChatNotificationsInfoDTO> Map(IList<ChatNotificationsInfoVM> chatNotificationsInfoVMs)
        {
            if (chatNotificationsInfoVMs == null || !chatNotificationsInfoVMs.Any())
            {
                return new List<ChatNotificationsInfoDTO>();
            }
            List<ChatNotificationsInfoDTO> chatNotificationsInfoDTOs = chatNotificationsInfoVMs
                .Select(chatNotificationsInfoVM => new ChatNotificationsInfoDTO()
                { 
                    TotalChatNotifications = chatNotificationsInfoVM.TotalChatNotifications
                }).ToList();
            return chatNotificationsInfoDTOs;
        }
    }
}