using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ChatRepository : BaseRepository<ChatClient>, IChatRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public ChatRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public void JoinRoom(int userId, int roomId)
        {
            // Add this user to the room
            // Add this user to the room
            AddUserRoom(userId, roomId);
        }

        public void AddUserRoom(int userId, int roomId)
        {
            if (!_oMCSDbContext.ChatRoomUsers.Any(a => a.UserId == userId && a.RoomId == roomId))
            {
                _oMCSDbContext.ChatRoomUsers.Add(new ChatRoomUser() { RoomId = roomId, UserId = userId });
                _oMCSDbContext.SaveChanges();
            }
        }

        public void UpdateUserActivity(int userId, DateTimeOffset lastActivity, UserStatus status)
        {
            var user = _oMCSDbContext.UserProfiles.FirstOrDefault(a => a.Id == userId);
            if (user != null)
            {
                user.LastActivity = lastActivity;
                user.Status = (int)status;
                _oMCSDbContext.SaveChanges();
            }
        }

        public void AddClient(int userId, string clientId, string userAgent, DateTimeOffset userLastActivity)
        {
            ChatClient client = GetClientById(clientId);
            if (client != null)
            {
                return;
            }
            client = new ChatClient
            {
                UserId = userId,
                UserAgent = userAgent,
                LastActivity = DateTimeOffset.UtcNow,
                LastClientActivity = userLastActivity,
                ConnectionId = clientId
            };
            _oMCSDbContext.ChatClients.Add(client);
            _oMCSDbContext.SaveChanges();
        }

        public void LeaveRoom(int userId, int roomId)
        {
            var roomUser = _oMCSDbContext.ChatRoomUsers.FirstOrDefault(a => a.UserId == userId && a.RoomId == roomId);
            if (roomUser != null)
            {
                _oMCSDbContext.ChatRoomUsers.Remove(roomUser);
                _oMCSDbContext.SaveChanges();
            }
        }
        public void RemoveUserRoom(UserProfile user, ChatRoom room)
        {
            var removedUser = _oMCSDbContext.ChatRoomUsers.FirstOrDefault(a => a.RoomId == room.Id && a.UserId == user.Id);
            if (removedUser != null)
            {
                _oMCSDbContext.ChatRoomUsers.Remove(removedUser);
                _oMCSDbContext.SaveChanges();
            }
        }
        public ChatMessage AddMessage(int userId, int roomId, string content)
        {


            var chatMessage = new ChatMessage
            {
                UserId = userId,
                Content = content,
                When = DateTimeOffset.UtcNow,
                RoomId = roomId,
                HtmlEncoded = false,
            };

            // _recentMessageCache.Add(chatMessage);

            _oMCSDbContext.ChatMessages.Add(chatMessage);
            _oMCSDbContext.SaveChanges();

            return chatMessage;
        }
        public ChatMessage AddMessage(ChatMessage chatMessage)
        {
            _oMCSDbContext.ChatMessages.Add(chatMessage);
            _oMCSDbContext.SaveChanges();
            return chatMessage;
        }
        public ChatMessage DeleteMessage(int messageId)
        {
            var message = _oMCSDbContext.ChatMessages.FirstOrDefault(a => a.Id == messageId);
            if (message != null)
            {
                message.Content = "";
                _oMCSDbContext.SaveChanges();
            }

            return message;
        }
        public List<string> GetAllowedClientIds(List<int> allowedUserKeys)
        {
            return _oMCSDbContext.ChatClients.Where(c => allowedUserKeys.Contains(c.UserId)).Select(c => c.ConnectionId).ToList();
        }


        public ChatMessage AddMessage(int userId, string roomName, string content)
        {
            var user = _oMCSDbContext.UserProfiles.FirstOrDefault(u => u.Id == userId);
            ChatRoom room = VerifyUserRoom(user, roomName);

            // REVIEW: Is it better to use room.EnsureOpen() here?
            if (room.Closed)
            {
                // throw new HubException(String.Format(LanguageResources.SendMessageRoomClosed, roomName));
            }

            var message = AddMessage(user.Id, room.Id, content);

            _oMCSDbContext.SaveChanges();

            return message;
        }

        public ChatRoom VerifyUserRoom(UserProfile user, string roomName)
        {
            if (String.IsNullOrEmpty(roomName))
            {
                // throw new HubException(LanguageResources.RoomJoinMessage);
            }

            //roomName = ChatService.NormalizeRoomName(roomName);

            ChatRoom room = _oMCSDbContext.ChatRooms.FirstOrDefault(r => r.Name == roomName);

            if (room == null)
            {
                //throw new HubException(String.Format(LanguageResources.RoomMemberButNotExists, roomName));
            }

            //if (!repository.IsUserInRoom(cache, user, room))
            //{
            //    throw new HubException(String.Format(LanguageResources.RoomNotMember, roomName));
            //}

            return room;
        }

        public void AppendMessage(int id, string content)
        {
            ChatMessage message = GetMessageById(id);

            message.Content += content;

            _oMCSDbContext.SaveChanges();
        }

        public void AddOwner(UserProfile ownerOrCreator, UserProfile targetUser, ChatRoom targetRoom)
        {
            if (!_oMCSDbContext.ChatRoomUsers.Any(a => a.UserId == targetUser.Id && a.RoomId == targetRoom.Id))
            {
                _oMCSDbContext.ChatRoomUsers.Add(new ChatRoomUser() { RoomId = targetRoom.Id, UserId = targetUser.Id });
            }
            // Make the user an owner
            if (targetRoom.Private)
            {
                if (!_oMCSDbContext.ChatRoomAllowedUsers.Any(a => a.UserId == targetUser.Id && a.RoomId == targetRoom.Id))
                {
                    _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = targetRoom.Id, UserId = targetUser.Id });

                }

            }
            _oMCSDbContext.SaveChanges();
        }

        public void RemoveOwner(UserProfile targetUser, ChatRoom targetRoom)
        {
            var removedUser = _oMCSDbContext.ChatRoomOwners.FirstOrDefault(a => a.RoomId == targetRoom.Id && a.UserId == targetUser.Id);
            if (removedUser != null)
            {
                // Remove user as owner of room
                _oMCSDbContext.ChatRoomOwners.Remove(removedUser);
                _oMCSDbContext.SaveChanges();
            }

        }

        public string DisconnectClient(string clientId)
        {
            // Remove this client from the list of user's clients
            ChatClient client = GetClientById(clientId, includeUser: true);

            // No client tracking this user
            if (client == null)
            {
                return null;
            }

            // Get the user for this client
            var user = client.User;

            if (user != null)
            {
                user.ConnectedClients.Remove(client);

                if (!user.ConnectedClients.Any())
                {
                    // If no more clients mark the user as offline
                    user.Status = (int)UserStatus.Offline;
                }
                _oMCSDbContext.ChatClients.Remove(client);
                _oMCSDbContext.SaveChanges();
            }

            return user.IdentityId;
        }

        public void AllowUser(UserProfile targetUser, ChatRoom targetRoom)
        {
            if (!_oMCSDbContext.ChatRoomAllowedUsers.Any(a => a.UserId == targetUser.Id && a.RoomId == targetRoom.Id))
            {
                _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = targetRoom.Id, UserId = targetUser.Id });

            }
            _oMCSDbContext.SaveChanges();
        }

        public void UnallowUser(UserProfile user, UserProfile targetUser, ChatRoom targetRoom)
        {
            var removedUser = _oMCSDbContext.ChatRoomAllowedUsers.FirstOrDefault(a => a.RoomId == targetRoom.Id && a.UserId == targetUser.Id);
            if (removedUser != null)
            {
                // Remove user as owner of room
                _oMCSDbContext.ChatRoomAllowedUsers.Remove(removedUser);
                _oMCSDbContext.SaveChanges();
            }

            // Make the user leave the room
            LeaveRoom(targetUser.Id, targetRoom.Id);

            _oMCSDbContext.SaveChanges();
        }

        public void LockRoom(UserProfile user, ChatRoom targetRoom)
        {
            targetRoom.Private = true;

            if (!_oMCSDbContext.ChatRoomAllowedUsers.Any(a => a.UserId == user.Id && a.RoomId == targetRoom.Id))
            {
                _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = targetRoom.Id, UserId = user.Id });

            }
            _oMCSDbContext.SaveChanges();
        }

        public void CloseRoom(UserProfile user, ChatRoom targetRoom)
        {

            // Make the room closed.
            targetRoom.Closed = true;

            _oMCSDbContext.SaveChanges();
        }

        public void OpenRoom(UserProfile user, ChatRoom targetRoom)
        {

            // Open the room
            targetRoom.Closed = false;
            _oMCSDbContext.SaveChanges();
        }

        public void AddAdmin(UserProfile admin, UserProfile targetUser)
        {

            // Make the user an admin
            //targetUser.IsAdmin = true;
            _oMCSDbContext.SaveChanges();
        }

        public void RemoveAdmin(UserProfile admin, UserProfile targetUser)
        {

            // Make the user an admin
            //targetUser.IsAdmin = false;
            _oMCSDbContext.SaveChanges();
        }

        public void BanUser(UserProfile admin, UserProfile targetUser)
        {
            //targetUser.IsBanned = true;
            _oMCSDbContext.SaveChanges();
        }

        public void UnbanUser(UserProfile admin, UserProfile targetUser)
        {
            //targetUser.IsBanned = false;
            _oMCSDbContext.SaveChanges();
        }

        public void ChangeStatus(int userId, UserStatus status)
        {
            var user = _oMCSDbContext.UserProfiles.FirstOrDefault(a => a.Id == userId);
            user.Status = (int)status;
            _oMCSDbContext.SaveChanges();
        }

        public static IEnumerable<UserProfile> Online(IEnumerable<UserProfile> source)
        {
            return source.Where(u => u.Status != (int)UserStatus.Offline);
        }

        public ChatMessage GetMessageById(int id)
        {
            return _oMCSDbContext.ChatMessages.Include(a => a.Room).FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<ChatRoom> GetAllowedRooms(int userId)
        {

            // All public and private rooms the user can see.
            return _oMCSDbContext.ChatRooms
                .Where(r =>
                       (!r.Private) ||
                       (r.Private && r.AllowedUsers.Any(u => u.UserId == userId)));
        }

        public List<ChatMessage> GetMessagesByRoom(string roomName)
        {
            return _oMCSDbContext.ChatMessages.Include(a => a.User).Where(r => r.Room.Name == roomName).OrderByDescending(m => m.When).Take(10)
                .ToList();
        }

        public List<ChatMessage> GetMessagesByRoomId(int roomId)
        {
            return _oMCSDbContext.ChatMessages.Include(a => a.User).Where(r => r.Room.Id == roomId).OrderByDescending(m => m.When).Take(10)
                .ToList();
        }

        public List<ChatMessage> GetPreviousMessages(int messageId)
        {
            var message = _oMCSDbContext.ChatMessages.FirstOrDefault(m => m.Id == messageId);
            return _oMCSDbContext.ChatMessages.Where(m => m.When < message.When && m.RoomId == message.RoomId).OrderByDescending(m => m.When).Take(10).ToList();
        }

        public void Remove(ChatClient client)
        {
            _oMCSDbContext.ChatClients.Remove(client);
            _oMCSDbContext.SaveChanges();
        }

        public ChatClient GetClientById(string clientId, bool includeUser = false)
        {
            if (includeUser)
            {
                return GetClientByIdWithUser(clientId);
            }

            return GetClientById(clientId);
        }

        public bool IsUserInRoom(int userId, int roomId)
        {
            return _oMCSDbContext.ChatRoomUsers.Any(a => a.UserId == userId && a.RoomId == roomId);
        }

        public object Reload(object entity)
        {
            _oMCSDbContext.Entry(entity).Reload();
            return entity;
        }

        public ChatMessagesStatus GetMessagesStatus(int userId, int roomId)
        {
            return _oMCSDbContext.MessagesStatus.FirstOrDefault(a => a.UserId == userId && a.RoomId == roomId);
        }

        public List<UserProfile> GetOnlineUsers(int roomId)
        {
            return _oMCSDbContext.ChatRoomUsers.Where(u => u.User.Status != (int)UserStatus.Offline && u.RoomId == roomId).Select(a => a.User).ToList();
        }

        public List<UserProfile> GetOnlineOwners(int roomId)
        {
            return _oMCSDbContext.ChatRoomOwners.Where(u => u.User.Status != (int)UserStatus.Offline && u.RoomId == roomId).Select(a => a.User).ToList();
        }

        public List<ChatMessagesStatus> GetMessagesStatus(int roomId)
        {
            return _oMCSDbContext.MessagesStatus.Where(a => a.RoomId == roomId).ToList();
        }

        public void UpdateMessageStatus(int userId, int roomId, int messageId)
        {
            var result = _oMCSDbContext.MessagesStatus.FirstOrDefault(c => c.UserId == userId && c.RoomId == roomId);
            result.MessageId = messageId;
            result.LastUpdatedDate = DateTime.UtcNow;
            _oMCSDbContext.SaveChanges();
        }

        public ChatMessagesStatus AddMessageStatus(ChatMessagesStatus messagesStatus)
        {
            _oMCSDbContext.MessagesStatus.Add(messagesStatus);
            _oMCSDbContext.SaveChanges();
            return messagesStatus;
        }

        public ChatRoom GetRoomByName(string roomName)
        {
            return _oMCSDbContext.ChatRooms.FirstOrDefault(r => r.Name == roomName);
        }

        public void BindRoomTransaction(int roomId, int transactionId)
        {
            var chatRoom = _oMCSDbContext.ChatRooms.FirstOrDefault(r => r.Id == roomId);
            chatRoom.TransactionId = transactionId;
            _oMCSDbContext.SaveChanges();

        }

        public ChatRoom AddOneToOneRoom(int callingUserId, int toUserId, string roomName, int? transactionId)
        {
            var room = new ChatRoom
            {
                Name = roomName,
                OneToOne = true,
                Private = true,
                TransactionId = transactionId
            };
            _oMCSDbContext.ChatRooms.Add(room);
            _oMCSDbContext.SaveChanges();
            if (callingUserId != toUserId)
            {
                _oMCSDbContext.ChatRoomOwners.Add(new ChatRoomOwner() { RoomId = room.Id, UserId = callingUserId });
                _oMCSDbContext.ChatRoomOwners.Add(new ChatRoomOwner() { RoomId = room.Id, UserId = toUserId });
                _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = room.Id, UserId = callingUserId });
                _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = room.Id, UserId = toUserId });
            }
            else
            {
                _oMCSDbContext.ChatRoomOwners.Add(new ChatRoomOwner() { RoomId = room.Id, UserId = callingUserId });
                _oMCSDbContext.ChatRoomAllowedUsers.Add(new ChatRoomAllowedUser() { RoomId = room.Id, UserId = callingUserId });

            }
            _oMCSDbContext.SaveChanges();
            return room;
        }

        public bool IsUserAllowed(ChatRoom room, UserProfile user)
        {
            return _oMCSDbContext.ChatRoomAllowedUsers.Any(a => a.UserId == user.Id && a.RoomId == room.Id);
        }

        public List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, int pageIndex, int pageSize, out int itemsCount)
        {
            var rooms = _oMCSDbContext.ChatRooms
                                        .Include(a => a.AllowedUsers)
                                        .Include(a => a.AllowedUsers.Select(c => c.User))
                                        .Where(a => (transactionId == null || a.TransactionId == transactionId) &&
                                                    a.OneToOne &&
                                                    (userId == -1 || a.AllowedUsers.Any(c => c.UserId == userId)) &&
                                                    (toUserId == null || a.AllowedUsers.Any(t => t.UserId == toUserId)) &&
                                                    (transactionId != null || (transactionId == null && !a.Name.Contains("TR"))) &&
                                                    a.Messages.Any());
            if (!rooms.Any())
            {
                itemsCount = 0;
            }
            itemsCount = rooms.Count();
            var skip = pageIndex * pageSize;
            return rooms.OrderByDescending(room => room.Messages.Max(message => message.When)).Skip(skip).Take(pageSize).ToList();
        }

        public List<ChatRoom> GetConversations(int userId, int? toUserId, int? transactionId, string roomName)
        {
            var rooms = _oMCSDbContext.ChatRooms
                                        .Include(a => a.AllowedUsers)
                                        .Include(a => a.AllowedUsers.Select(c => c.User))
                                        .Where(a => (transactionId == null || a.TransactionId == transactionId) &&
                                                    a.OneToOne &&
                                                    a.AllowedUsers.Any(c => c.UserId == userId) &&
                                                    (toUserId == null || a.AllowedUsers.Any(t => t.UserId == toUserId)) &&
                                                    (roomName == null || a.Name == roomName));

            return rooms.OrderByDescending(room => room.Messages.Max(message => message.When)).ToList();
        }

        public ChatMessage GetLastMessageInRoom(int roomId)
        {
            return _oMCSDbContext.ChatMessages.Where(c => c.RoomId == roomId).OrderByDescending(c => c.When).FirstOrDefault();
        }

        public ChatMessagesStatus GetLastMessageReadInRoom(int userId, int roomId)
        {
            return _oMCSDbContext.MessagesStatus.FirstOrDefault(c => c.UserId == userId && c.RoomId == roomId);
        }

        public int TotalNumberOfUnreadMessages(ChatMessagesStatus lastMessageReadInRoom, int roomId)
        {
            var messages = _oMCSDbContext.ChatMessages.Where(c => c.RoomId == roomId);
            return lastMessageReadInRoom == null ? messages.Count() : messages.Count(c => lastMessageReadInRoom.MessageId < c.Id);

        }

        public bool IsUnreadMessagesForUser(int userId)
        {
            var messages = _oMCSDbContext.MessagesStatus.Where(c => c.UserId == userId);
            if (messages.Any())
            {
                var unread = _oMCSDbContext.ChatRooms.Any(a => messages.Any(m => m.RoomId == a.Id && !a.Name.Contains("TR") && a.Messages.Any(c => c.Id > m.MessageId)));
                if (!unread)
                {
                    unread = _oMCSDbContext.ChatRooms.Any(a => a.AllowedUsers.Any(c => c.UserId == userId) && !a.Name.Contains("TR") && a.Messages.Any() && a.MessagesStatus.Any(c => c.RoomId != a.Id));
                }
                return unread;
            }
            else
            {
                return _oMCSDbContext.ChatRooms.Any(a => a.AllowedUsers.Any(c => c.UserId == userId) && !a.Name.Contains("TR") && a.Messages.Any());
            }
        }

        public ChatClient GetClientById(string clientId)
        {
            return _oMCSDbContext.ChatClients.FirstOrDefault(c => c.ConnectionId == clientId);
        }

        public ChatClient GetClientByIdWithUser(string clientId)
        {
            return _oMCSDbContext.ChatClients.Include(c => c.User).Include(a => a.User.ConnectedClients).FirstOrDefault(u => u.ConnectionId == clientId);
        }

        public string GetUserRoomPresence(int userId, int roomId)
        {
            return _oMCSDbContext.ChatRoomUsers.Any(a => a.UserId == userId && a.RoomId == roomId) ? "present" : "absent";
        }

        public IList<CollaborationUserInfo> GetAllCollaborationUsers(string userName, int pageIndex, int pageSize, string cultureName, out int itemsCount)
        {
            try
            {
                IQueryable<CollaborationUserInfo> collaborationUserInfos = _oMCSDbContext.UserProfiles
                                                                            .Where(u => u.IsActive == true && u.IsDeleted == false
                                                                                 && (userName == null || u.LocalizationIdentifier.Localizations
                                                                                 .Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.ToLower().Contains(userName.ToLower())))
                                                                             .Select(user => new

                                                                             {
                                                                                 user.Id,
                                                                                 user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text

                                                                             }).AsQueryable().Select(c => new CollaborationUserInfo
                                                                             {
                                                                                 UserId = c.Id,
                                                                                 UserName = c.Text != null? c.Text : string.Empty,
                                                                                 NotificationCount = 0
                                                                             }).AsQueryable();

                if (!collaborationUserInfos.Any())
                {
                    itemsCount = 0;
                }
                itemsCount = collaborationUserInfos.Count();
                var skip = pageIndex * pageSize;

                return collaborationUserInfos.OrderBy(u => u.UserName).Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<int> GetOfflineOrInactiveUsersInRoom(int userId, int roomId, int messageId)
        {
            return _oMCSDbContext.ChatRoomAllowedUsers
                     .Where(ChatRoomAllowedUser => ChatRoomAllowedUser.UserId != userId && 
                            ChatRoomAllowedUser.RoomId == roomId && 
                            (!ChatRoomAllowedUser.User.ChatMessagesStatus.Any() || ChatRoomAllowedUser.User.ChatMessagesStatus.Any(status => status.RoomId == roomId && status.MessageId < messageId)))
                     .Select(u => u.UserId)
                     .ToList();
        }

        #endregion
    }
}
