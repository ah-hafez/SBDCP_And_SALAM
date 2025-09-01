using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface IChatRepository
    {
        void JoinRoom(int userId, int roomId);
        void AddUserRoom(int userId, int roomId);
        void UpdateUserActivity(int userId, DateTimeOffset lastActivity, UserStatus status);
        void AddClient(int userId, string clientId, string userAgent, DateTimeOffset userLastActivity);
        void LeaveRoom(int userId, int roomId);
        void RemoveUserRoom(UserProfile user, ChatRoom room);
        ChatMessage AddMessage(int userId, int roomId, string content);
        ChatMessage AddMessage(ChatMessage chatMessage);
        ChatMessage DeleteMessage(int messageId);
        List<string> GetAllowedClientIds(List<int> allowedUserKeys);
        ChatMessage AddMessage(int userId, string roomName, string content);
        ChatRoom VerifyUserRoom(UserProfile user, string roomName);
        void AppendMessage(int id, string content);
        void AddOwner(UserProfile ownerOrCreator, UserProfile targetUser, ChatRoom targetRoom);
        void RemoveOwner(UserProfile targetUser, ChatRoom targetRoom);
        string DisconnectClient(string clientId);
        void AllowUser(UserProfile targetUser, ChatRoom targetRoom);
        void UnallowUser(UserProfile user, UserProfile targetUser, ChatRoom targetRoom);
        void LockRoom(UserProfile user, ChatRoom targetRoom);
        void CloseRoom(UserProfile user, ChatRoom targetRoom);
        void OpenRoom(UserProfile user, ChatRoom targetRoom);
        void AddAdmin(UserProfile admin, UserProfile targetUser);
        void RemoveAdmin(UserProfile admin, UserProfile targetUser);
        void BanUser(UserProfile admin, UserProfile targetUser);
        void UnbanUser(UserProfile admin, UserProfile targetUser);
        void ChangeStatus(int userId, UserStatus status);
        ChatMessage GetMessageById(int id);
        IQueryable<ChatRoom> GetAllowedRooms(int userId);
        List<ChatMessage> GetMessagesByRoom(string roomName);
        List<ChatMessage> GetMessagesByRoomId(int roomId);
        List<ChatMessage> GetPreviousMessages(int messageId);
        ChatClient GetClientById(string clientId, bool includeUser = false);
        bool IsUserInRoom(int userId, int roomId);
        object Reload(object entity);
        ChatMessagesStatus GetMessagesStatus(int userId, int roomId);
        List<UserProfile> GetOnlineUsers(int roomId);
        List<UserProfile> GetOnlineOwners(int roomId);
        List<ChatMessagesStatus> GetMessagesStatus(int roomId);
        void UpdateMessageStatus(int userId, int roomId, int messageId);
        ChatMessagesStatus AddMessageStatus(ChatMessagesStatus messagesStatus);
        ChatRoom GetRoomByName(string roomName);
        void BindRoomTransaction(int roomId, int transactionId);
        ChatRoom AddOneToOneRoom(int callingUserId, int toUserId, string roomName, int? transactionId);
        bool IsUserAllowed(ChatRoom room, UserProfile user);
        List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, int pageIndex, int pageSize, out int itemsCount);
        List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, string roomName);
        ChatMessage GetLastMessageInRoom(int roomId);
        ChatMessagesStatus GetLastMessageReadInRoom(int userId, int roomId);
        int TotalNumberOfUnreadMessages(ChatMessagesStatus lastMessageReadInRoom, int roomId);
        bool IsUnreadMessagesForUser(int userId);
        ChatClient GetClientById(string clientId);
        ChatClient GetClientByIdWithUser(string clientId);
        string GetUserRoomPresence(int userId, int roomId);
        IList<CollaborationUserInfo> GetAllCollaborationUsers(string userName, int pageIndex, int pageSize, string cultureName, out int itemsCount);
        List<int> GetOfflineOrInactiveUsersInRoom(int userId, int roomId, int messageId);
    }
}
