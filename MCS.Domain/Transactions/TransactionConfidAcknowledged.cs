using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
   public class TransactionConfidAcknowledged : EntityBase
    {
        public int TransactionId { get; set; }
        public int EntityId { get; set; }
        public OrgUnit Entity { get; set; } 
        public int? UserId { get; set; }
        public UserProfile User { get; set; } 
    }
}
