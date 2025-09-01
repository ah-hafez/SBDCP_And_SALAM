using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionPathDetails : EntityBase
    {
        public int TransactionPathId { get; set; }
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int ActionId { get; set; }
        public int Sort { get; set; }

        public virtual TransactionPath TransactionPath { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual Action Action { get; set; }
    }
}
