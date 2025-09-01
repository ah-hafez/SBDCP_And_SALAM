using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Business;
using MCS.Common;
using MCS.DTO;
using MCS.Service.Helpers;
using MCS.MCM.Domain;
using MCS.Service.Mappers;
using MCS.Framework;
using MCS.Common.TransactionContext;

namespace MCS.Service.Hubs
{
    [HubAuthorization]
    [HubName("chatServiceHub")]
    public class ChatServiceHub : Hub
    {
        private ITransactionContextScopeFactory context = new  TransactionContextScopeFactory();
        private static readonly TimeSpan _disconnectThreshold = TimeSpan.FromSeconds(10);
        public override System.Threading.Tasks.Task OnConnected()
        {
            CheckStatus();
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {
            using (var transactionContextScope = context.Create())
            {
                var user = GetCurrentUser();
                if (user == null)
                {
                    // The user isn't logged in 
                    throw new HubException("المستخدم غير موجود");
                }
                if (user == null)
                {
                    //_logger.Log("Reconnect failed user {0}:{1} doesn't exist.", userId, Context.ConnectionId);
                    return TaskAsyncHelper.Empty;
                }

                // Make sure this client is being tracked
                ChatBL.AddClient(user.Id, Context.ConnectionId, UserAgent, DateTimeOffset.UtcNow);
                user = GetCurrentUser();
                var currentStatus = (UserStatus)user.Status;

                if (currentStatus == UserStatus.Offline)
                {
                    //_logger.Log("{0}:{1} reconnected after temporary network problem and marked offline.", user.Name, Context.ConnectionId);

                    // Mark the user as inactive
                    user.Status = (int)UserStatus.Inactive;
                    ChatBL.ChangeStatus(user.Id, UserStatus.Inactive);
                    OnUserStatusChanged(user);
                    // If the user was offline that means they are not in the user list so we need to tell
                    // everyone the user is really in the room
                    var userViewModel = user;

                    foreach (var room in user.Rooms?.Select(a => a.ChatRoom).ToList())
                    {
                        var isOwner = user.OwnedRooms.Any(a => a.RoomId == room.Id);

                        // Tell the people in this room that you've joined
                        Clients.Group(room.Name).addUser(userViewModel, room.Name, isOwner);
                    }
                }
                else
                {
                    //_logger.Log("{0}:{1} reconnected after temporary network problem.", user.Name, Context.ConnectionId);
                }

                CheckStatus();
            }
            return base.OnReconnected();
        }
        public void Join()
        {
            Join(reconnecting: false);
        }
        public void Join(bool reconnecting)
        {
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    var user = GetCurrentUser();
                    if (user != null)
                    {
                        bool isNotify = ChatBL.IsUnreadMessagesForUser(user.Id);
                        Clients.Client(Context.ConnectionId).chatNotifyFlag(isNotify);
                    }
                    if (reconnecting)
                    {
                        //_logger.Log("{0}:{1} connected after dropping connection.", user.Name, Context.ConnectionId);
                        // If the user was marked as offline then mark them inactive
                        if (user.Status == (int)UserStatus.Offline)
                        {
                            user.Status = (int)UserStatus.Inactive;
                            ChatBL.ChangeStatus(user.Id, UserStatus.Inactive);
                            OnUserStatusChanged(user);
                        }
                        // Ensure the client is re-added
                        ChatBL.AddClient(user.Id, Context.ConnectionId, UserAgent, DateTimeOffset.UtcNow);
                        user = GetCurrentUser();
                    }
                    else
                    {
                        // _logger.Log("{0}:{1} connected.", user.Name, Context.ConnectionId);
                        // Update some user values

                        ChatBL.UpdateActivity(user.Id, Context.ConnectionId, UserAgent, DateTimeOffset.UtcNow);
                        user = GetCurrentUser();
                        OnUserStatusChanged(user);
                    }
                    ClientState clientState = GetClientState();
                    OnUserInitialize(clientState, user, reconnecting);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw new HubException("Contact System admin");
            }
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //_logger.Log("OnDisconnected({0})", Context.ConnectionId);

            DisconnectClient(Context.ConnectionId, useThreshold: true);

            return base.OnDisconnected(stopCalled);
        }
        public bool Send(string content, string roomName, int? toUserId = null)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return true;
            }

