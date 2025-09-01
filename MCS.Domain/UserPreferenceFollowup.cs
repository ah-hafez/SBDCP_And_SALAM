using System;
using System.Collections.Generic;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class UserPreferenceFollowup : EntityBase
    {
        public int? FollowUpOrgId { get; set; }
        public int? FollowUpUserId { get; set; }
        public int OrgUnitId { get; set; }
        public int UserPreferenceId { get; set; }
    }
}
