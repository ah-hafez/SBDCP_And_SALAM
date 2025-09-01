using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;

namespace MCS.Service
{
    public static class ChatMapper
    {
        public static ChatRoom ToChatRoom(this ChatRoomDTO model)
        {
            if (model == null)
                return null;

            return new ChatRoom
            {
                Id = model.Id,
                Name = model.Name,
                Closed = model.Closed,
                CreatedBy = model.CreatedBy,
                LastNudged = model.LastNudged,
                OneToOne = model.OneToOne,
                Private = model.Private,
                CreatedOn = model.CreatedOn,
                AllowedUsers = model.AllowedUsers?.Select(al => al.ToChatRoomAllowedUser()).ToList(),
                Owners = model.Owners?.Select(al => al.ToChatRoomOwner()).ToList(),
                Users = model.Users?.Select(al => al.ToChatRoomUser()).ToList(),
                //MessagesStatus = model.MessagesStatus?.Select(m => m.ToMessagesStatus()).ToList(),
                //Messages = model.Messages?.Select(m => m.ToChatMessage()).ToList(),
                Transaction = model.Transaction != null? TransactionMapper.Map(model.Transaction) : null
            };
        }
        public static ChatRoomDTO ToChatRoomDTO(this ChatRoom model)
        {
            if (model == null) return new ChatRoomDTO();

            return new ChatRoomDTO
            {
                Id = model.Id,
                Name = model.Name,
                Closed = model.Closed,
                CreatedBy = model.CreatedBy.Value,
                LastNudged = model.LastNudged,
                OneToOne = model.OneToOne,
                Private = model.Private,
                CreatedOn = model.CreatedOn,
                Transaction = model.Transaction != null ? TransactionMapper.Map(model.Transaction) : null
            };
        }
        public static ChatMessage ToChatMessage(this ChatMessageDTO model)
        {
            if (model == null)
                return null;

            return new ChatMessage
            {
                Id = model.Id,
                Content = model.Content,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                HtmlContent = model.HtmlContent,
                HtmlEncoded = model.HtmlEncoded,
                MessageType = model.MessageType,
                RoomId = model.RoomId,
                UserId = model.UserId,
                Source = model.Source,
                //User = model.User != null? UserProfileMapper.Map(model.User) : null,
                Room = model.Room.ToChatRoom(),
                When = model.When,
                //MessagesStatus = model.MessagesStatus?.Select(m => m.ToMessagesStatus()).ToList(),

            };
        }
        public static ChatMessageDTO ToChatMessageDTO(this ChatMessage model)
        {
            if (model == null) return null;

            return new ChatMessageDTO
            {
                Id = model.Id,
                Content = model.Content,
                CreatedBy = model.CreatedBy.Value,
                CreatedOn = model.CreatedOn,
                HtmlContent = model.HtmlContent,
                HtmlEncoded = model.HtmlEncoded,
                MessageType = model.MessageType,
                RoomId = model.RoomId,
                UserId = model.UserId,
                Source = model.Source,
                User = model.User != null ? UserProfileMapper.MapUserProfileChat(model.User) : null,
                //Room = model.Room?.ToChatRoomDTO(),
                When = model.When,
                //MessagesStatus = model.MessagesStatus?.Select(m => m.ToMessagesStatusDTO()).ToList(),

            };
        }
        public static ChatMessagesStatus ToMessagesStatus(this MessagesStatusDTO model)
        {
            if (model == null)
                return null;

            return new ChatMessagesStatus
            {
                Id = model.Id,
                MessageId = model.MessageId,
                LastUpdatedDate = model.LastUpdatedDate,
                Message = model.Message?.ToChatMessage(),
                RoomId = model.RoomId,
                UserId = model.UserId,
                Room = model.Room?.ToChatRoom(),
                ///User = model.User?.ToUser(),

            };
        }
        public static MessagesStatusDTO ToMessagesStatusDTO(this ChatMessagesStatus model)
        {
            if (model == null) return null;

            return new MessagesStatusDTO
            {
                Id = model.Id,
                MessageId = model.MessageId,
                CreatedBy = model.CreatedBy.Value,
                CreatedOn = model.CreatedOn,
                LastUpdatedDate = model.LastUpdatedDate,
                Message = model.Message?.ToChatMessageDTO(),
                RoomId = model.RoomId,
                UserId = model.UserId,
                Room = model.Room?.ToChatRoomDTO(),
                User = model.User != null ? UserProfileMapper.MapUserProfileChat(model.User) : null,
            };
        }

        public static ChatClient ToChatClient(this ChatClientDTO model)
        {
            if (model == null)
                return null;

            return new ChatClient
            {
                Id = model.Id,
                Name = model.Name,
                ConnectionId = model.ConnectionId,
                LastActivity = model.LastActivity,
                LastClientActivity = model.LastClientActivity,
                UserAgent = model.UserAgent,
                UserId = model.UserId
            };
        }
        public static ChatClientDTO ToChatClientDTO(this ChatClient model)
        {
            if (model == null)
                return null;

            return new ChatClientDTO
            {
                Id = model.Id,
                Name = model.Name,
                ConnectionId = model.ConnectionId,
                LastActivity = model.LastActivity,
                LastClientActivity = model.LastClientActivity ?? DateTimeOffset.UtcNow,
                UserAgent = model.UserAgent,
                UserId = model.UserId
            };
        }

        public static ChatRoomOwner ToChatRoomOwner(this ChatRoomOwnerDTO model)
        {
            if (model == null)
                return null;

            return new ChatRoomOwner
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoom(),
                ///User = model.User?.ToUser()
            };
        }
        public static ChatRoomOwnerDTO ToChatRoomOwnerDTO(this ChatRoomOwner model)
        {
            if (model == null)
                return null;

            return new ChatRoomOwnerDTO
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoomDTO(),
                ///User = model.User?.ToUserDTO()
            };
        }
        public static ChatRoomAllowedUser ToChatRoomAllowedUser(this ChatRoomAllowedUserDTO model)
        {
            if (model == null)
                return null;

            return new ChatRoomAllowedUser
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoom(),
                ///User = model.User?.ToUser()
            };
        }
        public static ChatRoomAllowedUserDTO ToChatRoomAllowedUseDTO(this ChatRoomAllowedUser model)
        {
            if (model == null)
                return null;

            return new ChatRoomAllowedUserDTO
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoomDTO(),
                ///User = model.User?.ToUserDTO()
            };
        }
        public static ChatRoomUser ToChatRoomUser(this ChatRoomUserDTO model)
        {
            if (model == null)
                return null;

            return new ChatRoomUser
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoom(),
                ///User = model.User?.ToUser()
            };
        }
        public static ChatRoomUserDTO ToChatRoomUserDTO(this ChatRoomUser model)
        {
            if (model == null)
                return null;

            return new ChatRoomUserDTO
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                ChatRoom = model.ChatRoom?.ToChatRoomDTO(),
                ///User = model.User?.ToUserDTO()
            };
        }
    }
}