            var message = new ChatMessageDTO
            {
                Content = content,
                Room = new ChatRoomDTO
                {
                    Name = roomName
                }
            };

            return Send(message);
        }
        public bool Send(ChatMessageDTO clientMessage)
        {
            using (var transactionContextScope = context.Create())
            {
                CheckStatus();
                var user = GetCurrentUser();
                if (user == null)
                {
                    // The user isn't logged in 
                    throw new HubException("المستخدم غير موجود");
                }
                if (string.IsNullOrEmpty(clientMessage.Room.Name))
                {
                    throw new HubException("غير موجودة");
                }
                var data = ChatBL.VerifyUserRoom(user.Id, clientMessage.Room.Name);
                var room = data.ToChatRoomDTO();
                room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
                if (room == null || room.Private && !user.AllowedRooms.Any(a => a.RoomId == room.Id))
                {
                    return false;
                }
                // REVIEW: Is it better to use the extension method room.EnsureOpen here?
                if (room.Closed)
                {
                    throw new HubException("مغلقة");
                }

                // Update activity *after* ensuring the user, this forces them to be active
                UpdateActivity(user, room);

                var addMessageData = ChatBL.AddMessage(user.Id, room.Id, clientMessage.Content);
                var chatMessage = addMessageData.ToChatMessageDTO();
                chatMessage.Room = addMessageData.Room.ToChatRoomDTO();
                var result = new MessageResultDTO()
                {
                    Id = chatMessage.Id,
                    Content = chatMessage.Content,
                    HtmlContent = chatMessage.HtmlContent,
                    User = user,
                    UserRoomPresence = user != null ? ChatBL.GetUserRoomPresence(chatMessage.UserId, room.Id) : "present",
                    When = chatMessage.When,
                    MessageType = chatMessage.MessageType,
                    SendTime = chatMessage.When.AddMinutes(TimeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    SendDate = chatMessage.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                };
                //DocumentsBL documentsBL = new DocumentsBL();
                //if (result.User != null && result.User.PicId.HasValue)
                //{
                //    result.UserImage = documentsBL.GetDocumentById(result.User.PicId.Value).BLOB;
                //}

                if (clientMessage.Id == 0)
                {
                    // If the client didn't generate an id for the message then just
                    // send it to everyone. The assumption is that the client has some ui
                    // that it wanted to update immediately showing the message and
                    // then when the actual message is roundtripped it would "solidify it".
                    Clients.Group(room.Name).addMessage(result, room.Name);
                    var ids = ChatBL.GetAllowedClientIds(room.AllowedUsers.Select(x => x.UserId).ToList());
                    Clients.Clients(ids).updateConversationList(result, room.Name);
                }
                else
                {
                    // If the client did set an id then we need to give everyone the real id first
                    Clients.OthersInGroup(room.Name).addMessage(result, room.Name);
                    // Now tell the caller to replace the message
                    Clients.Caller.replaceMessage(clientMessage.Id, result, room.Name);
                }

                var profileIds = ChatBL.GetOfflineOrInactiveUsersInRoom(user.Id, room.Id, result.Id);
                if (profileIds.Any())
                {
                    Clients.Clients(ChatBL.GetAllowedClientIds(profileIds)).chatPushNotification(new { Title = user.LocalName, Body = chatMessage.Content, RoomName = room.Name });
                }

                return true;
            }
        }
        public void SetLastMessageRead(int id)
        {
            using (var transactionContextScope = context.Create())
            {
                try
                {
                    var user = GetCurrentUser();
                    var messageData = ChatBL.GetMessageById(id);
                    var message = messageData.ToChatMessageDTO();
                    message.Room = messageData.Room.ToChatRoomDTO();
                    var data = ChatBL.VerifyUserRoom(user.Id, message.Room.Name);
                    var room = data.ToChatRoomDTO();
                    room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                    room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                    room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
                    if (room == null || (room.Private && !user.AllowedRooms.Any(a => a.RoomId == room.Id)))
                    {
                        throw new HubException("Your not in this room");
                    }
                    var result = new MessageStatusResultDTO();
                    ChatMessagesStatus messageStatus = ChatBL.GetMessagesStatus(user.Id, room.Id);
                    if (messageStatus != null)
                    {
                        ChatBL.UpdateMessageStatus(user.Id, room.Id, message.Id);
                        messageStatus.MessageId = message.Id;
                        messageStatus.LastUpdatedDate = DateTime.UtcNow;

                        result = new MessageStatusResultDTO()
                        {
                            UserId = messageStatus.UserId,
                            LastReadMessageId = messageStatus.MessageId,
                            RoomId = room.Id,
                            RoomName = room.Name,
                        };
                    }
                    else
                    {
                        messageStatus = new ChatMessagesStatus()
                        {
                            UserId = user.Id,
                            MessageId = message.Id,
                            RoomId = room.Id
                        };
                        messageStatus = ChatBL.AddMessageStatus(messageStatus);
                        result = new MessageStatusResultDTO()
                        {
                            UserId = messageStatus.UserId,
                            LastReadMessageId = messageStatus.MessageId,
                            RoomId = room.Id,
                            RoomName = room.Name,
                        };
                    }

                    Clients.Group(room.Name).newMessageRead(result, room.Name);
                    bool isNotify = ChatBL.IsUnreadMessagesForUser(user.Id);
                    Clients.Client(Context.ConnectionId).chatNotifyFlag(isNotify);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex.Message);
                    throw new HubException("Contact System admin");
                }
            }
        }
        public void DeleteMessage(int id)
        {
            try
            {
                var user = GetCurrentUser();

                var getMessageByIdData = ChatBL.GetMessageById(id);
                var message = getMessageByIdData.ToChatMessageDTO();
                message.Room = getMessageByIdData.Room.ToChatRoomDTO();

                var data = ChatBL.VerifyUserRoom(user.Id, message.Room.Name);
                var room = data.ToChatRoomDTO();
                room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
                if (room == null || room.Private && !user.AllowedRooms.Any(a => a.RoomId == room.Id))
                {
                    throw new HubException("Your not in this room");
                }
                if (message.UserId != user.Id)
                {
                    throw new HubException("You aren't the owner of this message");
                }
                var messageData = ChatBL.DeleteMessage(message.Id);
                message = messageData.ToChatMessageDTO();
                message.Room = messageData.Room.ToChatRoomDTO();
                Clients.Group(room.Name).onDeleteMessage(id, message, room.Name);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                throw new HubException("Contact System admin");
            }
        }
        public void UpdateActivity()
        {
            var user = GetCurrentUser();

            foreach (var room in user.Rooms.Select(a => a.ChatRoom).ToList())
            {
                UpdateActivity(user, room);
            }

            CheckStatus();
        }
        public void TabOrderChanged(string[] tabOrdering)
        {
            var user = GetCurrentUser();

            //ChatUserPreferencesDTO userPreferences = user.Preferences;
            //userPreferences.TabOrder = tabOrdering.ToList();
            //user.Preferences = userPreferences;



            Clients.User(user.UserName).updateTabOrder(tabOrdering);
        }
        public void Leave(string roomName)
        {
            using (var transactionContextScope = context.Create())
            {
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    throw new HubException("فارغ");
                }
                var data = ChatBL.VerifyRoom(roomName, mustBeOpen: false);
                var room = data.ToChatRoomDTO();
                room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
                var user = GetCurrentUser();
                LeaveRoom(user, room);
            }
        }
        public void JoinRoom(string roomName)
        {
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    var user = GetCurrentUser();
                    var data = ChatBL.VerifyRoom(roomName, mustBeOpen: false);
                    var room = data.ToChatRoomDTO();
                    room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                    room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                    room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
                    if (!ChatBL.IsUserInRoom(user.Id, room.Id))
                    {
                        ChatBL.JoinRoom(user.Id, room.Id);
                        //ChatUserPreferencesDTO userPreferences = user.Preferences;
                        //userPreferences.TabOrder.Add(room.Name);
                        //user.Preferences = userPreferences;

                    }
                    UserProfileDTO userViewModel = user;

                    var info = new RoomInfoResultDTO
                    {
                        Name = room.Name,
                        Private = room.Private,
                        Closed = room.Closed
                    };
                    var isOwner = user.OwnedRooms.Any(a => a.RoomId == room.Id);

                    // Tell all clients to join this room
                    //Clients.User(user.UserName).joinRoom(info);

                    // Tell the people in this room that you've joined
                    //Clients.Group(room.Name).addUser(userViewModel, room.Name, isOwner);

                    // Notify users of the room count change
                    OnRoomChanged(room);
                    foreach (var client in user.ConnectedClients)
                    {
                        Groups.Add(client.ConnectionId, room.Name);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void LogOut()
        {
            var user = GetCurrentUser();
            foreach (var client in user.ConnectedClients)
            {
                DisconnectClient(client.ConnectionId);
                Clients.Client(client.ConnectionId).logOut();
            }
        }
        public List<ConversationChatDTO> GetConversations(int? transactionId, int? toUserId, int pageIndex, int pageSize, string cultureName)
        {
           
            using (var transactionContextScope = context.Create())
            {
                var callingUser = GetCurrentUserEntity();
                var data = new List<ConversationChatDTO>();
                var conversationList = ChatBL.GetConversations(callingUser.Id, toUserId, transactionId, pageIndex, pageSize, out int itemsCount);
                conversationList.ForEach(room =>
                {
                    var conversation = new ConversationChatDTO { RoomName = room.Name };
                    var chatUser = room.AllowedUsers.Count < 2 ? callingUser :room.AllowedUsers.First(c => c.UserId != callingUser.Id).User;
                    conversation.UserId = chatUser.Id;
                    conversation.Name = chatUser.LocalName == null? chatUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : chatUser.LocalName;
                    conversation.Status = chatUser.Status != null? (UserStatus)chatUser.Status : UserStatus.Offline;
                    var lastMessage = ChatBL.GetLastMessageInRoom(room.Id);
                    var lastMessageReadInRoom = ChatBL.GetLastMessageReadInRoom(callingUser.Id, room.Id);
                    if (lastMessage == null)
                    {
                        conversation.TotalNumberOfUnreadMessages = 0;
                    }
                    else
                    {
                        conversation.TotalNumberOfUnreadMessages = ChatBL.TotalNumberOfUnreadMessages(lastMessageReadInRoom, room.Id);
                    }
                    conversation.LastMessage = lastMessage == null ? "" : lastMessage.Content;
                    conversation.When = lastMessage?.When.AddMinutes(TimeZone) ?? DateTimeOffset.UtcNow.AddMinutes(TimeZone);
                    conversation.SendTime = conversation.When.ToString("hh:mm tt", CultureInfo.InvariantCulture) ?? DateTimeOffset.UtcNow.AddMinutes(TimeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    conversation.PicId = null;//chatUser.PicId;
                                              //DocumentsBL documentsBL = new DocumentsBL();
                                              //if (chatUser.PicId != null)
                                              //{
                                              //    conversation.UserImage = documentsBL.GetDocumentById(conversation.PicId.Value).BLOB;
                                              //}




                data.Add(conversation);
                });

                bool isNotify = ChatBL.IsUnreadMessagesForUser(callingUser.Id);
                Clients.Client(Context.ConnectionId).chatNotifyFlag(isNotify);

                return data;
            }
        }

        public List<ConversationChatDTO> GetConversationByName(int? transactionId, int? toUserId, string roomName, string cultureName)
        {
            using (var transactionContextScope = context.Create())
            {
                var callingUser = GetCurrentUserEntity();
                var data = new List<ConversationChatDTO>();
                var conversationList = ChatBL.GetConversations(callingUser.Id, toUserId, transactionId, roomName);
                conversationList.ForEach(room =>
                {
                    var conversation = new ConversationChatDTO { RoomName = room.Name };
                    var chatUser = room.AllowedUsers.Count < 2 ? callingUser : room.AllowedUsers.First(c => c.UserId != callingUser.Id).User;
                    conversation.UserId = chatUser.Id;
                    conversation.Name = chatUser.LocalName == null ? chatUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : chatUser.LocalName;
                    conversation.Status = chatUser.Status != null ? (UserStatus)chatUser.Status : UserStatus.Offline;
                    var lastMessage = ChatBL.GetLastMessageInRoom(room.Id);
                    var lastMessageReadInRoom = ChatBL.GetLastMessageReadInRoom(callingUser.Id, room.Id);
                    if (lastMessage == null)
                    {
                        conversation.TotalNumberOfUnreadMessages = 0;
                    }
                    else
                    {
                        conversation.TotalNumberOfUnreadMessages = ChatBL.TotalNumberOfUnreadMessages(lastMessageReadInRoom, room.Id);
                    }
                    conversation.LastMessage = lastMessage == null ? "" : lastMessage.Content;
                    conversation.When = lastMessage?.When.AddMinutes(TimeZone) ?? DateTimeOffset.UtcNow.AddMinutes(TimeZone);
                    conversation.SendTime = conversation.When.ToString("hh:mm tt", CultureInfo.InvariantCulture) ?? DateTimeOffset.UtcNow.AddMinutes(TimeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    conversation.PicId = null;

                    data.Add(conversation);
                });

                //bool isNotify = ChatBL.TotalNumberOfUnreadMessagesForUser(callingUser.Id);
                //Clients.Client(Context.ConnectionId).chatNotifyFlag(isNotify);

                return data;
            }
        }

        public List<MessageResultDTO> GetPreviousMessages(int messageId)
        {
            using (var transactionContextScope = context.Create())
            {
                //DocumentsBL documentsBL = new DocumentsBL();
                List<MessageResultDTO> previousMessages = new List<MessageResultDTO>();
                var messagesData = ChatBL.GetPreviousMessages(messageId);
                messagesData.ForEach(message =>
                {
                    var result = new MessageResultDTO()
                    {
                        Id = message.Id,
                        Content = message.Content,
                        HtmlContent = message.HtmlContent,
                        User = message.User != null ? UserProfileMapper.MapUserProfileChat(message.User) : null,
                        UserRoomPresence = message.User != null ? ChatBL.GetUserRoomPresence(message.User.Id, message.RoomId) : "present",
                        When = message.When,
                        MessageType = message.MessageType,
                        SendTime = message.When.AddMinutes(TimeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                        SendDate = message.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };
                //if (result.User != null && result.User.PicId.HasValue)
                //{
                //    result.UserImage = documentsBL.GetDocumentById(result.User.PicId.Value).BLOB;
                //}
                previousMessages.Add(result);
                });
                //previousMessages.Reverse();
                return previousMessages;
            }
        }
        public void Typing(string roomName)
        {
            var user = GetCurrentUser();
            var data = ChatBL.VerifyUserRoom(user.Id, roomName);
            var room = data.ToChatRoomDTO();
            room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
            room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
            room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();
            if (room == null || (room.Private && !user.AllowedRooms.Any(a => a.RoomId == room.Id)))
            {
                return;
            }

            UpdateActivity(user, room);
            Clients.Group(room.Name).setTyping(user, room.Name);
        }
        public RoomInfoResultDTO GetRoomInfo(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return null;
            }

            using (var transactionContextScope = context.Create())
            {
                var user = GetCurrentUser();
                var data = ChatBL.VerifyUserRoom(user.Id, roomName);
                var room = data.ToChatRoomDTO();
                room.AllowedUsers = data.AllowedUsers.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
                room.Owners = data.Owners.Select(a => a.ToChatRoomOwnerDTO()).ToList();
                room.Users = data.Users.Select(c => c.ToChatRoomUserDTO()).ToList();

                if (room == null || (room.Private && !user.AllowedRooms.Any(a => a.RoomId == room.Id)))
                {
                    return null;
                }
                return GetRoomInfoCore(room);
            }
        }

        public List<CollaborationUserInfoDTO> GetAllCollaborationUsers(string userName, int pageIndex, int pageSize, string cultureName)
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IList<CollaborationUserInfo> collaborationUserInfos = ChatBL.GetAllCollaborationUsers(userName, pageIndex, pageSize, cultureName, out int itemsCount);

                List<CollaborationUserInfoDTO> collaborationUserInfoDTOs = ConversationMapper.Map(collaborationUserInfos);

                return collaborationUserInfoDTOs;

            }
        }

        public RoomInfoResultDTO OneToOneRoom(int toUserId, int? transactionId, string roomName, bool isForShare = false)
        {
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    int callingUserId = GetCurrentUser().Id;
                    if (string.IsNullOrWhiteSpace(roomName))
                    {
                        string roomPrefex = "DM";
                        if (isForShare)
                            roomPrefex = "TR_SL_" + Guid.NewGuid();
                        else if (transactionId.HasValue && transactionId.Value == -1 && !isForShare)
                        {
                            roomPrefex = "TR_" + Guid.NewGuid();
                            transactionId = null;
                        }
                        else if (transactionId.HasValue && transactionId.Value != -1 && !isForShare)
                            roomPrefex = "TR_" + transactionId.ToString();

                        roomName = callingUserId > toUserId ? string.Format("{0}_{1}_{2}", roomPrefex, callingUserId, toUserId) : string.Format("{0}_{1}_{2}", roomPrefex, toUserId, callingUserId);
                    }

                    ChatRoom chatRoom = null;
                    chatRoom = ChatBL.GetRoomByName(roomName);
                    if (chatRoom == null)
                    {
                        chatRoom = ChatBL.AddOneToOneRoom(callingUserId, toUserId, roomName, transactionId);
                    }

                    JoinRoom(roomName);
                    RoomInfoResultDTO roomInfo = GetRoomInfo(roomName);

                    return roomInfo;
                }
            }
            catch (Exception ex)
            {
                throw new HubException("Error creating room!");
            }
        }

        public void InviteTransactionShareLetter(int toUserId, string cultureName)
        {
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    var chatUser = GetCurrentUser();
                    var userName = chatUser.LocalName;

                    var toClientId = ChatBL.GetAllowedClientIds(new List<int>() { toUserId });

                    if (toClientId != null && toClientId.Count > 0)
                    {
                        Clients.Client(toClientId[0]).inviteShareLetter(userName, chatUser.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HubException("Error inviting user!");
            }
        }

        public void RejectShareLetterInvitation(int fromUserId)
        {
            using (var transactionContextScope = context.Create())
            {
                var toClientId = ChatBL.GetAllowedClientIds(new List<int>() { fromUserId });
                if (toClientId != null && toClientId.Count > 0)
                {
                    Clients.Client(toClientId[0]).rejectShareLetter();
                }
            }
        }

        public void AcceptShareLetterInvitation(int fromUserId, string roomName)
        {
            using (var transactionContextScope = context.Create())
            {
                var toClientId = ChatBL.GetAllowedClientIds(new List<int>() { fromUserId });
                if (toClientId != null && toClientId.Count > 0)
                {
                    JoinRoom(roomName);
                    RoomInfoResultDTO roomInfo = GetRoomInfo(roomName);

                    Clients.Client(toClientId[0]).acceptShareLetter(roomInfo);
                }
            }
        }

        public void ShareLetterContent(string content, string roomName)
        {
            if (!string.IsNullOrWhiteSpace(roomName))
            {
                Clients.Group(roomName).receiveLetterContent(content, roomName);
            }
        }

        public void CreateTransactionChatWindow(int transactionId, string transactionNumber, string roomName)
        {
            if (!string.IsNullOrWhiteSpace(roomName))
            {
                using (var transactionContextScope = context.Create())
                {
                    var currentUser = GetCurrentUser();
                    var chatRoom = ChatBL.GetRoomByName(roomName);
                    var userIds = ChatBL.GetAllowedClientIds(chatRoom.AllowedUsers.Where(u => u.UserId != currentUser.Id).Select(r => r.UserId).ToList());
                    Clients.Clients(userIds).createTransactionChatWindow(transactionId, transactionNumber, roomName);
                }
            }
        }

        public void BindRoomsToTransaction(int transactionId, string roomIds)
        {
            if (!string.IsNullOrWhiteSpace(roomIds))
            {
                using (var transactionContextScope = context.Create())
                {
                    var roomList = roomIds.Split(',');
                    ChatBL.BindRoomsTransaction(roomList, transactionId);
                }
            }
        }

        public void BusySharing(int toUserId)
        {
            using (var transactionContextScope = context.Create())
            {
                var toClientId = ChatBL.GetAllowedClientIds(new List<int>() { toUserId });
                if (toClientId != null && toClientId.Count > 0)
                {
                    Clients.Client(toClientId[0]).busySharing();
                }
            }
        }

        private RoomInfoResultDTO GetRoomInfoCore(ChatRoomDTO room)
        {
            var info = new RoomInfoResultDTO
            {
                Id = room.Id,
                Name = room.Name,
                Users = ChatBL.GetOnlineUsers(room.Id).Select(a => UserProfileMapper.MapUserProfileChat(a)).ToList(),
                Owners = ChatBL.GetOnlineOwners(room.Id).Select(a => a.Id).ToList(),
                Closed = room.Closed
            };

            var recentMessages = new List<MessageResultDTO>();
            var messages = ChatBL.GetMessagesByRoom(room.Name);
            //DocumentsBL documentsBL = new DocumentsBL();

            messages.ForEach(message =>
            {
                var result = new MessageResultDTO()
                {
                    Id = message.Id,
                    Content = message.Content,
                    HtmlContent = message.HtmlContent,
                    User = message.User != null? UserProfileMapper.MapUserProfileChat(message.User) : null,
                    When = message.When,
                    MessageType = message.MessageType,
                    SendTime = message.When.AddMinutes(TimeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    SendDate = message.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                };
                //if (result.User != null && result.User.PicId.HasValue)
                //{
                //    result.UserImage = documentsBL.GetDocumentById(result.User.PicId.Value).BLOB;
                //}
                recentMessages.Add(result);
            });
            recentMessages.Reverse();

            info.RecentMessages = recentMessages;
            var usersMessageStatus = ChatBL.GetMessagesStatus(room.Id);
            info.UsersMessageStatus = new List<MessageStatusResultDTO>();
            if (usersMessageStatus.Any())
            {
                info.UsersMessageStatus = usersMessageStatus.Select(result => new MessageStatusResultDTO()
                {
                    LastReadMessageId = result.MessageId,
                    UserId = result.UserId,
                    RoomId = result.RoomId,
                    RoomName = room.Name
                }).ToList();
            }
            return info;
        }
        private void LeaveRoom(UserProfileDTO user, ChatRoomDTO room)
        {
            using (var transactionContextScope = context.Create())
            {
                ChatBL.LeaveRoom(user.Id, room.Id);
                Clients.Group(room.Name).leave(user, room.Name);

                foreach (var client in user.ConnectedClients)
                {
                    Groups.Remove(client.ConnectionId, room.Name);
                }

                OnRoomChanged(room);
            }
        }
        private void OnRoomChanged(ChatRoomDTO room)
        {
            using (var transactionContextScope = context.Create())
            {
                var info = new RoomInfoResultDTO
                {
                    Name = room.Name,
                    Private = room.Private,
                    Closed = room.Closed,
                    Count = ChatBL.GetOnlineUsers(room.Id).Count()
                };
                // notify all clients who can see the room
                if (!room.Private)
                {
                    Clients.All.updateRoom(info);
                }
                else
                {
                    Clients.Clients(ChatBL.GetAllowedClientIds(room.AllowedUsers.Select(a => a.UserId).ToList())).updateRoom(info);
                }
            }
        }
        private void DisconnectClient(string clientId, bool useThreshold = false)
        {
            if (context == null)
            {
                context = new TransactionContextScopeFactory();
            }
            using (var transactionContextScope = context.Create())
            {
                string userId = ChatBL.DisconnectClient(clientId);

                if (string.IsNullOrEmpty(userId))
                {
                    return;
                }

                if (useThreshold)
                {
                    Thread.Sleep(_disconnectThreshold);
                }
                //var user = GetCurrentUser();

                //// There's no associated user for this client id
                //if (user == null)
                //{
                //    return;
                //}
                //user = GetCurrentUser();

                //// The user will be marked as offline if all clients leave
                //if (user.Status == (int)UserStatus.Offline)
                //{
                //    OnUserStatusChanged(user);
                //    foreach (var room in user.Rooms.Select(a => a.ChatRoom).ToList())
                //    {
                //        var userViewModel = user;

                //        Clients.OthersInGroup(room.Name).leave(userViewModel, room.Name);
                //    }
                //}
            }
        }
        private bool OutOfSync
        {
            get
            {
                string version = Context.QueryString["version"];
                if (String.IsNullOrEmpty(version))
                {
                    return true;
                }
                return false;
            }
        }
        private int TimeZone
        {
            get
            {
                string timezone = Context.QueryString["timezone"];
                if (string.IsNullOrEmpty(timezone))
                {
                    return 180;
                }
                return (int.Parse(timezone) * -1);
            }
        }
        private string UserAgent
        {
            get
            {
                if (Context.Headers != null && Context.Headers.Any(a => a.Key == "User-Agent"))
                {
                    return Context.Headers["User-Agent"];
                }
                return null;
            }
        }

        private string LocalCulture
        {
            get
            {
                string culture = Context.QueryString["culture"];
                if (!string.IsNullOrWhiteSpace(culture))
                {
                    return culture;
                }
                return "ar";
            }
        }

        private void UpdateActivity(UserProfileDTO user, ChatRoomDTO room)
        {
            UpdateActivity(user);

            OnUpdateActivity(user, room);
        }
        private void OnUpdateActivity(UserProfileDTO user, ChatRoomDTO room)
        {
            Clients.Group(room.Name).updateActivity(user, room.Name);
        }
        private void OnUserStatusChanged(UserProfileDTO user)
        {
            Clients.All.userStatusChanged(user);
        }
        private void UpdateActivity(UserProfileDTO user)
        {
            ChatBL.UpdateActivity(user.Id, Context.ConnectionId, UserAgent, DateTimeOffset.UtcNow);
            OnUserStatusChanged(user);
        }
        private void LogOn(UserProfileDTO user, string clientId, bool reconnecting)
        {
            if (!reconnecting)
            {
                // Update the client state
                //Clients.Caller.id = user.IdentityId;
                Clients.Caller.name = user.UserName;
            }
            var rooms = new List<RoomInfoResultDTO>();
            var privateRooms = new List<RoomInfoResultDTO>();
            var userViewModel = user;
            var ownedRooms = user.OwnedRooms?.Select(r => r.RoomId).ToList();
            foreach (var room in user.Rooms.Select(a => a.ChatRoom).ToList())
            {
                var isOwner = ownedRooms.Contains(room.Id);
                // Tell the people in this room that you've joined
                Clients.Group(room.Name).addUser(userViewModel, room.Name, isOwner);
                // Add the caller to the group so they receive messages
                Groups.Add(clientId, room.Name);
                if (!reconnecting)
                {
                    // Add to the list of room names
                    rooms.Add(new RoomInfoResultDTO
                    {
                        Name = room.Name,
                        Private = room.Private,
                        Closed = room.Closed
                    });
                }
            }
            if (!reconnecting)
            {
                foreach (var r in user.AllowedRooms.Select(a => a.ChatRoom).ToList())
                {
                    privateRooms.Add(new RoomInfoResultDTO
                    {
                        Name = r.Name,
                        Private = r.Private,
                        Closed = r.Closed,
                    });
                }
                // Initialize the chat with the rooms the user is in
                Clients.Caller.logOn(rooms, privateRooms);//, user.Preferences);
            }
        }
        private ClientState GetClientState()
        {
            // New client state
            var jabbrState = GetCookieValue("jabbr.state");
            var clientState = string.IsNullOrEmpty(jabbrState) ? new ClientState() : JsonConvert.DeserializeObject<ClientState>(jabbrState);
            return clientState;
        }
        private void OnUserInitialize(ClientState clientState, UserProfileDTO user, bool reconnecting)
        {
            LogOn(user, Context.ConnectionId, reconnecting);
        }
        private UserProfileDTO GetCurrentUser(string userId = null)
        {
            UserProfile userData = null;
            UserProfileDTO userProfileDTO = null;

            userId = string.IsNullOrEmpty(userId) ? Context.User.GetUserId() : userId;
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>("transient_IUserManagementBL");
            userData = userManagementBL.GetUserChatByIdentity(userId);

            userProfileDTO = UserProfileMapper.MapUserProfileChat(userData);
            userProfileDTO.AllowedRooms = userData.AllowedRooms.Select(a => a.ToChatRoomAllowedUseDTO()).ToList();
            userProfileDTO.Rooms = userData.Rooms.Select(a => a.ToChatRoomUserDTO()).ToList();
            userProfileDTO.OwnedRooms = userData.OwnedRooms.Select(a => a.ToChatRoomOwnerDTO()).ToList();
            userProfileDTO.ConnectedClients = userData.ConnectedClients?.Select(a => a.ToChatClientDTO()).ToList();
            userProfileDTO.LocalName = userData.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == LocalCulture).FirstOrDefault().Text;

            return userProfileDTO;

        }
        private UserProfile GetCurrentUserEntity(string userId = null)
        {
                userId = string.IsNullOrEmpty(userId) ? Context.User.GetUserId() : userId;
                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>("transient_IUserManagementBL");

                var userData = userManagementBL.GetUserChatByIdentity(userId);
                userData.LocalName = userData.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == LocalCulture).FirstOrDefault().Text;
                return userData;
        }
        private string GetCookieValue(string key)
        {
            Cookie cookie;
            Context.RequestCookies.TryGetValue(key, out cookie);
            string value = cookie?.Value;
            return value != null ? Uri.UnescapeDataString(value) : null;
        }
        private void CheckStatus()
        {
            if (OutOfSync)
            {
                Clients.Caller.outOfSync();
            }
        }

        public void test()
        {
        }
    }
}