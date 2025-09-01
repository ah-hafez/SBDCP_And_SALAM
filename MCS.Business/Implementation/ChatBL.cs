using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class ChatBL : BaseBL, IChatBL
    {
        public static void JoinRoom(int userId, int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.JoinRoom(userId, roomId);
        }
        public static ChatMessage GetMessageById(int id)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetMessageById(id);
        }
        public static ChatMessagesStatus GetMessagesStatus(int userId, int roomId)
        {

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetMessagesStatus(userId, roomId);
        }
        public static object Reload(object entity)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.Reload(entity);
        }
        public static ChatMessagesStatus AddMessageStatus(ChatMessagesStatus messagesStatus)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.AddMessageStatus(messagesStatus);
        }
        public static void UpdateMessageStatus(int userId, int roomId, int messageId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.UpdateMessageStatus(userId, roomId, messageId);
        }
        public static List<ChatMessagesStatus> GetMessagesStatus(int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetMessagesStatus(roomId);
        }
        public static List<UserProfile> GetOnlineUsers(int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetOnlineUsers(roomId);
        }
        public static List<UserProfile> GetOnlineOwners(int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetOnlineOwners(roomId);
        }

        public static string GetUserRoomPresence(int userId, int roomId)
        {

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetUserRoomPresence(userId, roomId);
        }


        public static ChatRoom GetRoomByName(string roomName)
        {
            roomName = NormalizeRoomName(roomName);

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            ChatRoom room = chatRepository.GetRoomByName(roomName);
            return room;
        }

        public static void BindRoomsTransaction(string[] roomIds, int transactionId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            foreach (var item in roomIds)
            {
                chatRepository.BindRoomTransaction(Convert.ToInt32(item), transactionId);
            }
        }

        public static ChatRoom VerifyUserRoom(int userId, string roomName)
        {
            roomName = NormalizeRoomName(roomName);

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            ChatRoom room = chatRepository.GetRoomByName(roomName);


            if (room == null)
            {
                return null;
                //throw new HubException(String.Format(LanguageResources.RoomMemberButNotExists, roomName));
            }

            if (!chatRepository.IsUserInRoom(userId, room.Id))
            {
                return null;
                //throw new HubException(String.Format(LanguageResources.RoomNotMember, roomName));
            }
            return room;
        }

        public static bool IsUserInRoom(int userId, int roomId)
        {

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.IsUserInRoom(userId, roomId);
        }

        public static List<ChatMessage> GetMessagesByRoom(string roomName)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetMessagesByRoom(roomName);
        }

        public static List<ChatMessage> GetMessagesByRoomId(int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetMessagesByRoomId(roomId);
        }

        public static List<ChatMessage> GetPreviousMessages(int messageId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetPreviousMessages(messageId);
        }

        public static ChatRoom AddOneToOneRoom(int callingUserId, int toUserId, string roomName, int? transactionId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.AddOneToOneRoom(callingUserId, toUserId, roomName, transactionId);
        }
        public static List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, int pageIndex, int pageSize, out int itemsCount)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetConversations(userId, toUserId, transactionId, pageIndex, pageSize, out itemsCount);
        }
        public static List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, string roomName)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetConversations(userId, toUserId, transactionId, roomName);
        }
        public static ChatMessage GetLastMessageInRoom(int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetLastMessageInRoom(roomId);
        }
        public static ChatMessagesStatus GetLastMessageReadInRoom(int userId, int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetLastMessageReadInRoom(userId, roomId);
        }
        public static int TotalNumberOfUnreadMessages(ChatMessagesStatus lastMessageReadInRoom, int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.TotalNumberOfUnreadMessages(lastMessageReadInRoom, roomId);
        }
        public static bool IsUnreadMessagesForUser(int userId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.IsUnreadMessagesForUser(userId);
        }
        public static bool IsUserAllowed(ChatRoom room, UserProfile user)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.IsUserAllowed(room, user);
        }
        public static void UpdateUserActivity(int userId, DateTimeOffset lastActivity, UserStatus status)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.UpdateUserActivity(userId, lastActivity, status);
        }
        public static void UpdateActivity(int userId, string clientId, string userAgent, DateTimeOffset userLastActivity)
        {
            AddClient(userId, clientId, userAgent, userLastActivity);
            UpdateUserActivity(userId, userLastActivity, UserStatus.Active);
        }
        public static void AddClient(int userId, string clientId, string userAgent, DateTimeOffset userLastActivity)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.AddClient(userId, clientId, userAgent, userLastActivity);
        }
        public static void LeaveRoom(int userId, int roomId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.LeaveRoom(userId, roomId);
        }
        public static List<string> GetAllowedClientIds(List<int> allowedUserKeys)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetAllowedClientIds(allowedUserKeys);
        }
        public static ChatRoom VerifyRoom(string roomName, bool mustBeOpen = true)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            roomName = NormalizeRoomName(roomName);

            var room = chatRepository.GetRoomByName(roomName);

            if (room == null)
            {
                return null;
                //throw new HubException(String.Format(LanguageResources.RoomNotFound, roomName));
            }

            if (room.Closed && mustBeOpen)
            {
                return null;
                //throw new HubException(String.Format(LanguageResources.RoomClosed, roomName));
            }

            return room;
        }
        public static ChatMessage AddMessage(int userId, int roomId, string content)
        {

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.AddMessage(userId, roomId, content);
        }
        public static ChatMessage AddMessage(ChatMessage chatMessage)
        {

            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.AddMessage(chatMessage);
        }
        public static ChatMessage DeleteMessage(int messageId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.DeleteMessage(messageId);
        }
        public static ChatMessage AddMessage(int userId, string roomName, string content)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.AddMessage(userId, roomName, content);
        }
        public static void AddNotification(int mentionedUserkey, int fromUserKey, int messageKey, int roomKey, string content, bool isDev = true)
        {
        }
        public static void AppendMessage(int id, string content)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.AppendMessage(id, content);
        }
        public static void KickUser(UserProfile callingUser, UserProfile targetUser, ChatRoom targetRoom)
        {
            LeaveRoom(targetUser.Id, targetRoom.Id);
        }
        public static string DisconnectClient(string clientId)
        {
            try
            {
                IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
                return chatRepository.DisconnectClient(clientId);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static void LockRoom(UserProfile user, ChatRoom targetRoom)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.LockRoom(user, targetRoom);
        }
        public static void CloseRoom(UserProfile user, ChatRoom targetRoom)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.CloseRoom(user, targetRoom);
        }
        public static void OpenRoom(UserProfile user, ChatRoom targetRoom)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.OpenRoom(user, targetRoom);
        }
        public static void AddAdmin(UserProfile admin, UserProfile targetUser)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.AddAdmin(admin, targetUser);
        }
        public static void RemoveAdmin(UserProfile admin, UserProfile targetUser)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.RemoveAdmin(admin, targetUser);
        }
        public static void ChangeStatus(int userId, UserStatus status)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            chatRepository.ChangeStatus(userId, status);
        }

        public static IList<CollaborationUserInfo> GetAllCollaborationUsers(string userName, int pageIndex, int pageSize, string cultureName, out int itemsCount)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetAllCollaborationUsers(userName, pageIndex, pageSize, cultureName, out itemsCount);
        }

        public static List<int> GetOfflineOrInactiveUsersInRoom(int userId, int roomId, int messageId)
        {
            IChatRepository chatRepository = IoC.Resolve<ChatRepository>();
            return chatRepository.GetOfflineOrInactiveUsersInRoom(userId, roomId, messageId);
        }

        private static bool IsValidRoomName(string name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, "^[\\w-_]{1,30}$");
        }
        //internal static string GetUserRoomPresence(User user, ChatRoom room)
        //{
        //    return user.Rooms.Contains(room) ? "present" : "absent";
        //}
        internal static string NormalizeRoomName(string roomName)
        {
            return roomName.StartsWith("#") ? roomName.Substring(1) : roomName;
        }
    }
}

