using System.Collections.Generic;

namespace MCS.DTO
{
    public class AllowedAssignmentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public int EntityId { get; set; }
        public UserProfileDTO User { get; set; }
        public UserProfileDTO ToUser { get; set; } 
        public OrgUnitDTO Entity { get; set; }
        public virtual List<AllowedAssignmentDTO> SelectedUserAllowedAssignment { get; set; }
    }
}
