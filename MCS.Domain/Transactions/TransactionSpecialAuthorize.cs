using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class TransactionSpecialAuthorize : EntityBase, IAuditable
    {
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public DateTime? ExpiredDate { get; set; }


    }
}
