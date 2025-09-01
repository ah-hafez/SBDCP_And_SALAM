using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service
{
    public static class ConversationMapper
    {
        public static ConversationDTO Map(Collaboration conversation)
        {
            if (conversation != null)
            {
                ConversationDTO conversationDTO = new ConversationDTO()
                {
                    Id = conversation.Id,
                    ReceiverName = conversation.Receiver.LocalName,
                    SenderName = conversation.Sender.LocalName,
                    Text = conversation.Text,
                    SenderId = conversation.Sender.Id,
                    DateH = conversation.DateH,
                    Date = conversation.Date.ToString()
                };

                return conversationDTO;
            }

            return null;
        }

        public static List<ConversationDTO> Map(IList<Collaboration> conversations)
        {

            if (conversations == null || !conversations.Any())
            {
                return null;
            }
            List<ConversationDTO> conversationDTOs = new List<ConversationDTO>();

            foreach (Collaboration conversation in conversations)
            {
                conversationDTOs.Add(Map(conversation));
            }

            return conversationDTOs;
        }

        public static ChatNotificationsInfoDTO Map(ChatNotificationsInfo chatNotificationsInfo)
        {
            if (chatNotificationsInfo != null)
            {
                ChatNotificationsInfoDTO chatNotificationsInfoDTO = new ChatNotificationsInfoDTO()
                {
                    TotalChatNotifications = chatNotificationsInfo.TotalUserNotifications
                };

                return chatNotificationsInfoDTO;
            }

            return null;
        }

        public static CollaborationUserInfoDTO Map(CollaborationUserInfo collaborationUserInfo)
        {
            if (collaborationUserInfo != null)
            {
                CollaborationUserInfoDTO collaborationUserInfoDTO = new CollaborationUserInfoDTO()
                {
                    NotificationCount = collaborationUserInfo.NotificationCount,
                    UserId = collaborationUserInfo.UserId,
                    UserName = collaborationUserInfo.UserName,
                    Status = collaborationUserInfo.Status,
                    OrgUnitId = collaborationUserInfo.OrgUnitId
                };

                return collaborationUserInfoDTO;
            }

            return null;
        }

        public static List<CollaborationUserInfoDTO> Map(IList<CollaborationUserInfo> collaborationUserInfos)
        {
            if (collaborationUserInfos == null || !collaborationUserInfos.Any())
            {
                return null;
            }

            List<CollaborationUserInfoDTO> collaborationUserInfoDTOs = new List<CollaborationUserInfoDTO>();

            foreach (CollaborationUserInfo collaborationUserInfo in collaborationUserInfos)
            {
                collaborationUserInfoDTOs.Add(ConversationMapper.Map(collaborationUserInfo));
            }

            return collaborationUserInfoDTOs;
        }
    }
}