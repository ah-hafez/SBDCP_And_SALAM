using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
   public class TransactionElcOutBoundDTO 
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int EntityId { get; set; }
        public OrgUnitDTO Entity { get; set; } 
        public int? UserId { get; set; }
        public UserProfileDTO User { get; set; }
        public bool Ishidden { get; set; } 
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
