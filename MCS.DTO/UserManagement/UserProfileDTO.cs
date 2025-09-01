using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string LocalName { get; set; }

        public string Category { get; set; }
        public int? CategoryId { get; set; }
        public string Email { get; set; }

        public List<LocalizationDTO> Names { get; set; }
        public List<int?> OrgUnits { get; set; }

        public List<int> UserGroups { get; set; }

        public List<string> OrgUnitsNames { get; set; }
        public bool IsSelected { get; set; }
        public string MainOrgUnitName { get; set; }

        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public int GroupId { get; set; }
        public string RoleName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public int? UserImageId { get; set; }

        public virtual ICollection<ChatRoomOwnerDTO> OwnedRooms { get; set; }
        public virtual ICollection<ChatRoomUserDTO> Rooms { get; set; }
        public DateTime LastActivity { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<ChatClientDTO> ConnectedClients { get; set; }
        public virtual ICollection<ChatRoomAllowedUserDTO> AllowedRooms { get; set; }
        public bool IsDeleted { get; set; }
        public int? ExternalId { get; set; }
        public bool? PendingRegestration { get; set; }
        public bool AllowMobile { get; set; }
        public string InternalNumber { get; set; }
        public string ApiKey { get; set; }
        public int TitileId { get; set; }
        public string Title { get; set; }
        public bool IsVipUser { get; set; }
        public int TransactionProcessingPeriod { get; set; }
        public string UserNationalId { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public List<UserGroupDTO> UserGroupDTOs { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LastLogout { get; set; }
        public List<OrgUnitDTO> OrgUnitDTOs { get; set; }
        public int MainOrgUnitId { get; set; }



    }
}
