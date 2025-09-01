using System;

namespace MCS.DTO
{
    public class FollowUpAuditTrailDTO
    {
        public int Id { get; set; }
        public int FollowupId { get; set; }
        public int ProccessId { get; set; }
        public string ProccessDescription { get; set; }
        public DateTime ProccessDate { get; set; }
        public int UserId { get; set; }
        public virtual UserProfileDTO User { get; set; }
        public int EntityId { get; set; }

        public virtual OrgUnitDTO Entity { get; set; }

    }
}
