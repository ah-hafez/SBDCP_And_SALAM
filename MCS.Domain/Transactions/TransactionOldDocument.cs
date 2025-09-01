using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCS.Domain
{
    public class TransactionOldDocument : EntityBase, IAuditable
    {
        public string Content { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int TransactionId { get; set; }

    }
}
