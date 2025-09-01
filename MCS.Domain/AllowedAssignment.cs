using System;
using System.Collections.Generic;
using MCS.Framework.Entities;
using MCS.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCS.Domain
{
    public class AllowedAssignment : EntityBase
    {
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public int EntityId { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public virtual OrgUnit Entity { get; set; }

    }
}
