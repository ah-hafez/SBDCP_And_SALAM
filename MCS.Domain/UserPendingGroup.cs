using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserPendingGroup : EntityBase
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual Group Group { get; set; }
        public bool IsApproved { get; set; }


    }
}
