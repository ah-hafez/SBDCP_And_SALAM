using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class SavedTransactionAssignment : EntityBase, IAuditable
    {
        public string AssignmentList { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

    }

}
