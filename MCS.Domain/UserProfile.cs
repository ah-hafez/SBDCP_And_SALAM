using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserProfile : EntityBase, IAuditable
    {
        public string IdentityId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public int TitleId { get; set; }
        public int? CategoryId { get; set; }
        public int TransactionProcessingPeriod { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LocalName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsInternal { get; set; }
        public string UserNationalId { get; set; }
        public bool AllowMobile { get; set; }
        public DateTimeOffset? LastActivity { get; set; }
        public int? Status { get; set; }
        public int MainOrgUnitId { get; set; }
        public int Gender { get; set; }
        public bool IsManager { get; set; }
        public string Password { get; set; }
        public int? ExternalId { get; set; }
        public virtual Document UserImage { get; set; }
        public virtual Lookup Title { get; set; }
        public virtual UserCategory Category { get; set; }
        public virtual UserProfile DirectManager { get; set; }
        public virtual IList<OrgUnit> OrgUnits { get; set; }
        public virtual IList<UserGroup> UserGroups { get; set; }
        public virtual IList<UserPermission> Permissions { get; set; }
        //public int GroupId { get; set; }
        //public virtual Group Group { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public virtual ICollection<ChatRoomOwner> OwnedRooms { get; set; }
        public virtual ICollection<ChatRoomUser> Rooms { get; set; }
        public virtual ICollection<ChatClient> ConnectedClients { get; set; }
        public virtual ICollection<ChatRoomAllowedUser> AllowedRooms { get; set; }
        public virtual ICollection<ChatMessagesStatus> ChatMessagesStatus { get; set; }
        public bool? PendingRegestration { get; set; }
        public string InternalNumber { get; set; }
        public string ApiKey { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LastLogout { get; set; }
        public int? UserMobileClassId { get; set; }
        public virtual Lookup UserMobileClass { get; set; }


    }
}
